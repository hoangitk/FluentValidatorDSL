using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace FluentValidatorDSL
{
    public class Transpiler
    {
        private readonly Listener _listener;

        public Transpiler()
        {
            _listener = new Listener();
        }

        public FluentValidatorDSLParser.CompileUnitContext GenerateAST(string input)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new FluentValidatorDSLLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new FluentValidatorDSLParser(tokens);
            parser.ErrorHandler = new BailErrorStrategy();

            return parser.compileUnit();
        }
        public string GenerateTranspiledCode(string inputText)
        {
            var astree = GenerateAST(inputText);
            ParseTreeWalker.Default.Walk(_listener, astree);
            return _listener.Output;
        }
    }
}
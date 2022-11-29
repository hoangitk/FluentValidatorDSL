using Microsoft.CodeAnalysis;
using Microsoft.CSharp.RuntimeBinder;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace FluentValidatorDSL
{
    public class Scriptor
    {
        private static readonly MetadataReference[] References =
        {
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(RuntimeBinderException).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ExpressionType).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location),
            MetadataReference.CreateFromFile(typeof(FluentValidation.IValidationContext).GetTypeInfo().Assembly.Location),
        };

        private static readonly Lazy<Scriptor> _Instance = new Lazy<Scriptor>(() => new Scriptor());
        public static readonly Scriptor Instance = _Instance.Value;

        private Transpiler _transpiler;

        public Scriptor()
        {
            _transpiler = new Transpiler();
        }

        public async Task RunAsync(object model, string dslCode)
        {
            ScriptOptions scriptOptions = ScriptOptions.Default;
            scriptOptions = scriptOptions.AddReferences(References);
            scriptOptions = scriptOptions.AddImports(
                "System",
                "FluentValidation");

            //var script = CSharpScript.Create(csharpCode, scriptOptions, model.GetType());
            //await script.RunAsync(globals: model);
            var csharpCode = _transpiler.GenerateTranspiledCode(dslCode);
            await CSharpScript.RunAsync(
                code: csharpCode,
                options: scriptOptions,
                globals: model);
        }
    }
}
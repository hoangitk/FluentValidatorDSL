using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FluentValidatorDSL
{
    public class Listener : FluentValidatorDSLBaseListener
    {
        private StringBuilder _scriptBuilder;

        public string Output
        {
            get { return _scriptBuilder.ToString(); }
        }

        public Listener()
        {
            _scriptBuilder = new StringBuilder();
        }

        public override void EnterRuleFor([NotNull] FluentValidatorDSLParser.RuleForContext context)
        {
            string propertyName = context.ID().GetText();
            _scriptBuilder.Append($"RuleFor(x => x.{propertyName})");
        }

        public override void ExitRuleFor([NotNull] FluentValidatorDSLParser.RuleForContext context)
        {
            _scriptBuilder.AppendLine(";");
        }

        public override void EnterValidator([NotNull] FluentValidatorDSLParser.ValidatorContext context)
        {
            var validatorName = context.ID().GetText();
            _scriptBuilder.Append($".{validatorName}(");
        }

        public override void ExitValidator([NotNull] FluentValidatorDSLParser.ValidatorContext context)
        {
            _scriptBuilder.Append(")");
        }

        public override void EnterCallFunction([NotNull] FluentValidatorDSLParser.CallFunctionContext context)
        {
            var functionName = context.ID().GetText();
            _scriptBuilder.Append($"{functionName}(");
        }

        public override void ExitCallFunction([NotNull] FluentValidatorDSLParser.CallFunctionContext context)
        {
            _scriptBuilder.Append(")");
        }

        public override void EnterExprList([NotNull] FluentValidatorDSLParser.ExprListContext context)
        {
            var expList = context.expression();
            for (int i = 0; i < expList.Length; i++)
            {
                if (i > 0) _scriptBuilder.Append(", ");

                this.ResolveExpression(expList[i]);
            }
        }

        private void ResolveExpression(FluentValidatorDSLParser.ExpressionContext context)
        {
            if (context.callFunction() != null)
            {
                this.EnterCallFunction(context.callFunction());
            }
            else if (context.NUMBER() != null)
            {
                var number = context.NUMBER().GetText();
                _scriptBuilder.Append(number);
            }
            else if (context.BOOLEAN() != null)
            {
                var boolean = context.BOOLEAN().GetText();
                _scriptBuilder.Append(boolean);
            }
            else if (context.STRING() != null)
            {
                var text = context.STRING().GetText().Replace('\'', '"');
                _scriptBuilder.Append($"{text}");
            }
            else if (context.ID() != null)
            {
                var propertyName = context.ID().GetText();
                _scriptBuilder.Append($"x => x.{propertyName}");
            }
            else
            {
            }
        }
    }
}
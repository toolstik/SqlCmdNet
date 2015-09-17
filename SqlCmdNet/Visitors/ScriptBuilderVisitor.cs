using System.Text;
using SqlCmdNet.Statements;

namespace SqlCmdNet.Visitors
{
    class ScriptBuilderVisitor : ISqlCmdParserVisitor
    {
        private readonly StringBuilder _builder;
        public ScriptBuilderVisitor()
        {
            _builder = new StringBuilder();
        }

        public void AcceptNotSupported(NotSupportedStatement statement)
        {
            _builder.AppendLine("--Command is not supported");
            _builder.AppendLine(string.Format("--{0}", statement.InitialString));
        }

        public void AcceptSetVar(SetVarStatement statement)
        {
            _builder.AppendLine("--SETVAR processed");
            _builder.AppendLine(string.Format("--{0}", statement.InitialString));
        }

        public void AcceptRegularString(NoneStatement statement)
        {
            _builder.AppendLine(statement.AfterVarEvaluation);
        }

        public void AcceptBatchDelimiter(GoStatement statement)
        {
            _builder.AppendLine(statement.InitialString);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
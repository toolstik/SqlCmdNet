using SqlCmdNet.Statements;

namespace SqlCmdNet.Visitors
{
    interface ISqlCmdParserVisitor
    {
        void AcceptNotSupported(NotSupportedStatement statement);

        void AcceptSetVar(SetVarStatement statement);

        void AcceptRegularString(NoneStatement statement);

        void AcceptBatchDelimiter(GoStatement statement);
    }
}
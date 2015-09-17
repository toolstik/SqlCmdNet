using System.Text.RegularExpressions;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    class NotSupportedParser : RegexParserBase
    {
        private readonly StatementType _statementType;

        public NotSupportedParser(string regexString, StatementType statementType) : base(regexString)
        {
            _statementType = statementType;
        }

        protected override bool TryGetStatement(Match match, out Statement statement)
        {
            statement = new NotSupportedStatement
            {
                StatementType = _statementType
            };

            return true;
        }
    }
}
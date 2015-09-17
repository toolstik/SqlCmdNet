using System;
using System.Text.RegularExpressions;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    class RegexSimpleParser : RegexParserBase
    {
        private readonly Func<Statement> _factory;

        public RegexSimpleParser(string regexString, Func<Statement> factory) : base(regexString)
        {
            _factory = factory;
        }

        protected override bool TryGetStatement(Match match, out Statement statement)
        {
            statement = _factory();
            return statement != null;
        }
    }
}
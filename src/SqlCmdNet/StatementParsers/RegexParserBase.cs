using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    abstract class RegexParserBase : IStatementParser
    {
        private readonly Regex _regex;

        protected RegexParserBase(string regexString)
        {
            if (!regexString.StartsWith("^"))
                regexString = "^" + regexString;

            if (!regexString.EndsWith("$"))
                regexString = regexString + "$";

            _regex = new Regex(regexString,
                RegexOptions.Compiled |
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline |
                RegexOptions.ExplicitCapture
                );
        }

        public bool TryParse(string commandLine, out Statement statement)
        {
            statement = null;
            var preparedLine = commandLine.Replace("\t", " ").Trim();

            var match = _regex.Match(preparedLine);

            if (!match.Success)
                return false;

            if (TryGetStatement(match, out statement))
            {
                statement.InitialString = commandLine;
                return true;
            }

            return false;
        }

        protected abstract bool TryGetStatement(Match match, out Statement statement);

    }
}
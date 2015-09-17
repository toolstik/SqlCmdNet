using System;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    abstract class SplitParser : IStatementParser
    {
        private readonly string[] _separator;

        protected SplitParser(string[] separator)
        {
            _separator = separator;
        }

        public bool TryParse(string commandLine, out Statement statement)
        {
            var preparedLine = commandLine.Replace("\t", " ").Trim();

            return TryParse(preparedLine.Split(_separator, StringSplitOptions.None), out statement);
        }

        protected abstract bool TryParse(string[] parts, out Statement statement);

    }
}
using System.Collections.Generic;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    interface IStatementParser
    {
        bool TryParse(string commandLine, out Statement statement);
    }
}
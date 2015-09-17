using System;
using System.Collections.Generic;
using SqlCmdNet.StatementParsers;
using SqlCmdNet.Statements;

namespace SqlCmdNet
{
    /// <summary>
    /// Отвечает за распознавание команд SQLCMD
    /// </summary>
    class LineParser
    {
        private static readonly IStatementParser[] _commands;

        static LineParser()
        {

            _commands = new IStatementParser[]
            {
                new NotSupportedParser("^:?ED", StatementType.ED),
                new NotSupportedParser("^:?RESET", StatementType.RESET),
                new NotSupportedParser("^:ERROR.*", StatementType.ERROR),
                new NotSupportedParser("^:?!!.*", StatementType.CMD),
                new NotSupportedParser("^:PERFTRACE.*", StatementType.PERFTRACE),
                new NotSupportedParser("^:?QUIT.*", StatementType.QUIT),
                new NotSupportedParser("^:?EXIT.*", StatementType.EXIT),
                new NotSupportedParser("^:HELP.*",StatementType.HELP),
                new NotSupportedParser("^:XML.*", StatementType.XML),
                new NotSupportedParser("^:R.*", StatementType.R),
                new NotSupportedParser("^:SERVERLIST", StatementType.SERVERLIST),
                new SetVarParser(), 
                new NotSupportedParser("^:LISTVAR", StatementType.LISTVAR),
                new NotSupportedParser("^:LIST", StatementType.LIST),
                new NotSupportedParser("^:ON ERROR", StatementType.ONERROR),
                new RegexSimpleParser("^GO", () => new GoStatement()), 
            };

        }

        /// <summary>
        /// Если строка является командой, то вернуть инфу о ней, иначе <see cref="NoneStatement"/>
        /// </summary>
        /// <param name="scriptLine"></param>
        /// <returns></returns>
        public Statement ParseLine(string scriptLine)
        {
            foreach (var command in _commands)
            {
                Statement result;
                if (command.TryParse(scriptLine, out result))
                {
                   return result;
                }
            }

            return new NoneStatement()
            {
                InitialString = scriptLine
            };
        }

    }
}
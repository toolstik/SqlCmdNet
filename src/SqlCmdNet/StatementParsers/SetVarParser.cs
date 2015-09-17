using System.Text.RegularExpressions;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    class SetVarParser : RegexParserBase
    {
        private const string Regex = @"^:SETVAR +(?<name>[^ ]+)(?<value>.+)?";
        public SetVarParser()
            : base(Regex)
        {
        }

        protected override bool TryGetStatement(Match match, out Statement statement)
        {
            statement = null;

            var setVar = new SetVarStatement
            {
                VariableName = match.Groups["name"].Value
            };

            if(!match.Groups["value"].Success)
                return false;

            var value = match.Groups["value"].Value.Trim();

            if (value.StartsWith("\"") ^ value.EndsWith("\""))
                return false;

            value = value.Trim('\"');

            setVar.VariableValue = value;

            statement = setVar;

            return true;
        }
    }
}
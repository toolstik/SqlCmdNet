using System;
using System.IO;
using System.Linq;
using SqlCmdNet.Statements;

namespace SqlCmdNet.StatementParsers
{
    class SetVarSplitParser : SplitParser
    {
        public SetVarSplitParser() : base(new []{" "})
        {
        }

        protected override bool TryParse(string[] parts, out Statement statement)
        {
            statement = null;

            var setVar = new SetVarStatement();

            if (parts.Count(p => p.Length > 0) < 3)
                return false;

            if(!string.Equals(parts[0], ":setvar", StringComparison.InvariantCultureIgnoreCase))
                return false;

            setVar.VariableName = parts.Skip(1).First(p => p.Length > 0);

            var value = string.Join(" ", parts.Skip(2));

            if(value.StartsWith("\"") ^ value.EndsWith("\""))
                return false;

            value = value.Trim('\"');

            setVar.VariableValue = value;

            statement = setVar;

            return true;
        }
    }

}
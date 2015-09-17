namespace SqlCmdNet.Statements
{
    public class SetVarStatement : Statement
    {
        public string VariableName { get; set; }
        
        public string VariableValue { get; set; }
    }
}
namespace SqlCmdNet
{
    public enum StatementType
    {
        NONE,
        ED,
        RESET,
        ERROR,
        CMD, //!!
        PERFTRACE,
        QUIT,
        EXIT,
        HELP,
        XML,
        R,
        SERVERLIST,
       // SETVAR,
        LISTVAR,
        LIST,
        ONERROR,
        GO
    }
}
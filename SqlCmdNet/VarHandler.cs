using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqlCmdNet
{
    class VarHandler
    {
        private readonly Dictionary<string, string> _variables;
        private static readonly Regex _regex;

        static VarHandler()
        {
            _regex = new Regex(@"\$\((?<var>[^ ]+?)\)",
                RegexOptions.Compiled |
                RegexOptions.Singleline);
        }

        public VarHandler()
        {

            _variables = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"SQLCMDUSER", ""},
                {"SQLCMDPASSWORD", ""},
                //{"SQLCMDSERVER", sqlConn.DataSource},
                //{"SQLCMDWORKSTATION", sqlConn.WorkstationId},
                //{"SQLCMDDBNAME", sqlConn.Database},
                //{"SQLCMDLOGINTIMEOUT", sqlConn.ConnectionTimeout.ToString()},
                {"SQLCMDSTATTIMEOUT", "0"},
                {"SQLCMDHEADERS", "0"},
                {"SQLCMDCOLSEP", ""},
                {"SQLCMDCOLWIDTH", "0"},
                {"SQLCMDPACKETSIZE", "4096"},
                {"SQLCMDERRORLEVEL", "0"},
                {"SQLCMDMAXVARTYPEWIDTH", "256"},
                {"SQLCMDMAXFIXEDTYPEWIDTH", "0"},
                {"SQLCMDEDITOR", "edit.com"},
                {"SQLCMDINI", ""},
                {"SQLCMDREAL", "0"}
            };
        }

        /// <summary>
        /// Заменяет в переданной строке шаблоны переменных, на их значения
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public string Handle(string commandString)
        {
            return _regex.Replace(commandString, match =>
            {
                string value;
                
                return _variables.TryGetValue(match.Groups["var"].Value, out value) 
                    ? value 
                    : match.Value;
            });
        }

        public void SetVar(string name, string value)
        {
            if (_variables.ContainsKey(name))
            {
                _variables[name] = value;
                return;
            }

            _variables.Add(name, value);
        }

        public void RemoveVar(string name)
        {
            if (_variables.ContainsKey(name))
            {
                _variables.Remove(name);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SqlCmdNet.Statements;
using SqlCmdNet.Visitors;

namespace SqlCmdNet
{

    /// <summary>
    /// Производит чтение скрипта, при этом использует <see cref="LineParser"/> для распознавания команд
    /// и <see cref="VarHandler"/> для распознавания переменных 
    /// </summary>
    class SqlCmdParser
    {
        private readonly ISqlCmdParserVisitor _visitor;
        private readonly VarHandler _varHandler;
        private readonly ScriptContext _scriptContext;

        private Dictionary<Type, Action<Statement>> _handlers;
        private readonly LineParser _lineParser;

        public SqlCmdParser(ISqlCmdParserVisitor visitor)
        {
            _visitor = visitor;
            _handlers = new Dictionary<Type, Action<Statement>>();
            _varHandler = new VarHandler();
            _lineParser = new LineParser();
            _scriptContext = new ScriptContext();

            RegisterHandlers();
        }

        public void Parse(TextReader reader)
        {
            while (true)
            {
                var scriptLine = reader.ReadLine();

                if (scriptLine == null)
                    return;

                var statement = _lineParser.ParseLine(scriptLine);

                Action<Statement> handler;
                if (_handlers.TryGetValue(statement.GetType(), out handler))
                {
                    handler(statement);
                }

            }
        }

        private void RegisterHandlers()
        {
            Register<NoneStatement>(statement =>
            {
                var replacement = _varHandler.Handle(statement.InitialString);
                statement.AfterVarEvaluation = replacement;

                _scriptContext.Add(replacement);

                _visitor.AcceptRegularString(statement);
            });

            Register<NotSupportedStatement>((statement) => _visitor.AcceptNotSupported(statement));

            Register<SetVarStatement>((statement) => _visitor.AcceptSetVar(statement));

            Register<GoStatement>(statement => _visitor.AcceptBatchDelimiter(statement));
        }



        private void Register<T>(Action<T> handler)
            where T : Statement
        {
            Action<Statement> envelope;

            if (typeof (T) == typeof (NoneStatement))
            {
                envelope = statement =>
                {
                    handler((T) statement);
                };
            }
            else
            {
                var noneHandler = _handlers[typeof (NoneStatement)];

                envelope = statement =>
                {
                    if(!_scriptContext.IsOpen())
                        handler((T)statement);
                    else
                    {
                        var noneStatement = new NoneStatement() {InitialString = statement.InitialString};
                        noneHandler(noneStatement);
                    }
                };
            }

            _handlers.Add(typeof(T), envelope);
        }

    }
}

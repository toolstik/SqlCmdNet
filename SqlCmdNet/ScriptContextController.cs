using System;
using System.IO;
using System.Linq;

namespace SqlCmdNet
{
    class ScriptContext
    {
        private BoundedExpression openedBy;

        private BoundedExpression[] _registeredExpressions;

        public ScriptContext()
        {
            _registeredExpressions = new[]
            {
                new BoundedExpression("/*", "*/"),
                new BoundedExpression("'", "'"),
            };
        }

        public void Add(string scriptLine)
        {
            using (var reader = new StringReader(scriptLine))
            {
                Analyse(reader);
            }
            
        }

        private void Analyse(TextReader reader)
        {
            var bounder = "";

            while (true)
            {
                var intChar = reader.Read();
                if(intChar == -1)
                    return;

                bounder = bounder + Convert.ToChar(intChar);

                if (IsOpen())
                {
                    bounder = ProcessOpened(bounder);
                }
                else
                {
                    bounder = ProcessClosed(bounder);
                }
            }
        }

        private string ProcessClosed(string bounder)
        {
            foreach (var expresson in _registeredExpressions)
            {
                if (expresson.LeftBounder == bounder)
                {
                    openedBy = expresson;
                    return string.Empty;
                }

                if (expresson.LeftBounder.StartsWith(bounder))
                {
                    return bounder;
                }
            }

            return string.Empty;
        }

        private string ProcessOpened(string bounder)
        {

            if (openedBy.RightBounder == bounder)
            {
                openedBy = null;
                return string.Empty;
            }

            if (openedBy.RightBounder.StartsWith(bounder))
            {
                return bounder;
            }

            return string.Empty;
        }

        public bool IsOpen()
        {
            return openedBy != null;
        }

        private class BoundedExpression
        {
            public BoundedExpression(string leftBounder, string rightBounder, params string[] exceptions)
            {
                LeftBounder = leftBounder;
                RightBounder = rightBounder;
                Exceptions = exceptions;
            }

            public string LeftBounder { get; private set; }
            public string RightBounder { get; private set; }

            public string[] Exceptions { get; private set; }
        }
    }
}
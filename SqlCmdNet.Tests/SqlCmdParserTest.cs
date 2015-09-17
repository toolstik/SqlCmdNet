using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SqlCmdNet.Statements;
using SqlCmdNet.Visitors;

namespace SqlCmdNet.Tests
{
    [TestFixture]
    public class SqlCmdParserTest
    {

        private TestVisitor _visitor;
        private SqlCmdParser _parser;

        [SetUp]
        public void SetUp()
        {
            _visitor = new TestVisitor();
            _parser = new SqlCmdParser(_visitor);
        }

        [Test]
        public void RegularScriptIsArrayOfRegularLines()
        {
            const string script = 
@"select *
from Table
where a = 1";

            var array = GetMethodArray(script);

            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[0]);
            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[1]);
            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[2]);
        }

        [Test]
        public void TwoRegularWithBatchDelimiter()
        {
            const string script =
@"select 1
go
RETURN";

            var array = GetMethodArray(script);

            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[0]);
            Assert.AreEqual(VisitorMethod.AcceptBatchDelimiter, array[1]);
            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[2]);
        }


        [Test]
        public void CommandInsideMultilineCommentIsNotCommand()
        {
            const string script =
@"/*
GO
*/";

            var array = GetMethodArray(script);

            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[1]);
        }

        [Test]
        public void CommandInsideMultilineStringIsNotCommand()
        {
            const string script =
@"DECLARE @t NVARCHAR(MAX) = N'
GO
'";

            var array = GetMethodArray(script);

            Assert.AreEqual(VisitorMethod.AcceptRegularString, array[1]);
        }

        private VisitorMethod[] GetMethodArray(string input)
        {
            using (var reader = new StringReader(input))
            {
                _parser.Parse(reader);

                return _visitor.GetSequence();
            }
        }

        private class TestVisitor : ISqlCmdParserVisitor
        {

            private readonly List<VisitorMethod> _methodSequence;

            public TestVisitor()
            {
                _methodSequence = new List<VisitorMethod>();
            }

            public void AcceptNotSupported(NotSupportedStatement statement)
            {
                _methodSequence.Add(VisitorMethod.AcceptNotSupported);
            }

            public void AcceptSetVar(SetVarStatement statement)
            {
                _methodSequence.Add(VisitorMethod.AcceptSetVar);
            }

            public void AcceptRegularString(NoneStatement statement)
            {
                _methodSequence.Add(VisitorMethod.AcceptRegularString);
            }

            public VisitorMethod[] GetSequence()
            {
                return _methodSequence.ToArray();
            }

            public void AcceptBatchDelimiter(GoStatement statement)
            {
                _methodSequence.Add(VisitorMethod.AcceptBatchDelimiter);
            }
        }

        private enum VisitorMethod
        {
            AcceptNotSupported,
            AcceptSetVar,
            AcceptRegularString,
            AcceptBatchDelimiter
        }
    }
}
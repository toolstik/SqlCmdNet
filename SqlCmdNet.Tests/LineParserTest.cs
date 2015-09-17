using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SqlCmdNet.Statements;

namespace SqlCmdNet.Tests
{
    [TestFixture]
    public class LineParserTest
    {
        private readonly LineParser _parser;

        public LineParserTest()
        {
            _parser = new LineParser();
        }

        [Test]
        public void SelectIsNoneStatement()
        {
            var statement = _parser.ParseLine("select 1");
            
            Assert.IsInstanceOf<NoneStatement>(statement);
        }

        [Test]
        [TestCase(":setvar VAR_NAME Value")]
        [TestCase("  :setvar VAR_NAME Value")]
        [TestCase(":setvar   VAR_NAME Value")]
        [TestCase(":setvar VAR_NAME   Value")]
        [TestCase(":setvar   VAR_NAME   Value")]
        public void SetVarWithoutQuotes(string input)
        {
            var statement = _parser.ParseLine(input) as SetVarStatement;

            Assert.IsNotNull(statement);
            Assert.AreEqual("VAR_NAME", statement.VariableName);
            Assert.AreEqual("Value", statement.VariableValue);

        }


        [Test]
        [TestCase(":setvar VAR_NAME \"Value with quotes\"")]
        [TestCase("   :setvar VAR_NAME \"Value with quotes\"")]
        public void SetVarWithQuotes(string input)
        {
            var statement = _parser.ParseLine(input) as SetVarStatement;

            Assert.IsNotNull(statement);
            Assert.AreEqual("VAR_NAME", statement.VariableName);
            Assert.AreEqual("Value with quotes", statement.VariableValue);

        }

        [Test]
        [TestCase(":setvar VAR_NAME ")]
        [TestCase(":setvar VAR_NAME \"Value with quotes")]
        [TestCase(":setvar VAR_NAME Value with quotes\"")]
        public void SetVarInvalid(string input)
        {
            var statement = _parser.ParseLine(input);

            Assert.IsInstanceOf<NoneStatement>(statement);

        }

        [Test]
        [TestCase("go")]
        [TestCase("  GO")]
        public void GoValid(string input)
        {
            var statement = _parser.ParseLine(input);

            Assert.IsInstanceOf<GoStatement>(statement);
        }

        [Test]
        [TestCase(":go")]
        public void GoInvalid(string input)
        {
            var statement = _parser.ParseLine(input);

            Assert.IsInstanceOf<NoneStatement>(statement);
        }

        [Test]
        [TestCase("--GO")]
        [TestCase("--:setvar VAR_NAME Value")]
        public void CommentedCommandIsNotCommand(string input)
        {
            var statement = _parser.ParseLine(input);

            Assert.IsInstanceOf<NoneStatement>(statement);
        }

    }
}

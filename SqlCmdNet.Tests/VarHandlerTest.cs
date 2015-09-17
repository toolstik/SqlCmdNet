using NUnit.Framework;

namespace SqlCmdNet.Tests
{
    [TestFixture]
    public class VarHandlerTest
    {
        private VarHandler _varHandler;

        [SetUp]
        public void SetUp()
        {
            _varHandler = new VarHandler();
        }


        [Test]
        [TestCase("$(Unknown123)")]
        [TestCase("'$(Unknown123)'")]
        public void UnknownVarIsNotHandled(string inputString)
        {
            var replacement = _varHandler.Handle(inputString);

            Assert.AreEqual(inputString, replacement);
        }

        [Test]
        [TestCase("Var1", "Value1", "$(Var1)", ExpectedResult = "Value1")]
        public string KnownVarIsHandled(string varName, string varValue, string inputString)
        {
            _varHandler.SetVar(varName, varValue);

            return _varHandler.Handle(inputString);
        }

        [Test]
        public void ManyKnownVarsAreHandled()
        {
            var varName1 = "Var1";
            var varName2 = "Var2";
            var varValue1 = "VarValue1";
            var varValue2 = "VarValue2";

            var input = "IF('$(Var1)' = $(Var2))";

            _varHandler.SetVar(varName1, varValue1);
            _varHandler.SetVar(varName2, varValue2);

            var replacement = _varHandler.Handle(input);

            Assert.AreEqual("IF('VarValue1' = VarValue2)", replacement);
        }
    }
}
using NUnit.Framework;

namespace SqlCmdNet.Tests
{
    [TestFixture]
    public class ScriptContextTest
    {
        private ScriptContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ScriptContext();
        }

        [Test]
        public void OpenCommentShouldBeOpen()
        {
            _context.Add("/*");

            Assert.IsTrue(_context.IsOpen());
        }

        [Test]
        public void OpenStringShouldBeOpen()
        {
            _context.Add("a = '");

            Assert.IsTrue(_context.IsOpen());
        }

        [Test]
        public void OpenStringInsideClosedCommentShouldBeClosed()
        {
            _context.Add("/*a = '");
            _context.Add("    */");

            Assert.IsFalse(_context.IsOpen());
        }

        [Test]
        public void OpenCommentInsideClosedStringShouldBeClosed()
        {
            _context.Add("'");
            _context.Add("/*'");

            Assert.IsFalse(_context.IsOpen());
        }

    }
}
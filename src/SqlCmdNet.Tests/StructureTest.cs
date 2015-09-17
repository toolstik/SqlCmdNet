using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SqlCmdNet.Statements;

namespace SqlCmdNet.Tests
{
    [TestFixture]
    public class StructureTest
    {

        private Assembly GetTestingAssembly()
        {
            return typeof (SqlCmdParser).Assembly;
        }

        [Test]
        public void PublicMembers()
        {
            var publicTypes = GetTestingAssembly().GetExportedTypes()
                .Where(t => typeof(StatementType) != t)
                .Where(t => !typeof(Statement).IsAssignableFrom(t));

            Assert.IsEmpty(publicTypes);
        }

    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.Tests.General.Rebar
{
    [TestFixture]
    public class AciRebarSectionTests
    {
        [Test]
        public void RebarSectionReturnsArea()
        {
            RebarSection sec = new RebarSection(ACI.Entities.RebarDesignation.No6);
            Assert.AreEqual(0.44, sec.Area);
        }
        
    }
}

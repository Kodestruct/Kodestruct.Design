 
using Kodestruct.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI;
using Xunit;

namespace Kodestruct.Concrete.Tests.General.Rebar
{
     
    public class AciRebarSectionTests
    {
        [Fact]
        public void RebarSectionReturnsArea()
        {
            RebarSection sec = new RebarSection(ACI.Entities.RebarDesignation.No6);
            Assert.Equal(0.44, sec.Area);
        }
        
    }
}

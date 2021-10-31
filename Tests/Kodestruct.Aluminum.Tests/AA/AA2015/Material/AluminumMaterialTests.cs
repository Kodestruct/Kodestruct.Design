using Kodestruct.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Aluminum.AA.Entities.Material;
using Kodestruct.Aluminum.AA.AA2015;
using Xunit;

namespace Kodestruct.Aluminum.Tests.Materials
{
     
    public partial class AluminumMaterialTests
    {
        IAluminumMaterial m;
        void GetMaterial()
        {
            m = new AluminumMaterial("6061","T6",	 "0.062 or greater", "std structural profile");

        }

        [Fact]
        public void Alloy6061_T6ReturnsF_ty()
        {
            GetMaterial();
            double F_ty = m.F_ty;
            Assert.Equal(35.0, F_ty);
        }
     
    }
}

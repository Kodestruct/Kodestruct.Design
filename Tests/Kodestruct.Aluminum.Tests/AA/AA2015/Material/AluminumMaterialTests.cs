using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Aluminum.AA.Entities.Material;
using Kodestruct.Aluminum.AA.AA2015;


namespace Kodestruct.Aluminum.Tests.Materials
{
    [TestFixture]
    public partial class AluminumMaterialTests
    {
        IAluminumMaterial m;
        void GetMaterial()
        {
            m = new AluminumMaterial("6061","T6",	 "0.062 or greater", "std structural profile");

            //CalcLog cl = new CalcLog();
            //SteelMaterialCatalog sm = new SteelMaterialCatalog(SteelMaterialId, d_b, cl);
            //return sm;
        }

        [Test]
        public void Alloy6061_T6ReturnsF_ty()
        {
            GetMaterial();
            //string SteelMaterialId = "A992";
            //double d_b = 0.0;
            //AlMaterialCatalog sm = GetSteelMaterial(SteelMaterialId, d_b);
            double F_ty = m.F_ty;
            Assert.AreEqual(35.0, F_ty);
        }
     
    }
}

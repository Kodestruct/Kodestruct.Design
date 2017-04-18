#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
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

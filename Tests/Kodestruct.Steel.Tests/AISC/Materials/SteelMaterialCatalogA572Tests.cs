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
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.Materials
{
    [TestFixture]
    public partial class SteelMaterialCatalog572Tests
    {

        SteelMaterialCatalog GetSteelMaterial(string SteelMaterialId, double d_b)
        {
            CalcLog cl = new CalcLog();
            SteelMaterialCatalog sm = new SteelMaterialCatalog(SteelMaterialId, d_b, cl);
            return sm;
        }

        [Test]
        public void A572Gr50SteelReturnsF_y()
        {
            string SteelMaterialId = "A529Grade50";
            double d_b = 0.75;
            SteelMaterialCatalog sm = GetSteelMaterial(SteelMaterialId, d_b);
            double F_y = sm.YieldStress;
            Assert.AreEqual(50.0, F_y);
        }
        [Test]
        public void A572Gr50SteelReturnsF_u()
        {
            string SteelMaterialId = "A529Grade50";
            double d_b = 0.0;
            SteelMaterialCatalog sm = GetSteelMaterial(SteelMaterialId, d_b);
            double F_u = sm.UltimateStress;
            Assert.AreEqual(65.0, F_u);
        }

        [Test]
        public void A572Gr50SteelReturnsE()
        {
            string SteelMaterialId = "A529Grade50";
            double d_b = 0.0;
            SteelMaterialCatalog sm = GetSteelMaterial(SteelMaterialId, d_b);
            double E = sm.ModulusOfElasticity;
            Assert.AreEqual(29000.0, E);
        }

        [Test]
        public void A572Gr50SteelReturnsG()
        {
            string SteelMaterialId = "A529Grade50";
            double d_b = 0.0;
            SteelMaterialCatalog sm = GetSteelMaterial(SteelMaterialId, d_b);
            double G = sm.ShearModulus;
            Assert.AreEqual(11200.0, G);
        }
    }
}

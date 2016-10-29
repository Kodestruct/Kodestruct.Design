#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

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
 
using System;
using System.Collections.Generic;
using System.Linq;
using Kodestruct.Common.Entities;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14
{
    /// <summary>
    ///  This class encpsulates sectional shear provisions per ACI.
    /// </summary>
    public partial class OneWayShearReinforcedSectionNonPrestressed : AnalyticalElement
    {


        public OneWayShearReinforcedSectionNonPrestressed(double d, IRebarMaterial TransverseRebarMaterial, double A_v, double s)
        {
            this.d = d;
            this.A_v = A_v;
            this.s = s;
            this.RebarMaterial = TransverseRebarMaterial;
        }

        public OneWayShearReinforcedSectionNonPrestressed(double d, IRebarMaterial TransverseRebarMaterial, double s)
        {
            this.d = d;
            this.s = s;
            this.RebarMaterial = TransverseRebarMaterial;
        }

        
                double d; 
                double A_v; 
                double s; 

        public double GetSteelShearStrength()
        {
            double f_yt = rebarMaterial.YieldStress;
            double V_s = ((A_v * f_yt * d) / (s));
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phi = f.Get_phi_ShearReinforced();
            return phi * V_s;
        }

        public double GetRequiredShearReinforcementArea(double V_s)
        {
            double f_yt = rebarMaterial.YieldStress;
            StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
            double phi = f.Get_phi_ShearReinforced();
            double A_v = ((V_s * s) / (f_yt*phi * d));
            return A_v;
        }
        private IRebarMaterial rebarMaterial;

        public IRebarMaterial RebarMaterial
        {
            get { return rebarMaterial; }
            set { rebarMaterial = value; }
        }

    }
}

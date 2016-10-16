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
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.Entities.Welds.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    //CJP groove weld strength never governs over  base material
    //this class is included here as a placeholder
    public class CJPGrooveWeld : GrooveWeld, IWeld
    {
        public CJPGrooveWeld(double F_y, double F_u, double F_EXX, double Size,double A_nBase, double l, ICalcLog Log)
            : base(F_y, F_u,F_EXX, Size,A_nBase,l, Log)
        {

        }

        public CJPGrooveWeld(double Fy, double Fu, double F_EXX, double Size, double A_nBase, double l)
            : base(Fy, Fu, F_EXX, Size, A_nBase,l)
        {

        }

        public double GetStrength(WeldLoadType LoadType, double theta, bool IgnoreBaseMetal)
        {

            double phiR_n1 = 0.0;
            double phi_1 = 0.0;
            switch (LoadType)
            {
                case WeldLoadType.WeldTensionNormal:
                    phi_1 = 0.75;
                    phiR_n1 = phi_1 * this.BaseMaterial.UltimateStress;
                    break;
                case WeldLoadType.WeldCompressionNormal:
                    //Compressive stress need not be considered in design of welds joining the parts.
                    phi_1 = 0.75;
                    phiR_n1 = phi_1 * this.BaseMaterial.UltimateStress;
                    break;
                case WeldLoadType.WeldShear:
                    phi_1 = 0.75;
                    phiR_n1 = phi_1 * 0.6 * this.BaseMaterial.UltimateStress; //per J4
                    break;
                case WeldLoadType.WeldCompressionSpliceFinishedToBear:
                    phi_1 = 0.75;
                    phiR_n1 = phi_1 * this.BaseMaterial.UltimateStress;
                    break;
                case WeldLoadType.WeldCompressionSpliceNotFinishedToBear:
                    phi_1 = 0.75;
                    phiR_n1 = phi_1 * this.BaseMaterial.UltimateStress;
                    break;

            }
            return phiR_n1*A_nBase;
        }

        public double GetWeldArea()
        {
            return Size * Length;
        }
    }
}

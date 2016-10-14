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

using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using d = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltDescriptions;
using f = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltFormulas;
using v = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltValues;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class BoltSlipCritical : Bolt, IBoltSlipCritical
    {
        public double GetSlipResistance()
        {
            double mu = GetSlipCoefficient();
            double Du =pretensionMultiplier;
            double hf = GetFactorForFillers();
            double Tb = T_b;
            int ns = numberOfSlipPlanes;

            //(J3-4)
            double R_n = mu * Du * hf * Tb * ns;

            

            double phiR_n = 0;


                    double phi = GetPhiFactor();
                    phiR_n = R_n * phi;
                    
            return phiR_n;
        }

        internal double GetPhiFactor()
        {
            switch (HoleType)
            {
                case BoltHoleType.STD:
                    return 1.0;
                case BoltHoleType.SSL_Perpendicular:
                    return 1.0;
                case BoltHoleType.SSL_Parallel:
                    return 0.85;
                case BoltHoleType.OVS:
                    return 0.85;
                case BoltHoleType.LSL_Parallel:
                    return 0.7;
                case BoltHoleType.LSL_Perpendicular:
                    return 0.7;
                default:
                    return 0.85;
 
            }
        }

    }
}

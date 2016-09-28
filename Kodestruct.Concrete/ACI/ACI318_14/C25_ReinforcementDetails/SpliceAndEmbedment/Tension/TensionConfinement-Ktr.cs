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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Reports; 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Common.CalculationLogger;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class DevelopmentTension :Development
    {



        public double GetKtr(double transverseRebarArea, double transverseRebarSpacing, double numberOfBarsAlongSplittingPlane)
        {
            if (transverseRebarArea==0.0||transverseRebarSpacing==0.0 ||numberOfBarsAlongSplittingPlane==0.0)
            {
                // It shall be permitted to use Ktr = 0 as a design simplification even
                //  if transverse reinforcement is present.
                return 0.0;
            }
                //Atr = total cross-sectional area of all transverse reinforcement which is 
                //within the spacing s and which crosses
                //the potential plane of splitting through the reinforcement being developed, in.2
                //for example this would be the area of the sirrups
                double Atr = transverseRebarArea;

                //Maximum spacing of transverse reinforcement within ld, center-to-center, in.
                //for example the stirrup spacing
                // refer to PCA notes Example 4.3

                double s = transverseRebarSpacing;

                // n is usually the number of bars being develeoped
                // refer to PCA notes Example 4.3
                double n = numberOfBarsAlongSplittingPlane;

            

            double Ktr = 40 * Atr / (s * n);

            return Ktr;
        }

           
        public double GetConfinementTerm(double cb, double Ktr)
        {
            if (db==0.0)
            {
                throw new Exception("Rebar diameter cannot be 0.0. Check input");
            }
            double ConfinementTerm;

            double conf = (cb + Ktr) / db;


            if (conf>2.5)
            {
                conf=ConfinementTerm = 2.5;
                
            }
            else
            {
                ConfinementTerm = conf;
            }
            return conf;
        }
    }
}

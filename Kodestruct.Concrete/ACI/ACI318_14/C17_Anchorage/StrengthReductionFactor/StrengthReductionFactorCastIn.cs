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

namespace Kodestruct.Concrete.ACI318_14.Anchorage
{
    public class StrengthReductionFactorCastIn: StrengthReductionFactorBase
    {

        public static double GetStrengthReductionFactorForConcrete(
            SupplementaryReinforcmentCondition Condition,
            AnchorReliabilityAndSensitivityCategory Category,
            AnchorLoadType LoadType)
        {
            double phi = 0.0;

            if (Condition == SupplementaryReinforcmentCondition.A)
            {
                if (LoadType == AnchorLoadType.Tension)
                {
                    switch (Category)
                    {
                        case AnchorReliabilityAndSensitivityCategory.Category1:
                            phi = 0.75;
                            break;
                        case AnchorReliabilityAndSensitivityCategory.Category2:
                            phi = 0.65;
                            break;
                        case AnchorReliabilityAndSensitivityCategory.Category3:
                            phi = 0.55;
                            break;
                        default:
                            phi = 0.55;
                            break;
                    }
                }
                else //LoadType.Shear
                {
                    phi = 0.75;
                }
            }
            else
            {
                if (LoadType == AnchorLoadType.Tension)
                {
                    switch (Category)
                    {
                        case AnchorReliabilityAndSensitivityCategory.Category1:
                            phi = 0.65;
                            break;
                        case AnchorReliabilityAndSensitivityCategory.Category2:
                            phi = 0.55;
                            break;
                        case AnchorReliabilityAndSensitivityCategory.Category3:
                            phi = 0.45;
                            break;
                        default:
                            phi = 0.45;
                            break;
                    }
                }
                else //LoadType.Shear
                {
                    phi = 0.7;
                }
            }

            return phi;
        }
    }
}

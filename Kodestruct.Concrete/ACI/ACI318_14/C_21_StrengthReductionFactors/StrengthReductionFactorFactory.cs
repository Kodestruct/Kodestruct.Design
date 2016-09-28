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
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI318_14
{
    public class StrengthReductionFactorFactory
    {
        /// <summary>
        /// Strength reduction factor per Table 21.2.2
        /// </summary>
        /// <param name="failureMode">Compression, tension-controlled or transitional</param>
        /// <param name="ConfinementReinforcementType"></param>
        /// <param name="epsilon_t">Actual calculated tensile strain</param>
        /// <param name="epsilon_ty">Yield strain</param>
        /// <returns></returns>
        public double Get_phiFlexureAndAxial(FlexuralFailureModeClassification failureMode,
            ConfinementReinforcementType ConfinementReinforcementType, double epsilon_t, double epsilon_ty)
        {
            epsilon_t = Math.Abs(epsilon_t);
            switch (failureMode)
            {
                case FlexuralFailureModeClassification.CompressionControlled:
                    if (ConfinementReinforcementType == ACI.ConfinementReinforcementType.Spiral)
                    {
                        return 0.75;
                    }
                    else
                    {
                        return 0.65;
                    }
                    break;
                case FlexuralFailureModeClassification.Transition:
                    if (ConfinementReinforcementType == ACI.ConfinementReinforcementType.Spiral)
                    {
                        return 0.75 + 0.15 * (epsilon_t - epsilon_ty) / (0.005 - epsilon_ty);
                    }
                    else
                    {
                        return 0.65 + 0.25 * (epsilon_t - epsilon_ty) / (0.005 - epsilon_ty);
                    }
                    break;
                case FlexuralFailureModeClassification.TensionControlled:
                    return 0.9;
                    break;
                default:
                    return 0.65;
                    break;
            }

        }

        /// <summary>
        /// Failure mode per Table 21.2.2
        /// </summary>
        /// <param name="epsilon_t">Actual calculated tensile strain</param>
        /// <param name="epsilon_ty">Yield strain</param>
        /// <returns></returns>
        public FlexuralFailureModeClassification GetFlexuralFailureMode(double epsilon_t, double epsilon_ty)
        {
            double epsilon_tAbs = Math.Abs(epsilon_t);
            if (epsilon_tAbs <= epsilon_ty)
            {
                return FlexuralFailureModeClassification.CompressionControlled;
            }
            else if (epsilon_tAbs > epsilon_ty && epsilon_tAbs < 0.005)
            {
                return FlexuralFailureModeClassification.Transition;
            }
            else
            {
                return FlexuralFailureModeClassification.TensionControlled;
            }
        }

        /// <summary>
        /// Strength reduction factor for shear
        /// </summary>
        /// <returns></returns>
        public double Get_phi_ShearReinforced()
        {
            return 0.75;
        }

    }
}

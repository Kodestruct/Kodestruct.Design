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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI
{
    public class ConcreteFlexuralStrengthResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">Depth of compression block</param>
        /// <param name="phiM_n" Moment strength</param>
        /// <param name="FlexuralFailureModeClassification"> Identifies if section is tension-controlled, transitional or compression-controlled</param>
        /// <param name="epsilon_t">Controlling tensile strain</param>
        /// <param name="epsilon_t">Controlling bar tensile yield strain</param>
        public ConcreteFlexuralStrengthResult(double a, double phiM_n, 
            FlexuralFailureModeClassification FlexuralFailureModeClassification,
            double epsilon_t, double epsilon_ty) 
        {
                this.a                    =a                   ;
                this.phiM_n               =phiM_n              ;
                this.FlexuralFailureModeClassification = FlexuralFailureModeClassification;
                this.epsilon_t            = epsilon_t;
                this.epsilon_ty = epsilon_ty;
        }

        public ConcreteFlexuralStrengthResult(IStrainCompatibilityAnalysisResult nominalResult, 
            FlexuralCompressionFiberPosition FlexuralCompressionFiberPosition, double beta1)
        {
            UpdateValuesFromResult(nominalResult, FlexuralCompressionFiberPosition,beta1);
        }

        private void UpdateValuesFromResult(IStrainCompatibilityAnalysisResult nominalResult, 
            FlexuralCompressionFiberPosition FlexuralCompressionFiberPosition, double beta1)
        {
            LinearStrainDistribution strainDistribution = nominalResult.StrainDistribution;

            double d = strainDistribution.Height;

            if (FlexuralCompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                this.a = strainDistribution.NeutralAxisTopDistance * beta1;
                epsilon_t = strainDistribution.BottomFiberStrain;
            }
            else
            {
                this.a = (d - strainDistribution.NeutralAxisTopDistance) * beta1;
                epsilon_t = strainDistribution.TopFiberStrain;
            }

            IRebarMaterial controllingBarMaterial = nominalResult.ControllingTensionBar.Point.Rebar.Material;
            epsilon_ty = controllingBarMaterial.YieldStrain;
        }
 
        public double a { get; set; }

        public double phiM_n { get; set; }

        public FlexuralFailureModeClassification FlexuralFailureModeClassification { get; set; }

        public double epsilon_t { get; set; }

        public double epsilon_ty { get; set; }
    }
}

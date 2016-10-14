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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
 
 
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Exceptions;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamTee : FlexuralMemberTeeBase
    {

        public double GetCompressionFlangeLocalBucklingCapacity(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Mn = 0.0;

            if (SectionTee != null)
            {
                CompactnessClassFlexure cClass = GetFlangeCompactnessClass();
                if (cClass == CompactnessClassFlexure.Noncompact || cClass == CompactnessClassFlexure.Slender)
                {
                    double lambda = GetLambdaFlange();
                    double lambdapf = this.GetLambdapf(compressionFiberLocation);
                    double lambdarf = this.GetLambdarf(compressionFiberLocation);

                    double Sxc = GetSectionModulusCompressionSxc(compressionFiberLocation);
                    double Fy = this.Section.Material.YieldStress;
                    double E = this.Section.Material.ModulusOfElasticity;

                    double My = GetYieldingMoment_My(compressionFiberLocation);

                    if (cClass == CompactnessClassFlexure.Noncompact)
                    {
                        double Mp = this.GetMajorNominalPlasticMoment();
                        Mn = GetMnFlbNoncompact(Mp,Sxc,lambda,lambdarf,lambdapf,My);

                    }
                    else
                    {
                        Mn = GetMnFlbSlender(Sxc,lambda);
                    }

                }
                else
                {

                }
                if (cClass == CompactnessClassFlexure.Compact)
                {
                    throw new LimitStateNotApplicableException("Flange local buckling");
                }

            }

            return Mn;
        }

        private double GetMnFlbSlender(double Sxc, double lambda)
        {
            double Mn;
            Mn = (0.7 * E * Sxc) / Math.Pow(lambda, 2); //(F9-7)
            return Mn;
        }

        private double GetMnFlbNoncompact(double Mp, double Sxc, double lambda, double lambda_rf, double lambda_pf, double My )
        {
            double Mn;
            Mn = Mp - (Mp - 0.7 * Fy * Sxc) * ((lambda - lambda_pf) / (lambda_rf - lambda_pf)); //(F9-6)
            Mn = Mn > 1.6 * My ? 1.6 * My : Mn;
            return Mn;
        }

    }
}

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
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Exceptions;




namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamIDoublySymmetricCompactWebOnly : BeamIDoublySymmetricCompact
    {
        //Compression Flange Local Buckling F3.2

        public double GetCompressionFlangeLocalBucklingCapacity()
        {
            double M_n = 0.0;
            double phiM_n = 0.0;
            ISectionI sectionI = Section.Shape as ISectionI;
            if (sectionI!=null)
	            {
                ShapeCompactness.IShapeMember compactness = new ShapeCompactness.IShapeMember(Section, IsRolledMember,FlexuralCompressionFiberPosition.Top);
                CompactnessClassFlexure cClass = compactness.GetFlangeCompactnessFlexure();
                if (cClass== CompactnessClassFlexure.Noncompact || cClass== CompactnessClassFlexure.Slender)
                {
                    double lambda = this.GetLambdaCompressionFlange(FlexuralCompressionFiberPosition.Top);
                    double lambdapf = this.GetLambdapf(FlexuralCompressionFiberPosition.Top);
                    double lambdarf = this.GetLambdarf(FlexuralCompressionFiberPosition.Top);
                    //note: section is doubly symmetric so top flange is taken

                    double Sx = this.Section.Shape.S_xTop;
                    double Fy = this.Section.Material.YieldStress;
                    double E = this.Section.Material.ModulusOfElasticity;
                    //double Zx = Section.SectionBase.Z_x;

                    if (cClass== CompactnessClassFlexure.Noncompact)
	                {
                        double Mp = this.GetMajorNominalPlasticMoment();
		                 M_n = Mp - (Mp - 0.7 * Fy * Sx) * ((lambda - lambdapf) / (lambdarf - lambdapf)); //(F3-1)
	                }
                      else
	                {
                        double kc = this.Getkc();
                        M_n = 0.9 * E * kc * Sx / Math.Pow(lambda, 2); //(F3-2)
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
            phiM_n = 0.9 * M_n;
            return phiM_n;
        }
    }
}

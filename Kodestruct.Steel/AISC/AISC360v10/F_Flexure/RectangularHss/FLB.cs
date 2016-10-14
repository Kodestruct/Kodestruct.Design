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
using Kodestruct.Steel.AISC.SteelEntities;






namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamRectangularHss : FlexuralMemberRhsBase, ISteelBeamFlexure
    {
        //Compression Flange Local Buckling F7.2


        public double GetCompressionFlangeLocalBucklingCapacity(FlexuralCompressionFiberPosition CompressionLocation, MomentAxis MomentAxis)
        {
            double Mn = 0.0;
            double phiM_n = 0.0;

            if (Section != null)
            {

                    double lambda = this.GetLambdaCompressionFlange(CompressionLocation, MomentAxis);
                    double lambdapf = this.GetLambdapf(CompressionLocation, MomentAxis);
                    double lambdarf = this.GetLambdarf(CompressionLocation, MomentAxis);
                    //note: section is doubly symmetric so top flange is taken

                    double S = GetSectionModulus(MomentAxis);
                    double Fy = this.Section.Material.YieldStress;
                    double E = this.Section.Material.ModulusOfElasticity;

                    CompactnessClassFlexure cClass = GetFlangeCompactness(CompressionLocation, MomentAxis);

                    if (cClass == CompactnessClassFlexure.Noncompact)
                    {
                        double Mp = this.GetMajorNominalPlasticMoment();
                        Mn = GetMnNoncompact(Mp, Fy, S, lambda);

                    }
                    else
                    {
                        double Sxe = GetEffectiveSectionModulusX(MomentAxis);
                        Mn = GetMnSlender(Sxe, Fy);
                    }

                
            }
            phiM_n = 0.9 * Mn;
            return phiM_n;
        }

        private double GetSectionModulus(MomentAxis MomentAxis)
        {
            double S = 0 ;
            if (MomentAxis == MomentAxis.XAxis)
            {
                S = this.Section.Shape.S_xTop;
            }
            else if (MomentAxis == MomentAxis.YAxis)
            {
                S = this.Section.Shape.S_yLeft;
            }
            else
            {
                throw new FlexuralBendingAxisException();
            }
            return S;
        }

        private double GetMnNoncompact(double Mp, double Fy, double S, double lambda )
        {
            double Mn = Mp - (Mp - Fy * S) * (3.57 * lambda * Math.Sqrt(Fy / E) - 4.0); //(F7-2)
            Mn = Mn > Mp ? Mp : Mn;
            return Mn;
        }
        
        private double GetMnSlender(double Se, double Fy)
        {
            double Mn = Fy * Se; //(F7-3)
            return Mn;
        }
    }
}

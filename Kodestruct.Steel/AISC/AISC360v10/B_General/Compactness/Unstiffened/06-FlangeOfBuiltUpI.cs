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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;
 
 
namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public class FlangeOfBuiltUpI : UnstiffenedElementCompactness
    {
        ISectionI section;
        FlexuralCompressionFiberPosition compressionFiberPosition;

        public FlangeOfBuiltUpI(ISteelMaterial Material, ISectionI s, 
            FlexuralCompressionFiberPosition compressionFiberPosition)
            :base(Material)
        {
            this.section = s;
            double bf=0;
            double tf=0;
            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    bf = s.b_fTop;
                    tf = s.t_fTop;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    bf = s.b_fTop;
                    tf = s.t_fTop;
                    break;
                default:
                    throw new Exception("Compression fiber location different from to or bottom is not supported");
                    
            }

            base.Overhang = bf;
            base.Thickness = tf;
        }

//[a] kc = 4 sqrt (h/tw) but shall not be taken less than 0.35 nor greater than 0.76 for calculation purposes.
//[b] FL = 0.7Fy for major axis bending of compact and noncompact web built-up I-shaped members with Sxt /Sxc = 0.7;
//FL = FySxt /Sxc = 0.5Fy for major-axis bending of compact and noncompact web built-up I-shaped members with Sxt /Sxc < 0.7.
        public double Get_kc()
        {
            double kc;
            double h = section.d-(section.t_fTop+section.t_fBot);
            double tw = section.t_w;
            kc = 4.0 * Math.Sqrt(h / tw);
            kc = kc > 0.76 ? 0.76 : kc;
            kc = kc < 0.35 ? 0.35 : kc;
            return kc;
        }

        public double Get_FL()
        {
            double Sxt = compressionFiberPosition == FlexuralCompressionFiberPosition.Top ? section.S_xBot : section.S_xTop;
            double Sxc = compressionFiberPosition == FlexuralCompressionFiberPosition.Bottom? section.S_xBot: section.S_xBot;
            double SRatio;
            if (Sxt == 0 || Sxc==0)
            {
                throw new Exception("Section Modulus cannot be zero for the section");
            }
            else
            {
                SRatio = Sxt / Sxc;
            }
            double FL;
            double Fy = Material.YieldStress;
            if (SRatio>=0.7)
            {
                FL = 0.7 * Fy;
            }
            else
            {
                FL = Fy * SRatio;
                FL = FL < 0.5*Fy? 0.5*Fy : FL;
            }
            throw new NotImplementedException();
        }

        public override double GetLambda_r(StressType stress)
        {
            double E = Material.ModulusOfElasticity;
            double kc = Get_kc();
            double Fy = Material.YieldStress;

            if (stress== StressType.Axial)
            {

                   return 0.64*Math.Sqrt((kc*E)/Fy);
            }
            else
            {
                return 0.95 * Math.Sqrt((kc * E) / Fy);

            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {

                return 0.38 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }
            
        }
    }
}

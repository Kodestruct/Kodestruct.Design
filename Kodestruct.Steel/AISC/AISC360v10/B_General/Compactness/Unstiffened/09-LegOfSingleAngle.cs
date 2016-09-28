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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
 
 
 


namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public class LegOfSingleAngle : UnstiffenedElementCompactness
    {
        ISectionAngle angle;

          public LegOfSingleAngle(ISteelMaterial Material, double Overhang, double Thickness)
            :base(Material,Overhang,Thickness)
        {
            
        }

        public LegOfSingleAngle(ISteelMaterial Material, ISectionAngle angle, bool LongLegProjecting)
            :base(Material)
        {
            this.angle = angle;

            double VLeg = angle.d;
            double HLeg = angle.b;

            double LongLeg = Math.Max(VLeg, HLeg);
            double ShortLeg = Math.Min(VLeg, HLeg);

            if (LongLegProjecting==true)
            {
                Overhang = LongLeg;
            }
            else
            {
                Overhang = ShortLeg;
            }

            base.Overhang = Overhang;
            base.Thickness = angle.t;
        }


        public override double GetLambda_r(StressType stress)
        {
            if (stress== StressType.Flexure)
            {
                return 0.95 * SqrtE_Fy();
            }
            else
            {
                return 0.45 * SqrtE_Fy();
            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 0.54 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }
            
        }
    }
}

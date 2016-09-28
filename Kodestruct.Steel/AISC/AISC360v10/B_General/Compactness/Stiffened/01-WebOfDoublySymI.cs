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
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;
 
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public class WebOfDoublySymI: StiffenedElementCompactness
    {
          ISectionI SectionI;

          public WebOfDoublySymI(ISteelMaterial Material, double Width, double Thickness)
            :base(Material,Width,Thickness)
        {
            
        }

          public WebOfDoublySymI(ISteelMaterial Material, ISectionI ISectionI)
            :base(Material)
        {
            this.SectionI = ISectionI;
            ISectionI s = ISectionI;
            this.Width = s.h_web;
            if (this.Width<=0)
            {
                throw new Exception("Clear web distance cannot be less than or equal to 0");
            }
            this.Thickness = s.t_w;
        }

          public WebOfDoublySymI(ISteelMaterial Material):base(Material)
          {

          }


        public override double GetLambda_r(StressType stress)
        {
            if (stress== StressType.Flexure)
            {
                return 5.7 * SqrtE_Fy();
            }
            else
            {
                return 1.49 * SqrtE_Fy();
            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 3.76 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }
            
        }

    }
}

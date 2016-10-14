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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;
 
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public class WebOfRhs : StiffenedElementCompactness
    {
        ISectionTube SectionTube;

        public WebOfRhs(ISteelMaterial Material, double Depth, double Thickness)
            :base(Material,Depth,Thickness)
        {
            
        }

        public WebOfRhs(ISteelMaterial Material, ISectionTube sectionTube,MomentAxis MomentAxis) //, double OutsideCornerRadius = -1.0)
            :base(Material)
        {
            this.SectionTube = sectionTube;
            ISectionTube s = sectionTube;
            double td = s.t_des;
            //if (OutsideCornerRadius==-1.0)
            //{
            //    this.Width = s.B - 3.0 * td;
            //}
            //else
            //{
            //    if (OutsideCornerRadius<0)
            //    {
            //        throw new Exception("Invalid RHS corner radius. Must be over 0");
            //    }
            //    this.Width = s.B - 2.0 * OutsideCornerRadius;
            //}

            this.Width = GetWebWallHeight_h(MomentAxis);
            this.Thickness = td;
        }

        protected virtual double GetWebWallHeight_h(MomentAxis MomentAxis)
        {
            double tdes = SectionTube.t_des;
            double h =0;
            //B4-1b. Stiffened Elements
            if (MomentAxis == MomentAxis.XAxis)
            {
                h = SectionTube.H - 3.0 * tdes;
            }
            else if (MomentAxis == MomentAxis.YAxis)
            {
                h = SectionTube.B - 3.0 * tdes;
            }

            return h;
        }

        public override double GetLambda_r(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 5.70 * SqrtE_Fy();
            }
            else
            {
                return 1.40 * SqrtE_Fy();
            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 2.42 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }

        }

    }
}

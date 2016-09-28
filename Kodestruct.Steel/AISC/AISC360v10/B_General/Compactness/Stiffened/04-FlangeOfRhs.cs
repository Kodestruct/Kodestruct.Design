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
    public class FlangeOfRhs : StiffenedElementCompactness
    {
        ISectionTube SectionTube;

        public FlangeOfRhs(ISteelMaterial Material, double Depth, double Thickness)
            :base(Material,Depth,Thickness)
        {
            
        }

        public FlangeOfRhs(ISteelMaterial Material, ISectionTube SectionTube, MomentAxis MomentAxis) //double OutsideCornerRadius=-1.0)
            :base(Material)
        {
            this.SectionTube = SectionTube;
            ISectionTube s = SectionTube;
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
            this.Width = GetFlangeWidth_bf(MomentAxis);

            this.Thickness = td;
        }


        protected virtual double GetFlangeWidth_bf(MomentAxis MomentAxis)
        {
            double b = 0 ;

            double tdes = SectionTube.t_des;
            double h;
            //B4-1b. Stiffened Elements
            if (MomentAxis == MomentAxis.XAxis)
            {
                b = SectionTube.B - 3.0 * tdes;
            }
            else if (MomentAxis == MomentAxis.YAxis)
            {
                b = SectionTube.H - 3.0 * tdes;
            }

            return b;
            //if (sectionTube.CornerRadiusOutside == -1.0)
            //{
            //    b = SectionTube.B - 3.0 * sectionTube.t_des;
            //}
            //else
            //{
            //    b = sectionTube.B - 2.0 * sectionTube.CornerRadiusOutside;
            //}

        }

        public override double GetLambda_r(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 1.40 * SqrtE_Fy();
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
                return 1.12 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }

        }

    }
}

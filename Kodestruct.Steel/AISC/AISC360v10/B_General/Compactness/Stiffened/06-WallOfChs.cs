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
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
 
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public class WallOfChs : StiffenedElementCompactness
    {
        ISectionPipe SectionPipe;
        private double diameter;

        public double Diameter
        {
            get { return diameter; }
            set { diameter = value; }
        }

        public override double Width
        {
            get { return diameter; }
            set { diameter = value; }
        }

        private double thickness;

        public override double Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
        

        public WallOfChs(ISteelMaterial Material, double Diameter, double Thickness)
            :base(Material)
        {
            this.diameter = Diameter;
            this.Thickness = Thickness;
        }

        public WallOfChs(ISteelMaterial Material, ISectionPipe SectionPipe)
            :base(Material)
        {
            this.SectionPipe = SectionPipe;
            ISectionPipe s = SectionPipe;
            double td = s.t_des;

            this.diameter = s.D;
            this.Thickness = s.t_des;
        }


        public override double GetLambda()
        {
            return diameter / thickness;
        }

        public override double GetLambda_r(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 0.31 * SqrtE_Fy();
            }
            else
            {
                return 0.11 * SqrtE_Fy();
            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 0.07 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }

        }

    }
}

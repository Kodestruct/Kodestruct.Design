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
 


namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public partial class IShapeSlenderElements : IShapeCompact
    {

        public override double GetReductionFactorForStiffenedElementQa(double Fcr = 0)
        {
            double Fy = Section.Material.YieldStress;
            double f = Fcr == 0.0 ? Fy : Fcr;

            double Qa;
            double E = Section.Material.ModulusOfElasticity;

            double webLambda = GetWebLambda();

            if (webLambda >= 1.49 * Math.Sqrt(E / f))
            {
                double b = GetWebClearDistance();
                double tw = GetWebThickness();
                
                double be = 1.92 * tw * Math.Sqrt(E / f) * (1.0 - 0.34 / webLambda * Math.Sqrt(E / f)); //(E7-17)
                be = be > b ? b : be;
                be = be < 0 ? 0 : be;

                double bLost = b - be;
                double ALost = bLost * tw;

                double A = GetSectionGrossArea();
                Qa = (A - ALost) / A; //(E7-16)
            }
            else
            {
                Qa = 1.0;
            }

            return Qa;
        }

        private double GetSectionGrossArea()
        {
            return Section.Shape.A;
        }

        protected virtual double GetWebThickness()
        {
            return SectionI.t_w;
        }

        protected virtual double GetWebClearDistance()
        {
            return this.SectionI.d - (SectionI.h_web);
            //todo fillet area ...
        }

        private double GetWebLambda()
        {
            double h = GetWebClearDistance();
            double t = GetWebThickness();

            return h / t;
        }
    }
}

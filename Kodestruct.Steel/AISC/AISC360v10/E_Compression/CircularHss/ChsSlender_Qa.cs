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
using Kodestruct.Steel.AISC.SteelEntities;
 


namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public partial class ChsSlender : ChsNonSlender
    {

        public override double GetReductionFactorForStiffenedElementQa(double Fcr = 0)
        {
            double F_y = Section.Material.YieldStress;
            double f = Fcr == 0.0 ? F_y : Fcr;

            double Qa;
            double E = Section.Material.ModulusOfElasticity;
            double D = this.SectionPipe.D;
            double t = this.SectionPipe.t_des;

            if (D/t>0.45*SqrtE_Fy())
            {
                throw new Exception(String.Format("The use of pipe sections with D/t ratio = {0}, greater than 0.45*E/F_y is not recommended by AISC specification.", Math.Round(D/t) ));
            }
            else
	        {

                if (D/t>0.11*SqrtE_Fy())
                {

                    Qa=((0.038*E) / (F_y*(D / t)))+((2.0) / (3.0)); //(E7-19)
                }
                else
                {
                    Qa = 1.0;
                }
            }

            return Qa;
        }

        private double GetSectionGrossArea()
        {
            return Section.Shape.A;
        }



        private double SqrtE_Fy()
        {
            double E = Section.Material.ModulusOfElasticity;
            double Fy = Section.Material.YieldStress;
            return Math.Sqrt(E / Fy);
        }


    }
}

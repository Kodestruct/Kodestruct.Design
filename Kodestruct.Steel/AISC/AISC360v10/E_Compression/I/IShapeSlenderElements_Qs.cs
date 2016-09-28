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
 

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public partial class IShapeSlenderElements : IShapeCompact
    {

        public override double GetReductionFactorForUnstiffenedElementQs()
        {
            double Qs;
            double lambdaFlange = GetFlangeLambda();
            double Fy = Section.Material.YieldStress;
            double E = Section.Material.ModulusOfElasticity;

            if (this.IsRolled == true)
            {
                if (lambdaFlange > 0.56 * SqrtE_Fy())
                {
                    if (lambdaFlange < 1.03 * SqrtE_Fy())
                    {
                        Qs = 1.415 - 0.74 * lambdaFlange * Math.Sqrt(Fy/E); //(E7-5)
                    }
                    else
                    {

                        Qs = 0.69 * E / (Fy * Math.Pow(lambdaFlange, 2)); //(E7-6)
                    }
                }
                else
                {
                    Qs = 1.0;  //(E7-4)
                } 
            }

            else
            {
                if (lambdaFlange > 0.64 * SqrtE_kc_Fy())
                {
                    double kc = Get_kc();

                    if (lambdaFlange<=1.17*SqrtE_kc_Fy())
                    {
                        //E7-8
                        Qs =1.415*-0.65*(lambdaFlange)*Math.Sqrt(Fy/(E*kc));
                    }
                    else
                    {
                        //E7-9
                        Qs = 0.9 * E * kc / (Fy * Math.Pow(lambdaFlange,2));
                    }
                }
                else
                {
                    return 1.0;
                }
            }

            return Qs;
        }


        private double Get_kc()
        {
            double htw = GetWebLambda();
            return 4.0 / GetWebLambda();
        }

        private double GetFlangeLambda()
        {
            double tf_top = this.SectionI.t_fTop;
            double tf_bot = this.SectionI.t_fBot;
            double bf_top = this.SectionI.b_fTop;
            double bf_bot = this.SectionI.b_fBot;
            double bt_Ratio = Math.Max(bf_top/ tf_bot, bf_bot/ tf_bot);

            return bt_Ratio;
        }

        private double SqrtE_Fy()
        {
            double E = Section.Material.ModulusOfElasticity;
            double Fy = Section.Material.YieldStress;
            return Math.Sqrt(E / Fy);
        }

        private double SqrtE_kc_Fy()
        {
            double E = Section.Material.ModulusOfElasticity;
            double Fy = Section.Material.YieldStress;
            double kc = Get_kc();
            return Math.Sqrt(E*kc / Fy);
        }
    }
}

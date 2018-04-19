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
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public  class FlexuralBucklingCriticalStressCalculator
    {
        public virtual double GetF_e(double E, double Slenderness)
        {
            double Fe = 0;

            if (Slenderness != 0)
            {
                //(E3-4)
                Fe = Math.Pow(Math.PI, 2) * E / Math.Pow(Slenderness, 2);
            }
            else
            {
                return double.PositiveInfinity;
            }

            return Fe;

        }


       public double GetCriticalStressFcr(double Fe, double Fy, double Q = 1.0)
        {


            double Fcr = 0.0;



            if (Fe != double.PositiveInfinity)
            {
                double stressRatio = Q * Fy / Fe;

                if (stressRatio > 2.25)
                {
                    Fcr = 0.877 * Fe; // (E3-3) if Q<1 then (E7-3)
                }
                else
                {
                    Fcr = Q * Math.Pow(0.658, stressRatio) * Fy; //(E3-2)  if Q<1 then (E7-2)
                }
            }
            else
            {
                Fcr = Fy;
            }




            return Fcr;
        }
    }
}

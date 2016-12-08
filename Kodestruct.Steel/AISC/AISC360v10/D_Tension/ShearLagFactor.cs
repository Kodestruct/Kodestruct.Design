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

namespace Kodestruct.Steel.AISC.AISC360v10
{
    public class ShearLagFactor
    {

        public double GetShearLagFactor(ShearLagCase Case, double x_bar, double b_plate, double l,
            double B, double H, double A_g, double A_connected, bool IsOpenTensionSection)
        {
            ShearLagFactorBase shearLagCase;
            switch (Case)
            {
                case ShearLagCase.Case1:
                     shearLagCase = new ShearLagCase1();
                    break;
                case ShearLagCase.Case2:
                    shearLagCase = new ShearLagCase2(x_bar,l);
                    break;
                case ShearLagCase.Case3:
                    shearLagCase = new ShearLagCase3();
                    break;
                case ShearLagCase.Case4:
                    shearLagCase = new ShearLagCase4(b_plate,l);
                    break;
                case ShearLagCase.Case5:
                    shearLagCase = new ShearLagCase5(B, l);
                    break;
                case ShearLagCase.Case6a:
                     shearLagCase = new ShearLagCase6(true,B,H,l);
                    break;
                case ShearLagCase.Case6b:
                    shearLagCase = new ShearLagCase6(false, B, H, l);
                    break;
                case ShearLagCase.Case7a:
                    shearLagCase = new ShearLagCase7(2,H,b_plate,x_bar,l);
                    break;
                case ShearLagCase.Case7b:
                    shearLagCase = new ShearLagCase7(3, H, b_plate, x_bar, l);
                    break;
                case ShearLagCase.Case7c:
                    shearLagCase = new ShearLagCase7(4, H, b_plate, x_bar, l);
                    break;
                case ShearLagCase.Case8a:
                    shearLagCase = new ShearLagCase8(2, x_bar, l);
                    break;
                case ShearLagCase.Case8b:
                    shearLagCase = new ShearLagCase8(3, x_bar, l);
                    break;
                case ShearLagCase.Case8c:
                    shearLagCase = new ShearLagCase8(4, x_bar, l);
                    break;
                default:
                    shearLagCase = new ShearLagCase2(x_bar, l);
                    break;
            }
            double U= shearLagCase.GetShearLagFactor();
            if (IsOpenTensionSection == false)
            {
                return U;
            }
            else
            {
                //AISC section D3
                //For open cross sections such as W, M, S, C or HP shapes, WTs, STs, and single and
                //double angles, the shear lag factor, U, need not be less than the ratio of the gross area
                //of the connected element(s) to the member gross area. This provision does not apply
                //to closed sections, such as HSS sections, nor to plates
                double Umax = A_connected / A_g;
                U = U < Umax ? Umax : U;
                return U;
            }
        }

    }
}

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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateRectangularHss : BasePlateTypeBase
    {
         public BasePlateRectangularHss(double B_bp, double N_bp, double B, double H, double t, double f_c, double F_y, double A_2, double f_anchor)
         :base(B_bp,N_bp,f_c,F_y, A_2, f_anchor)
        {
            this.B = B;
            this.H = H;
            this.t = t;
        }
         public double H { get; set; }

         public double B { get; set; }

        public double t { get; set; }

        public override double GetLength(double P_u=0)
        {

            double m = Get_m();
            double n = Get_n();

            return Math.Max(m, n);
        }

        public override double Get_m()
        {
            double m = ((N_bp - 0.95 * H) / (2.0));
            return m;
        }

        public  override double Get_n()
        {
            double n = ((B_bp - 0.95 * B) / (2.0));
            return n;
        }

        public override double Get_l_tension(BendingAxis Axis)
        {
            if (Axis == BendingAxis.Major)
            {
                return f_anchor - H / 2.0 + t/ 2.0;
            }
            else
            {
                return f_anchor - B / 2.0 + t / 2.0;
            }
        }
    }
}

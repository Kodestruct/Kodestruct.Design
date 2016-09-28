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
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateCircularHss : BasePlateTypeBase
    {

        public BasePlateCircularHss(double B_bp, double N_bp, double D, double f_c, double F_y, double A_2)
            :base(B_bp,N_bp, f_c, F_y, A_2)
        {
            this.D = D;
        }

        public double D { get; set; }
        public override double GetLength(double P_u=0)
        {
            return Get_m();
        }

        public override double Get_m()
        {
            double diag = Math.Sqrt(Math.Pow(N_bp, 2) + Math.Pow(B_bp, 2));
            double m = ((diag - 0.8 * D) / (2.0));
            return m;
        }

        public override double Get_n()
        {
            return Get_m();
        }
    }
}

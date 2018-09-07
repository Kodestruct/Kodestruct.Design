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
    public abstract class BasePlateTypeBase : IBasePlate
    {

        public BasePlateTypeBase(double B_bp, double N_bp, double f_c, double F_y, double A_2, double f_anchor)
        {
            this.B_bp = B_bp;
            this.N_bp = N_bp;
            A_1 = this.B_bp * this.N_bp;
            if (A_2!=0)
            {
                this.A_2 = A_2; 
            }
            else
            {
                this.A_2 = A_1;
            }
            this.f_c = f_c;
            this.F_y = F_y;
            this.f_anchor = f_anchor;
        }

        public double GetphiP_p()
        {
            double phi = 0.65; //Bearing
            double P_p = 0.85 * f_c * A_1 * Math.Sqrt(((A_2) / (A_1)));
            double phiP_p = phi * P_p;
            return phiP_p;
        }
        public double f_c{get; set;}
        public double F_y { get; set; }
        public double A_1 { get; set; }

        public double f_anchor { get; set; }
        private double _A_2;

        public double A_2
        {
            get { 
                
                return _A_2; }
            set { 
                _A_2 = value; }
        }
        
        public abstract double GetLength(double P_u);

        public double B_bp { get; set; }

        public double  N_bp { get; set; }

        public abstract double Get_m();
        public abstract double Get_n();

        public abstract double Get_l_tension(BendingAxis Axis);


    }
}

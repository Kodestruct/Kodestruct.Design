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
using Kodestruct.Steel.AISC.SteelEntities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public  partial class PryingActionElement: SteelDesignElement
    {
            double d_b ;
            double d_holePrime;
            double b_stem ;
            double a_edge ;
            double p      ;
            double B_bolt ;
            double F_u;
            double t;
            public PryingActionElement(double d_b, double d_holePrime, double b_stem, double a_edge, double p, double B_bolt, double F_u)
        {
                this.d_b =d_b;
                this.d_holePrime = d_holePrime;
                this.b_stem =b_stem;
                this.a_edge =a_edge;
                this.p      =p     ;
                this.B_bolt =B_bolt;
                this.F_u = F_u;
        }

        //t_c is the material thickness required to develop the design bolt tension

            private double _t_c;

            public double t_c
            {
                get {
                    _t_c = Get_t_c();
                    return _t_c; }

            }
            
        private double Get_t_c()
            {
                        if (F_u==0)
                        {
                            throw new Exception("Prying calculation failed. Material ultimate strength cannot be zero.");
                        }
                double phi = 0.9;
                double b_prime = Get_b_prime();
                double t_c = Math.Sqrt(((4 * (B_bolt) * (b_prime)) / (phi * (p) * (F_u))));
                return t_c;
            }

        private double  _delta;

        public double  delta
        {
            get {
                _delta = Get_delta();
                return _delta; }

        }
        
        double Get_delta()
        {
            if (p==0)
            {
                throw new Exception("Prying calculation failed. Bolt pitch cannot be zero.");
            }
            return 1 - d_holePrime / p;
        }


        private double _rho;

        public double rho
        {
            get {
                _rho = Get_rho();
                return _rho; }

        }
        
        double Get_rho()
        {
            if (a_prime ==0 || b_prime == 0)
            {
                throw new Exception("Prying calculation failed. Bolt edge distances a_prime and b_prime cannot be zero.");
            }
            return a_prime / b_prime;
        }

        private double _a_prime;

        private double a_prime
        {
            get {
                _a_prime = Get_a_prime();
                return _a_prime; }
 
        }
        
        double Get_a_prime()
        {
            return a_edge + d_b / 2.0;

        }



        private double _b_prime;

        private double b_prime
        {
            get
            {
                _b_prime = Get_b_prime();
                return _b_prime;
            }

        }
        double Get_b_prime()
        {
            return b_stem - d_b / 2.0;
        }
    }
}

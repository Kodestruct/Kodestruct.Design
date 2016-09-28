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

namespace Kodestruct.Analysis.Torsion
{
    public abstract class TorsionalFunctionBase : ITorsionalFunction
    {
        public TorsionalFunctionBase(double G, double J, double L, double z, double a)
        {
            this.G=  G;
            this.J=  J;
            this.L=  L;
            this.z=  z;
            this.a = a;

            CheckInputValues();
        }

        public TorsionalFunctionBase(double G, double J, double L, double z, double a,
            double T, double alpha)
        {
            this.G = G;
            this.J = J;
            this.L = L;
            this.z = z;
            this.a = a;
            this.T = T;

            CheckInputValues();

        }
        public TorsionalFunctionBase(double G, double J, double L, double z, double a,
    double t)
        {
            this.G = G;
            this.J = J;
            this.L = L;
            this.z = z;
            this.a = a;
            this.t = t;

            CheckInputValues();

        }

        private void CheckInputValues()
        {
            if (z < 0)
            {
                throw new Exception("Distance z cannot be less than zero.");
            }
            else if (z>L)
            {
                throw new Exception("Distance z cannot be larger than length L.");
            }
        }
        protected double G {get;set;}
        protected double J {get;set;}
        protected double L {get;set;}
        protected double z {get;set;}
        protected double a { get; set; }
        protected double alpha { get; set; }

        protected double  t { get; set; }
        protected double T { get; set; }

        public abstract double Get_theta();
        public abstract double Get_theta_1();
        public abstract double Get_theta_2();
        public abstract double Get_theta_3();


        private double _c;

        private double _c_1;
        private bool _c_1Set;
        protected double c_1
        {
            get
            {
                if (_c_1Set == false)
                {
                    _c_1 =Get_c_1();
                    _c_1Set = true;
                }
                return _c_1;
            }

        }

        private double _c_2;
        private bool _c_2Set;
        protected double c_2
        {
            get
            {
                if (_c_2Set == false)
                {
                    _c_2 = Get_c_2();
                    _c_2Set = true;
                }
                return _c_2;
            }

        }

        private double _c_3;
        private bool _c_3Set;
        protected double c_3
        {
            get
            {
                if (_c_3Set == false)
                {
                    _c_3 = Get_c_3();
                    _c_3Set = true;
                }
                return _c_3;
            }

        }

        private double _c_4;
        private bool _c_4Set;
        protected double c_4
        {
            get
            {
                if (_c_4Set == false)
                {
                    _c_4 = Get_c_4();
                    _c_4Set = true;
                }
                return _c_4;
            }

        }


        private double _c_5;
        private bool _c_5Set;
        protected double c_5
        {
            get
            {
                if (_c_5Set == false)
                {
                    _c_5 = Get_c_5();
                    _c_5Set = true;
                }
                return _c_5;
            }

        }

        private double _H;
        private bool _HSet;
        protected double H
        {
            get
            {
                if (_HSet == false)
                {
                    _H = Get_H();
                    _HSet = true;
                }
                return _H;
            }

        }

        private double _S;
        private bool _SSet;
        protected double S
        {
            get
            {
                if (_SSet == false)
                {
                    _S = Get_S();
                    _SSet = true;
                }
                return _S;
            }

        }

        protected virtual double Get_c_1()  { return 0.0;}
        protected virtual double Get_c_2(){ return 0.0;}
        protected virtual double Get_c_3(){ return 0.0;}
        protected virtual double Get_c_4() { return 0.0; }
        protected virtual double Get_c_5() { return 0.0; }
        protected virtual double Get_H() { return 0.0; }
        protected virtual double Get_S() { return 0.0; }
    }
}

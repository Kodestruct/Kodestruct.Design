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

namespace Kodestruct.Steel.AISC.UFM
{
    public class UFMCaseNoMomentsAtInterfaces: UFMCase
    {

        public UFMCaseNoMomentsAtInterfaces(double d_b, double d_c, double theta, 
            double alpha, double beta, double P_u, double R_beam, bool IncludeDistortionalMomentForces, 
            double M_d, double A_ub )
            :base(d_b,d_c,theta)
                
        {
            this.alpha = alpha;
            this.beta = beta;
            this.P_u = P_u;
            this.R_beam = R_beam;
            this.IncludeDistortionalMomentForces=IncludeDistortionalMomentForces;
            this.M_d                            =M_d                            ;
            this.A_ub = A_ub;
        }


           public double  R_beam { get; set; }
           public  double P_u     {get; set;}
           public  double alpha   {get; set;}
           public  double beta { get; set; }

            bool   IncludeDistortionalMomentForces {get; set;}
            double M_d                             {get; set;}
            double A_ub                            { get; set; }


        private void CalculateBasicParameters()
        {
            _r = Math.Sqrt(Math.Pow((alpha + e_c), 2) + Math.Pow((beta + e_b), 2));

            //Distortional forces
            CalculateDistortionalParameters();

            double SignedH_d = P_u >= 0 ? _H_d : -H_d;
            double SignedV_d = P_u >= 0 ? _V_d : -V_d;

            double H_b_ND = alpha * ((P_u) / (_r));  //without effect of distortional forces
            double V_b_ND = e_b * ((P_u) / (_r))  ;  //without effect of distortional forces
            double H_c_ND = e_c * ((P_u) / (_r))  ;  //without effect of distortional forces
            double V_c_ND = beta * ((P_u) / (_r)) ;  //without effect of distortional forces

            _H_b = H_b_ND - SignedH_d;   //with effect of distortional forces
            _V_b = V_b_ND + SignedV_d;   //with effect of distortional forces
            _H_c = H_c_ND - SignedH_d;   //with effect of distortional forces
            _V_c = V_c_ND + SignedV_d;  //with effect of distortional forces

            //beam interface

            //per discussion in AISC Design guide 29 page 69 Example 5.1
            //The Vub and Vab force is reversible as the brace force goes from tension to compression, but the gravity beam 
            //shear always remains in the same direction. Therefore, Vub and the reaction should always be added even if shown in 
            //opposite directions 

            if (_V_b >= 0)
            {
                _V_bc = V_b_ND + R_beam + SignedV_d;
            }
            else
            {
                _V_bc = -V_b_ND + R_beam - SignedV_d;
            }
            double SignedA_ub = P_u >= 0 ? A_ub : -A_ub;

            _H_bc = H_c_ND + SignedA_ub - SignedH_d;

            BasicPropertiesAreCalculated = true;
        }

        bool BasicPropertiesAreCalculated;

        private double _r;

        public double r
        {
            get {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _r; }
          }

        private double  _H_b;

        /// <summary>
        /// Horizontal force at beam-gusset interface
        /// </summary>
        public double  H_b
        {
            get {
                if (BasicPropertiesAreCalculated ==false)
                {
                    CalculateBasicParameters();
                }
                return _H_b; }
        }

        /// <summary>
        /// Vertical force at beam-gusset interface
        /// </summary>
        private double _V_b;

        public double V_b
        {
            get {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _V_b; }

        }

        /// <summary>
        /// Horizontal force at column-gusset interface
        /// </summary>
        private double _H_c;

        public double H_c
        {
            get {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _H_c; }

        }

        private double _V_c;

        /// <summary>
        /// Vertical force at column-gusset interface
        /// </summary>
        public double V_c
        {
            get {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _V_c; }
        }


        /// <summary>
        /// Horizontal force at beam-column interface
        /// </summary>
        private double _H_bc;

        public double H_bc
        {
            get
            {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _H_bc;
            }

        }

        private double _V_bc;

        /// <summary>
        /// Vertical force at beam-column interface
        /// </summary>
        public double V_bc
        {
            get
            {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateBasicParameters();
                }
                return _V_bc;
            }
        }
        
        //Distortional forces

        /// <summary>
        /// Horizontal distortional force at gusset
        /// </summary>
        protected double _H_d;

        public double H_d
        {
            get
            {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateDistortionalParameters();
                }
                return _H_d;
            }

        }

        protected double _V_d;

        /// <summary>
        /// Vertical distortional force at gusset
        /// </summary>
        public double V_d
        {
            get
            {
                if (BasicPropertiesAreCalculated == false)
                {
                    CalculateDistortionalParameters();
                }
                return _V_d;
            }
        }

        protected virtual void CalculateDistortionalParameters()
        {
            if (IncludeDistortionalMomentForces ==true)
            {
                _H_d = Math.Abs(M_d) / (beta + e_b);
                _V_d = beta / alpha * _H_d;
            }
            else
            {
                _H_d =  0.0;
                _V_d = 0.0;
            }
            
        }
        
    }
}

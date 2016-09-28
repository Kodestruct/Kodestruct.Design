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
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.UFM
{

    /// <summary>
    /// Nonconcentric brace  force case 
    /// </summary>
    public class UFMCase1NonconcentricBraceForce : UFMCaseNoMomentsAtInterfaces
    {
        public UFMCase1NonconcentricBraceForce(double d_b, double d_c, double theta,
            double alpha, double beta, double alpha_bar, double beta_bar, double x_brace, 
            double y_brace, double P_u, double R_beam,
            double Z_beam, double Z_column,
            bool IncludeDistortionalMomentForces,
            double M_d, double A_ub)
            : base(d_b, d_c, theta, alpha, beta, P_u, R_beam, IncludeDistortionalMomentForces, M_d, A_ub)
        {
            this.alpha_bar = alpha_bar;
            this.beta_bar = beta_bar;
            this.x_brace = x_brace;
            this.y_brace = y_brace;
            this.P_u = P_u;
            this.Z_beam = Z_beam;
            this.Z_column = Z_column;

            CalculateForces();
        }

        private void CalculateForces()
        {

            double c = Math.Cos(theta.ToRadians());
            double s = Math.Sin(theta.ToRadians());

            double e = (e_b - y_brace) * s -(e_c - x_brace) * c;
            double eta = ((Z_beam) / (Z_beam + 2.0 * Z_column));

            double M = P_u * e;

            double H_prime = (((1 - eta) * M) / (beta_bar + e_b));
            double V_prime = ((M - H_prime * beta_bar) / (alpha_bar));

            

            if (e>=0)
            {
                _H_uc = H_c + H_prime;
                _V_uc = V_c + V_prime;
                _V_ub = V_b - V_prime; // +R_beam;
                _H_ub = H_b - H_prime;
            }
            else
            {
                _H_uc = H_c - H_prime;
                _V_uc = V_c - V_prime;
                _V_ub = V_b + V_prime; // -R_beam;
                _H_ub = H_b + H_prime;
            }
        }



        double alpha_bar { get; set; }
        double beta_bar { get; set; }
        double x_brace { get; set; }
        double y_brace { get; set; }
        double P_u { get; set; }
        double Z_beam { get; set; }
        double Z_column { get; set; }

        #region Basic Output forces


        private double _H_ub;

        public double H_ub
        {
            get
            {
                return _H_ub;
            }
        }


        private double _V_ub;

        public double V_ub
        {
            get
            {
                return _V_ub;
            }

        }

        private double _H_uc;

        public double H_uc
        {
            get
            {
                return _H_uc;
            }

        }

        private double _V_uc;

        public double V_uc
        {
            get
            {
                return _V_uc;
            }
        }
        #endregion

    }
}

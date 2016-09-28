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
    /// <summary>
    /// General UFM case, if the gusset-to-beam connection is the more flexible 
    /// </summary>
    public class UFMGeneralMomentAtColumnGussetInterface : UFMCaseNoMomentsAtInterfaces
    {
        public UFMGeneralMomentAtColumnGussetInterface(double d_b, double d_c, double theta,
            double alpha, double beta, double beta_bar, double P_u, double R_beam, bool IncludeDistortionalMomentForces,
            double M_d, double A_ub)
            : base(d_b, d_c, theta, alpha, beta, P_u, R_beam, IncludeDistortionalMomentForces, M_d, A_ub)
        {
            this.beta_bar = beta_bar;
        }

        double beta_bar;

        #region Basic Output forces


        private double _H_ub;

        public double H_ub
        {
            get
            {
                _H_ub = H_b;
                return _H_ub;
            }
        }


        private double _V_ub;

        public double V_ub
        {
            get
            {
                _V_ub = V_b;
                return _V_ub;
            }

        }

        private double _H_uc;

        public double H_uc
        {
            get
            {
                _H_uc = H_c;
                return _H_uc;
            }

        }

        private double _V_uc;

        public double V_uc
        {
            get
            {
                _V_uc = V_c;
                return _V_uc;
            }
        }
        #endregion


        private double _M_uc;

        public double M_uc
        {
            get
            {
                _M_uc = H_c*(beta - beta_bar);
                return _M_uc;
            }
        }
    }
}

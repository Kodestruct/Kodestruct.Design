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

namespace Kodestruct.Steel.AISC.UFM
{
    /// <summary>
    /// General UFM case, if the gusset-to-column connection is the more flexible 
    /// </summary>
    public class UFMGeneralMomentAtBeamGussetInterface : UFMCaseNoMomentsAtInterfaces
    {
        public UFMGeneralMomentAtBeamGussetInterface(double d_b, double d_c, double theta,
            double alpha, double beta, double alpha_bar, double P_u, double R_beam, bool IncludeDistortionalMomentForces,
            double M_d, double A_ub)
            : base(d_b, d_c, theta, alpha, beta, P_u, R_beam, IncludeDistortionalMomentForces, M_d, A_ub)
        {
            this.alpha_bar = alpha_bar;
        }

        double alpha_bar;

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


        private double _M_ub;

        public double M_ub
        {
            get
            {
                _M_ub = V_b*(alpha-alpha_bar);
                return _M_ub;
            }
        }
    }
}

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
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.UFM
{
    public class UFMCase3NoGussetToColumnConnection : UFMCaseNoMomentsAtInterfaces
    {
        public UFMCase3NoGussetToColumnConnection(double d_b, double d_c, double theta,
            double alpha, double alpha_bar, double P_u, double R_beam, bool IncludeDistortionalMomentForces,
            double M_d, double A_ub)
            : base(d_b, d_c, theta, alpha, 0.0, P_u, R_beam, IncludeDistortionalMomentForces, M_d, A_ub)
        {
            alpha_bar = this.alpha_bar;
            CalculateForces();
        }

        private void CalculateForces()
        {
            _H_ub = P_u * Math.Sin(theta.ToRadians());
            _V_ub = P_u * Math.Cos(theta.ToRadians());
            _M_ub = _V_ub * (alpha_bar - alpha);
            _M_bc = _V_ub * e_c;
            _H_uc = 0.0;
            _V_uc = 0.0;
            _V_bc = _V_ub - R_beam;
        }

        double alpha_bar;


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



        private double _M_ub;

        public double M_ub
        {
            get
            {
                return _M_ub;
            }
        }

        private double _M_bc;

        public double M_bc
        {
            get
            {
                return _M_bc;
            }
        }

        private double _V_bc;

        public double V_bc
        {
            get
            {
                return _V_bc;
            }
        }
    }
}

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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.AISC360v10.Shear;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public  partial class RhsTrussGappedKConnection : RhsTrussBranchConnection
    {

        public RhsTrussGappedKConnection(SteelRhsSection Chord, SteelRhsSection MainBranch, double thetaMain,
            SteelRhsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeMain, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord)
            : base(Chord, MainBranch, thetaMain, ForceTypeMain, SecondBranch, thetaSecond, ForceTypeSecond, IsTensionChord, P_uChord,  M_uChord)
        {

        }
        /// <summary>
        /// K2-14
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordWallPlastificationStrength(bool IsMainBranch)
        {

            double P_n = 0.0;
            double phi = 0.90;
            P_n = ((F_y * Math.Pow(t, 2) * (9.8 * beta_eff * Math.Sqrt(gamma)) * Q_f) / (sin_theta));

            double phiP_n = phi * P_n;
            return new SteelLimitStateValue(phiP_n, true);
        }

        /// <summary>
        /// K2-15
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetBranchPunchingStrength()
        {
            if (B_b == H_b)
            {
                return new SteelLimitStateValue(-1, false);
            }
            else
            {
                if (B_b > B - 2.0 * t)
                {
                    return new SteelLimitStateValue(-1, false);
                }
                else
                {

                    double P_n = 0.0;
                    double phi = 0.95;

                    P_n = ((0.6 * F_y * t * B * (2 * eta + beta + beta_eop)) / (sin_theta));

                    double phiP_n = 0;
                    return new SteelLimitStateValue(phiP_n, true);
                }
            }

        }

        /// <summary>
        /// Shear strength of sidewalls in gap region
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordSidewallShearStrength()
        {

            double P_n = 0.0;
            double phi = 0.9;

            double phiP_n = 0;

            ShearMemberBox shearMember = new ShearMemberBox(this.Chord.Section, this.Chord.Material);
            double phiV_nChord = shearMember.GetShearStrength();
            phiP_n = phiV_nChord / sin_theta;
            return new SteelLimitStateValue(phiP_n, true);

        }

        /// <summary>
        /// K2-16
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetBranchYieldingFromUnevenLoadDistributionStrength(bool IsMainBranch =true)
        {
            this.IsMainBranch = IsMainBranch;

            double P_n = 0.0;
            double phi = 0.95;

            if (B_b == H_b || B/t >15.0)
            {
                return new SteelLimitStateValue(-1, false);
            }
            else
            {
                double b_eoi = ((10.0) / (((B) / (t)))) * (((F_y * t) / (F_yb * t_b))) * B_b;
                b_eoi = b_eoi > B_b ? B_b : b_eoi;

                P_n = F_yb * t_b * (2 * H_b + B_b + b_eoi - 4.0 * t_b);

                double phiP_n = 0;
                return new SteelLimitStateValue(phiP_n, true);
            }


        }

        protected override SteelRhsSection getBranch()
        {
            return this.MainBranch;
        }

        protected override double GetQ_fInCompression()
        {
            double Q_f = 1.3 - 0.4 * ((U) / (beta_eff));
            return Q_f;
        }

        private double _beta_eff;

        public double beta_eff
        {
            get {
                _beta_eff = Get_betaEff();
                return _beta_eff; }
            set { _beta_eff = value; }
        }

        private double Get_betaEff()
        {
            double beta_eff   = 0.0;
            double B_bComp  =MainBranch.Section.B;
            double B_bTens  =SecondBranch.Section.B;
            double H_bComp  =MainBranch.Section.H;
            double H_bTens = SecondBranch.Section.H;

            beta_eff = ((((B_bComp + H_bComp) + (B_bTens + H_bTens))) / (4 * B));
            return beta_eff;
        }
        

    }

}

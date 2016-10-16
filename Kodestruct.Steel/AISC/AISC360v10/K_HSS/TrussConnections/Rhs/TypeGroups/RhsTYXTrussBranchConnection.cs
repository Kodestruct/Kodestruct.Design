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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class RhsTYXTrussBranchConnection: RhsTrussBranchConnection
    {

        public RhsTYXTrussBranchConnection(SteelRhsSection Chord, SteelRhsSection BranchMain, double thetaMain,
            SteelRhsSection BranchSecondary, double thetaSecondary, AxialForceType ForceTypeMain, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord)
            : base( Chord,  BranchMain,  thetaMain, ForceTypeMain,  BranchSecondary,  thetaSecondary, ForceTypeSecond, IsTensionChord,  P_uChord,  M_uChord)
        {

        }


        /// <summary>
        /// K2-7
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordWallPlastificationStrength(bool IsMainBranch)
        {
            double P_n = 0.0;
            double phi = 1.0;

            if (beta <=0.85)
            {
                //K2-7
                P_n=((F_y*Math.Pow(t, 2)*(((2*eta) / ((1-beta)))+((4.0) / (Math.Sqrt(1-beta))))*Q_f) / (sin_theta));
                double phiP_n = phi * P_n;
                return new SteelLimitStateValue(phiP_n,true);
            }
            else
                {
                    return new SteelLimitStateValue(-1, false);
                }
        }

        /// <summary>
        /// K2-8
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetBranchPunchingStrength()
        {

            double P_n = 0.0;
            double phi = 0.95;


            if (beta>0.85 && beta <= 1-1.0/gamma)
            {
                //K2-8
                 P_n = 0.6 * F_y * t * B * (((2 * eta + 2 * beta_eop)) / (sin_theta));
                 double phiP_n = phi * P_n;
                 return new SteelLimitStateValue(phiP_n, true);
            }
            else if (B/t < 10)
            {
                //K2-8
                P_n = 0.6 * F_y * t * B * (((2 * eta + 2 * beta_eop)) / (sin_theta));
                double phiP_n = phi * P_n;
                return new SteelLimitStateValue(phiP_n, true);
            }
            else
            {
                return new SteelLimitStateValue(-1, false);
            }
        }
        /// <summary>
        /// K2-9
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordSidewallLocalYieldingStrength()
        {

            double P_n = 0.0;
            double phi = 1.0;

            if (beta == 1.0)
            {
                P_n = ((2 * F_y * t * (5 * k + l_b)) / ( sin_theta));
                double phiP_n = phi * P_n;
                return new SteelLimitStateValue(phiP_n, true);
            }
            else
            {
                return new SteelLimitStateValue(-1, false);
            }
        }
        /// <summary>
        /// K2-10. Note that X connection overrides this method with K2-11
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordSidewallLocalCripplingStrength()
        {

            double P_n = 0.0;
            double phi = 0.75;

            if (beta == 1.0)
            {
                if (ForceTypeMain == AxialForceType.Compression && ForceTypeMain == AxialForceType.Reversible)
                {
                    P_n = ((1.6 * Math.Pow(t, 2) * (1 + ((3 * l_b) / (H - 3 * t))) * Math.Sqrt(E * F_y) * Q_f) / (sin_theta));
                    double phiP_n = phi * P_n;
                    return new   SteelLimitStateValue(phiP_n, false);
                }
                else
                {
                    return new SteelLimitStateValue(-1, false);
                }
            }
            else
            {
                return new SteelLimitStateValue(-1, false);
            }
        }

        /// <summary>
        /// K2-13
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetBranchYieldingFromUnevenLoadDistributionStrength(bool IsMainBranch = true)
        {
            this.IsMainBranch = IsMainBranch;

            double b_eoi;
            
            double P_n = 0.0;
            double phi = 0.95;

            if (beta >0.85)
            {
                //beoi is  effective width of branch face transverse to the chord;
                b_eoi=((10.0) / (((B) / (t))))*(((F_y*t) / (F_yb*t_b)))*B_b;
                b_eoi = b_eoi >B_b? B_b: b_eoi;
                P_n=F_yb*t_b*(2*H_b+2.0*b_eoi-4.0*t_b);
                double phiP_n = 0.95*P_n;

                return new SteelLimitStateValue(phiP_n, false);
            }
            else
            {
 
                return new SteelLimitStateValue(-1, false);
            }
        }

        /// <summary>
        /// Chord shear check
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordSidewallShearStrength()
        {
            throw new NotImplementedException();
        }


        protected override SteelRhsSection getBranch()
        {
            return this.MainBranch;
        }

        protected override double GetQ_fInCompression()
        {
            double Q_f = 1.3 - 0.4 * ((U) / (beta));
            return Q_f;
        }
    }


}

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
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.Steel.Entities;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public  partial class RhsTrussOverlappedConnection : RhsTrussBranchConnection
    {
        public RhsTrussOverlappedConnection(SteelRhsSection Chord, SteelRhsSection MainBranch, double thetaMain,
            SteelRhsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeMain, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord,
            double O_v)
            : base(Chord, MainBranch, thetaMain, ForceTypeMain, SecondBranch, thetaSecond, ForceTypeSecond, IsTensionChord,  P_uChord,  M_uChord)
        {
            this.O_v = O_v;
        }

        //Applicable limit states


        public override SteelLimitStateValue GetBranchYieldingFromUnevenLoadDistributionStrength(bool IsMainBranch)
        {

            this.IsMainBranch = IsMainBranch;
            double P_ni = GetP_nForOverlappingBranch();
            double phi = 0.95;
            double phiP_n = 0.0;

            if (IsMainBranch == true)
            {
                //overlapped branch
                double P_nj = P_ni * (((F_ybj * A_bj) / (F_ybi * A_bi)));
                phiP_n = phi * P_nj;
                return new SteelLimitStateValue(phiP_n, true);
            }
            else
            {
                //overlapping branch
                phiP_n = phi * P_ni;
                return new SteelLimitStateValue(phiP_n, true);
            }

        }

        private double GetP_nForOverlappingBranch()
        {

            double P_ni = 0.0;

            //K2-17
            if (O_v >= 0.25 && O_v < 0.5)
            {
                P_ni = F_ybi * t_bi * (((O_v) / (50.0)) * (2 * H_bi - 4 * t_bi) + b_eoi + b_eov);
            }
            //K2-18
            else if (O_v >= 0.5 && O_v < 0.8)
            {
                P_ni = F_ybi * t_bi * (2 * H_bi - 4 * t_bi + b_eoi + b_eov);
            }
            //K2-19
            else if (O_v >= 0.8 && O_v < 1)
            {
                P_ni = F_ybi * t_bi * (2 * H_bi - 4 * t_bi + B_bi + b_eov);
            }
            else
            {
                throw new Exception("Specified overlap is outside of code-specified range. Please spcify overlap to be between 0.25 and 1.0");
            }

            return P_ni;
        }

        protected override SteelRhsSection getBranch()
        {
            return this.MainBranch;
        }

        /// <summary>
        /// Overlap
        /// </summary>
        public double O_v { get; set; }


        protected override double GetQ_fInCompression()
        {
            return 0.0; // No provisions for Q_f of overlapped conections
            //check code
        }

        private SteelRhsSection overlappedBranch;

        public SteelRhsSection OverlappedBranch
        {
            get { return MainBranch; }
            set { overlappedBranch = value; }
        }

        private SteelRhsSection overlappingBranch;

        public SteelRhsSection OverlappingBranch
        {
            get { return SecondBranch; }
            set { overlappingBranch = value; }
        }



        private double _b_eoi;

        public double b_eoi
        {
            get
            {
                _b_eoi = Get_b_eoi();
                return _b_eoi;
            }
            set { _b_eoi = value; }
        }

        /// <summary>
        /// K2-20
        /// </summary>
        /// <returns></returns>
        private double Get_b_eoi()
        {

            double b_eoi = ((10.0) / (((B) / (t)))) * (((F_y * t) / (F_ybi * t_bi))) * B_bi; 
            b_eoi = b_eoi > B_bi ? B_bi : b_eoi;
            return b_eoi;
        }

        /// <summary>
        /// K2-21+
        /// </summary>
        private double _b_eov;

        public double b_eov
        {
            get
            {
                _b_eov = Get_b_eov();
                return _b_eov;
            }
            set { _b_eov = value; }
        }

        private double Get_b_eov()
        {

            double b_eov=((10) / (((B_bj) / (t_bj))))*(((F_ybj*t_bj) / (F_ybi*t_bi)))*B_bi;
            b_eov = b_eov > B_bi ? B_bi : b_eov; 
            return b_eov;
        }
        

 
        public double F_ybi
        {
            get { return OverlappingBranch.Material.YieldStress; }

        }


        public double F_ybj
        {
            get { return OverlappedBranch.Material.YieldStress; }

        }


        public double B_bi
        {
            get { return OverlappingBranch.Section.B; }
        }


        public double B_bj
        {
            get { return OverlappedBranch.Section.B; }
        }


        public double H_bi
        {
            get { return OverlappingBranch.Section.H; }
        }


        public double H_bj
        {
            get { return OverlappedBranch.Section.H; }
        }



        public double t_bi
        {
            get { return OverlappingBranch.Section.t_des; }
        }


        public double t_bj
        {
            get { return OverlappedBranch.Section.t_des; }
        }



        public double A_bi
        {
            get { return OverlappingBranch.Section.A; }
        }


        public double A_bj
        {
            get { return OverlappedBranch.Section.A; }
        }

     
    }

}

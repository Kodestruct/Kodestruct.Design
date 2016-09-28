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
 
 
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Exceptions;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamINoncompactWeb : FlexuralMemberIBase
    {
        double Cb;
        double Lp; double Lr;
        double rt;
        double F_L;
        double Sxc;

        // Lateral-Torsional Buckling F4.2
        public double GetLateralTorsionalBucklingCapacity(FlexuralCompressionFiberPosition compressionFiberPosition, double L_b, double Cb)
        {
            double M_n = 0.0;

            double R_pc = GetRpc(compressionFiberPosition);
            double M_yc = GetCompressionFiberYieldMomentMyc(compressionFiberPosition);
            F_L = GetStressFL(compressionFiberPosition);
   
            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Sxc = Section.Shape.S_xTop;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Sxc = Section.Shape.S_xBot;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }
            double r_t = GetEffectiveRadiusOfGyration_r_t(compressionFiberPosition);
            double h_o = this.SectionI.h_o; 
            double L_r = GetL_r();
            double L_p = GetL_p(r_t);
            LateralTorsionalBucklingType BucklingType = GetLateralTorsionalBucklingType(L_b, L_p, L_r);

            switch (BucklingType)
            {
                case LateralTorsionalBucklingType.NotApplicable:
                    M_n = double.PositiveInfinity;
                    break;
                case LateralTorsionalBucklingType.Inelastic:
                    M_n = Cb * (R_pc * M_yc - (R_pc * M_yc - F_L * Sxc) * ((L_b - L_p) / (L_r - L_p))); //(F4-2)
                    M_n = M_n > R_pc * M_yc ? R_pc * M_yc : M_n;
                    break;
                case LateralTorsionalBucklingType.Elastic:
                    double Iyc =  GetIyc(compressionFiberPosition);
                    double Iy = SectionI.I_x;
                    J = Iyc / Iy <= 0.23 ? 0.0 : J;
                    double Fcr = GetFcr(L_b);
                    break;
                default:
                    break;
            }


            double phiM_n = 0.9 * M_n;
            return phiM_n;
        }

        //(F4-7)
        internal double GetL_p(double rt)
        {
            double Lp = 1.1 * rt * Math.Sqrt(E / Fy); //(F4-7)
            return Lp;
        }

        //(F4-8)
        internal double GetL_r()
        {
            double Lr = 1.95 * rt * E / F_L * Math.Sqrt((J / (Sxc * ho)) + Math.Sqrt(Math.Pow(J / (Sxc * ho), 2.0) + 6.76 * Math.Pow(F_L / E, 2.0)));  // (F4-8)
            return Lr;
        }

        //(F4-5)

        internal double GetFcr(double L_b)
        {
            double Fcr;
            double pi2 = Math.Pow(Math.PI, 2);
            Fcr = Cb * pi2 * E / (Math.Pow(L_b / rt, 2)) * Math.Sqrt(1.0 + 0.078 * J / (Sxc * ho) * Math.Pow(L_b / rt, 2)); //(F4-5)

            return Fcr;
        }
    }
}

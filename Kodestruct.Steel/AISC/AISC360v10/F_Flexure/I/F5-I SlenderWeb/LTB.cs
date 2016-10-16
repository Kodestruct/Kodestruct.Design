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
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamISlenderWeb : FlexuralMemberIBase
    {

        double Cb;
        double L_p; double Lr;
        double Rpg;

        // Lateral-Torsional Buckling F5.2
        public double GetLateralTorsionalBucklingStrength(FlexuralCompressionFiberPosition compressionFiberPosition, double L_b, double Cb)
        {
            double Mn = 0.0;
            double r_t = GetEffectiveRadiusOfGyration_r_t(compressionFiberPosition);
            L_p = GetLp(r_t);
            Lr = GetLr(r_t);
            Rpg = GetRpg(compressionFiberPosition);
            double Sxc = compressionFiberPosition == FlexuralCompressionFiberPosition.Top ? Sxtop : Sxbot;


            LateralTorsionalBucklingType BucklingType = GetLateralTorsionalBucklingType(L_b, L_p, Lr);
            double Fcr = 0.0;

            switch (BucklingType)
            {
                case LateralTorsionalBucklingType.NotApplicable:
                    Mn = double.PositiveInfinity;
                    break;
                case LateralTorsionalBucklingType.Inelastic:
                    Fcr = GetFcrLateralTorsionalBucklingInelastic(L_b);

                    break;
                case LateralTorsionalBucklingType.Elastic:
                    Fcr = GetFcrLateralTorsionalBucklingElastic(r_t, L_b);

                    break;

            }
            if (Mn != double.PositiveInfinity)
            {

                Mn = Rpg * Fcr * Sxc; //(F5-2)
            }
            double phiM_n = 0.9 * Mn;
            return phiM_n;

        }


        public  double GetLr(double r_t)
        {
            double pi = Math.PI;
            double Lr = pi * r_t * Math.Sqrt(E / 0.7 * Fy); //(F5-5)
            return Lr;
        }

        //(F4-7)
        internal  double GetLp(double r_t)
        {
            double Lp = 1.1 * r_t * Math.Sqrt(E / Fy); //(F4-7)
            return Lp;
        }

        public  double GetFcrLateralTorsionalBucklingInelastic(double L_b)
        {
            double Fcr = 0.0;

            Fcr = Cb * (Fy - (0.3 * Fy) * ((L_b - L_p) / (Lr - L_p))); //(F5-3)
            Fcr = Fcr > Fy ? Fy : Fcr;
            return Fcr;
        }

        public  double GetFcrLateralTorsionalBucklingElastic(double r_t, double L_b)
        {
            double Fcr = 0.0;
            double pi2 = Math.Pow(Math.PI, 2);
            Fcr = (Cb * pi2 * E) / Math.Pow(L_b / r_t, 2); //(F5-4)
            Fcr = Fcr > Fy ? Fy : Fcr;
            return Fcr;
        }

    }
}

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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.Steel.Entities;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamSolid : FlexuralMember
    {

        public SteelLimitStateValue GetLateralTorsionalBucklingStrength(double L_b, double C_b)
        {
            double phiM_n = 0.0;
            double L_p = GetL_p( );
            double L_r = GetL_r( );
            double d = get_d();
            double t = get_t();
            double S = GetS();

            double M_n = 0.0;
            SteelLimitStateValue ls = new SteelLimitStateValue();


            if (L_b<=L_p)
            {
                
                ls.IsApplicable = false;
                ls.Value = -1;
                return ls;
            }
            else
            {

                double M_y = GetM_y();
                if (L_b<=L_r)
                {
                    //(F11-2)
                    M_n=C_b*(1.52-0.274*(((L_b*d) / (Math.Pow(t, 2))))*(((F_y) / (E))))*M_y;
                }
                else
                {
                    //(F11-3)
                    double F_cr = GetCriticalStress(C_b,L_b,t,d);
                    M_n = F_cr * S;

                }

            }

            phiM_n = 0.9 * M_n;
            ls.Value = phiM_n;
            ls.IsApplicable = true;

            return ls;
        }

        private double GetCriticalStress(double C_b, double L_b, double t, double d)
        {
            //(F11-4)
            double F_cr = ((1.9 * E * C_b) / (((L_b * d) / (Math.Pow(t, 2)))));
            return F_cr;
        }

        public double GetL_r()
        {
            double t = get_t( );
            double d = get_d( );
           double Lr=  ((1.9 * E * t * t) / (F_y * d));

           return Lr;
        }

        public double GetL_p()
        {
            double t = get_t( );
            double d = get_d( );
            double L_p = ((0.08 * E * t * t) / (F_y * d));

            return L_p;
        }

    }
}

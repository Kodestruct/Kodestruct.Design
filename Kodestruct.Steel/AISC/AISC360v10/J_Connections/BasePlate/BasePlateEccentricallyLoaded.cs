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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateEccentricallyLoaded:BasePlateConcentricallyLoaded
    {

        public BasePlateEccentricallyLoaded(IBasePlate Plate)
            :base(Plate)
        {
            N_bp = Plate.N_bp;
            B_bp = Plate.B_bp;
            f_c = Plate.f_c;
        }

        private void CalculateBasePlateCommonParameters(double P_u, double M_u, BendingAxis Axis)
        {
            double phi_c = 0.65;
            A_1 = Plate.B_bp * Plate.N_bp;
            A_2 = Plate.A_2;
            q_pMax = Get_q_pMax(f_c, Axis, A_1, A_2);
            f_pMax = Get_f_pMax(f_c, A_1, A_2);
            e = M_u / P_u;
        }

        double N_bp;
        double B_bp;
        double e;
        double A_1;
        double A_2;
        double q_pMax;
        double f_pMax;
        double f_c;

        public virtual double GetMinimumThicknessEccentricLoadStrongAxis(double P_u, double M_u, BendingAxis Axis, double f_anchor)
        {

            double F_y = Plate.F_y;
           // double Y = GetBearingLengthY(P_u, M_u, Axis, f_anchor);

            bool meetsMinimumSize = DetermineIfBasePlateMeetsMinimumSizeLimits(P_u, M_u, Axis, f_anchor);
            if (meetsMinimumSize == false)
            {
                throw new Exception("Base plate does not meet minimum size requirements. Please revise.");
            }

            double Y = GetBearingLengthY(P_u, M_u, Axis, f_anchor);
            CalculateBasePlateCommonParameters(P_u, M_u,Axis);


            // calculate m
            double m = Plate.Get_m();
            double n = Plate.Get_n();

            double f_pMax = Get_f_pMax(f_c, A_1, A_2);
            
            double f_p = Get_f_p(P_u, M_u,Y,Axis);


            double t_p;

            if (Y>=m) 
            {
                //DG01 3.3.14a-2
                double l = Math.Max(m, n);
                t_p = 1.49 * l * Math.Sqrt(((f_p) / F_y));
            }
            else
            {
                //DG01 3.3.15a-2
                t_p = 2.11 * Math.Sqrt(((f_p * Y * (m - (Y / 2.0))) / F_y));
            }

            return t_p;


        }

        private double Get_f_p(double P_u, double M_u, double Y, BendingAxis Axis)
        {

            double f_p;  
            double e_critical = Get_e_critical(P_u, M_u, Axis);
            if (e <= e_critical)
            {
                //Small moment base plate
               f_p= P_u / (B_bp * Y);
            }
            else
            {
                //Large moment base plate
                f_p = Get_f_pMax(f_c, A_1, A_2);
            }

            return f_p;
        }


        public virtual double GetTensionForceEccentricLoadStrongAxis(double P_u, double M_u, BendingAxis Axis, double f_anchor)
        {
            double Y = GetBearingLengthY(P_u, M_u, Axis, f_anchor);
            CalculateBasePlateCommonParameters(P_u, M_u, Axis);
            double T = q_pMax * Y - P_u;

            return T;


        }


        private double Get_e_critical(double P_u, double M_u, BendingAxis Axis)
        {
            CalculateBasePlateCommonParameters(P_u, M_u, Axis);
            double e_crit = ((N_bp) / (2)) - ((P_u) / (2 * q_pMax));
            return e_crit;
        }

        public bool DetermineIfBasePlateMeetsMinimumSizeLimits(double P_u, double M_u,  BendingAxis Axis, double f_anchor)
        {
            CalculateBasePlateCommonParameters(P_u, M_u, Axis);

            double Cr1 = Math.Pow((f_anchor+N_bp / 2.0), 2);
            double Cr2 =(2.0*P_u*(e+f_anchor)) / q_pMax;
             if (Cr1<Cr2)
	        {
		         return false;
	        }
             else
	        {
                 return true;
	        }
           
        }

        double GetBearingLengthY(double P_u, double M_u, BendingAxis Axis, double f_anchor)
        {
            CalculateBasePlateCommonParameters(P_u, M_u, Axis);
            double Y = 0.0;

            double e_critical = Get_e_critical(P_u, M_u, Axis);
            if (e <= e_critical)
            {
                //Small moment base plate
                Y = N_bp - 2.0 * e;
            }
            else
            {
                //Large moment base plate
                //DG01  3.4.3
                double Y1 = (f_anchor + ((N_bp) / (2))) + Math.Sqrt(Math.Pow((f_anchor + ((N_bp) / (2))), 2) - ((2 * P_u * (e + f_anchor)) / q_pMax));
                double Y2 = (f_anchor + ((N_bp) / (2))) - Math.Sqrt(Math.Pow((f_anchor + ((N_bp) / (2))), 2) - ((2 * P_u * (e + f_anchor)) / q_pMax));
                List<double> BearingLengths = new List<double>() { Y1, Y2 };
                Y = BearingLengths.Where(y => y > 0 && y < N_bp).Max();
            }


            return Y;
        }

        double Get_q_pMax(double f_c, BendingAxis Axis,double A_1, double A_2)
        {
            double f_pMax = Get_f_pMax(f_c,A_1, A_2);
            double q_pMax = 0.0;

            if (Axis == BendingAxis.Major)
            {
                q_pMax = f_pMax * B_bp;
            }
            else
            {
                q_pMax = f_pMax * N_bp;
            }

            return q_pMax;
        }

        double Get_f_pMax(double f_c, double A_1, double A_2)
        {
            double phi_c = 0.65;
            double f_pMax = phi_c*(0.85 * f_c) * Math.Sqrt(((A_2) / (A_1)));
            return f_pMax;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.WebOpenings
{
    public class WebOpeningGeneral
    {

        public static double GetReinforcementDevelopmentLength(double a_o, double t_w,
            double b_r, double t_r, bool IsSingleSideReinforcement)
        {
            double l_1_1 = a_o / 4.0;

            double A_r = IsSingleSideReinforcement == true? t_r*b_r : 2.0*t_r*b_r;
            
            double l_1_2 = A_r * Math.Sqrt(3.0) / (2.0 * t_w);

            double l_1 = Math.Max(l_1_1, l_1_2);
            return l_1;
        }

        public static double GetReinforcementWeldRequiredStrength(double a_o, double t_w, 
            double b_r, double t_r, double F_y, bool IsSingleSideReinforcement, bool IsCompositeBeam)
        {
            double phi = IsCompositeBeam == true ? 0.85 : 0.9;

           double A_r = IsSingleSideReinforcement == true? t_r*b_r : 2.0*t_r*b_r;

           double P_r1 = A_r * F_y;
           double P_r2 = F_y * t_w * a_o / (2.0 * Math.Sqrt(2.0));
          
           double P_r = Math.Min(P_r1, P_r2);

           return P_r;
        }

        public static double GetMaximumOpeningHeight(double a_o, double d, double e,
          double t_f, double t_w, double F_y, bool IsCompositeBeam)
        {
            double h_oMax;
            double h_oMax1 = 0.7 * d;
            if (IsCompositeBeam == false)
            {
                double s_teeMax = 0.15 * d;
                double h_oMax2 = d - s_teeMax * 2.0 - e * 2.0; //top tee limitation
                double h_oMax3 = d - s_teeMax * 2.0 + e * 2.0; ; //bottom tee limitation
                double h_oMax4 = GetOpeningParameterLimit(IsCompositeBeam,d,a_o);

                List<double> h = new List<double>()
                {
                    h_oMax1, 
                    h_oMax2,
                    h_oMax3
                };

                h_oMax = h.Min();
            }
            else
            {
                double s_teeMax = 0.12 * d;
                double h_oMax2 = d - s_teeMax * 2.0 + e * 2.0; ; //bottom tee limitation
                double h_oMax3 = GetOpeningParameterLimit(IsCompositeBeam, d, a_o);

                List<double> h = new List<double>()
                {
                    h_oMax1, 
                    h_oMax2,
                    h_oMax3
                };

                h_oMax = h.Min();
            }

            return h_oMax;
        }

        private static double GetOpeningParameterLimit(bool IsCompositeBeam, double d,double a_o)
        {
            double h;
 
            if (IsCompositeBeam == false)
            {
                double h1 = 0.4667*d-0.04714*Math.Pow(d*(98.0*d-75.0*a_o),0.5);
                double h2 = 0.4667 * d + 0.04714 * Math.Pow(d * (98.0 * d - 75.0 * a_o), 0.5);
                if (h2<0 )
                {
                    return h1;
                }
                else
                {
                    return Math.Max(h1, h2);
                }
            }
            else
            {
                double h1 = 0.5 * d - 0.28867 * Math.Pow(d * (3.0 * d - 2.0 * a_o), 0.5);
                double h2 = 0.5 * d + 0.28867 * Math.Pow(d * (3.0 * d - 2.0 * a_o), 0.5);
                if (h2 < 0)
                {
                    return h1;
                }
                else
                {
                    return Math.Max(h1, h2);
                }
            }
        }

        public static double GetMaximumOpeningWidth(double h_o, double d,
           double t_f, double t_w,  double F_y, bool IsCompositeBeam, bool IsSingleSideReinforcement)
        {
            double a_oMax, a_oMax1;
            double W_t_Ratio = (d - 2.0 * t_f) / t_w;
            if (W_t_Ratio > 520/Math.Sqrt(F_y))
            {
                throw new Exception("Width ot thickness ratio is too large. Consider reducing web d or inreasing t_w");
            }
            else
            {
                if (W_t_Ratio < 420/Math.Sqrt(F_y))
                {
                    a_oMax1 = 3.0 * d;
                }
                else
                {
                    a_oMax1 = 2.2 * d;
                }
            }
            double a_oMax2;

            if (IsCompositeBeam == false)
            {
                 a_oMax2 =(5.6-(6.0*h_o/d))*h_o;
            }
            else
            {
                a_oMax2 = (6.0 - (6.0 * h_o / d)) * h_o;
            }

            double a_oMaxGen = Math.Min(a_oMax1, a_oMax2);
           

            //Equation 3-34
            if (IsSingleSideReinforcement == true)
           {
               double a_oMaxGenS = 2.5 * h_o;
               a_oMax = Math.Min(a_oMaxGen, a_oMaxGenS);
           }
            else
            {
                a_oMax = a_oMaxGen;
            }

            return a_oMax;
        }

        public static double GetMinimumClearSpacing(double a_o,double h_o, double d,double t_w, double F_y, 
            double V_u, bool IsCompositeBeam)
        {
           double V_bar_p = d * t_w * 0.6 * F_y;
            double phi = IsCompositeBeam == true? 0.85 : 0.9;
            double phiV_bar_p = phi*V_bar_p;

            double S_min1 = 2.0*d;
            double S_min2 = h_o;
            double S_min3 = a_o;
            //3-37b
            double S_min4 = a_o * ((V_u / phiV_bar_p) / (1.0 - V_u / phiV_bar_p));

            List<double> Smins = new List<double>()
            {
                 S_min1, 
                 S_min2, 
                 S_min3, 
                 S_min4 
            };

            double S_min = Smins.Max();
            return S_min;
        }

    }
}

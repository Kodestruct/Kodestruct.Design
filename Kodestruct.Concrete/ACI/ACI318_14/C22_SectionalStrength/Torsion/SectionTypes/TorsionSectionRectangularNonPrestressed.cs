using Kodestruct.Common.Section.SectionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public class TorsionSectionRectangularNonPrestressed : IConcreteTorsionalShape
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RectangularShape">Rectangular shape</param>
        /// <param name="c_transv_ctr">Cover to center of transverse reinforcement</param>
        public TorsionSectionRectangularNonPrestressed(SectionRectangular RectangularShape,
           IConcreteMaterial Material, double c_transv_ctr )
        {
            this.RectangularShape = RectangularShape;
            this.c_transv_ctr = c_transv_ctr;
            this.IsPrestressed = IsPrestressed;
            this.Material = Material;
        }

        SectionRectangular RectangularShape { get; set; }
        public IConcreteMaterial Material { get; set; }
        double c_transv_ctr { get; set; }
        bool IsPrestressed { get; set; }

        public double GetA_oh()
        {
            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double A_oh = (b - 2.0 * c_transv_ctr) * (h - 2.0 * c_transv_ctr);
            return A_oh;
        }



        public double Get_T_th(double N_u)
        {

            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double T_th = 0.0;
            double A_cp = GetA_cp();
            double p_cp = Get_p_cp();
            double lambda = Material.lambda;
            double Sqrt_f_c = Material.Sqrt_f_c_prime;
            double A_g = b * h;

                if (N_u == 0)
                {
                    T_th = 4 * lambda * Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp)));
                }
                else
                {
                    T_th = 4 * lambda *Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp))) * Math.Sqrt(1.0 + ((N_u) / (4.0 * A_g * lambda *Sqrt_f_c)));
                }

                return T_th;
        }


        public double GetA_cp()
        {
            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double A_cp = b * h;
            return A_cp;
        }

        public double Get_p_h()
        {
            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double p_h = 2.0 * ((b - 2.0 * c_transv_ctr) + (h - 2.0 * c_transv_ctr));
            return p_h;
        }

        public double Get_p_cp()
        {
            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double p_cp = 2 * (b + h);
            return p_cp;
        }
    }
}

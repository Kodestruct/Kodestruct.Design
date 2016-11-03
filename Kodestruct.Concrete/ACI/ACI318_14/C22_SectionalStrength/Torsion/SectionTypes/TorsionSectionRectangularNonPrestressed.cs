using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Concrete.ACI318_14;
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
        /// <param name="c_transv_ins">Cover to inside face of transverse reinforcement</param>
        public TorsionSectionRectangularNonPrestressed(SectionRectangular RectangularShape,
           IConcreteMaterial Material, double c_transv_ins )
        {
            this.RectangularShape = RectangularShape;
            this.c_transv_ins = c_transv_ins;
            this.IsPrestressed = IsPrestressed;
            this.Material = Material;
        }

        SectionRectangular RectangularShape { get; set; }
        public IConcreteMaterial Material { get; set; }
        double c_transv_ins { get; set; }
        bool IsPrestressed { get; set; }

        public double GetA_oh()
        {
            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double A_oh = (b - 2.0 * c_transv_ins) * (h - 2.0 * c_transv_ins);
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
                    T_th = lambda * Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp)));
                }
                else
                {
                    T_th = lambda *Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp))) * Math.Sqrt(1.0 + ((N_u) / (A_g * lambda *Sqrt_f_c)));
                }

            StrengthReductionFactorFactory srf = new StrengthReductionFactorFactory();
            double phi = srf.Get_phi_Torsion();
            return phi*T_th;
        }

        public double Get_T_cr(double N_u)
        {

            double b = RectangularShape.B;
            double h = RectangularShape.H;
            double T_cr = 0.0;
            double A_cp = GetA_cp();
            double p_cp = Get_p_cp();
            double lambda = Material.lambda;
            double Sqrt_f_c = Material.Sqrt_f_c_prime;
            double A_g = b * h;

            if (N_u == 0)
            {
                T_cr = 4 * lambda * Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp)));
            }
            else
            {
                T_cr = 4 * lambda * Sqrt_f_c * (((Math.Pow(A_cp, 2)) / (p_cp))) * Math.Sqrt(1.0 + ((N_u) / (4.0 * A_g * lambda * Sqrt_f_c)));
            }

            StrengthReductionFactorFactory srf = new StrengthReductionFactorFactory();
            double phi = srf.Get_phi_Torsion();
            return phi * T_cr;

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
            double p_h = 2.0 * ((b - 2.0 * c_transv_ins) + (h - 2.0 * c_transv_ins));
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {
        double b;
        double d;
        double F_b;
        double l_e;
        double E_min;
        double C_M_Fb;
        double C_M_E;
        double C_t_Fb;
        double C_t_E;
        double C_c;
        double C_I;
        double lambda;

        protected override bool DetermineIfMemberIsLaterallyBraced()
        {
            //determine lateral bracing requirements to skip stability checks
             return false;
        }

        /// <summary>
        /// Finds the controlling case of beam stability factor and volume factor
        /// </summary>
        /// <param name="b"></param>
        /// <param name="d"></param>
        /// <param name="F_b"></param>
        /// <param name="L"></param>
        /// <param name="l_e"></param>
        /// <param name="E_min"></param>
        /// <param name="C_M_Fb"></param>
        /// <param name="C_M_E"></param>
        /// <param name="C_t_Fb"></param>
        /// <param name="C_t_E"></param>
        /// <param name="C_c">Curvature Factor</param>
        /// <param name="C_I">Stress Interaction Factor</param>
        /// <param name="lambda"></param>
        /// <param name="IsSouthernPine"></param>
        /// <returns></returns>
        public double GetStabilityFactorC_L (double b, double d, double F_b, double L,
            double l_e, double E_min, double C_M_Fb, double C_M_E, double C_t_Fb, double C_t_E,  double C_c, double C_I, double lambda,
            bool IsSouthernPine)
        {
            double C_i_E = 1.0; //Incising factor is not applicable to glulams
            double C_T = 1.0; //Buckling Stiffness factor  is not applicable to glulams

            this.b = b;
            this.d = d;
            this.l_e = l_e;
            this.F_b = F_b;
            this.E_min = E_min;
            this.C_M_Fb = C_M_Fb;
            this.C_M_E = C_M_E;
            this.C_t_Fb = C_t_Fb;
            this.C_t_E = C_t_E;
            this.C_c = C_c;
            this.C_I = C_I;
            this.lambda = lambda;

            double C_L = GetC_L(b, d, l_e, E_min, C_M_E, C_t_E, C_i_E, C_T);


            return C_L; 
        }

    
        protected override double GetF_b_AdjustedForBeamStability()
        {
            double K_F = GetFormatConversionFactor_K_F(Entities.ReferenceDesignValueType.Bending);
            double phi = GetStrengthReductionFactor_phi(Entities.ReferenceDesignValueType.Bending); //Table N.3.2;
            //from Table 5.3.1  except the beam stability factor CL flat use factor Cfu and volume factor CV applied.
            return F_b * C_M_Fb * C_t_Fb * C_c * C_I * K_F * phi * lambda; 
        }

    }
}

using Kodestruct.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners
{
    public partial class DowelFastenerBase : WoodFastener
    {

        public DowelFastenerBase(double D, double G, double theta)
        {
            this.D = D;
            this.G = G;
            this.theta = theta;
            R_dParametersCalculated = false;
        }

        /// <summary>
        /// Angle to grain
        /// </summary>
        public double theta { get; set; }
        /// <summary>
        /// Diameter
        /// </summary>
        public double D { get; set; }

        /// <summary>
        /// Specific Gravity
        /// </summary>
        public double G { get; set; }

        public double GetDowelBearingStrength()
        {
            double F_e_perp = 0;
            double F_e_para = 0;

            if (D<0.25)
            {
                F_e_perp =16600*Math.Pow(G, 1.84);
                F_e_para =F_e_perp;
            }
            else if (D<=1.0)
            {
                F_e_perp = ((6100*Math.Pow(G, 1.45)) / (Math.Sqrt(D)));
                F_e_para = 11200*G;
            }
            else
            {
                throw new Exception("Dowel diameters over 1 inch diameter not supported.");
            }

            double F_e_theta=(F_e_para*F_e_perp) / (F_e_para*Math.Pow(Math.Sin(theta),2.0)+F_e_perp*Math.Pow(Math.Cos(theta),2));

            return F_e_theta;
        }


        private double _R_d1;

        public double R_d1
        {
            get {
                if (R_dParametersCalculated == false)
                {
                    CalculateR_dParameters();
                }
                return _R_d1; }
            set { _R_d1 = value; }
        }



        private double _R_d2;

        public double R_d2
        {
            get {
                if (R_dParametersCalculated == false)
                {
                    CalculateR_dParameters();
                }
                return _R_d2; }
            set { _R_d2 = value; }
        }

        private double _R_d3;

        public double R_d3
        {
            get {
                if (R_dParametersCalculated ==false)
                {
                    CalculateR_dParameters();
                }
                return _R_d3; }
            set { _R_d3 = value; }
        }

        bool R_dParametersCalculated;
        private void CalculateR_dParameters()
        {
            //Table 12.3.1B

            double K_theta = 1.0 + 0.25 * (((theta) / (90.0)));
            double K_D;
            if (D<=0.17)
	            {
		             K_D = 2.2;
	            }
            else
            {
                K_D = (10 * D) + 0.5;
            }

            if (D < 0.25)
            {
                _R_d1 = K_D;
                _R_d2 = K_D;
                _R_d3 = K_D;
            }
            else if (D <= 1.0)
            {
                _R_d1 = 4 * K_theta;
                _R_d2 = 3.6 * K_theta;
                _R_d3 = 3.2*K_theta;
            }
            else
            {
                throw new Exception("Dowel diameters over 1 inch diameter not supported.");
            }

            R_dParametersCalculated = true;
        }

        /// <summary>
        /// Per Table 11.3.1  Applicability of Adjustment Factors for Connections 
        /// </summary>
        /// <param name="Z">Reference lateral design value</param>
        /// <param name="C_M">Wet Service Factor</param>
        /// <param name="C_t">Temperature Factor </param>
        /// <param name="C_g">Group Action Factor </param>
        /// <param name="C_delta">Geometry Factor</param>
        /// <param name="C_eg">End Grain Factor </param>
        /// <param name="C_tn">Toe-Nail Factor </param>
        /// <param name="lambda">Time Effect Factor </param>
        /// <returns></returns>
        public double GetAdjustedLateralStrength(double Z, 
             double C_M, double  C_t, double C_g, double C_delta,
            double C_eg, double C_tn, double lambda, MechanicalDowelConnectionType MechanicalDowelConnectionType, 
            bool IsDiaphragmConnection)
        {
            double C_di = GetDiaphragmFactor(MechanicalDowelConnectionType, IsDiaphragmConnection);
            double Z_prime = Z * C_M * C_t * C_g * C_delta * C_eg * C_di * C_tn * 3.32 * 0.65 * lambda;
            return Z_prime;
        }

    }
}

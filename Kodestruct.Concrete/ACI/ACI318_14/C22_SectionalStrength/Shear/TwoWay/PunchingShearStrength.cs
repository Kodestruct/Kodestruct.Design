using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public partial class ConcreteSectionTwoWayShear
    {

        #region Shear strength
        /// <summary>
        /// Shear strength (as stress) per table 22.6.5.2
        /// </summary>
        /// <returns></returns>
        public double GetTwoWayStrengthForUnreinforcedConcrete()
        {
            double f_c = Material.SpecifiedCompressiveStrength;
            double v_cControlling = 0.0;

            double lambda = Material.lambda;

            if (AtColumnFace == true)
            {


                double beta = GetBeta();
                double alpha_s = Get_alpha_s();
                double b_o = Get_b_o();


                double v_c1 = lambda * 4.0 * Math.Sqrt(f_c);
                double v_c2 = lambda * (2.0 + ((4.0) / (beta))) * Math.Sqrt(f_c);
                double v_c3 = lambda * (((alpha_s * d) / (b_o)) + 2.0) * Math.Sqrt(f_c);

                List<double> vc_s = new List<double>()
            {
                v_c1,
                v_c2,
                v_c3
            };
                v_cControlling = vc_s.Min();
            }
            else
            {
                v_cControlling = lambda * 2.0 * Math.Sqrt(f_c);
            }

            double phi = 0.75;
            return phi * v_cControlling;
        }

        /// <summary>
        /// Column coefficient alpha per section 22.6.5.3
        /// </summary>
        /// <returns></returns>
        private double Get_alpha_s()
        {
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    return 40.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    return 20.0;
                    break;
                default:
                    return 40.0;
                    break;
            }
        }

        /// <summary>
        /// Ratio of column sides per footnote Table 22.6.5.2
        /// </summary>
        /// <returns></returns>
        private double GetBeta()
        {

            if (c_x > c_y)
            {
                return c_x / c_y;
            }
            else
            {
                return c_y / c_x;
            }
        }


        #endregion
    }
}

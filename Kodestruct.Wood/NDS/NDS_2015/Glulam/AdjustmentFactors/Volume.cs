using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="L">Length of bending member between points of zero moment, in </param>
        /// <param name="IsSouthernPine"></param>
        /// <returns></returns>
        public double  GetVolumeFactor_C_v(double  d, double L, bool IsSouthernPine)
        {
            double L_ft = L / 12.0;  // length of bending member between points of zero moment, ft

            double x = IsSouthernPine ? 20 : 10;
            //NDS Eq. 5.3-1
            double C_v = Math.Pow((((21.0) / (L_ft))), 1/x) * Math.Pow((((12.0) / (d))), 1/x) * Math.Pow((((5.125) / (d))), 1/x);
            C_v = C_v > 1.0 ? 1.0 : C_v;
            
            return C_v;
        }
    }
}

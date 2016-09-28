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
using Kodestruct.Common.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public partial class ExtendedSinglePlate : AnalyticalElement
    {
        public double StabilizedExtendedSinglePlateTorsionalMoment(double R_u, double t_p, double t_w)
        {
            //Thornton Fortney "On the Need for Stiffeners for and the Effect of Lap Eccentricity on Extended Single-Plate Connections"
            //paper explains the effect of torsion due to lap eccentricity
                double M_tu = R_u * ((t_w + t_p) / (2)); //Manual equation 10-8a
                return M_tu;
        }

        public double StabilizedExtendedSinglePlateTorsionalStrength(double b_f,double F_yb, double L_bm,double R_u,double t_w,double d_pl,double t_p, double F_yp)
       {

        double phi_b=0.9;
        double phi_v=1.0;

        //Manual equation 10-7a
        //Thornton Fortney "On the Need for Stiffeners for and the Effect of Lap Eccentricity on Extended Single-Plate Connections"
        //paper explains the effect of torsion due to lap eccentricity
        double phiM_tu=(phi_v*(0.6*F_yp)-((R_u) / (d_pl*t_p)))*((d_pl*Math.Pow(t_p, 2)) / (2))+((2*Math.Pow(R_u, 2)*(t_w+t_p)*b_f) / ((phi_b*F_yb)*L_bm*Math.Pow(t_w, 2))) ;

        return phiM_tu;
        }
    }

}

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
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners.DowelType
{
    public class DoubleShearFastener : SingleShearFastener
        {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="D">diameter, in. (see 12.3.7) </param>
        /// <param name="F_yb">dowel bending yield strength, psi </param>
        /// <param name="G">Specific gravity</param>
        /// <param name="l_m">main member dowel bearing length, in.   </param>
        /// <param name="l_s">side member dowel bearing length, in</param>
        /// <param name="F_em">main member dowel bearing strength, psi (see Table 12.3.3) </param>
        /// <param name="F_es">side member dowel bearing strength, psi (see Table 12.3.3)</param>
        /// <param name="theta">Angle to the grain</param>
        public DoubleShearFastener(double D, double F_yb, double G, double l_m, double l_s,
            double F_em, double F_es, double theta):
            base( D,  F_yb,  G,  l_m ,  l_s, F_em,  F_es,  theta)
        {
          

        }


        public override double GetZ()
        {
            double Z_Im = Get_Z_Im();
            double Z_Is = Get_Z_Is();
            double Z_IIIs = Get_Z_IIIs();
            double Z_IV = Get_Z_IV();

            List<double> Z = new List<double>()
            {
                Z_Im  ,
                Z_Is  ,
                Z_IIIs,
                Z_IV  
            };
            return Z.Min();
        }


        private double Get_Z_Im()
        {
            double Z_Im =  ((D * l_m * F_em) / (R_d1)); ;
            return Z_Im;
        }

        private double Get_Z_Is()
        {
            double Z_Is = ((D * l_s * F_es) / (R_d1)); ;
            return Z_Is;
        }


        private double Get_Z_IIIs()
        {
            double Z_IIIs = ((2.0 * k_3 * D * l_s * F_em) / ((2.0 + R_e) * R_d3)); ;
            return Z_IIIs;
        }

        private double Get_Z_IV()
        {
            double Z_IV = ((Math.Pow(2.0 * D, 2)) / (R_d3)) * Math.Sqrt(((k_3 * F_em * F_yb) / (3.0 * (1 + R_e)))); ;
            return Z_IV;
        }
    }
}

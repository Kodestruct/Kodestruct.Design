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
    public class SingleShearFastener:   DowelFastenerBase
        {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="D">diameter, in. (see 12.3.7) </param>
        /// <param name="F_yb">Dowel bending yield strength, psi </param>
        /// <param name="G">Specific gravity</param>
        /// <param name="l_m">Main member dowel bearing length, in.   </param>
        /// <param name="l_s">Side member dowel bearing length, in</param>
        /// <param name="F_em">Main member dowel bearing strength, psi (see Table 12.3.3) </param>
        /// <param name="F_es">Sside member dowel bearing strength, psi (see Table 12.3.3)</param>
        /// <param name="theta">Angle to the grain</param>
        public SingleShearFastener (double D, double F_yb, double G, double l_m , double l_s,
            double F_em, double F_es, double theta):
            base(D,G,theta)
        {
          
        this.F_yb  =F_yb ;
        this.l_m   =l_m  ;
        this.l_s   =l_s  ;
        this.F_em  =F_em ;
        this.F_es = F_es;
        k_ParametersCalculated = false;
        R_eAndR_tCalculated = false;
        }

        bool k_ParametersCalculated;
        public double F_yb  {get; set;}
        public double l_m { get; set; }
        public double l_s   {get; set;}
        public double F_em  {get; set;}
        public double F_es { get; set; }


        private double _k_1;

        public double k_1
        {
            get {
                if (k_ParametersCalculated == false)
                {
                    Calculate_k_Parameters();
                }
                return _k_1; }
            set { _k_1 = value; }
        }

        private double _k_2;

        public double k_2
        {
            get {
                if (k_ParametersCalculated==false)
                {
                    Calculate_k_Parameters();
                }
                
                return _k_2; }
            set { _k_2 = value; }
        }

        private double _k_3;

        public double k_3
        {
            get {
                if (k_ParametersCalculated == false)
                {
                    Calculate_k_Parameters();
                }
                return _k_3; }
            set { _k_3 = value; }
        }


        bool R_eAndR_tCalculated;
        private double _R_e;

        public double R_e
        {
            get {
                if (R_eAndR_tCalculated == false)
                {
                    GetRs();
                }
                return _R_e; }
            set { _R_e = value; }
        }

        private double _R_t;

        public double R_t
        {
            get
            {
                if (R_eAndR_tCalculated == false)
                {
                    GetRs();
                }
                return _R_t;
            }
            set { _R_t = value; }
        }

        private void GetRs()
        {
            double R_e = ((F_em) / (F_es));
            double R_t = ((l_m) / (l_s));
            R_eAndR_tCalculated = true;
        }


        private void Calculate_k_Parameters()
        {
            double R_e = ((F_em) / (F_es));
            double R_t = ((l_m) / (l_s));

            _k_1 = ((Math.Sqrt(R_e + 2.0 * Math.Pow(R_e, 2) * (1 + R_t + Math.Pow(R_t, 2)) + Math.Pow(R_t, 2) * Math.Pow(R_e, 3)) - R_e * (1 + R_t)) / (1 + R_e));
            _k_2 = -1 + Math.Sqrt(2 * (1.0 + R_e) + ((2 * F_yb * (1 + 2.0 * R_e) * Math.Pow(D, 2)) / (3.0 * F_em * Math.Pow(l_m, 2))));
            _k_3 = -1.0 + Math.Sqrt(((2 * (1.0 + R_e)) / (R_e)) + ((2.0 * F_yb * (2 + R_e) * Math.Pow(D, 2)) / (3.0 * F_em * Math.Pow(l_s, 2))));

        }

        public virtual double GetZ()
        {
            double Z_Im    =Get_Z_Im    ();
            double Z_Is    =Get_Z_Is    ();
            double Z_II    =Get_Z_II    ();
            double Z_IIIm  =Get_Z_IIIm  ();
            double Z_IIIs  =Get_Z_IIIs  ();
            double Z_IV    =Get_Z_IV    ();

            List<double> Z = new List<double>()
            {
                Z_Im  ,
                Z_Is  ,
                Z_II  ,
                Z_IIIm,
                Z_IIIs,
                Z_IV  
            };
            return Z.Min();
        }

        private double Get_Z_Im()
        {
            double Z_Im = ((D * l_m * F_em) / (R_d1));
            return Z_Im;
        }

        private double Get_Z_Is()
        {
            double Z_Is = ((D * l_s * F_es) / (R_d1));
            return Z_Is;
        }

        private double Get_Z_II()
        {
            double Z_II = ((k_1 * D * l_s * F_es) / (R_d2));
            return Z_II;
        }

        private double Get_Z_IIIm()
        {
            double Z_IIIm = ((k_2 * D * l_m * F_em) / ((1 + 2 * R_e) * R_d3));
            return Z_IIIm;
        }

        private double Get_Z_IIIs()
        {
            double Z_IIIs = ((k_3 * D * l_s * F_em) / ((2.0 + R_e) * R_d3));
            return Z_IIIs;
        }

        private double Get_Z_IV()
        {
            double Z_IV = ((Math.Pow(D, 2)) / (R_d3)) * Math.Sqrt(((k_3 * F_em * F_yb) / (3 * (1 + R_e))));
            return Z_IV;
        }
    }
}

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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Shear
{
    public abstract partial class ShearMemberGeneral : IShearMember
    {

       protected double t_w { get; set; }
       protected double h { get; set; }
       protected ISteelMaterial material { get; set; }
       protected double a { get; set; }
       protected double D { get; set; }
       protected double L_v { get; set; }
       protected double C_v { get; set; }
       protected double phi { get; set; }
       protected bool IsTeeShape { get; set; }

       public ShearMemberGeneral()
       {

       }
       public ShearMemberGeneral(double h, double t_w, double a, ISteelMaterial material, bool IsTeeShape = false)
        {
            this.t_w     =t_w     ;
            this.h       =h       ;
            this.material=material;
            this.a = a;
            this.IsTeeShape = IsTeeShape;
        }

        public virtual double GetShearStrength()
        {
            double C_v = GetC_v();
            double A_w = h * t_w;
            double phi = Get_phi();
            //(G2-1)
            double phiV_n = 0.6 * F_y * A_w * C_v;
            return phiV_n;
        }
        private double GetC_v()
        {
            double C_v;
            if ((h / t_w) <= 2.24 * Math.Sqrt(((E) / (F_y))))
            {
                //(G2-2)
                C_v = 1.0;
            }
            else
            {
                double k_v = Get_k_v();
                if ((h / t_w) <= 1.1 * Math.Sqrt(((k_v * E) / F_y)))
                {
                    //(G2-3)
                    C_v = 1.0;
                }
                else if ((h / t_w) <= 1.37 * Math.Sqrt(((k_v * E) / F_y)))
                {
                    //(G2-4)
                    C_v = ((1.1 * Math.Sqrt(((k_v * E) / F_y))) / (h / t_w));
                }
                else
                {
                    //(G2-5)
                    C_v = ((1.51 * k_v * E) / (Math.Pow((((h) / (t_w))), 2) * F_y));
                }
            }
            return C_v;
        }

        protected abstract double Get_k_v();

        private double Get_phi()
        {
            if ((h / t_w) <= 2.24 * Math.Sqrt(((E) / (F_y))))
            {
                return 1.0;
            }
            else
            {
                return 0.9;
            }
        }

        private double _E;

        protected double E
        {
            get {
                if (material!=null)
                {
                    _E = material.ModulusOfElasticity; 
                }
                return _E; }
        }


        private double _Fy;

        protected double F_y
        {
            get {
                if (material != null)
                {
                    _Fy = material.YieldStress;
                }
                return _Fy; }
        }


    }
}

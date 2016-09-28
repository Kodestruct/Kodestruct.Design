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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Shear
{
    public partial class ShearMemberCircular : IShearMember

    {
        double D;
        double t_nom;
        bool Is_SAW_member;
        ISteelMaterial material;
        double L_v;

        public ShearMemberCircular(double D, double t_nom, bool Is_SAW_member, double L_v, ISteelMaterial material)
        {
            this.D=              D;
            this.t_nom=              t_nom;
            this.Is_SAW_member=  Is_SAW_member;
            this.material = material;
            this.L_v = L_v;
        }
        public double GetShearStrength()
        {
            double phi = 0.9;
            double A_g = GetArea();
            double F_cr = Get_F_cr();
            double V_n = ((F_cr * A_g) / 2.0);
            double phiV_n = phi * V_n;
            return phiV_n;
        }

        private double GetArea()
        {
            double t = Is_SAW_member == true ? t_nom : 0.93 * t_nom;
            double A_g= Math.PI*Math.Pow(D,2)-Math.Pow(D-2*t,2)/4.0;
            return A_g;
        }

        private double Get_F_cr()
        {
            double E = material.ModulusOfElasticity;
            double F_y = material.YieldStress;
            double t = Is_SAW_member == true ? t_nom : 0.93 * t_nom;

            double F_cr1=((1.6*E) / (Math.Sqrt((L_v / D))*Math.Pow(((D / t)), 5.0/4.0)));
            double F_cr2=((0.78*E) / (Math.Pow((((D) / t)), 3.0/2.0)));
            double F_cr3=0.6*F_y;
            List<double> F_crList = new List<double>() { F_cr1, F_cr2, F_cr3 };
            var F_cr = F_crList.Min();
            return F_cr;
        }
    }
}

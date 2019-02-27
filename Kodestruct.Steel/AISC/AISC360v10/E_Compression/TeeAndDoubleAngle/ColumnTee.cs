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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.Steel.Entities;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public  class ColumnTee: IShapeCompact//ColumnFlexuralAndTorsionalBuckling
    {
        ISectionTee SectionTee;
        public ColumnTee(ISteelSection Section, bool IsRolled, double L_x, double L_y, double L_z)
            :base(Section, IsRolled, L_x, L_y,  L_z)
        {

        }

        protected override bool CheckValidShape(ISection Shape)
        {
            if (Shape is ISectionTee)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void SetShape(ISection Shape)
        {
            SectionTee =  Shape as ISectionTee;
        }

        public override double GetTorsionalElasticBucklingStressFe(bool IsEccentricallyLaterallyConstrained)
        {
            double pi2 = Math.Pow(Math.PI, 2);
            double E = Section.Material.ModulusOfElasticity;
            //double Lz = L_ez;


            double G = Section.Material.ShearModulus; //ksi
            double J =   SectionTee.J;
            double I_x = SectionTee.I_x;
            double I_y = SectionTee.I_y;
            double r_x = SectionTee.r_x;
            double r_y = SectionTee.r_y;
            double A_g = SectionTee.A;
            //double C_w = Section.Shape.C_w;
            double C_w = 0;  //Per AISC design examples


            double x_o = 0;
            double y_o = GetShearCenter();
            double r_o_2 = Get_r_o_2(x_o, y_o, I_x, I_y, A_g);
            double H = Get_H(r_o_2, x_o, y_o);
            double F_ey = Get_F_ey(L_ey, r_y,E);
            double F_ez = Get_F_eZ(A_g, r_o_2, L_ez, G,J, C_w, E);

            double F_cr = (((F_ey + F_ez) / (2.0 * H))) * (1.0 - Math.Sqrt(1.0- ((4.0*F_ey*F_ez * H) / (Math.Pow((F_ey + F_ez), 2)))));
            return F_cr;
            

        }

        private double Get_F_eZ(double A_g, double r_o_2, double L_ez, double G, double J, double C_w, double E)
        {
            double pi2 = Math.Pow(Math.PI, 2);
            double F_ez = (((pi2*E*C_w) / (Math.Pow((L_ez), 2)))+G*J)*((1) / (A_g * r_o_2));
            return F_ez;
        }

        private double Get_F_ey(double L_cy, double r_y, double E)
        {
            double pi2 = Math.Pow(Math.PI, 2);
            double F_ey = ((pi2 * E) / (Math.Pow((((L_cy) / (r_y))), 2)));
            return F_ey;
        }

        private double Get_H(double r_o_2, double x_o, double y_o)
        {
            double H = 1 - ((Math.Pow(x_o, 2) + Math.Pow(y_o, 2)) / (r_o_2));
            return H;
        }

        private double Get_r_o_2(double x_o, double y_o, double I_x, double I_y, double A_g)
        {
            double r_o_2 = Math.Pow(x_o, 2) + Math.Pow(y_o, 2) + (I_x + I_y) / A_g;
            return r_o_2;
        }

        private double GetShearCenter()
        {
            //Per AISC Design examples
            double y_o = (SectionTee.d - SectionTee.y_Bar) - SectionTee.t_f / 2.0;
            return y_o;
        }
        protected override bool CheckIfTorsionalBucklingApplicable(double L_ex, double L_ey, double L_ez)
        {
            return true;
        }
    }
}

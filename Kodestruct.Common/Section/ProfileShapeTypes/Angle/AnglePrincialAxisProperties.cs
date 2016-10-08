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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section.SectionTypes
{
    public partial class SectionAngle : CompoundShape, ISectionAngle, ISliceableSection
    {


        public void CalculatePrincipalAxisProperties()
        {

            double I_xy = GetProductOfInertia();

            double alpha_Rad = GetAlphaRad(I_xy);
            _angle_alpha = alpha_Rad.ToDegrees();

            _I_w = GetI_w(I_xy, alpha_Rad);
            _I_z = GetI_z(I_xy, alpha_Rad);


            _r_z = Math.Sqrt(_I_z / A);
            _r_w = Math.Sqrt(_I_w / A);


            //TODO: Section modulus
            //double YTop = YMax - Centroid.Y;
            //double YBot = Centroid.Y - YMin;


            //double XLeft = Centroid.X - XMin;
            //double XRight = XMax - Centroid.X;


            //double ZTop = XLeft * cos_a - YTop * sin_a;
            //double ZBot = XLeft * cos_a - YBot * sin_a;

            //double wLeft = XLeft * cos_a - YTop * sin_a;
            //double wRight = XLeft * cos_a - YBot * sin_a;


            PrincipalAxisPropertiesCalculated=true;
        }

        private void Calculate_beta_w()
        {
            if (b != d)
            {


                double b_beta;
                double d_beta;
                if (b > d)
                {
                    b_beta = b;
                    d_beta = d;
                }
                else
                {
                    b_beta = d;
                    d_beta = b;
                }



                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }

                double alpha_Rad = _angle_alpha.ToRadians();

                double x_c = CentroidYAxisRect - XMin;
                double y_c = Centroid.Y - YMin;

                double cos_a = Math.Cos(alpha_Rad);
                double sin_a = Math.Sin(alpha_Rad);

                //Coordinates of point A
                double a_w = (b_beta - x_c) * cos_a - y_c * sin_a;
                double a_z = -(b_beta - x_c) * sin_a - y_c * cos_a;

                //Coordinates of point B
                double b_w = -x_c * cos_a - y_c * sin_a;
                double b_z = x_c * sin_a - y_c * cos_a;

                //Coordinates of point C
                double c_w = -x_c * cos_a + (d_beta - y_c) * sin_a;
                double c_z = x_c * sin_a + (d_beta - y_c) * cos_a;


                //Beta_w
                double cot_a = 1 / Math.Tan(alpha_Rad);
                double tan_a = Math.Tan(alpha_Rad);

                double IntegralForSegment1 = ((Math.Pow(b_w, 4) * cot_a) / (12)) - ((Math.Pow(b_w, 3) * b_z) / (3)) + ((Math.Pow(c_w, 4) * cot_a) / (4)) + ((b_z * Math.Pow(c_w, 3)) / (3)) - ((Math.Pow(b_w, 4) * Math.Pow(cot_a, 3)) / (4)) + ((Math.Pow(c_w, 4) * Math.Pow(cot_a, 3)) / (4)) - b_w * Math.Pow((b_z - b_w * cot_a), 3) + c_w * Math.Pow((b_z - b_w * cot_a), 3) - ((b_w * Math.Pow(c_w, 3) * cot_a) / (3)) - ((3 * Math.Pow(b_w, 2) * cot_a * Math.Pow((b_z - b_w * cot_a), 2)) / (2)) - Math.Pow(b_w, 3) * Math.Pow(cot_a, 2) * (b_z - b_w * cot_a) + ((3 * Math.Pow(c_w, 2) * cot_a * Math.Pow((b_z - b_w * cot_a), 2)) / (2)) + Math.Pow(c_w, 3) * Math.Pow(cot_a, 2) * (b_z - b_w * cot_a);
                double IntegralForSegment2 = ((Math.Pow(b_w, 4) * tan_a) / (12)) + ((Math.Pow(b_w, 3) * b_z) / (3)) + ((Math.Pow(c_w, 4) * tan_a) / (4)) - ((b_z * Math.Pow(c_w, 3)) / (3)) - ((Math.Pow(b_w, 4) * Math.Pow(tan_a, 3)) / (4)) + ((Math.Pow(c_w, 4) * Math.Pow(tan_a, 3)) / (4)) + b_w * Math.Pow((b_z + b_w * tan_a), 3) - c_w * Math.Pow((b_z + b_w * tan_a), 3) - ((b_w * Math.Pow(c_w, 3) * tan_a) / (3)) - ((3 * Math.Pow(b_w, 2) * tan_a * Math.Pow((b_z + b_w * tan_a), 2)) / (2)) + Math.Pow(b_w, 3) * Math.Pow(tan_a, 2) * (b_z + b_w * tan_a) + ((3 * Math.Pow(c_w, 2) * tan_a * Math.Pow((b_z + b_w * tan_a), 2)) / (2)) - Math.Pow(c_w, 3) * Math.Pow(tan_a, 2) * (b_z + b_w * tan_a);

                double z_o = b_z;

                _beta_w = 1.0 / I_w * (t * (IntegralForSegment1 + IntegralForSegment2)) - 2.0 * b_z;
            }
            else
            {
                _beta_w = 0;
            }
            beta_wCalculated = true;
        }
        private double GetAlphaRad(double I_xy)
        {
            double tan_alpha2 = -2.0 * I_xy / (I_x - I_y);
            double alpha2 = Math.Atan(tan_alpha2);
            double alpha_rad = alpha2 / 2.0;
            return alpha_rad;
        }

        private double GetI_z(double I_xy, double alpha_Rad)
        {
            double cos_a = Math.Cos(alpha_Rad);
            double sin_a = Math.Sin(alpha_Rad);

            double _I_z = I_x * Math.Pow(sin_a, 2) + I_y * Math.Pow(cos_a, 2) + 2 * I_xy * sin_a * cos_a;
            return _I_z;
        }

        private double GetI_w(double I_xy, double alpha_Rad)
        {

            double cos_a = Math.Cos(alpha_Rad);
            double sin_a = Math.Sin(alpha_Rad);


            double _I_w = I_x * Math.Pow(cos_a, 2) + I_y * Math.Pow(sin_a, 2) - 2 * I_xy * sin_a * cos_a;
            return _I_w;
        }

        private double GetProductOfInertia()
        {
            List<CompoundShapePart> compoundShapesX = GetCompoundRectangleXAxisList();
            Point2D c = this.Centroid;
            double I_xy = compoundShapesX.Sum(cs => cs.GetArea()*(cs.GetCentroid().X - c.X)*(cs.GetCentroid().Y-c.Y));
            return I_xy;
        }


    }
}

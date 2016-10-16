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

namespace Kodestruct.Common.Mathematics
{
    public class Vector2d
    {
       public Vector2d(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Gets an angle between 0 and 90 degrees when second vector is specified
        /// </summary>
        /// <param name="Vector">Second Vector</param>
        /// <returns></returns>
        public double GetAngle0to90(Vector2d Vector)
        {
            double A = GetMagnitude(this);
            double B = GetMagnitude(Vector);
            double ABdotProduct = GetDotProduct(Vector);

            double angleRad = Math.Acos(Math.Abs(ABdotProduct/(A*B))) ;
            double angleDeg = angleRad*180.0/Math.PI;
            return angleDeg;
        }

        public Vector2d GetPerpendicularVector()
        {
            //Assuming that the second vector is 0i +0j +1k
            //A X B=(AyBz -AzBy)i -(AxBz -AzBx)j +(AxBy -AyBx)k
            return new Vector2d (this.Y, -this.X);
         
        }

        private double GetMagnitude(Vector2d v)
        {
            double m = Math.Sqrt(Math.Pow(v.X,2)+Math.Pow(v.Y,2));
            return m;
        }
        private double GetMagnitude()
        {
            return this.GetMagnitude(new Vector2d(this.X, this.Y));
        }

        public Vector2d GetUnit()
        {
            double M = GetMagnitude();
            Vector2d Unit = new Vector2d(X/M, Y/M);
            return Unit;
        }

        private double GetDotProduct(Vector2d Vector)
        {
            Vector2d A = this;
            Vector2d B = Vector;
            return A.X*B.X+A.Y*B.Y;

        }

        public double FindDistance(Vector2d A)
        {
            //Find Cross Product of PositionVector X Force Unit Vector
            //which is the moment vector for a unit force
            Vector2d B = GetUnit();
            
            double Ax = A.X;
            double Ay = A.Y;
            double Az = 0;
            
            double Bx = B.X;
            double By = B.Y;
            double Bz = 0;



            double i =Ay*Bz -Az*By;
            double j = -(Ax*Bz -Az*Bx);
            double k = Ax*By -Ay*Bx;

            double Mag = Math.Sqrt(i * i + j * j + k * k);

            return Mag;
        }
    }
}

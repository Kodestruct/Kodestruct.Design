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
    public class Point2D
    {
        public Point2D(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public static bool operator ==(Point2D a, Point2D b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point2D a, Point2D b)
        {
            return !(a == b);
        }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Point2D p = obj as Point2D;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double GetDistanceToPoint(Point2D otherPoint)
        {
            double dx = this.X - otherPoint.X;
            double dy = this.Y - otherPoint.Y;
            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }
    }
}

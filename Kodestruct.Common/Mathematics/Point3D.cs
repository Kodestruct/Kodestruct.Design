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
using System.Text;

namespace Kodestruct.Common.Mathematics
{
    public class Point3D : IEquatable<Point3D>, ICloneable
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        int roundDigits;

        public Point3D()
        {

        }

        public Point3D(double X, double Y, double Z ) : this(null, X, Y, Z )
        {

        }
        public Point3D(double X, double Y) : this(null, X, Y, 0)
        {

        }
        public Point3D(string Name, double X, double Y, double Z)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            roundDigits = 3;
        }
        public Point3D(Point2D p) : this(null, p.X, p.Y, 0)
        {

        }

        public Point3D(Point2D point2D, double Z) : this(point2D.Name, point2D.X, point2D.Y, Z)
        {

        }

        public static bool operator ==(Point3D a, Point3D b)
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
            return Math.Round(a.X, a.roundDigits) == Math.Round(b.X, a.roundDigits) && Math.Round(a.Y, a.roundDigits) == Math.Round(b.Y, a.roundDigits) && Math.Round(a.Z, a.roundDigits) == Math.Round(b.Z, a.roundDigits);
        }

        public static bool operator !=(Point3D a, Point3D b)
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
            Point3D p = obj as Point3D;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == Math.Round(p.X, p.roundDigits)) && (Y == Math.Round(p.Y, p.roundDigits)) && (Z == Math.Round(p.Z, p.roundDigits));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double GetDistanceToPoint(Point3D otherPoint)
        {
            double dx = this.X - otherPoint.X;
            double dy = this.Y - otherPoint.Y;
            double dz = this.Z - otherPoint.Z;
            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2) + Math.Pow(dz, 2));
        }

        public bool Equals(Point3D other)
        {
            // If parameter is null return false.
            if (other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == Math.Round(other.X, other.roundDigits)) && (Y == Math.Round(other.Y, other.roundDigits)) && (Z == Math.Round(other.Z, other.roundDigits));
        }

        public static Point3D GetFromListByCoordinates(List<Point3D> points, double X, double Y, double Z, double Tolerance)
        {
            Point3D pt = new Point3D(X, Y, Z);
            foreach (var p in points)
            {
                var dist = p.GetDistanceToPoint(pt);
                if (dist <= Tolerance)
                {
                    return p;
                }
            }
            return null;
        }

        public Point2D ToPoint2D()
        {
            return new Point2D(this.Name, this.X, this.Y);
        }
        public Point3D GetScaledClone(double ScaleFactor)
        {
            var clone = (Point3D)this.Clone();
            clone.X = clone.X * ScaleFactor;
            clone.Y = clone.Y * ScaleFactor;
            clone.Z = clone.Z * ScaleFactor;

            return clone;
        }

        protected Point3D(Point3D another)
        {
            this.X = another.X;
            this.Y = another.Y;
            this.Z = another.Z;
            this.Name = another.Name;
        }
        public object Clone()
        {
            return new Point3D(this);
        }
    }
}

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
using System.Windows;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.SteelEntities.Welds
{
    /// <summary>
    /// Weld element is a small subdivision of weld segment. Weld element can be represented as a point.
    /// </summary>
    public class FilletWeldElement:  WeldBase, IWeldElement
    {
        public Point2D NodeI { get; set; }
        public Point2D NodeJ { get; set; }
        public bool IsLoadedOutOfPlane { get; set; }

        public FilletWeldElement(Point2D StartNode, Point2D EndNode, double Leg, double ElectrodeStrength, bool IsLoadedOutOfPlane =false)
            :base(Leg,ElectrodeStrength)
        {
            this.NodeI = StartNode;
            this.NodeJ = EndNode;
            this.IsLoadedOutOfPlane = IsLoadedOutOfPlane;
        }


        private double length;

        public double Length
        {
            get 
            { 
                double dx = NodeJ.X - NodeI.X;
                double dy = NodeJ.Y-NodeI.Y;
                length = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                return length; 
            }

        }

        public double GetCentroidDistanceToNode(Point2D IC)
        {
            Point2D WeldCentroid = this.Location;
            return WeldCentroid.GetDistanceToPoint(IC);

        }


        public double CalculateNominalShearStrength(double pi, double thetaInput)
        {
            double theta = GetAdjustedTheta(thetaInput);
            double F_EXX = this.WeldElectrodeStrength;
            double sin_theta = Math.Sin(thetaInput.ToRadians());
            double L = this.Length;
            double E = Math.Sqrt(2.0) / 2.0 * this.LegLength;
            //AISC formula 8-3 Page 8-10 of the steel manual
            double Fnwi = 0.60 * F_EXX *E* (1.0 + 0.5 * Math.Pow(sin_theta, 1.5)) * Math.Pow(pi * (1.9 - 0.9 * pi), 0.3)*L;
            return Fnwi;
        }

        private double GetAdjustedTheta(double OriginalTheta)
        {
            double theta = OriginalTheta;

            if (theta > 180) theta = theta % 180;
            if (theta > 90) theta = 180 - theta;
            if (theta < 0) theta = -theta;

            return theta;
        }


        ///
        Point2D location;
        /// <summary>
        /// Weld element centroid location.
        /// </summary>
        public Point2D Location
        {
            get
            {
                double x;
                double y;
                if (IsLoadedOutOfPlane ==true)
                {
                    x = 0;
                }
                else
                {
                    x = (NodeJ.X + NodeI.X) / 2.0;
                }
                
                y = (NodeJ.Y + NodeI.Y) / 2.0;
                if (location == null)
                {
                    location = new Point2D(x, y);
                }
                else
                {
                    location.X = x;
                    location.Y = y;
                }

                return location;
            }
            set
            {
                location = value;
            }

        }


        public double GetAngleBetweenElementAndProjectionFromPoint(Point2D IC)
        {
               //Move system to centroid
               Point2D c = this.Location;
               double dx = Location.X;
               double dy = Location.Y;
               Vector VI = new Vector(NodeI.X - dx, NodeI.Y - dy);
               Vector VIC = new Vector(IC.X - dx, IC.Y - dy);
               
                double Angle= Vector.AngleBetween(VI, VIC);
                return Angle;
        }

        public double GetAngleOfForceResultant(Point2D IC)
        {
            double AngleBetweenTheVectorFromICAndElement = this.GetAngleBetweenElementAndProjectionFromPoint(IC);
            return 90.0 - AngleBetweenTheVectorFromICAndElement;
        }

        public double GetLineMomentOfInertiaYAroundPoint(Point2D center)
        {
            double m = (this.NodeJ.X - this.NodeI.X);
            double m2 =Math.Pow(m,2);
            double l = this.Length;
            double dx = (this.NodeJ.X + this.NodeI.X) / 2.0 - center.X;
            double dx2 = Math.Pow(dx, 2);
            double Iy = l * m2 / 12.0 + l * dx2; //property as line
            return Iy*this.LegLength;

        }
        public double GetLineMomentOfInertiaXAroundPoint(Point2D center)
        {
            double n = (this.NodeJ.Y - this.NodeI.Y);
            double n2 = Math.Pow(n, 2);
            double l = this.Length;
            double dy = (this.NodeJ.Y + this.NodeI.Y) / 2.0 - center.Y;
            double dy2 = Math.Pow(dy, 2);
            double Iy = l * n2 / 12.0 + l * dy2;//property as line
            return Iy * this.LegLength;
        }
        public double GetLinePolarMomentOfInetriaAroundPoint(Point2D center)
        {
            double Ix = this.GetLineMomentOfInertiaXAroundPoint(center);
            double Iy = this.GetLineMomentOfInertiaYAroundPoint(center);
            return Ix + Iy;
        }


        /// <summary>
        /// For fillet weld limit deformation is the deformation at fracture
        /// </summary>
        public double LimitDeformation { get; set; }

        /// <summary>
        /// Ultimate load deformation
        /// </summary>
        public double UltimateLoadDeformation { get; set; }

        public double GetAngleTheta(Point2D Center)
        {
            double Dx = Center.X - this.Location.X;
            double Dy = Center.Y - this.Location.Y;

            //Define vector from the IC to weld element centroid
            Vector2d PositionVector = new Vector2d(Dx, Dy);
            Vector2d ForceVector = new Vector2d(Dy, -Dx);

            Vector2d ElementLineVector = new Vector2d(this.NodeJ.X - this.NodeI.X, this.NodeJ.Y - this.NodeI.Y);
            double theta = ForceVector.GetAngle0to90(ElementLineVector);
            return theta;
        }

        public double GetDistanceToPoint(Point2D point)
        {
            return Math.Sqrt(Math.Pow(point.X - this.Location.X, 2) + Math.Pow(point.Y - this.Location.Y, 2));
        }

        public double DistanceFromCentroid { get; set; }
    }
}

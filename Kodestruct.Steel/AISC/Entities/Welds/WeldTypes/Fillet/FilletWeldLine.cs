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
using Kodestruct.Steel.AISC.AISC360v10.Connections.Weld;

namespace Kodestruct.Steel.AISC.SteelEntities.Welds
{
    /// <summary>
    /// Weld line is a straight line implementation of weld segment base class.
    /// </summary>
    public class FilletWeldLine : WeldSegmentBase
    {
        public Point2D NodeI{get; set;}
        public Point2D NodeJ { get; set; }
        public bool IsLoadedOutOfPlane { get; set; }

        /// <summary>
        /// Angle from vertical (degrees)
        /// </summary>
        public double theta { get; set; }

        private double length;

        public double Length
        {
            get 
            {
                length = GetLength();
                return length; 
            }

        }

        public virtual double GetStrength(bool IgnoreDirectionalityEffects=false)
        {
            FilletWeld weld = new FilletWeld(0, 0, this.ElectrodeStrength, this.Leg, 0, Length);
            double phiR_n = 0;
            if (IgnoreDirectionalityEffects ==true)
            {
                 phiR_n = weld.GetStrength(WeldLoadType.WeldShear, 0, true); 
            }
            else
            {
                 phiR_n = weld.GetStrength(WeldLoadType.WeldShear, theta, true); 
            }
            
            return phiR_n;
        }

        private double GetLength()
        {
            double dx2 = Math.Pow(NodeJ.X-NodeI.X,2);
            double dy2 = Math.Pow(NodeJ.Y-NodeI.Y,2);
            double L = Math.Sqrt(dx2 + dy2);
            return L;
        }

        /// <summary>
        /// Fillet weld line constructor
        /// </summary>
        /// <param name="p1">Point i</param>
        /// <param name="p2">Point j</param>
        /// <param name="leg">Weld size (leg)</param>
        /// <param name="F_EXX">Electrode stength</param>
        /// <param name="NumberOfSubdivisions">Number of sub-segments, used for instantaneous center of rotation calculations</param>
        /// <param name="theta">Angle from vertical (degrees)</param>
        public FilletWeldLine(Point2D p1, Point2D p2, double leg, double F_EXX, int NumberOfSubdivisions,double theta=0.0,  bool IsLoadedOutOfPlane=false)
        {
            // TODO: Complete member initialization
            this.NodeI = p1;
            this.NodeJ = p2;
            this.Leg = leg;
            this.ElectrodeStrength = F_EXX;
            this.NumberOfSubdivisions = NumberOfSubdivisions;
            this.theta = theta;
            this.IsLoadedOutOfPlane = IsLoadedOutOfPlane;
        }


        protected override void CalculateElements()
        {
            WeldElements = new List<IWeldElement>();
            double dx = NodeJ.X - NodeI.X;
            double dy = NodeJ.Y - NodeI.Y;
            
            Vector seg = new Vector(dx, dy);
            int N = NumberOfSubdivisions;

            double segDx;
            double segDy;

            segDx = dx / N;
            segDy = dy / N;

            for (int i = 0; i < NumberOfSubdivisions; i++)
            {
                Point2D stPt = new Point2D(NodeI.X + i * segDx, NodeI.Y + i * segDy);
                Point2D enPt = new Point2D(NodeI.X + (i + 1) * segDx, NodeI.Y + (i + 1) * segDy);
                //Need to change this to be independent of fillet weld type
                FilletWeldElement weld = new FilletWeldElement(stPt, enPt, Leg, ElectrodeStrength, IsLoadedOutOfPlane);
                WeldElements.Add(weld);
            }
        }



        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            FilletWeldLine line = obj as FilletWeldLine;
            if ((System.Object)line == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (NodeI == line.NodeI) && (NodeJ == line.NodeJ);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override double GetInertiaYAroundPoint(Point2D center)
        {
            double Iy = 0;
            foreach (IWeldElement el in this.WeldElements)
            {
                double IyThis =el.GetLineMomentOfInertiaYAroundPoint(center);
                Iy = Iy + IyThis;
            }
            return Iy;
        }

        public override double GetInertiaXAroundPoint(Point2D center)
        {
            double Ix = 0;
            foreach (FilletWeldElement el in this.WeldElements)
            {
                double IxThis = el.GetLineMomentOfInertiaXAroundPoint(center);
                Ix = Ix + IxThis;
            }
            return Ix;
        }

        public override double GetPolarMomentOfInetriaAroundPoint(Point2D center)
        {
            throw new NotImplementedException();
        }



        public override double GetArea()
        {
            double A = 0;
            A = this.Length * this.Leg;

            return A;
        }


        public override double GetSumAreaDistanceX(Point2D refPoint)
        {

            double A = GetArea();
            double dx = this.Centroid.X - refPoint.X;
            return A*dx;
        }

        public override double GetSumAreaDistanceY(Point2D refPoint)
        {
            double A = GetArea();
            double dy = this.Centroid.Y - refPoint.Y;
            return A * dy;
        }


        public Point2D Centroid
        {
            get { return GetCentroid(); }
        }
        

        public Point2D GetCentroid()
        {
            double X = (NodeI.X + NodeJ.X) / 2.0;
            double Y = (NodeI.Y + NodeJ.Y) / 2.0;
            return new Point2D(X, Y);
        }

        private double _X_min;

        public double X_min
        {
            get {
                _X_min = Math.Min(NodeI.X, NodeJ.X);
                return _X_min; }
            set { _X_min = value; }
        }

        private double _X_max;

        public double X_max
        {
            get {
                _X_max = Math.Max(NodeI.X, NodeJ.X);
                return _X_max; }
            set { _X_max = value; }
        }

        private double _Y_min;

        public double Y_min
        {
            get {
                _Y_min = Math.Min(NodeI.Y, NodeJ.Y);
                return _Y_min; }
            set { _Y_min = value; }
        }

        private double _Y_max;

        public double Y_max
        {
            get {
                _Y_max = Math.Max(NodeI.Y, NodeJ.Y);
                return _Y_max; }
            set { _Y_max = value; }
        }
    }
}

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
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public class PerimeterLineSegment
    {
       public Point2D PointI { get; set; }
       public Point2D PointJ { get; set; }

        public PerimeterLineSegment(Point2D PointI, Point2D PointJ)
        {
            this.PointI = PointI;
            this.PointJ = PointJ;
        }

        private Point2D centroid;

        public Point2D Centroid
        {
            get {
                double X_max = Math.Max(PointI.X, PointJ.X);
                double X_min = Math.Min(PointI.X, PointJ.X);
                double Y_max = Math.Max(PointI.Y, PointJ.Y);
                double Y_min = Math.Min(PointI.Y, PointJ.Y);

                centroid = new Point2D((X_max + X_min) / 2.0, (Y_max + Y_min) / 2.0);
                return centroid; }
        }
        
        private double length;

        public double Length
        {
            get {
                length = Math.Sqrt(Math.Pow(PointJ.X - PointI.X, 2.0) + Math.Pow(PointJ.Y - PointI.Y, 2.0));
                return length; }
            set { length = value; }
        }

        #region Y exteme coodinates
        private double yMax;

        public double YMax
        {
            get { return Math.Max(PointI.Y, PointJ.Y); }

        }

        private double yMin;

        public double YMin
        {
            get { return Math.Min(PointI.Y, PointJ.Y); }
        } 
        #endregion

        #region X exteme coodinates
        private double xMax;

        public double XMax
        {
            get { return Math.Max(PointI.X, PointJ.X); }

        }

        private double xMin;

        public double XMin
        {
            get { return Math.Min(PointI.X, PointJ.X); }
        }
        #endregion
        
                


    }
}

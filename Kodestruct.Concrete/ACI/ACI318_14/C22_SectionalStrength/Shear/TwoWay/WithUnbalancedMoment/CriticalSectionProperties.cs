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
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public partial class ConcreteSectionTwoWayShear
    {



        #region Critical section properties


        private List<PerimeterLineSegment> AdjustSegments(Point2D cen)
        {
            List<PerimeterLineSegment> movedSegments = new List<PerimeterLineSegment>();
            foreach (var s in Segments)
            {

                PerimeterLineSegment adjustedSeg = new PerimeterLineSegment(
                    new Point2D(s.PointI.X - cen.X, s.PointI.Y - cen.Y),
                    new Point2D(s.PointJ.X - cen.X, s.PointJ.Y - cen.Y)
                    );
                movedSegments.Add(adjustedSeg);
            }


            if (this.ColumnType == PunchingPerimeterConfiguration.CornerLeftBottom ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerLeftTop ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerRightBottom ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerRightTop
                )
            {
                double J_x_bar = GetJx(movedSegments);
                double J_y_bar = GetJy(movedSegments);
                double J_xy_bar = GetJxy(movedSegments);

                //421.1R-13 Equation (B-10)
                double theta = Math.Atan(-2.0 * J_xy_bar / (J_x_bar - J_y_bar));

                //Calculate rotated coordinates per B-12 and B-13
                List<PerimeterLineSegment> rotatedSegments = new List<PerimeterLineSegment>();
                foreach (var s in movedSegments)
                {
                    PerimeterLineSegment rotatedSeg = new PerimeterLineSegment(
                    new Point2D(s.PointI.X * Math.Cos(theta) + s.PointI.Y * Math.Sin(theta), -s.PointI.X * Math.Sin(theta) + s.PointI.Y * Math.Cos(theta)),
                    new Point2D(s.PointJ.X * Math.Cos(theta) + s.PointJ.Y * Math.Sin(theta), -s.PointJ.X * Math.Sin(theta) + s.PointJ.Y * Math.Cos(theta))
                    );
                    rotatedSegments.Add(rotatedSeg);
                }

                return rotatedSegments;
            }
            else
            {
                return movedSegments;
            }

        }

        public double GetJxy(List<PerimeterLineSegment> segments)
        {
            //421.1R-13 Equation B-11
            double J_xy = d * segments.Sum(s =>
                s.Length / 6.0 * (2.0 * s.PointI.X * s.PointI.Y + s.PointI.X * s.PointJ.Y + s.PointJ.X * s.PointI.Y + 2.0 * s.PointJ.X * s.PointJ.Y)
                );
            return J_xy;
        }

        public double GetJy(List<PerimeterLineSegment> segments)
        {
            //421.1R-13 Equation B-8
            double J_y = d * segments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.X, 2.0) + s.PointI.X * s.PointJ.X + Math.Pow(s.PointJ.X, 2.0))
                );
            return J_y;
        }

        public double GetJx(List<PerimeterLineSegment> segments)
        {
            //421.1R-13 Equation B-9
            double J_x = d * segments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.Y, 2.0) + s.PointI.Y * s.PointJ.Y + Math.Pow(s.PointJ.Y, 2.0))
                );
            return J_x;
        }

        List<PerimeterLineSegment> adjustedSegments;
        public List<PerimeterLineSegment> AdjustedSegments
        {
            get {

                return adjustedSegments;
            }
            }
        private Point2D FindPunchingPerimeterCentroid()
        {
            double SumLs = Segments.Sum(s => s.Length);
            double SumLsTimesY = Segments.Sum(s => s.Centroid.Y * s.Length);
            double SumLsTimesX = Segments.Sum(s => s.Centroid.X * s.Length);

            double X = SumLsTimesX / SumLs;
            double Y = SumLsTimesY / SumLs;

            return new Point2D(X, Y);
        }

        private double Get_b_o()
        {
            double b_o = Segments.Sum(s => s.Length);
            return b_o;
        }
        #endregion


        #region Projection of critical section on principal axis
        private double Get_l_y(List<PerimeterLineSegment> RotatedSegments)
        {
            double YMin = RotatedSegments.Min(s => Math.Min(s.PointI.Y, s.PointJ.Y));
            double YMax = RotatedSegments.Max(s => Math.Max(s.PointI.Y, s.PointJ.Y));
            double ly = YMax - YMin;

            return ly;
        }

        private double Get_l_x(List<PerimeterLineSegment> RotatedSegments)
        {
            double XMin = RotatedSegments.Min(s => Math.Min(s.PointI.X, s.PointJ.X));
            double XMax = RotatedSegments.Max(s => Math.Max(s.PointI.X, s.PointJ.X));
            double lX = XMax - XMin;

            return lX;
        }
        #endregion


        private Point2D _PunchingPerimeterCentroid;

        public Point2D PunchingPerimeterCentroid
        {
            get
            {
                if (_PunchingPerimeterCentroid == null)
                {
                    _PunchingPerimeterCentroid = FindPunchingPerimeterCentroid();
                }
                return _PunchingPerimeterCentroid;
            }
            set { _PunchingPerimeterCentroid = value; }
        }


    }
}

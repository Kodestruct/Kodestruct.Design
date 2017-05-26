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

       

        #region Principal axis
        private List<PerimeterLineSegment> GetRotatedSegments(List<PerimeterLineSegment> adjustedSegments, double thetaRad)
        {

            List<PerimeterLineSegment> RotatedSegments = new List<PerimeterLineSegment>();


            if (ColumnType == PunchingPerimeterConfiguration.CornerLeftBottom ||
                ColumnType == PunchingPerimeterConfiguration.CornerLeftTop ||
                ColumnType == PunchingPerimeterConfiguration.CornerRightBottom ||
                ColumnType == PunchingPerimeterConfiguration.CornerRightTop)
            {


                
                List<PerimeterLineSegment> RotatedSegmentsInLocalAxis = new List<PerimeterLineSegment>();

                foreach (var seg in adjustedSegments)
                {
                    Point2D pi = GetRotatedPoint(seg.PointI, thetaRad);
                    Point2D pj = GetRotatedPoint(seg.PointJ, thetaRad);
                    PerimeterLineSegment newSeg = new PerimeterLineSegment(pi, pj);
                    RotatedSegments.Add(newSeg); //flip X and Y coordinates
                }
                
            }
            else
            {

                RotatedSegments= adjustedSegments;
            }

            return RotatedSegments;
        }

        private Point2D GetRotatedPoint(Point2D point2D, double thetaRad)
        {
            double newX = point2D.X * Math.Cos(thetaRad) + point2D.Y * Math.Sin(thetaRad);
            double newY = -point2D.X * Math.Sin(thetaRad) + point2D.Y * Math.Cos(thetaRad);
            return new Point2D(newX, newY);
        }

        private double Get_thetaRad(double J_xy, double J_x, double J_y)
        {
            //ACI 421R-08 Equation B-10
            double theta = Math.Atan(-2.0 * J_xy / (J_x - J_y)) / 2.0;
            return theta;
        }

        #endregion




    }
}

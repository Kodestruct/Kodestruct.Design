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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;


namespace Kodestruct.Common.Section.SectionTypes
{

    public class SectionThinWall : CompoundShape
    {
        private List<ThinWallSegment> segments;
        private List<ThinWallSegment> SegmentsCentered;


        public List<ThinWallSegment> Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public SectionThinWall(List<ThinWallSegment> Segments): base(null)
        {
            this.Segments = Segments;
            AdjustCoordinates();
        }


        private void AdjustCoordinates()
        {

            //move all rectangles to respect centroid
            Point2D centr = this.CalculateLineCentroidCoordinate();
            SegmentsCentered = this.Segments.Select(s =>
                new ThinWallSegment(new Line2D(
                    new Point2D(s.Line.StartPoint.X-centr.X,s.Line.StartPoint.Y-centr.Y),
                    new Point2D(s.Line.EndPoint.X-centr.X,s.Line.EndPoint.Y-centr.Y)),
                    s.WallThickness)
                ).ToList();

        }

        private Point2D CalculateLineCentroidCoordinate()
        {

            double Xcen = this.Segments.Sum(s =>
            s.Line.Length * s.WallThickness*(s.Line.EndPoint.X + s.Line.StartPoint.X)/2.0);



            double cenArea = this.Segments.Sum(s =>
            s.Line.Length * s.WallThickness

            );

            double cenX = Xcen/cenArea;

            double Ycen = this.Segments.Sum(s =>
            s.Line.Length * s.WallThickness*(s.Line.EndPoint.Y + s.Line.StartPoint.Y)/2.0);


            double cenY = Ycen / cenArea;

            return new Point2D(cenX, cenY);
        }

        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
           
            List<ThinWallSegment> YSegments = new List<ThinWallSegment>();

            List<double> YUnique = GetUniqueYCoordinates(SegmentsCentered);

            
            List<ThinWallSegment> crossingSegments = GetSubdividedYSegments(YUnique, SegmentsCentered);

                List<CompoundShapePart> ProjectedRectangles = GetProjectedYRectangles(crossingSegments);

                List<CompoundShapePart> ConsolidatedRectangles = GetConsolidatedRectangles(ProjectedRectangles);

                return ConsolidatedRectangles;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            List<ThinWallSegment> XSegments = GetRotatedSegments(SegmentsCentered);

            List<double> YUnique = GetUniqueYCoordinates(XSegments);

            List<ThinWallSegment> crossingSegments = GetSubdividedYSegments(YUnique, XSegments);

            List<CompoundShapePart> ProjectedRectangles = GetProjectedYRectangles(crossingSegments);

            List<CompoundShapePart> ConsolidatedRectangles = GetConsolidatedRectangles(ProjectedRectangles);

            return ConsolidatedRectangles;
        }

        private List<ThinWallSegment> GetRotatedSegments(List<ThinWallSegment> segments)
        {
            //Flip X and Y coordinates
            List<ThinWallSegment> newSegs = segments.Select(s =>
                {
                    ThinWallSegment sg = new ThinWallSegment(new Line2D(new Point2D(s.Line.StartPoint.Y, s.Line.StartPoint.X), new Point2D(s.Line.EndPoint.Y, s.Line.EndPoint.X)), s.WallThickness);
                    return sg;
                }).ToList();
            return newSegs;
        }

        #region Y coordinates

        private List<CompoundShapePart> GetConsolidatedRectangles(List<CompoundShapePart> ProjectedRectangles)
        {

            List<CompoundShapePart> consolidatedRectangles = new List<CompoundShapePart>();
            List<double> YUnique = GetUniqueYCoordinates(ProjectedRectangles);

            double YCentroid = 0;

            for (int i = 0; i < YUnique.Count() - 1; i++)
            {
                double height = YUnique[i + 1] - YUnique[i];

                YCentroid = (YUnique[i + 1] + YUnique[i]) / 2.0;

                var combinedWidth = ProjectedRectangles.Where(r =>
                        r.Ymin <= YUnique[i] && r.Ymax >= YUnique[i + 1]

                    ).Sum(r => r.b);
                consolidatedRectangles.Add(new CompoundShapePart(combinedWidth, height, new Point2D(0, YCentroid)));
                
            }

            return consolidatedRectangles;
        }



        private List<CompoundShapePart> GetProjectedYRectangles(List<ThinWallSegment> crossingSegments)
        {
            List<CompoundShapePart> ProjectedSegments = new List<CompoundShapePart>();
            foreach (var s in crossingSegments)
            {
                CompoundShapePart p;
                double dy = s.Line.YMax - s.Line.YMin;
                double dx = s.Line.XMax - s.Line.XMin;
                double L = s.Line.Length;
                if (dy / dx < 0.05)
                {
                    //HORIZONTAL LINE
                    p = new CompoundShapePart(dx, s.WallThickness, new Point2D(0,s.Line.YMax));
                }
                else
                {
                    //VERTICAL LINE
                    if (dx / dy < 0.05)
                    {
                        p = new CompoundShapePart(s.WallThickness, dy, new Point2D(0, (s.Line.YMax + s.Line.YMin)/2.0));
                    }
                    else
                    {
                        //SLOPED LINE
                        double angleFromHorizontal = Math.Atan(dy / dx);
                        p = new CompoundShapePart(dx / Math.Sin(angleFromHorizontal), dy, new Point2D(0, (s.Line.YMax- s.Line.YMin)/2.0));
                    }
                }

                ProjectedSegments.Add(p);
            }
            return ProjectedSegments.OrderBy(r => r.InsertionPoint.Y).ToList();
        }

        private List<ThinWallSegment> GetSubdividedYSegments(List<double> YUnique, List<ThinWallSegment> Segs)
        {

            List<ThinWallSegment> segments = new List<ThinWallSegment>();
            for (int i = 0; i < YUnique.Count() - 1; i++)
            {
                double DeltaY = YUnique[i + 1] - YUnique[i];

                List<ThinWallSegment> crossingSegments = Segs.Where(s =>
                    {
                        if (s.Line.YMax == s.Line.YMin)
	                        {
		                        if (s.Line.YMin ==YUnique[i])
	                            {
		                             return true;
	                            }
                                else
	                            {
                                    return false;
	                            }
	                        }
                        else
                        {
                            if (s.Line.YMin <= YUnique[i] && s.Line.YMax >= YUnique[i + 1])
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                     //s.Line.YMin <= YUnique[i] & s.Line.YMin >= YUnique[i + 1]
                    }
                     
                     ).ToList();

                if (crossingSegments.Count() == 0)
                {
                    //if this segment is a gap
                    segments.Add(new ThinWallSegment(YUnique[i], YUnique[i + 1], 0));
                }
                else
                {
                    //if this segment is NOT a gap
                    foreach (var s in Segs)
                    {
                            Line2D subLine = s.Line.GetSubSegment(YUnique[i], YUnique[i + 1]);
                            ThinWallSegment seg = new ThinWallSegment(subLine, s.WallThickness);
                            segments.Add(seg);
                       
                    }
                }
            }
            return segments;
        }

        private List<double> GetUniqueYCoordinates(List<ThinWallSegment> Segments)
        {
            List<double> uniqueCoordinates = Segments.Select(s => s.Line.YMin).ToList();
            uniqueCoordinates.AddRange(Segments.Select(s => s.Line.YMax).ToList());
            return uniqueCoordinates.Distinct().ToList().OrderBy(d => d).ToList();
        }
        private List<double> GetUniqueYCoordinates(List<CompoundShapePart> ProjectedRectangles)
        {
            List<double> uniqueCoordinates = ProjectedRectangles.Select(s => s.Ymin).ToList();
            uniqueCoordinates.AddRange(ProjectedRectangles.Select(s => s.Ymax).ToList());
            return uniqueCoordinates.Distinct().ToList().OrderBy(d => d).ToList();
        } 
        #endregion



        #region X coordinates


        //private List<CompoundShapePart> GetProjectedXRectangles(List<ThinWallSegment> crossingSegments)
        //{
        //    List<CompoundShapePart> ProjectedSegments = new List<CompoundShapePart>();
        //    foreach (var s in crossingSegments)
        //    {
        //        CompoundShapePart p;
        //        double dy = s.Line.XMax - s.Line.XMin;
        //        double dx = s.Line.XMax - s.Line.XMin;
        //        double L = s.Line.Length;
        //        if (dy / dx < 0.05)
        //        {
        //            p = new CompoundShapePart( s.WallThickness, dx, new Point2D(0, s.Line.XMin - s.WallThickness / 2.0));
        //        }
        //        else
        //        {
        //            if (dx / dy < 0.05)
        //            {
        //                p = new CompoundShapePart(dy, s.WallThickness, new Point2D(0, s.Line.XMin));
        //            }
        //            else
        //            {
        //                //Skewed segment
        //                double angleFromHorizontal = Math.Atan(dy / dx);
        //                p = new CompoundShapePart(dx / Math.Cos(angleFromHorizontal), dy, new Point2D(0, s.Line.XMin));
        //            }
        //        }

        //        ProjectedSegments.Add(p);
        //    }
        //    return ProjectedSegments;
        //}

        //private List<ThinWallSegment> GetSubdividedXSegments(List<double> XUnique)
        //{

        //    List<ThinWallSegment> segments = new List<ThinWallSegment>();
        //    for (int i = 0; i < XUnique.Count() - 1; i++)
        //    {
        //        double DeltaX = XUnique[i + 1] - XUnique[i];

        //        var crossingSegments = Segments.Where(s =>
        //             s.Line.XMin <= XUnique[i] & s.Line.XMin >= XUnique[i + 1]).ToList();

        //        if (crossingSegments.Count() == 0)
        //        {
        //            //if this segment is a gap
        //            segments.Add(new ThinWallSegment(XUnique[i], XUnique[i + 1], 0));
        //        }
        //        else
        //        {
        //            //if this segment is NOT a gap
        //            foreach (var s in Segments)
        //            {
        //                if (s.Line.XMin <= XUnique[i] && s.Line.XMax >= XUnique[i + 1])
        //                {
        //                    Line2D subLine = s.Line.GetSubSegment(XUnique[i], XUnique[i + 1]);
        //                    ThinWallSegment seg = new ThinWallSegment(subLine, s.WallThickness);
        //                    segments.Add(seg);
        //                }
        //            }
        //        }
        //    }
        //    return segments;
        //}

        //private List<double> GetUniqueXCoordinates(List<ThinWallSegment> Segments)
        //{
        //    List<double> uniqueCoordinates = Segments.Select(s => s.Line.XMin).ToList();
        //    uniqueCoordinates.AddRange(Segments.Select(s => s.Line.XMax).ToList());
        //    return uniqueCoordinates.Distinct().ToList().OrderBy(d => d).ToList();
        //}

        #endregion

        protected override void CalculateWarpingConstant()
        {
            _C_w = 0;
        }
    }
}

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

namespace Kodestruct.Common.Section
{
    public class SectionDoubleStacked : CompoundShape
    {
        public SectionDoubleStacked(CompoundShape ShapeTop, CompoundShape ShapeBottom, double DistanceBetweenShapeTops )
        {
            shapeTop =      ShapeTop;
            shapeBottom =   ShapeBottom;
            this.DistanceBetweenShapeTops = DistanceBetweenShapeTops;
        }
        CompoundShape shapeTop;
        CompoundShape shapeBottom;

        double DistanceBetweenShapeTops { get; set; }
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            double YMaxBottom = shapeBottom.YMax;
            double YMaxTop = shapeTop.YMax;
            double YShift = YMaxTop - YMaxBottom - DistanceBetweenShapeTops;
            List<CompoundShapePart> shapeTopShiftedRectangles = (List<CompoundShapePart>)shapeTop.GetCompoundRectangleXAxisList().Select(
                c => { c.InsertionPoint = new Mathematics.Point2D(c.InsertionPoint.X, c.InsertionPoint.Y - YShift); return c; }).ToList();
            var combinedRectanglesA = shapeTopShiftedRectangles.Concat(shapeBottom.GetCompoundRectangleXAxisList());
            List<CompoundShapePart> combinedRectangles = new List<CompoundShapePart>(combinedRectanglesA);
            List<Segment> uniqueSegments = FindUniqueSegments(combinedRectangles);
            List<CompoundShapePart> mergedRectangles = GetMergedRectangles(combinedRectangles, uniqueSegments);
 
            return mergedRectangles;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            //Align centroids
            double centroidUpper = shapeTop.Centroid.X;
            double centroidLower = shapeBottom.Centroid.X;

            double XShift = shapeTop.CentroidYAxisRect - shapeBottom.CentroidYAxisRect;

            List<CompoundShapePart> shapeTopShiftedRectangles = (List<CompoundShapePart>)shapeTop.GetCompoundRectangleYAxisList().Select(
                c => { c.InsertionPoint = new Mathematics.Point2D(c.InsertionPoint.X, c.InsertionPoint.Y - XShift); return c; }).ToList();
            var combinedRectanglesA = shapeTopShiftedRectangles.Concat(shapeBottom.GetCompoundRectangleYAxisList());
            List<CompoundShapePart> combinedRectangles = new List<CompoundShapePart>(combinedRectanglesA);
            List<Segment> uniqueSegments = FindUniqueSegments(combinedRectangles);
            List<CompoundShapePart> mergedRectangles = GetMergedRectangles(combinedRectangles, uniqueSegments);
            return mergedRectangles;
        }

        private List<CompoundShapePart> GetMergedRectangles(List<CompoundShapePart> allRectangles, List<Segment> uniqueSegments)
        {
            List<CompoundShapePart> mergedRectangles = new List<CompoundShapePart>();
            foreach (var seg in uniqueSegments)
            {
                List<CompoundShapePart> thisSegRects = new List<CompoundShapePart>();
                foreach (var r in allRectangles)
                {
                    if (r.Ymax<= seg.YMax && r.Ymin>=seg.YMin) //rectangle entirely a part of this segment
                    {
                        thisSegRects.Add(new CompoundShapePart(r.b, seg.Length, new Mathematics.Point2D(0,(seg.YMax-seg.YMin)/2.0)));
                    }
                    else
                    {
                        if (r.Ymin >=seg.YMax)
                        {
                             // do nothing since segment and rectangle are outside of range
                        }
                        else
                        {
                            if (r.Ymax <= seg.YMax && r.Ymin <= seg.YMin && r.Ymax>=seg.YMin) //rectangle overlaps the segment from bottom
                            {
                                thisSegRects.Add(new CompoundShapePart(r.b, r.Ymax - seg.YMin, new Mathematics.Point2D(0, (r.Ymax - seg.YMin) / 2.0)));
                            }
                            else if (r.Ymax >= seg.YMax && r.Ymin >= seg.YMin && r.Ymin<=seg.YMax)
                            {
                                thisSegRects.Add(new CompoundShapePart(r.b, seg.YMax - r.Ymin, new Mathematics.Point2D(0, (seg.YMax - r.Ymin) / 2.0)));
                            }
                        }

                    }
                    
                }
                double thisSegmentArea = thisSegRects.Sum(r => r.b * r.h);
                double thisSegmentWidth = thisSegmentArea / seg.Length;
                mergedRectangles.Add(new CompoundShapePart(thisSegmentWidth, seg.Length, new Mathematics.Point2D(0, seg.YMin+(seg.YMax - seg.YMin) / 2.0)));
            }
            return mergedRectangles;
        }

        private List<Segment> FindUniqueSegments(List<CompoundShapePart> combinedRectangles)
        {
            List<Segment> segments = new List<Segment>();
            List<double> maxPoints = combinedRectangles.Select(r => r.Ymax).ToList();
            List<double> minPoints = combinedRectangles.Select(r => r.Ymin).ToList();
            var allPoints = maxPoints.Union(minPoints);
            var uniquePoints = allPoints.Distinct().OrderBy(r =>r).ToList();
            for (int i = 0; i < uniquePoints.Count()-1; i++)
            {
                segments.Add(new Segment()
                    {
                        YMin = uniquePoints[i],
                        YMax = uniquePoints[i + 1]
                    });
            }
            return segments;

        }





        protected override void CalculateWarpingConstant()
        {
            throw new NotImplementedException();
        }

        private class Segment
        {
            public double YMin { get; set; }
            public double YMax { get; set; }

            private double length;

            public double Length
            {
                get {
                    length = YMax - YMin;
                    return length; }

            }
            
        }
    }
}

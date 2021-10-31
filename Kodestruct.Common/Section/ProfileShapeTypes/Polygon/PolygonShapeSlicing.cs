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

using ClipperLib;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
 
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
 

namespace Kodestruct.Common.Section.General
{
    public partial class PolygonShape : SectionBase, ISliceableSection, IMoveableSection
    {
        //Generic shape implements ISliceableSection methods
        //which are used for sectional analysis such as strain compatibility 
        //analysis. These methods return a slice of a shape given
        // the location of neutral axis



        //offset dimesions to make sure that the cutting shape is slightly larger than the 
        // shape being cut
        private double offsetX
        {
            get 
            {
                double deltaX = XMax - XMin;
                return deltaX * (0.05); 
            }
        }

        private double XMax_c
        {
            get
            {
                return XMax + offsetX;
            }
        }
        private double XMin_c
        {
            get
            {
                return XMin - offsetX;
            }
        }
        private double offsetY
        {
            get
            {
                double deltaY = YMax - YMin;
                return deltaY * (0.05);
            }
        }

        private double YMax_c
        {
            get
            {
                return YMax + offsetY;
            }
        }
        private double YMin_c
        {
            get
            {
                return YMin - offsetY;
            }
        }


        IMoveableSection GetCutPolygon(List<Point2D> CuttingRectanglePoints)
        {
            #region GPC
            ////Cutting polygon
            //Polygon CuttingPoly = new Polygon();
            //CuttingPoly.AddContour(CuttingRectanglePoints, false);


            ////Original polygon
            //Polygon OriginalPoly = new Polygon();
            //OriginalPoly.AddContour(Vertices, false);

            //Polygon result = OriginalPoly.Clip(GpcOperation.Intersection, CuttingPoly);
            //GenericShape shape = new GenericShape(result);
            //return shape; 
            #endregion

            #region Clipper
            Paths subj = GetPolyPaths(Vertices);
            Paths clip = GetPolyPaths(CuttingRectanglePoints);
	        Paths solution = new Paths();

	        Clipper c = new Clipper();
	        c.AddPaths(subj, PolyType.ptSubject,true);
	        c.AddPaths(clip, PolyType.ptClip,true);
	        c.Execute(ClipType.ctIntersection, solution, 
	        PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

            if (solution.Count == 0)
            {
                return null;
            }
            else
            {
                PolygonShape shape = new PolygonShape(solution,true);
                return shape; 
            }
            
            

            #endregion
        }

        double ScaleFactor = 10000000;

        private List<List<ClipperLib.IntPoint>> GetPolyPaths(List<Point2D> CuttingRectanglePoints)
        {
            //Because clipper library uses integers, scale all the points by a scale  factor
            int NumberOfPoints = CuttingRectanglePoints.Count;
            Path cutPath = new Path(NumberOfPoints);
            foreach (var p in CuttingRectanglePoints)
            {
                cutPath.Add(new IntPoint(Convert.ToInt64(p.X * ScaleFactor), Convert.ToInt64(p.Y * ScaleFactor)));
            }
            Paths subj = new Paths(1);
            subj.Add(cutPath);
            return subj;
        }
        public virtual IMoveableSection GetTopSliceSection(double PlaneOffset, SlicingPlaneOffsetType OffsetType)
        {
            if (PlaneOffset>= this.YMax-YMin)
            {
                return null;
            }
            List<Point2D> rectPoints = null;
            switch (OffsetType)
            {
                case SlicingPlaneOffsetType.Top:
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, YMax-PlaneOffset),
                            new Point2D(XMin_c, YMax_c),
                            new Point2D(XMax_c, YMax_c),
                            new Point2D(XMax_c, YMax-PlaneOffset)
                        };
                    break;
                case SlicingPlaneOffsetType.Centroid:

                    Point2D Centroid = GetElasticCentroidCoordinate();
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, Centroid.Y-PlaneOffset),
                            new Point2D(XMin_c, YMax_c),
                            new Point2D(XMax_c, YMax_c),
                            new Point2D(XMax_c, Centroid.Y-PlaneOffset)
                        };
                    break;
                case SlicingPlaneOffsetType.Bottom:
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, YMin-PlaneOffset),
                            new Point2D(XMin_c, YMax_c),
                            new Point2D(XMax_c, YMax_c),
                            new Point2D(XMax_c, YMin-PlaneOffset)
                        };
                    break;
                default:
                    break;
            }

            return GetCutPolygon(rectPoints);
        }

   

        public virtual IMoveableSection GetBottomSliceSection(double PlaneOffset, SlicingPlaneOffsetType OffsetType)
        {
            if (PlaneOffset >= this.YMax - YMin)
            {
                return null;
            }

            List<Point2D> rectPoints = null;
            switch (OffsetType)
            {
                case SlicingPlaneOffsetType.Top:
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, YMax-PlaneOffset),
                            new Point2D(XMin_c, YMin_c),
                            new Point2D(XMax_c, YMin_c),
                            new Point2D(XMax_c, YMax-PlaneOffset)
                        };
                    break;
                case SlicingPlaneOffsetType.Centroid:

                    Point2D Centroid = GetElasticCentroidCoordinate();
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, Centroid.Y-PlaneOffset),
                            new Point2D(XMin_c, YMin_c),
                            new Point2D(XMax_c, YMin_c),
                            new Point2D(XMax_c, Centroid.Y-PlaneOffset)
                        };
                    break;
                case SlicingPlaneOffsetType.Bottom:
                    rectPoints = new List<Point2D>
                        {
                            new Point2D(XMin_c, YMin-PlaneOffset),
                            new Point2D(XMin_c, YMin_c),
                            new Point2D(XMax_c, YMin_c),
                            new Point2D(XMax_c, YMin-PlaneOffset)
                        };
                    break;
                default:
                    break;
            }

            return GetCutPolygon(rectPoints);
        }

        public virtual IMoveableSection GetTopSliceOfArea(double Area)
        {
            return getSliceOfArea(Area, SLiceType.Top);
        }

        //variables used to store iteration data for finding a slice of a given area;
        double targetArea;
        IMoveableSection cutSection;
        public virtual IMoveableSection GetBottomSliceOfArea(double Area)
        {
            return getSliceOfArea(Area, SLiceType.Bottom);
        }

        enum SLiceType
        {
            Top,
            Bottom
        }

        private IMoveableSection getSliceOfArea(double Area, SLiceType sliceType)
        {
            double ConvergenceTolerance = this.A * 0.0001;
            double targetAreaDelta = 0.0;
            double AxisLocationDistanceMin = 0.0;
            double AxisLocationDistanceMax = this.YMax - this.YMin;

            //Iterate until the slice area is as required
            targetArea = Area;
            if (sliceType == SLiceType.Bottom)
            {

                double SliceAxisOffsetFromTop = RootFinding.Brent(new FunctionOfOneVariable(BottomAreaDeltaCalculationFunction),
                    AxisLocationDistanceMin, AxisLocationDistanceMax,
                    ConvergenceTolerance, targetAreaDelta);
            }
            else
            {
                double SliceAxisOffsetFromTop = RootFinding.Brent(new FunctionOfOneVariable(TopAreaDeltaCalculationFunction),
                    AxisLocationDistanceMin, AxisLocationDistanceMax,
                    ConvergenceTolerance, targetAreaDelta);
            }
            return cutSection; //the section was stored during the iteration
        }

        private double TopAreaDeltaCalculationFunction(double SliceAxisY)
        {
            cutSection = this.GetTopSliceSection(SliceAxisY, SlicingPlaneOffsetType.Top);

            double SliceArea = 0;
            if (cutSection!=null)
            {
                SliceArea = cutSection.A; 
            }
            else
            {
                if (SliceAxisY== this.YMax - this.YMin)
                {
                    SliceArea = this.A;
                }
            }

            return targetArea - SliceArea;
        }

        private double BottomAreaDeltaCalculationFunction(double SliceAxisY)
        {
            cutSection = this.GetBottomSliceSection(SliceAxisY, SlicingPlaneOffsetType.Top);
            
            double SliceArea = 0;
            if (cutSection != null)
            {
                SliceArea = cutSection.A;
            }
            else
            {
                if (SliceAxisY == this.YMax - this.YMin)
                {
                    SliceArea = this.A;
                }
            }

            return targetArea - SliceArea;
        }


        public Point2D GetElasticCentroidCoordinate()
        {

            Point2D centroid = new Point2D(0.0, 0.0 );
            double signedArea = 0.0;
            double x0 = 0.0; // Current vertex X
            double y0 = 0.0; // Current vertex Y
            double x1 = 0.0; // Next vertex X
            double y1 = 0.0; // Next vertex Y
            double a = 0.0;  // Partial signed area

            // For all vertices except last
            int i = 0;
            for (i = 0; i < vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return centroid;
        }

       
    }
}

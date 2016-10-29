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
using MoreLinq;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ClipperLib;

namespace Kodestruct.Common.Section.General
{
    public partial class PolygonShape : SectionBase, ISliceableSection, IMoveableSection
    {
        public PolygonShape(string Name, List<Point2D> Vertices)
            : base(Name)
        {
            //slices = new List<ISectionSlice>();
            this.Vertices = Vertices;

        }
        public PolygonShape(string Name)
            : base(Name)
        {
            //slices = new List<ISectionSlice>();

        }


        public PolygonShape(List<List<IntPoint>> Polygon, bool ScaleDowm)
        {
            
            List<Point2D> newVertices = new List<Point2D>();

            foreach (var pathPoint in Polygon[0])
            {
                Point2D pt = new Point2D(pathPoint.X/ScaleFactor, pathPoint.Y/ScaleFactor);
                newVertices.Add(pt);
            }
            Vertices = newVertices;
        }


        public PolygonShape(List<Point2D> Vertices)
            : base(null)
        {
            this.Vertices = Vertices;

        }
        private List<Point2D> vertices;

        public List<Point2D> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
        private List<Point2D> verticesAdjusted;

        public List<Point2D> VerticesAdjusted
        {
            get {
                if (verticesAdjusted == null)
                {
                    verticesAdjusted = AdjustVertexCoordinatesToElasticCentroid();
                }
                return verticesAdjusted; }

        }

        private List<Point2D> AdjustVertexCoordinatesToElasticCentroid()
        {
            Point2D c = GetElasticCentroidCoordinate();
            var newVertices = Vertices.Select(v =>
                {
                    Point2D vn = new Point2D(v.X -c.X, v.Y - c.Y);
                    return vn;
                }).ToList();
            return newVertices;
        }
        


        public double YMax
        {
            get
            {
                if (Vertices != null)
                {
                    Point2D YmaxPoint = Vertices.MaxBy(v => v.Y);
                    return YmaxPoint.Y;
                }

                return 0.0;
            }
        }

        public double YMin
        {
            get
            {
                if (Vertices != null)
                {
                    Point2D YMinPoint = Vertices.MinBy(v => v.Y);
                    return YMinPoint.Y;
                }
                return 0.0;
            }
        }

        public double XMax
        {
            get
            {
                if (Vertices != null)
                {
                    Point2D XmaxPoint = Vertices.MaxBy(v => v.X);
                    return XmaxPoint.Y;
                }

                return 0.0;
            }
        }

        public double XMin
        {
            get
            {
                if (Vertices != null)
                {
                    Point2D XMinPoint = Vertices.MinBy(v => v.X);
                    return XMinPoint.X;
                }
                return 0.0;
            }
        }


        protected double _x_Bar;

        public override double x_Bar
        {
            get {
                _x_Bar = this.GetElasticCentroidCoordinate().X;
                return _x_Bar; }
        }

        protected double _y_Bar;

        public override double y_Bar
        {
            get
            {
                _y_Bar = this.GetElasticCentroidCoordinate().Y;
                return _y_Bar;
            }
        }

        #region Secton Property Overrides


        private double _A;

        public override double A
        {
            get {
                _A = CalculateArea();
                return _A; }
            set { _A = value; }
        }

        //Polygon area calculation taken from here:
        //http://www.mathopenref.com/coordpolygonarea2.html
        private double CalculateArea()
        {
            double area = 0;                // Accumulates area in the loop
            int j = Vertices.Count - 1;     // The last vertex is the 'previous' one to the first



            for (int i = 0; i < Vertices.Count; i++)
            {
                area = area + (Vertices[j].X + Vertices[i].X) * (Vertices[j].Y - Vertices[i].Y);
                j = i;  //j is previous vertex to i
            }
            return Math.Abs(area / 2.0);
        }


        double _I_x;
        /// <summary>
        /// Generic shape moment of inertia:
        /// Calculate per equation listed here:
        /// http://mathoverflow.net/questions/73556/calculating-moment-of-inertia-in-2d-planar-polygon
        /// </summary>
        public new double I_x
        {
            get
            {
                _I_x = CalculateI_x();
                return _I_x;
            }
        }

        private double CalculateI_x()
        {
            double I_x = 0.0;

            if (VerticesAdjusted.Count > 1.0)
            {


                for (int i = 0; i < VerticesAdjusted.Count; i++)
                {
                    if (i != VerticesAdjusted.Count - 1)
                    {


                        double y_i = VerticesAdjusted[i].Y;
                        double y_ip1 = VerticesAdjusted[i + 1].Y;
                        double x_i = VerticesAdjusted[i].X;
                        double x_ip1 = VerticesAdjusted[i + 1].X;
                        I_x = I_x + GetSegmentContributionToI_x(y_i, y_ip1, x_i, x_ip1);
                    }
                    else
                    {
                        double y_i = VerticesAdjusted[i].Y;
                        double y_ip1 = VerticesAdjusted[0].Y;
                        double x_i = VerticesAdjusted[i].X;
                        double x_ip1 = VerticesAdjusted[0].X;
                        I_x = I_x + GetSegmentContributionToI_x(y_i, y_ip1, x_i, x_ip1);
                    }

                }

                I_x = Math.Abs(I_x / 12.0);
            }
            return I_x;
        }

        private double GetSegmentContributionToI_x(double y_i, double y_ip1, double x_i, double x_ip1)
        {
            //https://en.wikipedia.org/wiki/Second_moment_of_area
            double I_x = (Math.Pow(y_i, 2.0) + y_i * y_ip1 + Math.Pow(y_ip1, 2.0)) * (x_i * y_ip1 - x_ip1 * y_i);
            return I_x;
        }


        double _I_y;
        /// <summary>
        /// Generic shape moment of inertia:
        /// Calculate per equation listed here:
        /// http://mathoverflow.net/questions/73556/calculating-moment-of-inertia-in-2d-planar-polygon
        /// </summary>
        public new double I_y
        {
            get
            {
                _I_y = CalculateI_y();
                return _I_y;
            }
        }

        private double CalculateI_y()
        {
            double I_y = 0.0;

            if (VerticesAdjusted.Count > 1.0)
            {


                for (int i = 0; i < VerticesAdjusted.Count; i++)
                {
                    if (i != VerticesAdjusted.Count - 1)
                    {


                        double y_i = VerticesAdjusted[i].Y;
                        double y_ip1 = VerticesAdjusted[i + 1].Y;
                        double x_i = VerticesAdjusted[i].X;
                        double x_ip1 = VerticesAdjusted[i + 1].X;
                        I_y = I_y + GetSegmentContributionToI_y(y_i, y_ip1, x_i, x_ip1);
                    }
                    else
                    {
                        double y_i = VerticesAdjusted[i].Y;
                        double y_ip1 = VerticesAdjusted[0].Y;
                        double x_i = VerticesAdjusted[i].X;
                        double x_ip1 = VerticesAdjusted[0].X;
                        I_y = I_y + GetSegmentContributionToI_y(y_i, y_ip1, x_i, x_ip1);
                    }

                }

                I_y = Math.Abs(I_y / 12.0);
            }
            return I_y;
        }

        private double GetSegmentContributionToI_y(double y_i, double y_ip1, double x_i, double x_ip1)
        {
            //https://en.wikipedia.org/wiki/Second_moment_of_area
            double I_y = (Math.Pow(x_i, 2.0) + x_i * x_ip1 + Math.Pow(x_ip1, 2.0)) * (x_i * y_ip1 - x_ip1 * y_i);
            return I_y;
        }

        //private double J;

        //public new double MomentOfInertiaTorsional
        //{
        //    get { return J; }
        //    set { J = value; }
        //}

        //private double SxTop;

        //public new double SectionModulusXTop
        //{
        //    get { return SxTop; }
        //    set { SxTop = value; }
        //}

        //private double SxBot;

        //public new double SectionModulusXBot
        //{
        //    get { return SxBot; }
        //    set { SxBot = value; }
        //}

        //private double SyLeft;

        //public new  double SectionModulusYLeft
        //{
        //    get { return SyLeft; }
        //    set { SyLeft = value; }
        //}

        //private double SyRight;

        //public new double SectionModulusYRight
        //{
        //    get { return SyRight; }
        //    set { SyRight = value; }
        //}

        //private double Zx;

        //public new double PlasticSectionModulusX
        //{
        //    get { return Zx; }
        //    set { Zx = value; }
        //}

        //private double Zy;

        //public new double PlasticSectionModulusY
        //{
        //    get { return Zy; }
        //    set { Zy = value; }
        //}

        //private double rx;

        //public new double RadiusOfGyrationX
        //{
        //    get { return rx; }
        //    set { rx = value; }
        //}

        //private double ry;

        //public new double RadiusOfGyrationY
        //{
        //    get { return ry; }
        //    set { ry = value; }
        //}

        //private double xBar;

        //public new double CentroidXtoLeftEdge
        //{
        //    get { return xBar; }
        //    set { xBar = value; }
        //}

        //private double yBar;

        //public new double CentroidYtoBottomEdge
        //{
        //    get { return yBar; }
        //    set { yBar = value; }
        //}

        //private double xpBar;

        //public new double PlasticCentroidXtoLeftEdge
        //{
        //    get { return xpBar; }
        //    set { xpBar = value; }
        //}

        //private double ypBar;

        //public new double PlasticCentroidYtoBottomEdge
        //{
        //    get { return ypBar; }
        //    set { ypBar = value; }
        //}

        //private double Cw;

        //public new double WarpingConstant
        //{
        //    get { return Cw; }
        //    set { Cw = value; }
        //}

        #endregion




    }
}

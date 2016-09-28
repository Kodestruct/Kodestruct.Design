#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
using ClipperLib;

namespace Kodestruct.Common.Section.General
{
    public partial class GenericShape : SectionBase, ISliceableSection, IMoveableSection
    {
        public GenericShape(string Name, List<Point2D> Vertices)
            : base(Name)
        {
            //slices = new List<ISectionSlice>();
            this.Vertices = Vertices;

        }
        public GenericShape(string Name)
            : base(Name)
        {
            //slices = new List<ISectionSlice>();

        }


        public GenericShape(List<List<IntPoint>> Polygon)
        {

            List<Point2D> newVertices = new List<Point2D>();

            foreach (var pathPoint in Polygon[0])
            {
                Point2D pt = new Point2D(pathPoint.X, pathPoint.Y);
                newVertices.Add(pt);
            }
            Vertices = newVertices;
        }


        public GenericShape( List<Point2D> Vertices)
            : base(null)
        {
            this.Vertices = Vertices;

        }
        private List<Point2D>  vertices;

        public List<Point2D>  Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
        
        //private List<ISectionSlice> slices;

        //public List<ISectionSlice> Slices
        //{
        //    get { return slices; }
        //    set { slices = value; }
        //}
        
        //public void AddRectangularSlice(double width, double MinY, double MaxY)
        //{
        //    SectionSliceRectangular slice = new SectionSliceRectangular(width, MinY, MaxY);
        //}


        public double YMax
        {
            get 
            {
                if (Vertices!=null)
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
                    return  XmaxPoint.Y;
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
                    return  XMinPoint.X;
                }
                return 0.0;
            }
        }



        //#region Secton Property Overrides
        //private double A;

        //public new double Area
        //{
        //    get { return A; }
        //    set { A = value; }
        //}

        //private double Ix;

        //public new double MomentOfInertiaX
        //{
        //    get { return Ix; }
        //    set { Ix = value; }
        //}

        //private double Iy;

        //public new double MomentOfInertiaY
        //{
        //    get { return Iy; }
        //    set { Iy = value; }
        //}

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
        
        //#endregion



        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Generic shape moment of inertia:
        /// Calculate per equation listed here:
        /// http://mathoverflow.net/questions/73556/calculating-moment-of-inertia-in-2d-planar-polygon
        /// </summary>
        public double I_x
        {
            get { throw new NotImplementedException(); }
        }
    }
}

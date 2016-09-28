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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Common.Section
{
    /// <summary>
    /// A shape that has double circular flare. This shape is used to calculate double
    /// filleted area properties, for example fillet area of wide flange section
    /// </summary>
    public class PartWithDoubleFillet : CompoundShapePart
        {

        /// <summary>
        ///    Constructor 
        /// </summary>
        /// <param name="FilletSize">Height of circular spandrel (e.g "k" dimension for wide flange I-beams).</param>
        /// <param name="RectangleWidth">Solid rectangle width (e.g web width for wide flange I-beams). </param>
        /// <param name="InsertionPoint"> Insertion point (at wider part). </param>
        /// <param name="IsTopWidened">Defines if wider part is at the top or bottom.</param>
        /// 

        public PartWithDoubleFillet(double FilletSize, double RectangleWidth, Point2D InsertionPoint, bool IsTopWidened)
            {
                this.Size = FilletSize;
                this.InsertionPoint = InsertionPoint;
                isTopWidened = IsTopWidened;
                this.SolidRectangleWidth = RectangleWidth;
            }


        public override double GetHeight()
        {
            double h;
            if (Size>0)
            {
                h = Math.Abs(Centroid.Y - InsertionPoint.Y) * 2;
            }
            else
            {
                h = 0.0;
            }
           
            return h;
        }

        protected override double GetActualHeight()
        {
            return this.Size;
        }

        public override double GetWidth()
        {
            double b;
            if (Size>0)
            {
                double h = Math.Abs(Centroid.Y - InsertionPoint.Y) * 2;
                double A = GetArea();
                b = A / h;  
            }
            else
            {
                b = 0.0;
            }

            return b;
        }

        protected override double GetYmax()
        {
            return this.InsertionPoint.Y;
        }
        protected override double GetYmin()
        {
            return this.InsertionPoint.Y - this.Size;
        }
 
        protected bool isTopWidened;

        public bool IsTopWidened
        {
            get { return isTopWidened; }
        }


        public override double GetMomentOfInertia()
        {
            double IxCircularShapes = 2 * GetCircularSpandrelMomentOfInertia();
                double ybarCir = IsTopWidened ? (InsertionPoint.Y - GetCircularSpandrelCentroid()) - this.Centroid.Y : this.Centroid.Y -(InsertionPoint.Y + GetCircularSpandrelCentroid()) ;
            double IxCircularAreas = 2* GetCircularSpandrelArea() * Math.Pow(ybarCir, 2);
            double IxRectShape = b_rect * Math.Pow(r, 3) / 12.0;
                double ybarRect = IsTopWidened ? Centroid.Y - (InsertionPoint.Y - r / 2) : Centroid.Y - (InsertionPoint.Y + r / 2);
            double IxRectArea = SolidRectangleWidth * r * Math.Pow(ybarRect, 2);
            double Ix = IxCircularShapes + IxCircularAreas + IxRectShape + IxRectArea;
            return Ix;
        }

        //public override double GetMomentOfInertiaY()
        //{

        //    double IyCircularShapes = 2 * GetCircularSpandrelMomentOfInertia();
        //    double xbarCir = GetCircularSpandrelCentroid() + b_rect / 2;
        //    double IyCircularAreas = 2* GetCircularSpandrelArea() * Math.Pow(xbarCir, 2);
        //    double IyRectShape = r * Math.Pow(b_rect, 3) / 12.0;
        //    double Iy = IyCircularShapes + IyCircularAreas + IyRectShape;
        //    return Iy;
        //}

            protected double r;

        /// <summary>
        /// Circlar fillet radius
        /// </summary>
	        public double Size
	        {
		        get { return r;}
		        set { r = value;}
	        }

            protected double b_rect;

            public double SolidRectangleWidth
            {
                get { return b_rect; }
                set { b_rect = value; }
            }
            

        /// <summary>
        /// Area of combinedshape (1 rectangle + 2 quarter circles)
        /// </summary>
        /// <returns></returns>
            public override double GetArea()
            {
                if (Size>0)
                {
                    double A_rect = r * b_rect;
                    double A_circ = GetCircularSpandrelArea();
                    return 2 * A_circ + A_rect;
                }
                else
                {
                    return 0.0;
                }

            }
        protected double GetCircularSpandrelArea()
        {
            double A_circ = (1 - ((Math.PI) / (4))) * Math.Pow(r, 2);
            return A_circ;
        }

        protected double GetCircularSpandrelMomentOfInertia()
        {
            double IxCircularShapeFlat =  (1 - 5.0 / 16.0 * Math.PI) * Math.Pow(r, 4); // this is with respect to flat portion axis
            // to get the Moment of Inertia with respect to centroidal axis of the shape
            double IxCircularShape = IxCircularShapeFlat - (GetCircularSpandrelArea() * Math.Pow(GetCircularSpandrelCentroid(), 2));
            return IxCircularShape;
        }
        /// <summary>
        /// Centoid of the circular part of component
        /// </summary>
        /// <param name="IsTopWide"></param>
        /// <returns></returns>
        protected double GetCircularSpandrelCentroid()
        {
            double yc;
            double y_botWide ;
            y_botWide=(((10-3*Math.PI)*r) / (3*(4-Math.PI)));
            //if (IsTopWidened)
            //    {
            //        yc = r-y_botWide;
            //    }
            //else
            //    {
                    yc = y_botWide;
                //}
            return yc;
        }


        Point2D centroid;

        /// <summary>
        /// Gets centroid of combined component double quarter-circle + rectangle.
        /// </summary>
        /// <returns></returns>
            public override Point2D GetCentroid()
            {
                if (Size >0)
                {
                    if (centroid == null)
                    {
                        double y_c;
                        double y_circ;

                        y_circ = GetCircularSpandrelCentroid();
                        y_c = (2 * GetCircularSpandrelArea() * GetCircularSpandrelCentroid() + b_rect * r * r / 2) / GetArea();

                        if (this.isTopWidened == true)
                        {
                            centroid = new Point2D(InsertionPoint.X, InsertionPoint.Y - y_c);
                        }
                        else
                        {
                            centroid = new Point2D(InsertionPoint.X, InsertionPoint.Y + y_c);
                        }
                    } 
                }
                else
                {
                    centroid = new Point2D(InsertionPoint.X, InsertionPoint.Y);
                }

                return centroid;
            }

        }
}


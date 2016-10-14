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

namespace Kodestruct.Common.Section
{
    /// <summary>
    /// A shape that has a single circular flare. This shape is used to calculate double
    /// filleted area properties, for example fillet area of wide flange section
    /// </summary>
    public class PartWithSingleFillet : PartWithDoubleFillet
    {

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="FilletSize">Height of circular spandrel (e.g "k" dimension for wide flange I-beams).</param>
        /// <param name="RectangleWidth">Solid rectangle width (e.g web width for wide flange I-beams). </param>
        /// <param name="InsertionPoint"> Insertion point (at wider part). </param>
        /// <param name="IsTopWidened">Defines if wider part is at the top or bottom.</param>
        /// 

        public PartWithSingleFillet(double FilletSize, double RectangleWidth, Point2D InsertionPoint, bool IsTopWidened)
            : base(FilletSize, RectangleWidth, InsertionPoint, IsTopWidened)
        {

        }
        public override double GetMomentOfInertia()
        {
            //Contribution of circular segments:
            double IxCircularShapes = GetCircularSpandrelMomentOfInertia();
            double ybarCir = IsTopWidened ? (InsertionPoint.Y - GetCircularSpandrelCentroid()) - this.Centroid.Y : this.Centroid.Y - (InsertionPoint.Y + GetCircularSpandrelCentroid());
            double IxCircularAreas = GetCircularSpandrelArea() * Math.Pow(ybarCir, 2);
            double IxRectShape = SolidRectangleWidth * Math.Pow(r, 3) / 12.0;
            double ybarRect = IsTopWidened ? Centroid.Y - (InsertionPoint.Y - r / 2.0) : Centroid.Y - (InsertionPoint.Y + r / 2.0);
            double IxRectArea = SolidRectangleWidth * r * Math.Pow(ybarRect, 2);
            double Ix = IxCircularShapes + IxCircularAreas + IxRectShape + IxRectArea;
            return Ix;

        }

        //public override double GetMomentOfInertiaY()
        //{
        //    double IyCircularShape =  GetCircularSpandrelMomentOfInertia();
        //    double xbarCir = GetCircularSpandrelCentroid() + Centroid.X;
        //    double IyCircularArea = GetCircularSpandrelArea() * Math.Pow(xbarCir, 2);
        //    double IyRectShape = r * Math.Pow(b_rect, 3) / 12.0;
        //    double xbarRect = Centroid.X - b_rect/2.0;
        //    double IyRectArea = b_rect*r*Math.Pow(xbarRect,2) ;
        //    double Iy = IyCircularShape + IyCircularArea + IyRectShape + IyRectArea;
        //    return Iy;
        //}
        public override double GetArea()
        {
            if (Size>0.0)
            {
                double A_rect = r * b_rect;
                double A_circ = GetCircularSpandrelArea();
                return A_circ + A_rect;  
            }
            else
            {
                return 0.0;
            }

        }
        protected override double GetActualHeight()
        {
            return this.Size;
        }
        Point2D centroid;
        /// <summary>
        /// Gets centroid of combined component quarter-circle + rectangle.
        /// </summary>
        /// <returns></returns>
            public override Point2D GetCentroid()
            {
                if (Size >0)
                {
                    if (centroid == null)
                    {
                        double y_c, x_c;
                        double y_circ;

                        y_circ = GetCircularSpandrelCentroid();
                        y_c = (GetCircularSpandrelArea() * GetCircularSpandrelCentroid() + b_rect * r * r / 2) / GetArea();
                        x_c = (GetCircularSpandrelArea() * GetCircularSpandrelCentroid() + b_rect * r * b_rect / 2) / GetArea();

                        if (this.isTopWidened == true)
                        {
                            centroid = new Point2D(InsertionPoint.X + x_c, InsertionPoint.Y - y_c);
                        }
                        else
                        {
                            centroid = new Point2D(InsertionPoint.X + x_c, InsertionPoint.Y + y_c);
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


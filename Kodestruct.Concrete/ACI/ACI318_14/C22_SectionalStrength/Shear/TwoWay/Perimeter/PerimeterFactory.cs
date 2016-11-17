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
    public class PerimeterFactory
    {

        /// <summary>
        /// Returns the list of lines for calculation of punching shear perimeter properties
        /// </summary>
        /// <param name="Configuration">Configuration of punching perimeter</param>
        /// <param name="c_x">Column dimension parallel to X-axis</param>
        /// <param name="c_y">Column dimension  parallel to Y-axis</param>
        /// <param name="d"> Effective depth of punching shear perimeter</param>
        /// <param name="b_xCant">Slab cantilever extension beyond column face (along axis X)</param>
        /// <param name="b_yCant">Slab cantilever extension beyond column face (along axis Y)</param>
        /// <returns></returns>
        public PunchingPerimeterData GetPerimeterData(PunchingPerimeterConfiguration Configuration, double c_x, double c_y, double d,
        double b_xCant, double b_yCant, Point2D ColumnCenter)
        {
            List<PerimeterLineSegment> PerimeterSegments = GetPerimeterSegments(Configuration, c_x, c_y, d, b_xCant, b_yCant);
            return new PunchingPerimeterData(PerimeterSegments, ColumnCenter);
        }


         List<PerimeterLineSegment> GetPerimeterSegments(PunchingPerimeterConfiguration Configuration, double c_x, double c_y, double d, 
            double b_xCant =0, double b_yCant = 0)
        {
            double b_x; // punching perimeter dimension perpendicular to free edge
            double b_y; // punching perimeter dimension parallel to free edge 
            Point2D p1  =null;
            Point2D p2  =null;
            Point2D p3  =null;
            Point2D p4 = null;


            switch (Configuration)
            {
                case PunchingPerimeterConfiguration.Interior:
                     b_x = c_x +d;
                     b_y = c_y +d;

                    p1 = new Point2D(  -b_x / 2.0, -b_y / 2.0);
                    p2 = new Point2D(  -b_x / 2.0, b_y / 2.0);
                    p3 = new Point2D(  b_x / 2.0, b_y / 2.0);
                    p4 = new Point2D(  b_x / 2.0, -b_y / 2.0);

                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    b_y = c_y +d;

                    p1 = new Point2D(-c_x/2.0-b_xCant, -b_y / 2.0);
                    p2 = new Point2D(-c_x/2.0-b_xCant , b_y / 2.0);
                    p3 = new Point2D(c_x/2.0+d/2.0, b_y / 2.0);
                    p4 = new Point2D(c_x / 2.0+d/2.0, -b_y / 2.0);
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    b_y = c_y +d;

                    p1 = new Point2D(-c_x / 2.0- d/2.0, -b_y / 2.0);
                    p2 = new Point2D(-c_x / 2.0- d/2.0, b_y / 2.0);
                    p3 = new Point2D(c_x / 2.0+ b_xCant, b_y / 2.0);
                    p4 = new Point2D(c_x / 2.0+ b_xCant, -b_y / 2.0);
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    b_x = c_x +d;
                    p1 = new Point2D(-b_x / 2.0, -c_y / 2.0- d/2.0);
                    p2 = new Point2D(-b_x / 2.0, c_y / 2.0+ b_yCant);
                    p3 = new Point2D(b_x / 2.0, c_y / 2.0+ b_yCant);
                    p4 = new Point2D(b_x / 2.0, -c_y / 2.0- d/2.0);
                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    b_x = c_x + d;
                    p1 = new Point2D(-b_x / 2.0, -c_y / 2.0- b_yCant); 
                    p2 = new Point2D(-b_x / 2.0, c_y / 2.0+d/2.0);
                    p3 = new Point2D(b_x / 2.0, c_y / 2.0+d/2.0);
                    p4 = new Point2D(b_x / 2.0, -c_y / 2.0- b_yCant);
                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    p1 = new Point2D(-c_x / 2.0- b_xCant, -c_y / 2.0- d/2.0);
                    p2 = new Point2D(-c_x / 2.0- b_xCant, c_y / 2.0+ b_yCant);
                    p3 = new Point2D(c_x/2.0+d/2.0, c_y / 2.0+ b_yCant);
                    p4 = new Point2D(c_x / 2.0+d/2.0, -c_y / 2.0- d/2.0);
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    p1 = new Point2D(-c_x / 2.0- d/2.0, -c_y / 2.0- d/2.0);
                    p2 = new Point2D(-c_x / 2.0- d/2.0, c_y / 2.0+ b_yCant);
                    p3 = new Point2D(c_x / 2.0+ b_xCant, c_y / 2.0+ b_yCant);
                    p4 = new Point2D(c_x / 2.0 + b_xCant, -c_y / 2.0 - d / 2.0);
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    p1 = new Point2D(-c_x / 2.0- d/2.0, -c_y / 2.0- b_yCant);
                    p2 = new Point2D(-c_x / 2.0- d/2.0, c_y / 2.0+d/2.0);
                    p3 = new Point2D(c_x / 2.0+ b_xCant, c_y / 2.0+d/2.0);
                    p4 = new Point2D(c_x / 2.0+ b_xCant, -c_y / 2.0- b_yCant);
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    p1 = new Point2D(-c_x / 2.0- b_xCant,  -c_y / 2.0- b_yCant);
                    p2 = new Point2D(-c_x / 2.0- b_xCant, +c_y / 2.0+d/2.0);
                    p3 = new Point2D(c_x/2.0+d/2.0, c_y / 2.0+ b_yCant);
                    p4 = new Point2D(c_x / 2.0+d/2.0, -c_y / 2.0- b_yCant);
                    break;
                default:
                    break;
            }
            #region Coordinates obsolete
            //if (Configuration == PunchingPerimeterConfiguration.Interior)
            //{
            //    b_x = c_x +d/2;
            //    b_y = c_y +d/2;

            //    p1 = new Point2D(-b_x / 2.0, -b_y / 2.0);
            //    p2 = new Point2D(-b_x / 2.0, b_y / 2.0);
            //    p3 = new Point2D(b_x / 2.0, b_y / 2.0);
            //    p4 = new Point2D(b_x / 2.0, -b_y / 2.0);
            //}
            //else if (Configuration == PunchingPerimeterConfiguration.EdgeLeft || Configuration == PunchingPerimeterConfiguration.EdgeRight)
            //{
            //    b_x = c_x +d/2 / 2.0 + b_xCant;
            //    b_y = c_y +d/2;

            //    p1 = new Point2D(-b_x / 2.0, -b_y / 2.0);
            //    p2 = new Point2D(-b_x / 2.0, b_y / 2.0);
            //    p3 = new Point2D(b_x / 2.0, b_y / 2.0);
            //    p4 = new Point2D(b_x / 2.0, -b_y / 2.0);
            //}
            //else if (Configuration == PunchingPerimeterConfiguration.EdgeTop || Configuration == PunchingPerimeterConfiguration.EdgeBottom)
            //{
            //    b_x = c_x +d/2;
            //    b_y = c_y +d/2 / 2.0 + b_yCant;
            //}
            //else
            //{
            //    b_x = c_x +d/2 / 2.0;
            //    b_y = c_y +d/2 / 2.0;
            //}


            //p1 = new Point2D(-b_x / 2.0, -b_y / 2.0);
            //p2 = new Point2D(-b_x / 2.0, b_y / 2.0);
            //p3 = new Point2D(b_x / 2.0, b_y / 2.0);
            //p4 = new Point2D(b_x / 2.0, -b_y / 2.0); 
            #endregion

            switch (Configuration)
            {
                case PunchingPerimeterConfiguration.Interior:
                   

                    return new List<PerimeterLineSegment>()
                    {
                        new PerimeterLineSegment(p1,p2 ),
                        new PerimeterLineSegment(p2,p3 ),
                        new PerimeterLineSegment(p3,p4 ),
                        new PerimeterLineSegment(p4,p1 )
                    };

                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:


                    return new List<PerimeterLineSegment>()
                    {
                        new PerimeterLineSegment(p1,p4 ),
                        new PerimeterLineSegment(p4,p3 ),
                        new PerimeterLineSegment(p3,p2 ),
                    };

                case PunchingPerimeterConfiguration.EdgeRight:


                    return new List<PerimeterLineSegment>()
                    {
                        new PerimeterLineSegment(p4,p1 ),
                        new PerimeterLineSegment(p1,p2 ),
                        new PerimeterLineSegment(p2,p3 ),
                    };
                    break;

                case PunchingPerimeterConfiguration.EdgeTop:


                    return new List<PerimeterLineSegment>()
                    {
                        new PerimeterLineSegment(p2,p1 ),
                        new PerimeterLineSegment(p1,p4 ),
                        new PerimeterLineSegment(p4,p3 ),
                    };
                    break;

                case PunchingPerimeterConfiguration.EdgeBottom:


                    return new List<PerimeterLineSegment>()
                    {
                        new PerimeterLineSegment(p1,p2 ),
                        new PerimeterLineSegment(p2,p3 ),
                        new PerimeterLineSegment(p3,p4 ),
                    };
                    break;



                default:
                    throw new Exception("Unrecognized punching perimeter column type");
                    break;

                    throw new Exception("Corner cases were not implemented");
            }

        }
    }
}

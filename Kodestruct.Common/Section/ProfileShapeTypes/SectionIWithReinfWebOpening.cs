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
    /// <summary>
    /// Generic I-shape with geometric parameters provided in a constructor.
    /// This shape has sharp corners, as is typical for built-up shapes.
    /// </summary>
    public class SectionIWithReinfWebOpening : SectionI
    {


        public SectionIWithReinfWebOpening(string Name, double d, double b_f, double t_f, 
            double t_w, double h_o, double e, double b_r, double t_r,bool IsOneSidedReinforcement =false)
            : base( Name,  d,  b_f,  t_f,  t_w)
        {

            this.h_o =h_o ;
            this.e   =e   ;
            this.b_r =b_r ;
            this.t_r = t_r;
            this.IsOneSidedReinforcement = IsOneSidedReinforcement;

        }

        public SectionIWithReinfWebOpening(string Name, double d, double b_fTop, double b_fBot,
            double t_fTop, double t_fBot, double t_w, double h_o, double e, double b_r, double t_r, bool IsOneSidedReinforcement = false)
            : base( Name,  d,  b_fTop,  b_fBot, t_fTop,  t_fBot,  t_w)
        {

        }

            double h_o ;
            double e   ;
            double b_r ;
            double t_r;
            bool IsOneSidedReinforcement;

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// x-axis, each occupying full width of section.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            double t_f = this.t_f;
            double b_f = this.b_fTop;

            CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d - t_f / 2.0));
            CompoundShapePart BottomFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, t_f / 2.0));

            double h_webUpper = (d - 2.0 * t_f)/2.0 -(e+h_o/2.0+t_r);
            double y_WebUpper = d - t_f - h_webUpper / 2.0;
            CompoundShapePart WebUpper = new CompoundShapePart(t_w, h_webUpper, new Point2D(0, y_WebUpper));



            double h_webLower = (d - 2.0 * t_f) / 2.0 - (-e + h_o / 2.0 + t_r);
            double y_WebLower = t_f + h_webLower / 2.0;
            CompoundShapePart WebLower = new CompoundShapePart(t_w, h_webLower, new Point2D(0, y_WebLower));

            double y_ReinfUpper = d - t_f - h_webUpper - t_r / 2.0; ;
            CompoundShapePart ReinfUpper;
            if (IsOneSidedReinforcement == true)
            {
                ReinfUpper = new CompoundShapePart(b_r+t_w, t_r, new Point2D(0, y_ReinfUpper));
            }
            else
            {
                ReinfUpper = new CompoundShapePart(2.0*b_r + t_w, t_r, new Point2D(0, y_ReinfUpper));
            }
            

            double y_ReinfLower = t_f + h_webLower + t_r / 2.0; ;
            CompoundShapePart ReinfLower;

            if (IsOneSidedReinforcement == true)
            {
                ReinfLower = new CompoundShapePart(b_r+t_w, t_r, new Point2D(0, y_ReinfLower));
            }
            else
            {
                ReinfLower = new CompoundShapePart(2.0*b_r + t_w, t_r, new Point2D(0, y_ReinfLower));
            }

            CompoundShapePart Hole = new CompoundShapePart(0, h_o, new Point2D(0, d / 2.0 + e));

            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                 TopFlange,  
                 WebUpper,
                 ReinfUpper,
                 Hole,
                 ReinfLower,
                 WebLower,
                 BottomFlange
            };
            return rectX;
        }

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// y-axis, each occupying full height of section. The rectangles are rotated 90 deg., 
        /// because internally the properties are calculated  with respect to x-axis.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            double FlangeThickness = this.t_f;
            double FlangeWidth = this.b_fTop;

            List<CompoundShapePart> rectY;
            double FlangeOverhang = (b_f - t_w) / 2.0;
            if (b_r > FlangeOverhang)
            {
                if (IsOneSidedReinforcement ==true)
                {
                    CompoundShapePart Segment1 = new CompoundShapePart(2 * t_r, b_r - FlangeOverhang, new Point2D(0, b_f + (b_r - FlangeOverhang)/2.0));
                    CompoundShapePart Segment2 = new CompoundShapePart(2 * t_f+2*t_r, FlangeOverhang, new Point2D(0, b_f-FlangeOverhang / 2.0));
                    CompoundShapePart Web = new CompoundShapePart(d-h_o, t_w, new Point2D(0, b_f / 2.0));
                    CompoundShapePart Segment3 = new CompoundShapePart(2 * t_f, FlangeOverhang, new Point2D(0, FlangeOverhang / 2.0));

                        rectY = new List<CompoundShapePart>()
                            {
                                Segment1,
                                Segment2,
                                Web,
                                Segment3
                            };
                }
                else
                {
                    CompoundShapePart Segment1 = new CompoundShapePart(2 * t_r, b_r - FlangeOverhang, new Point2D(0, b_f + (b_r - FlangeOverhang) + (b_r - FlangeOverhang) / 2.0));
                    CompoundShapePart Segment2 = new CompoundShapePart(2 * t_f + 2 * t_r, FlangeOverhang, new Point2D(0, b_f+ (b_r - FlangeOverhang)- (FlangeOverhang / 2.0)));
                    CompoundShapePart Web = new CompoundShapePart(d - h_o, t_w, new Point2D(0, (b_r - FlangeOverhang)+ b_f / 2.0));
                    CompoundShapePart Segment3 = new CompoundShapePart(2 * t_f + 2 * t_r, b_r - FlangeOverhang, new Point2D(0, (b_r - FlangeOverhang)+(FlangeOverhang / 2.0)));
                    CompoundShapePart Segment4 = new CompoundShapePart(2 * t_r, b_r - FlangeOverhang, new Point2D(0,  (b_r - FlangeOverhang) / 2.0));

                    rectY = new List<CompoundShapePart>()
                            {
                                Segment1,
                                Segment2,
                                Web,
                                Segment3,
                                Segment4
                            };
                }
            }
            else
            {
                if (IsOneSidedReinforcement == true)
                {
                    CompoundShapePart Segment1 = new CompoundShapePart(2 * t_f, FlangeOverhang - b_r, new Point2D(0, b_f - (FlangeOverhang - b_r) / 2.0));
                    CompoundShapePart Segment2 = new CompoundShapePart(2 * t_f + 2 * t_r, FlangeOverhang, new Point2D(0, b_f - (FlangeOverhang - b_r) - (FlangeOverhang - b_r) / 2.0));
                    CompoundShapePart Web = new CompoundShapePart(d - h_o, t_w, new Point2D(0, b_f / 2.0));
                    CompoundShapePart Segment3 = new CompoundShapePart(2 * t_f, FlangeOverhang, new Point2D(0, FlangeOverhang / 2.0));

                    rectY = new List<CompoundShapePart>()
                            {
                                Segment1,
                                Segment2,
                                Web,
                                Segment3
                            };

                }
                else
                {
                    CompoundShapePart Segment1 = new CompoundShapePart(2 * t_f, FlangeOverhang - b_r, new Point2D(0, b_f - (FlangeOverhang - b_r) / 2.0));
                    CompoundShapePart Segment2 = new CompoundShapePart(2 * t_f + 2 * t_r, b_r, new Point2D(0, b_f - (FlangeOverhang - b_r) - ( b_r) / 2.0));
                    CompoundShapePart Web = new CompoundShapePart(d - h_o, t_w, new Point2D(0, b_f / 2.0));
                    CompoundShapePart Segment3 = new CompoundShapePart(2 * t_f+ 2 * t_r, b_r, new Point2D(0, (FlangeOverhang - b_r) + (b_r) / 2.0));
                    CompoundShapePart Segment4 = new CompoundShapePart(2 * t_f, FlangeOverhang - b_r, new Point2D(0, (FlangeOverhang - b_r) / 2.0));

                    rectY = new List<CompoundShapePart>()
                            {
                                Segment1,
                                Segment2,
                                Web,
                                Segment3,
                                Segment4
                            };

                }

            }


            return rectY;
        }

   
    }
}

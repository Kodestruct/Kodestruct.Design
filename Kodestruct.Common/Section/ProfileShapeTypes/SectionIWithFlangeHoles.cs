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

    public class SectionIWithFlangeHoles : SectionI
    {



        /// <summary>
        /// I-Section with flange holes. Use this section for net section flexural check
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="d">Depth</param>
        /// <param name="b_f">Flange width</param>
        /// <param name="t_f">Flange thickness</param>
        /// <param name="t_w">Web thickness</param>
        /// <param name="b_hole">Hole width</param>
        /// <param name="N_holes">Number of holes per flange (only 2 and 4 are allowed) </param>
        /// <param name="IsRolled"></param>
        /// 

        public SectionIWithFlangeHoles(string Name, double d, double b_f, double t_f, 
             double t_w,  double b_hole, double N_holes, 
            bool IsRolled = false)
            : base( Name,  d,  b_f,  t_f,  t_w)
        {

            this.Name =                    Name ; 
            this.d =                       d ; 
            this.b_f =                     b_f ; 
            this.t_f =                     t_f ; 
            this.t_w =                     t_w ; 
            this.b_hole =                  b_hole ; 
            this.N_holes =                 N_holes ;
            this.IsRolled = IsRolled ;
        }

            string Name; 
            double d; 
            double b_f; 
            double t_f; 
            double t_w; 
            public double b_hole  {get; set;}
            public double N_holes {get; set;}
            public bool IsRolled { get; set; }

            /// <summary>
            /// Defines a set of rectangles for analysis with respect to 
            /// x-axis, each occupying full width of section.
            /// </summary>
            /// <returns>List of analysis rectangles</returns>
            public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
            {
                CompoundShapePart TopFlange   =null;
                CompoundShapePart BottomFlange=null;
                if (N_holes==2)
                {
                    TopFlange =     new CompoundShapePart(b_f - 2.0 * b_hole, t_f, new Point2D(0, d - t_f / 2.0));
                    BottomFlange =  new CompoundShapePart(b_f - 2.0 * b_hole, t_f, new Point2D(0, t_f / 2.0));
                }
                else if (N_holes ==4)
                {
                    TopFlange =     new CompoundShapePart(b_f-4.0*b_hole, t_f, new Point2D(0, d - t_f / 2.0));
                    BottomFlange =  new CompoundShapePart(b_f-4.0*b_hole, t_f, new Point2D(0, t_f / 2.0));
                }
                else
                {
                    throw new Exception("Only section with 2 or 4 holes per flange are supported.");
                }

                CompoundShapePart Web = new CompoundShapePart(t_w, d - 2 * t_f, new Point2D(0, d / 2.0));

                List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                 TopFlange,  
                 Web,
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

                // I-shape converted to X-shape 

                List<CompoundShapePart> rectY;

                double FlangeThickness = this.t_f;
                double FlangeWidth = this.b_fTop;


                double overHang = (b_f - t_w) / 2.0;
                

                if (N_holes == 2)
                {
                    double h_tip = overHang / 2.0 - b_hole;
                    CompoundShapePart LeftFlangeTip = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, b_f - h_tip / 2.0));
                    CompoundShapePart LeftHoles = new CompoundShapePart(0, b_hole, new Point2D(0, (b_f - h_tip) - b_hole / 2.0));
                    CompoundShapePart LeftFlange = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (b_f - h_tip - b_hole) - h_tip / 2.0));

                    CompoundShapePart RightFlangeTip = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0,  h_tip / 2.0));
                    CompoundShapePart RightHoles = new CompoundShapePart(0, b_hole, new Point2D(0, ( h_tip) + b_hole / 2.0));
                    CompoundShapePart RightFlange = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (h_tip + b_hole) + h_tip / 2.0));

                    rectY = new List<CompoundShapePart>()
                    {
                        LeftFlangeTip  ,
                        LeftHoles     ,
                        LeftFlange    ,
                        RightFlangeTip ,
                        RightHoles   ,
                        RightFlange   ,

                    };
                
                }
                else if (N_holes == 4)
                {
                    double h_tip = (overHang  - 2.0*b_hole)/3.0;
                    CompoundShapePart LeftFlangeTip = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, b_f - h_tip / 2.0));
                    CompoundShapePart LeftHoles1 = new CompoundShapePart(0, b_hole, new Point2D(0, (b_f - h_tip) - b_hole / 2.0));
                    CompoundShapePart LeftFlange1 = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (b_f - h_tip - b_hole) - h_tip / 2.0));
                    CompoundShapePart LeftHoles2 = new CompoundShapePart(0, b_hole, new Point2D(0, (b_f - 2.0*h_tip-b_hole) - b_hole / 2.0));
                    CompoundShapePart LeftFlange2 = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (b_f - 2.0 * h_tip - 2.0*b_hole) - h_tip / 2.0));


                    CompoundShapePart RightFlangeTip = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, h_tip / 2.0));
                    CompoundShapePart RightHoles1 = new CompoundShapePart(0, b_hole, new Point2D(0, (h_tip) + b_hole / 2.0));
                    CompoundShapePart RightFlange1 = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (h_tip + b_hole) + h_tip / 2.0));
                    CompoundShapePart RightHoles2 = new CompoundShapePart(0, b_hole, new Point2D(0, (2.0 * h_tip + b_hole) + b_hole / 2.0));
                    CompoundShapePart RigthFlange2 = new CompoundShapePart(2 * t_f, h_tip, new Point2D(0, (2.0 * h_tip + 2.0 * b_hole) + h_tip / 2.0));

                    rectY = new List<CompoundShapePart>()
                    {
                        LeftFlangeTip  ,
                        LeftHoles1     ,
                        LeftFlange1    ,
                        LeftHoles2     ,
                        LeftFlange2    ,
                        RightFlangeTip ,
                        RightHoles1    ,
                        RightFlange1   ,
                        RightHoles2    ,
                        RigthFlange2
                    };
                }
                else
                {
                    throw new Exception("Only section with 2 or 4 holes per flange are supported.");
                }

                

                return rectY;
            }



   
    }
}

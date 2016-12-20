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
    /// Plate with holes, used for calculation of net properties of connection element
    /// </summary>
    public class SectionOfPlateWithHoles: SectionRectangular, ISectionRectangular, ISliceableSection //SectionBaseClass,
    {
            /// <summary>
            /// Creates instance of section cut through holes, in order to calculate net properties
            /// </summary>
            /// <param name="Name"></param>
            /// <param name="t_p">Plate thickness</param>
            /// <param name="d_p">Depth (vertical height) of plate</param>
            /// <param name="N_rows">Number of horizontal rows of bolts</param>
            /// <param name="d_hole">Bolt hole diamter</param>
            /// <param name="l_edgeTop">Centerline distance from top row of bolts to plate top edge</param>
            /// <param name="l_edgeBottom">Centerline distance from top row of bolts to plate bottom edge</param>
            /// <param name="Centroid">Plate centroid</param>
        public SectionOfPlateWithHoles(string Name, double t_p, double d_p, double N_rows, double d_hole, double l_edgeTop, double l_edgeBottom, Point2D Centroid)
            : base(Name,t_p,d_p,Centroid)
        {
            this.N_rows= N_rows;
            this.d_hole= d_hole;
            this.t_p= t_p;
            this.d_p= d_p;
            this.l_edgeTop= l_edgeTop;
            this.l_edgeBottom= l_edgeBottom;
        }

            double N_rows;
            double d_hole;
            double t_p;
            double d_p;
            double l_edgeTop;
            double l_edgeBottom;

        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            Point2D cg = Centroid;
            List<CompoundShapePart> rectangles = new List<CompoundShapePart>();
            double s = (d_p - (l_edgeTop + l_edgeBottom)) / (N_rows - 1);
            double a_cTop = l_edgeTop - d_hole / 2.0;
            double a_cBot = l_edgeBottom - d_hole / 2.0;
            double l_clear = s - d_hole;

            CompoundShapePart rb = new CompoundShapePart(t_p, a_cBot, new Point2D(0, cg.Y+a_cBot / 2.0));
            double y_bot = a_cBot;

            rectangles.Add(rb);
            for (int i = 0; i < (N_rows-1); i++)
            {
                CompoundShapePart hole = new CompoundShapePart(0, d_hole, new Point2D(0,cg.Y+ y_bot + d_hole / 2.0));
                y_bot = y_bot + d_hole;
                rectangles.Add(hole);
                CompoundShapePart solid = new CompoundShapePart(t_p, l_clear, new Point2D(0,cg.Y+ y_bot + l_clear / 2.0));
                y_bot = y_bot + l_clear;
                rectangles.Add(solid);
            }
            CompoundShapePart lasthole = new CompoundShapePart(0, d_hole, new Point2D(0, cg.Y+y_bot + d_hole / 2.0));
            y_bot = y_bot + d_hole;
            rectangles.Add(lasthole);

            CompoundShapePart rt = new CompoundShapePart(t_p, a_cTop, new Point2D(0, cg.Y + y_bot+a_cTop / 2.0));
            rectangles.Add(rt);

            return rectangles;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            List<CompoundShapePart> rectangles = new List<CompoundShapePart>();
            rectangles.Add(new CompoundShapePart(B-d_hole*N_rows, H, new Point2D(0, 0)));
            return rectangles;
        }

        protected override void CalculateWarpingConstant()
        {
            _C_w = 0.0;
        }

        protected override void CalculateTorsionalConstant()
        {
            _J = 0;
        }
    }
}

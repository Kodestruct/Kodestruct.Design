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
    public class SectionI : CompoundShape, ISectionI
    {


        public SectionI(string Name, double d, double b_f, double t_f, 
            double t_w)
            : base(Name)
        {
            this._d = d;
            this._b_f = b_f;
            this._t_f = t_f;
            this._b_fTop = b_f;
            this._t_fTop = t_f;
            this._b_fBot = b_f;
            this._t_fBot = t_f;
            this._t_w = t_w;
        }

        public SectionI(string Name, double d, double b_fTop, double b_fBot,
            double t_fTop, double t_fBot, double t_w)
            : base(Name)
        {
            this._d = d;
            this._b_fTop = b_fTop;
            this._t_fTop = t_fTop;
            this._b_fBot = b_fBot;
            this._t_fBot = t_fBot;
            this._t_w = t_w;
        }

        #region Properties specific to I-Beam


        private double _b_f;

        public double b_f
        {
            get { return _b_f; }
            set { _b_f = value; }
        }
        

        private double _d;

        public double d
        {
            get { return _d; }
        }


        private double _h_o;

        public double h_o
        {
            get {
                double df = _d - (this.t_f / 2.0 + this.t_fBot / 2.0);
                return _h_o; }
        }

        private double _b_fTop;

        public double b_fTop
        {
            get { return _b_fTop; }
        }

        private double _t_f;

        public double t_f
        {
            get { return _t_f; }
        }

        private double _b_fBot;

        public double b_fBot
        {
            get { return _b_fBot; }
        }

        private double _t_fBot;

        public double t_fBot
        {
            get { return _t_fBot; }
        }


        private double _t_fTop;

        public double t_fTop
        {
            get { return _t_fTop; }
        }

        private double _t_w;

        public double t_w
        {
            get { return _t_w; }
        }

        //private double filletDistance;

        //public double FilletDistance
        //{
        //    get { return filletDistance; }
        //    set { filletDistance = value; }
        //}



        double _T;
        public double T
        {
            get
            {
                _T = _d - t_fBot - t_f;
                return _T;
            }
        } 
        #endregion


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
            double FlangeThickness = this.t_f;
            double FlangeWidth = this.b_fTop;



            // I-shape converted to X-shape 
            double FlangeOverhang = (b_f - t_w) / 2.0;
            CompoundShapePart LeftFlange = new CompoundShapePart(2* t_f, FlangeOverhang, new Point2D(0, b_f - FlangeOverhang/2.0));
            CompoundShapePart RightFlange = new CompoundShapePart(2*t_f, FlangeOverhang, new Point2D(0, FlangeOverhang/2.0));
            CompoundShapePart Web = new CompoundShapePart(d, t_w, new Point2D(0, b_f / 2.0));

            List<CompoundShapePart> rectY = new List<CompoundShapePart>()
            {
                LeftFlange,
                Web,
                RightFlange
            };
            return rectY;
        }



        protected override void CalculateWarpingConstant()
        {
            //AISC Design Guide 09 Eqn 3.5
            double h = this.d - (this.t_fBot + t_fTop) / 2.0;
            this._C_w = (((I_y * h * h)) / (4.0));

        }

        public double h_web
        {
            get 
            {
                return d - (t_fTop + t_fBot);
            }
        }
    }
}

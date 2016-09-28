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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;


namespace Kodestruct.Common.Section.SectionTypes
{
    /// <summary>
    /// Generic tee shape with geometric parameters provided in a constructor.
    /// </summary>
    public class SectionTee:CompoundShape, ISectionTee
    {
        public SectionTee(string Name, double d, double b_f, double t_f, double t_w, bool IsInverted=false)
            :base(Name)
        {
            this._d = d;
            this._b_f = b_f;
            this._t_f = t_f;
            this._t_w = t_w;
            this.isInverted = IsInverted;
        }

        #region Properties specific to Tees

        public bool isInverted { get; set; }

        private double _d;

        public double d
        {
            get { return _d; }

        }

        private double _b_f;

        public double b_f
        {
            get { return _b_f; }
        }

        private double _t_f;

        public double t_f
        {
            get { return _t_f; }
        }

        private double _t_w;

        public double t_w
        {
            get { return _t_w; }
        }

        private double _T;

        public double StemHeight
        {
            get { return _T; }
            set { _T = value; }
        }
        
        #endregion

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// x-axis, each occupying full width of section.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            List<CompoundShapePart> rectX;
            if (isInverted == false)
            {
                    rectX = new List<CompoundShapePart>()
                    {
                        new CompoundShapePart(b_f,t_f, new Point2D(0,d-t_f/2)),
                        new CompoundShapePart(t_w,d-t_f, new Point2D(0,(d-t_f)/2))
                    };
            }
            else
            {
                 rectX = new List<CompoundShapePart>()
                    {
                        new CompoundShapePart(t_w,d-t_f, new Point2D(0,t_f+(d-t_f)/2)),
                        new CompoundShapePart(b_f,t_f, new Point2D(0,t_f/2))
                    };
            }

            return rectX;
        }

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// y-axis, each occupying full height of section. The rectangles are rotated 90 deg., because internally the properties are calculated  with respect to x-axis.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            double FlangeOverhang = (b_f - t_w) / 2;
            List<CompoundShapePart> rectY = new List<CompoundShapePart>()
            {
                new CompoundShapePart(t_f,FlangeOverhang, new Point2D(0 ,b_f-FlangeOverhang/2)),
                new CompoundShapePart(d,t_w, new Point2D(0,b_f/2)),
                new CompoundShapePart(FlangeOverhang,t_f, new Point2D(0,FlangeOverhang/2)),
            };
            return rectY;
        }


        protected override void CalculateWarpingConstant()
        {
            double d_prime = d - ((t_f) / (2.0));
            _C_w = ((Math.Pow(b_f, 3) * Math.Pow(t_f, 3)) / (144.0)) + ((Math.Pow(d_prime, 3) * Math.Pow(t_w, 3)) / (36.0));
        }
    }
}

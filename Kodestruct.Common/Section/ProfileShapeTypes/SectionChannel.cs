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
    /// Generic channel shape with geometric parameters provided in a constructor.
    /// The corners are assumed to be sharp 90-degree corners, as would be typical 
    /// for a shape built-up from plates.
    /// </summary>
    public class SectionChannel : CompoundShape, ISectionChannel
    {

        public SectionChannel(string Name, double d, double b_f, 
            double t_f, double t_w)
            : base(Name)
        {
            this._d = d;
            this._b_f = b_f;
            this._t_f = t_f;
            this._t_w = t_w;
            IsWeakAxis = false;
            AreFlangeTipsDown = false;
        }
        public SectionChannel(string Name, double d, double b_f,
         double t_f, double t_w, bool IsWeakAxis, bool AreFlangeTipsDown)
            : base(Name)
        {
            this._d = d;
            this._b_f = b_f;
            this._t_f = t_f;
            this._t_w = t_w;
            this.IsWeakAxis = IsWeakAxis;
            this.AreFlangeTipsDown = AreFlangeTipsDown;
        }

        public bool IsWeakAxis { get; set; }
        public bool AreFlangeTipsDown { get; set; }

        #region Section properties specific to channel

        private double _d;

        public double d
        {
            get { return _d; }
        }


        private double _h_o;

        public double h_o
        {
            get
            {
                _h_o = d - t_f / 2.0 - t_f / 2.0;
                return _h_o;
            }

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

        private double flangeClearDistance;

        public double FlangeClearDistance
        {
            get
            {
                flangeClearDistance = d - 2*t_f;
                return flangeClearDistance;
            }
        }


        private double _k;

        public double k
        {
            get { return _k; }
            set { _k = value; }
        }

        
        #endregion

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// x-axis, each occupying full width of section.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            if (IsWeakAxis == false)
            {
                return getXList();
            }
            else 
            {
                return getYList();
            }
        }

        private List<CompoundShapePart> getXList()
        {
            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                new CompoundShapePart(b_f,t_f, new Point2D(0,d/2-t_f/2)),
                new CompoundShapePart(t_w,d-2*t_f, new Point2D(0,0)),
                new CompoundShapePart(b_f,t_f, new Point2D(0,-(d/2-t_f/2)))
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
            List<CompoundShapePart> Ylist;
            if (IsWeakAxis == false)
            {
                Ylist= getYList(); 
            }
            else
            {
                Ylist = getXList(); 
            }
            return Ylist;
        }


        private List<CompoundShapePart> getYList()
        {
            //Converted to TEE
            //Insertion point measured from top
            List<CompoundShapePart> rectY;

            if (AreFlangeTipsDown == true)
            {

                rectY = new List<CompoundShapePart>()
                {
                    new CompoundShapePart(d, t_w, new Point2D(0, -t_w/2.0)),
                    new CompoundShapePart(2*t_f,b_f-t_w, new Point2D(0, -(t_w+(b_f -t_w)/2))),
                };
            }
            else
            {
                rectY = new List<CompoundShapePart>()
                {
                    new CompoundShapePart(2*t_f,b_f-t_w, new Point2D(0, ((b_f -t_w)/2))),
                    new CompoundShapePart(d, t_w, new Point2D(0,(b_f -t_w)+t_w/2.0)),
                    
                };
            }
            return rectY;
        }


        protected override void CalculateWarpingConstant()
        {
            ////AISC Design Guide 09 3.20
            double h = d - t_f;

            ////AISC Design Guide 09 3.21
            double b_prime = b_f - ((t_w) / (2.0));

            ////AISC Design Guide 09 3.19
            double E_o = ((t_f * Math.Pow(b_prime, 2)) / (2.0 * b_prime * t_f + ((h * t_w) / (3.0))));

            ////AISC Design Guide 09 3.18
           _C_w = ((Math.Pow(h, 2) * Math.Pow(b_prime, 2) * t_f * (b_prime - 3.0 * E_o)) / (6.0)) + Math.Pow(E_o, 2) * I_x;

        }
    }
}

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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;


namespace Kodestruct.Common.Section.Predefined
{
    /// <summary>
    /// Predefined C-section is used for channel shapes having known properties 
    /// from catalog (such as AISC shapes)
    /// </summary>
    public class PredefinedSectionChannel : SectionPredefinedBase, ISectionChannel, ISliceableShapeProvider
    {
        AiscCatalogShape s;
        public PredefinedSectionChannel(AiscCatalogShape section)
            : base(section)
        {
            s = section;
            this._d = section.d;
            this._b_f = section.bf;
            this._t_f = section.tf;
            this._t_w = section.tw;
            this._k = section.kdes;
            OverrideCentroids();
        }

        private void OverrideCentroids()
        {
            _x_Bar = s.x_Bar;
            _y_Bar = d / 2;
            _x_pBar = s.x_pBar;
            _y_pBar = d / 2;

            ElasticCentroidCoordinate = new Mathematics.Point2D(x_Bar, y_Bar);
            PlasticCentroidCoordinate = new Mathematics.Point2D(x_pBar, y_pBar);
        }

        public ISliceableSection GetSliceableShape()
        {
            SectionChannelRolled secI = new SectionChannelRolled("", this.d, this.b_f, this.t_f, this.t_w,this.k);
            return secI;
        }

        double _d;

        public double d
        {
            get { return _d; }
        }
        double _h_o;

        public double h_o
        {
            get { return _h_o; }
        }
        double flangeClearDistance;

        public double FlangeClearDistance
        {
            get { return flangeClearDistance; }
        }
        double _t_f;

        public double t_f
        {
            get { return _t_f; }
        }
        double _b_f;

        public double b_f
        {
            get { return _b_f; }
        }
        double _t_w;

        public double t_w
        {
            get { return _t_w; }
            set { _t_w = value; }
        }
        double _k;

        public double k
        {
            get { return _k; }
            set { _k = value; }
        }


        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

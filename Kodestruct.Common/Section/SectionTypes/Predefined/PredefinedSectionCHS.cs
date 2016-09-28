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
    /// Predefined circular HSS (pipe) is used for circular hollow sections (CHS) having known properties 
    /// from catalog (such as AISC shapes)
    /// </summary>
    public class PredefinedSectionCHS : SectionPredefinedBase, ISectionPipe
    {
        public PredefinedSectionCHS(AiscCatalogShape section)
            : base(section)
        {
            this._D = section.OD;
            this._t = section.tnom;
            this._t_des = section.tdes;
            OverrideCentroids();
        }

        private void OverrideCentroids()
        {
            _x_Bar = D / 2;
            _y_Bar = D / 2;
            _x_pBar = D / 2;
            _y_pBar = D / 2;


            ElasticCentroidCoordinate = new Mathematics.Point2D(x_Bar, y_Bar);
            PlasticCentroidCoordinate = new Mathematics.Point2D(x_pBar, y_pBar);
        }
        public PredefinedSectionCHS(
        double D,
        double t,ISection section): base(section)
        {
            this._D = D;
            this._t=t;
            this._t_des = t_des;
        }


        //public ISliceableSection GetSliceableShape()
        //{
        //    SectionPipe secI = new SectionPipe("",this.D,this.t_des);
        //    return secI;
        //}

        double _D;
        public double D
        {
            get { return _D; }
        }

        double _t;
        public double t
        {
            get { return _t; }
        }

        double _t_des;
        public double t_des
        {
            get { return _t_des; }
        }

        public ISection GetWeakAxisClone()
        {
            throw new NotImplementedException();
        }

        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;


namespace Kodestruct.Common.Section.Predefined
{
    /// <summary>
    /// Predefined  rectangular HSS section is used for rectangular or square hollow sections (RHS) having known properties 
    /// from catalog (such as AISC shapes)
    /// </summary>
    public class PredefinedSectionRHS : SectionPredefinedBase, ISectionTube, ISliceableShapeProvider
    {
        public PredefinedSectionRHS(AiscCatalogShape section)
            : base(section)
        {
            this._B = section.B;
            this._H = section.H;
            this._t_des = section.tdes;
            this._t_nom = section.tnom;
            OverrideCentroids();
        }
        //public PredefinedSectionRHS(double B, double H, double CornerRadiusOutside, ISection section)
        //    : base(section)
        //{
        //    this._B=B;
        //    this._H=H;
        //    this.cornerRadiusOutside=CornerRadiusOutside;

        //}
        private void OverrideCentroids()
        {
           _x_Bar = B / 2;
           _y_Bar = H / 2;
           _x_pBar = B / 2;
           _y_pBar = H / 2;


            ElasticCentroidCoordinate = new Mathematics.Point2D(x_Bar, y_Bar);
            PlasticCentroidCoordinate = new Mathematics.Point2D(x_pBar, y_pBar);
        }

        public ISliceableSection GetSliceableShape()
        {
            SectionTube secI = new SectionTube("", this.H, this.B, this.t_nom, this.t_des, CornerRadiusOutside);
            return secI;
        }


        double _B;

        public double B
        {
            get { return _B; }
        }
        double _H;

        public double H
        {
            get { return _H; }
        }



        public ISection GetWeakAxisClone()
        {
            throw new NotImplementedException();
        }

        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}


        private double _t_nom;

        public double t_nom
        {
            get { return _t_nom; }
        }
 


        private double _t_des;

        public double t_des
        {
            get { return _t_des; }
        }

        double cornerRadiusOutside;

        double CornerRadiusOutside
        {
            get
            {
                return cornerRadiusOutside;
            }
            set
            {
                cornerRadiusOutside = value;
            }
        }


        double ISectionTube.CornerRadiusOutside
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

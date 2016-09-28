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
    /// Predefined L-section is used for single angles having known properties 
    /// from catalog (such as AISC shapes)
    /// </summary>
    public class PredefinedSectionAngle : SectionPredefinedBase, ISectionAngle, ISliceableShapeProvider
    {

        public PredefinedSectionAngle(AiscCatalogShape section, AngleOrientation AngleOrientation, AngleRotation AngleRotation)
            : base(section)
        {
            //this._d = section.d;
            //this._b = section.b;
            Set_b_and_d(section.d, section.b, AngleOrientation);
            this._t = section.t;
            this.AngleOrientation = AngleOrientation;
            this.AngleRotation = AngleRotation;
 
        }

        private void Set_b_and_d(double d, double b, AngleOrientation AngleOrientation)
        {
            double LongLeg = b >= d ? b : d;
            double ShortLeg = b < d ? b : d;

            if (AngleOrientation == Common.AngleOrientation.LongLegVertical)
            {
                this._d = LongLeg;
                this._b = ShortLeg;
            }
            else
            {
                this._d = ShortLeg;
                this._b = LongLeg;
            }
        }



        public PredefinedSectionAngle(
                                    double Height,
                                    double Thickness,
                                    double Width,
                                    double MomentOfInertiaPrincipalMajor,
                                    double MomentOfInertiaPrincipalMinor,
                                    double SectionModulusPrincipalMajor,
                                    double SectionModulusPrincipalMinor,
                                    double RadiusOfGyrationPrincipalMajor,
                                    double RadiusOfGyrationPrincipalMinor, ISection section)
            : base(section)
        {
            this._d                         =Height                         ;
            this._t                      =Thickness                      ;
            this._b                          =Width                          ;
            this._I_w  =MomentOfInertiaPrincipalMajor  ;
            this._I_z  =MomentOfInertiaPrincipalMinor  ;
            this._S_w   =SectionModulusPrincipalMajor   ;
            this._S_z   =SectionModulusPrincipalMinor   ;
            this._r_w =RadiusOfGyrationPrincipalMajor ;
            this._r_z =RadiusOfGyrationPrincipalMinor ;
        }

        double _d;

        public double d
        {
            get { return _d; }
            set { _d = value; }
        }
        double _t;

        public double t
        {
            get { return _t; }
        }
        double _b;

        public double b
        {
            get { return _b; }
        }

        double _I_w;

        public double I_w
        {
            get { return _I_w; }
        }
        double _I_z;

        public double I_z
        {
            get { return _I_z; }
        }

        double _S_w;

        public double S_w
        {
            get { return _S_w; }
        }
        double _S_z;

        public double S_z
        {
            get { return _S_z; }
        }

        double _r_w;

        public double r_w
        {
            get { return _r_w; }
        }
        double _r_z;

        public double r_z
        {
            get { return _r_z; }
        }


        public ISection GetWeakAxisClone()
        {
            throw new NotImplementedException();
        }

        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}

        double _Angle_alpha;
        public double Angle_alpha
        {
            get
            {
                return _Angle_alpha;
            }

        }


        double _beta_w;
        public double beta_w
        {
            get
            {
                return _beta_w;
            }
        }

        private AngleOrientation angleOrientation;

        public AngleOrientation AngleOrientation
        {
            get { return angleOrientation; }
            set { angleOrientation = value; }
        }

        private AngleRotation angleRotation;

        public AngleRotation AngleRotation
        {
            get { return angleRotation; }
            set { angleRotation = value; }
        }

        public ISliceableSection GetSliceableShape()
        {
            SectionAngle L = new SectionAngle("", this.d, this.b, this.t, this.AngleRotation, this.AngleOrientation);
            return L;
        }
    }
}

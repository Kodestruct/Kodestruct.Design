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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Common;





namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberDoubleAngleBase : FlexuralMember
    {

        public ISectionAngle Angle { get; set; }
        AngleOrientation AngleOrientation { get; set; }

        public FlexuralMemberDoubleAngleBase(ISteelSection section, ICalcLog CalcLog, AngleOrientation AngleOrientation)
            : base(section, CalcLog)
        {
            sectionDoubleAngle = null;
            ISectionDoubleAngle s = Section as ISectionDoubleAngle;
            this.AngleOrientation = AngleOrientation;

            if (s == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionDoubleAngle));
            }
            else
            {
                sectionDoubleAngle = s;
                Angle = s.Angle;
                compactness = new ShapeCompactness.AngleMember(Angle, section.Material, AngleOrientation);
            }
        }


        ShapeCompactness.AngleMember compactness;

        private ISectionDoubleAngle sectionDoubleAngle;

        public ISectionDoubleAngle ISectionDoubleAngle
        {
            get { return sectionDoubleAngle; }
            set { sectionDoubleAngle = value; }
        }

        protected virtual CompactnessClassFlexure GetFlangeCompactnessClass()
        {
            if (Angle!=null)
            {
                //if (Angle.AngleOrientation== LongLegVertical)
                //{
                //return compactness.HorizontalLegCompactness.GetCompactnessFlexure();
                //}
                //else
                //{
                //return compactness.VerticalLegCompactness.GetCompactnessFlexure();
                //}
            }
            throw new NotImplementedException();
        }

        public virtual CompactnessClassFlexure GetStemCompactnessClass()
        {
           // return compactness.GetWebCompactnessFlexure();
            throw new NotImplementedException();
        }

        protected virtual double GetLambdaStem()
        {
            if (sectionDoubleAngle != null)
            {
                //double lambdaStem = compactness.GetWebLambda();
                //return lambdaStem;
                throw new NotImplementedException();
            }
            else
            {
                throw new SectionNullException(typeof(ISectionTee));
            }
        }

        protected virtual double GetLambdaFlange()
        {
            if (sectionDoubleAngle != null)
            {
                //double lambdaFlange = compactness.GetCompressionFlangeLambda();
                //return lambdaFlange;
                throw new NotImplementedException();
            }
            else
            {
                throw new SectionNullException(typeof(ISectionTee));
            }
        }

        protected virtual double GetLambdapf(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            //return compactness.GetFlangeLambda_p(StressType.Flexure);
            throw new NotImplementedException();
        }

        protected virtual double GetLambdarf(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            //return compactness.GetFlangeLambda_r(StressType.Flexure);
            throw new NotImplementedException();
        }
    }
}

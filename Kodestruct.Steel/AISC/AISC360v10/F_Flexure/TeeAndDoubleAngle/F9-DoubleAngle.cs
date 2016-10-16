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
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Common;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamDoubleAngle : FlexuralMemberDoubleAngleBase
    {
        public BeamDoubleAngle(ISteelSection section, ICalcLog CalcLog, AngleRotation AngleRotation, AngleOrientation AngleOrientation)
            : base(section, CalcLog, AngleOrientation)
        {

            if (section is ISectionDoubleAngle)
            {

                    SectionDoubleAngle = section as ISectionDoubleAngle;


            }
            else
            {
                throw new SectionWrongTypeException(typeof(ISectionTube));
            }
            this.AngleOrientation = AngleOrientation;
            this.AngleRotation = AngleRotation;

            GetSectionValues();
        }

        ISectionTee SectionTee;
        ISectionDoubleAngle SectionDoubleAngle;
        AngleRotation AngleRotation;
        AngleOrientation AngleOrientation;



        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;

        }


        double E;
        double Fy;



    }
}

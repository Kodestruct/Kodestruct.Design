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
using Kodestruct.Common.Exceptions;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberRhsBase : FlexuralMember
    {
        public FlexuralMemberRhsBase(ISteelSection section, ICalcLog CalcLog)
            : base(section,  CalcLog)
        {
            //sectionTube = null;

            if (Section.Shape is ISectionTube || Section.Shape is ISectionBox)
            {
                this.Section = section;
                if (Section.Shape is ISectionTube)
                {
                    ShapeTube = Section.Shape as ISectionTube;
                }
                else
                {
                    ShapeBox = Section.Shape as ISectionBox;
                }
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" flexural calculation of box or rectangular HSS beam");
            }

        }

        private ISectionTube sectionTube;

        public ISectionTube ShapeTube
        {
            get { return sectionTube; }
            set { sectionTube = value; }
        }


        private ISectionBox shapeBox;

        public ISectionBox ShapeBox
        {
            get { return shapeBox; }
            set { shapeBox = value; }
        }
        
    }
}

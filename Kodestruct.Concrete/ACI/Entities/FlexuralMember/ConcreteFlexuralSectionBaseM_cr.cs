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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;



namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteFlexuralSectionBase : ConcreteSectionLongitudinalReinforcedBase, IConcreteFlexuralMember
    {
        public double GetCrackingMoment( FlexuralCompressionFiberPosition compFiberPosition)
        {
            double f_r = this.Section.Material.ModulusOfRupture;
            //24.2.3.5b

            if (compFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                return this.Section.SliceableShape.S_xBot * f_r;
            }
            else
            {
                return Section.SliceableShape.S_xTop * f_r;
            }
        }


    }
}

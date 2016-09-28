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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;

 

namespace  Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public class AffectedElementInFlexureAndShear: AffectedElementInShear //, ISteelFlexuralMember
    {

        public AffectedElementInFlexureAndShear(ISteelSection Section,  ICalcLog CalcLog)
            : base(Section, CalcLog)
        {

        }

        public AffectedElementInFlexureAndShear(ISection Section, ISteelMaterial Material, ICalcLog CalcLog)
            :base(Section,Material, CalcLog)
        {

        }

       #region ISteelFlexuralMember Members

       public double UnbracedLengthFlexure
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

       public double EffectiveLengthFactorFlexure
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

       public double GetFlexuralCapacityMajorAxis( FlexuralCompressionFiberPosition compressionFiberLocation)
       {
           throw new NotImplementedException();
       }

       public double GetFlexuralCapacityMinorAxis( FlexuralCompressionFiberPosition compressionFiberLocation)
       {
           throw new NotImplementedException();
       }

       #endregion
    }
}

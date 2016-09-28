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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    /// <summary>
    ///  This class encpsulates  flexural analysis per ACI.
    /// </summary>
    public partial class ConcreteSectionFlexure: ConcreteFlexuralSectionBase
    {

        /// <summary>
        ///  Constructor used for flexure and axial load analysis.
        /// </summary>
        public ConcreteSectionFlexure(IConcreteSection Section,
            List<RebarPoint> LongitudinalBars, ICalcLog log)
            : base(Section, LongitudinalBars, log)
        {
            
        }




    }
}

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
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class ReinforcedConcreteBeamNonprestressed
    {
        public ReinforcedConcreteBeamNonprestressed(IConcreteSection Section, ConfinementReinforcementType ConfinementReinforcementType, ICalcLog log)
        {
            this.ConcreteSection = Section;
            this.Log = log;
            this.ConfinementReinforcementType = ConfinementReinforcementType;
        }


       ConfinementReinforcementType ConfinementReinforcementType {get; set;}
        private IConcreteSection concreteSection;

        public IConcreteSection ConcreteSection
        {
            get { return concreteSection; }
            set { concreteSection = value; }
        }


        private ICalcLog log;

        public ICalcLog Log
        {
            get { return log; }
            set { log = value; }
        }

        public ConcreteFlexuralStrengthResult GetFlexuralDesignStrength(List<RebarPoint> LongitudinalBars, FlexuralCompressionFiberPosition CompressionFiber, ConfinementReinforcementType ConfinementReinforcementType)
        {
            ConcreteSectionFlexure s = new ConcreteSectionFlexure(this.ConcreteSection, LongitudinalBars, Log,ConfinementReinforcementType );
            return s.GetDesignFlexuralStrength(CompressionFiber);
        }


    }
}

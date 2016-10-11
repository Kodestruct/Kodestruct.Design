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

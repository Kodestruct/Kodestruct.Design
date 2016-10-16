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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public class PrestressedConcreteSection : PrestressedBeamSectionBase
    {

        public PrestressedConcreteSection(IPrestressedConcreteSection Section, List<RebarPoint> LongitudinalBars,
            CrossSectionLocationType LocationType, MemberClass MemberClass, ICalcLog log, ConfinementReinforcementType ConfinementReinforcementType)
            : base(Section,LongitudinalBars, log, ConfinementReinforcementType)
        {
            this.crossSectionLocationType = LocationType;
            this.memberClass = MemberClass;
            this.prestressedSection = Section;

        }

        IPrestressedConcreteSection prestressedSection;

        private CrossSectionLocationType crossSectionLocationType;

        public CrossSectionLocationType CrossSectionLocationType
        {
            get { return crossSectionLocationType; }
            set { crossSectionLocationType = value; }
        }

        private MemberClass memberClass;

        public MemberClass MemberClass
        {
            get { return memberClass; }
            set { memberClass = value; }
        }
        

        protected override double GetAllowableTension(StageType Stage)
        {
            double fc = this.Section.Material.SpecifiedCompressiveStrength;
            double fci = this.prestressedSection.Material.InitialConcreteStrengthAtPrestress;
            double fct = 0;
            double sqrt_fc = Section.Material.Sqrt_f_c_prime;
            double sqrt_fci = Math.Sqrt(fci);
          
            switch (Stage)
            {
                case StageType.Jacking:
                    throw new InvalidStageException("allowable concrete tension");
                case StageType.Transfer:
                    switch (crossSectionLocationType)
	                    {
                            case CrossSectionLocationType.TypicalInterior:
                                fct= 3.0 * sqrt_fci;
                                break;
                            case CrossSectionLocationType.EndOfSImplySupported:
                                fct= 6.0 * sqrt_fci;
                                break;

	                    }
                    break;
                case StageType.Service:
                    switch (memberClass)
	                    {
                            case MemberClass.U:
                                fct = 7.5 * sqrt_fc;
                                break;
                            case MemberClass.T:
                                fct= 12.0 * sqrt_fc;
                                break;
                            case MemberClass.C:
                                fct= double.PositiveInfinity;
                                break;
                            case MemberClass.U_TwoWaySlab:
                                fct= 6.0* sqrt_fc;
                                break;
	                    }
                    break;

            }
            return fct;
        }

        protected override double GetAllowableCompression(StageType Stage, LoadType LoadType)
        {
            double fc = this.Section.Material.SpecifiedCompressiveStrength;
            double fci = this.prestressedSection.Material.InitialConcreteStrengthAtPrestress;
            double fcc = 0;
            switch (Stage)
            {
                case StageType.Jacking:
                    throw new InvalidStageException("allowable concrete compression");
                case StageType.Transfer:
                    switch (crossSectionLocationType)
	                    {
                        case CrossSectionLocationType.TypicalInterior:
                                fcc = 0.6 * fci;
                            break;
                        case CrossSectionLocationType.EndOfSImplySupported:
                                fcc = 0.7 * fci;
                            break;
	                    }
                    break;
                case StageType.Service:
                    switch (LoadType)
	                    {
                            case LoadType.Sustained:
                                fcc = 0.45 * fc;
                                break;
                            case LoadType.Total:
                                fcc = 0.6 * fc;
                                break;
	                    }
                    break;

            }
            return fcc;
        }


    }
}

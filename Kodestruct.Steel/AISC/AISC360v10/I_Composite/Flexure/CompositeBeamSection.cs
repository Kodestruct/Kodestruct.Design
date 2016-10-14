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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Composite
{
    public partial class CompositeBeamSection: AnalyticalElement
    {
        public CompositeBeamSection()
        {
            ICalcLog log = new CalcLog();
            this.Log = log;
        }

        public CompositeBeamSection(ICalcLog log): base(log)
        {

        }
        public CompositeBeamSection(ISliceableSection SteelSection, double SlabEffectiveWidth,
            double SlabSolidThickness, double SlabDeckThickness, double F_y, double f_cPrime)
        {
            this.SteelSection =       SteelSection ;
            this.SlabEffectiveWidth = SlabEffectiveWidth ;
            this.SlabSolidThickness = SlabSolidThickness ;
            this.SlabDeckThickness =  SlabDeckThickness ;
            this.SumQ_n =             SumQ_n ;
            this.F_y =                F_y ;
            this.f_cPrime =           f_cPrime ;
        }

        public ISliceableSection SteelSection { get; set; }
        public double SlabEffectiveWidth { get; set; }
        public double SlabSolidThickness { get; set; }
        public double SlabDeckThickness { get; set; }
        public double SumQ_n { get; set; }
        public double C_Slab { get; set; }
        public double F_y { get; set; }
        public double f_cPrime { get; set; }
    }
}

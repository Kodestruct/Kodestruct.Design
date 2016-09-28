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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Reports; 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Concrete.ACI318_14
{

 
    public partial class DevelopmentCompression:Development
    {
        public DevelopmentCompression(IConcreteMaterial Concrete, Rebar Rebar, 
        ICalcLog log, bool IsConfinedCompressionRebar, double ExcessReinforcementRatio = 1.0)
            : base(Concrete, Rebar, ExcessReinforcementRatio, log)
        {
            this.IsConfinedCompressionRebar = IsConfinedCompressionRebar;
        }

        //12.3.3 (b) For reinforcement enclosed within spiral reinforcement
        //not less than 1/4 in. diameter and not more
        //than 4 in. pitch or within No. 4 ties in conformance
        //with 7.10.5 and spaced at not more than 4 in. on center
        //length ldc shall be permitted to be
        //multiplied by the factor 0.75

        private bool isConfinedCompressionRebar;

        public bool IsConfinedCompressionRebar
        {
            get { return isConfinedCompressionRebar; }
            set { isConfinedCompressionRebar = value; }
        }


    }
}

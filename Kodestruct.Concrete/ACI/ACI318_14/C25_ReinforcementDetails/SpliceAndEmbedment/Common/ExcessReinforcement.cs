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
using Kodestruct.Common.Reports; 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class Development : AnalyticalElement, IDevelopment
    {
        //25.4.10.1 Reduction of development lengths defned 
        //in 25.4.2.1(a), 25.4.3.1(a), 25.4.6.1(a), 25.4.7.1(a), and 
        //25.4.9.1(a) shall be permitted by use of the ratio (As,required)/
        //(As,provided), except where prohibited by 25.4.10.2. The modifed
        //development lengths shall not be less than the respective 
        //minimums specifed in 25.4.2.1(b), 25.4.3.1(b), 25.4.3.1(c), 
        //25.4.6.1(b), 25.4.7.1(b), and 25.4.9.1(b).

           
        public double CheckExcessReinforcement(double ld, bool IsTensionReinforcement, bool IsHook)
        {
            if (ExcessFlexureReinforcementRatio <= 1.0)
            {
                if (ExcessFlexureReinforcementRatio < 1.0)
                {
                    double AsReqdToAsProvided = excessFlexureReinforcementRatio;

                    if (IsTensionReinforcement!=true)
                    {
                            throw new Exception("Hooks should not be used to develop rebar in compression");

                    }
                    ld = excessFlexureReinforcementRatio * ld;
                }
            }
            else
            {
                throw new Exception("Exceess reinforcement ratio cannot be more than 1.0");

            }
            return ld;
        }
    }
}

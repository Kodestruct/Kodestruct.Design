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
 
using Kodestruct.Concrete.ACI318_14.Anchorage.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI318_14.Anchorage.LimitStates
{
    public class Bond : AnchorageConcreteLimitState
    {

        public Bond (int n, double h_eff, bool IsHookedBolt, double d_a, double e_h, double A_brg, AnchorInstallationType AnchorType,
            AnchorReliabilityAndSensitivityCategory AnchorCategory, ConcreteCrackingCondition ConcreteCondition
            )
            : base(n, h_eff, AnchorType)
            {
                //this.IsHookedBolt = IsHookedBolt;
                //this.d_a = d_a;
                //this.e_h = e_h;
                //this.A_brg = A_brg;
                //this.AnchorType = AnchorType;
                //this.AnchorCategory = AnchorCategory;
                //this.ConcreteCondition = ConcreteCondition;
            }


        public override double GetNominalStrength()
        {
            throw new NotImplementedException();

        }
    }
}

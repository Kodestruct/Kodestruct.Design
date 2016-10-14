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

namespace Kodestruct.Loads.ASCE7.ASCE7_10.Combinations
{
    public class CombinedEffects
    {
        public CombinationResult GetMaximumCombination(List<LoadCombination> Combinations)
        {
            double MaxVal = double.NegativeInfinity;
            double MinVal = double.PositiveInfinity;
            double MaxAbsVal = 0;
            CombinationResult ComboResult = new CombinationResult();
            foreach (var c in Combinations)
            {
                double val = c.D + c.L + c.L_r + c.S + c.E + c.F + c.H + c.R + c.T + c.W+c.F_a+c.W_i+c.D_i;
                if (val>=MaxVal)
                {
                    ComboResult.MaxValue = MaxVal;
                    ComboResult.MaxCombination = c;
                }
                if (val<=MinVal)
                {
                    ComboResult.MinValue = MaxVal;
                    ComboResult.MinCombination = c;
                }
                if (Math.Abs(val)>MaxAbsVal)
                {
                    ComboResult.MaxAbsoluteValue = Math.Abs(val);
                }
            }
            return ComboResult;
        }
    }
}

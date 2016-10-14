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


namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public static  partial class FilletWeldLimits
    {
        public static double GetEffectiveLegth(double w_weld, double l, bool IsEndLoaded )
        {
            double l_eff = l;
            if (IsEndLoaded ==true)
            {
                if (l <= 100 * w_weld)
                {
                    l_eff = l;
                }
                else if (l > 300 * w_weld)
                {
                    l_eff = 180 * w_weld;
                }
                else
                {
                    double beta = 0.0;
                    beta = 1.2 - 0.002 * (((l) / (w_weld)));
                    l_eff = l * beta;
                } 
            }
            else
            {
                l_eff = l;
            }
            return l_eff;
        }
    }
}

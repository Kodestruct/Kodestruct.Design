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
using Kodestruct.Common.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {
 
        /// <summary>
        /// Time effect factor from NDS 2015 section N.3.3 (lambda)
        /// </summary>
        /// <param name="CombinationType"> Type of combination, Table N3.</param>
        /// <param name="IsConnection">If connection, lambda greater than 1.0 will not apply. </param>
        /// <param name="IsConnection">If pressure-treated with water-borne preservativesor fire retardant chemicals, lambda greater than 1.0 will not apply. </param>
        /// <returns></returns>
        /// 
        public double GetTimeEffectFactor(LoadCombinationType CombinationType, 
            bool IsConnection, bool IsTreated=false)
        {
            switch (CombinationType)
            {
                case LoadCombinationType.DeadLoadOnly:
                    return 0.6;
                    break;
                case LoadCombinationType.FullLiveLoadStorage:
                    return 0.7;
                    break;
                case LoadCombinationType.FullLiveLoad:
                    return 0.8;
                    break;
                case LoadCombinationType.FullLiveLoadImpact:
                    if (!IsConnection && !IsTreated)
                    {
                        return 1.25;
                    }
                    else
                    {
                        return 1.0;
                    }
                    
                    break;
                case LoadCombinationType.FullLiveLoadWithWind:
                    return 0.8;
                    break;
                case LoadCombinationType.FullWind:
                    return 1.0;
                    break;
                case LoadCombinationType.FullEarthquake:
                    return 1.0;
                    break;
                default:
                    return 1.0;
                    break;
            }
        }
    }
}

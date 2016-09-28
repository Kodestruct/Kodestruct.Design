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
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI318_14.Anchorage
{
    public partial class ConcreteAnchorageElement
    {
        /// <summary>
        /// Critical edge distance required to develop the basic strength as controlled by concrete breakout or bond of a post-installed anchor in tension in uncracked concrete without supplementary reinforcement to control splitting, per 17.7.6
        /// </summary>
        /// <param name="AnchorageType"></param>
        /// <param name="h_ef"></param>
        /// <returns></returns>
        public double GetCriticalEdgeDistance(AnchorageType AnchorageType, double h_ef)
        {
            double c_ac;
            switch (AnchorageType)
            {
                case AnchorageType.Adhesive:
                    c_ac = 2 * h_ef;
                    break;
                case AnchorageType.Undercut:
                    c_ac = 2.5 * h_ef;
                    break;
                case AnchorageType.TorqueControlledExpansion:
                    c_ac = 4 * h_ef;
                    break;
                case AnchorageType.DisplacementControlledExpansion:
                    c_ac = 4 * h_ef;
                    break;
                default:
                    c_ac = 4 * h_ef;
                    break;
            }
            return c_ac;

        }
    }
}

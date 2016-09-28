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

namespace Kodestruct.Concrete.ACI318_14.Anchorage.Base
{
    public abstract class AnchorageLimitState
    {
        /// <summary>
        /// Number of anchors
        /// </summary>
        public int n { get; set; }

        /// <summary>
        /// Cast-in or post-installed anchor
        /// </summary>
        public AnchorInstallationType AnchorType { get; set; }


        public AnchorageLimitState(
            int n, AnchorInstallationType AnchorType
            )
        {
            this.n = n;
            this.AnchorType = AnchorType;
        }


        public abstract double GetNominalStrength();
    }
}

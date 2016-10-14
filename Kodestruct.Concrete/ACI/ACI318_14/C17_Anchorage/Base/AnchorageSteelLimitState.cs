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

namespace Kodestruct.Concrete.ACI318_14.Anchorage.Base
{
    public abstract class AnchorageSteelLimitState : AnchorageLimitState
    {

        public AnchorageSteelLimitState(
            int n,
            double futa,
            double fya,
            double A_se_N,
            AnchorSteelElementFailureType SteelFailureType, AnchorInstallationType AnchorType
            )
            : base(n, AnchorType)
        {

            this.n=     n;
            this.futa=  futa;
            this.fya=   fya;
            this.A_se_N = A_se_N;

        }

        /// <summary>
        /// Ductile versus Non-ductile steel element. 
        /// </summary>
        public AnchorSteelElementFailureType SteelFailureType { get; set; }

        /// <summary>
        /// Ultimate stress of anchor steel
        /// </summary>
            public double futa { get; set; }

        /// <summary>
            /// Yield stress of anchor steel
        /// </summary>
            public double fya { get; set; }

        /// <summary>
            /// Effective cs area of an anchor in tension
        /// </summary>
            public double A_se_N { get; set; }

    }
}

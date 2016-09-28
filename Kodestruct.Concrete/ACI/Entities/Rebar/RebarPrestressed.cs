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

namespace Kodestruct.Concrete.ACI
{
    public class RebarPrestressed: Rebar, IPrestressedReinforcement
    {
        public double JackingStress { get; set; }
        public double TransferStress { get; set; }
        public double EffectiveStress { get; set; }

        public PrestressingType PrestressingType { get; set; }
        public StrandType StrandType { get; set; }

        public RebarPrestressed(double Diameter, IRebarMaterial rebarMaterial,
            PrestressingType PrestressingType, StrandType StrandType, double JackingStress, double TransferStress, double EffectiveStress)
        : base(Diameter, rebarMaterial)
            {

            }

        public RebarPrestressed(double Diameter, IRebarMaterial rebarMaterial,
            PrestressingType PrestressingType, StrandType StrandType, double EffectiveStress)
            : this(Diameter, rebarMaterial, PrestressingType, StrandType, 0, 0, EffectiveStress)
            {

            }

        public IPrestressedRebarMaterial Material { get; set; }


    }
}

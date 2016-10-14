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
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamTee : FlexuralMemberTeeBase
    {

        //Yielding F9.1
        public double GetYieldingMomentCapacity(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Mp = GetMajorNominalPlasticMoment();
            double Mn = 0;
            double My = GetYieldingMoment_My(compressionFiberLocation);

            switch (compressionFiberLocation)
            {
                case FlexuralCompressionFiberPosition.Top:
                    //(a) For stems in tension
                    Mn = Mp > 1.6 * My ? 1.6 * My : Mp; //(F9-2)
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    //(b) For stems in compression
                    Mn = Mp >  My ?  My : Mp; //(F9-3)
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Mn;
        }

        private double GetYieldingMoment_My(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Sxt = GetSectionModulusTensionSxt(compressionFiberLocation);
            double Sxc = GetSectionModulusCompressionSxc(compressionFiberLocation);
            double Sx = Math.Min(Sxc, Sxt);
            double Fy = Section.Material.YieldStress;
            double My = Sx * Fy;
            return My;
        }


    }
}

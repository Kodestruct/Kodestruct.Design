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
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamAngle : FlexuralMemberAngleBase
    {
        //Yielding F10.1
        public double GetYieldingMomentCapacityGeometric(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Mn = 0;
            double My;

            if (compressionFiberLocation== FlexuralCompressionFiberPosition.Top ||compressionFiberLocation== FlexuralCompressionFiberPosition.Bottom )
            {
                My = GetYieldingMomentGeometricXCapacity(compressionFiberLocation, FlexuralAndTorsionalBracingType.FullLateralBracing);
            }
            else
            {
                throw new CompressionFiberPositionException();
            }

            Mn = 1.5 * My; //(F10-1)

            double phiM_n = 0.9 * Mn;
            return phiM_n;
        }

        protected double GetSectionModulus(FlexuralCompressionFiberPosition compressionFiberLocation, bool UseCompressionSectionModulusOnly, FlexuralAndTorsionalBracingType BracingType)
        {
            double Sxt = GetSectionModulusTensionSxt(compressionFiberLocation);
            double Sxc = GetSectionModulusCompressionSxc(compressionFiberLocation);

            double Sx;
            if (UseCompressionSectionModulusOnly == true)
            {
                Sx=Sxc;
            }
            else
	        {
                Sx=Math.Min(Sxc, Sxt);
	        }
            if (BracingType == FlexuralAndTorsionalBracingType.NoLateralBracing)
            {
                Sx = 0.8 * Sx;
            }

            return Sx;
        }

        private double GetYieldingMomentGeometricXCapacity(FlexuralCompressionFiberPosition compressionFiberLocation, FlexuralAndTorsionalBracingType BracingType)
        {
            double S_x = GetSectionModulus(compressionFiberLocation, false, BracingType);
            double Fy = Section.Material.YieldStress;
            double My = S_x * Fy;
            return My;
        }

        //private double GetYieldingMomentGeometricYCapacity(FlexuralCompressionFiberPosition compressionFiberLocation)
        //{
        //    double Syt = GetSectionModulusTensionSyt(compressionFiberLocation);
        //    double Syc = GetSectionModulusCompressionSyc(compressionFiberLocation);
        //    double Sy = Math.Min(Syc, Syt);
        //    double Fy = Section.Material.YieldStress;
        //    double My = Sy * Fy;
        //    return My;
        //}


    }
}

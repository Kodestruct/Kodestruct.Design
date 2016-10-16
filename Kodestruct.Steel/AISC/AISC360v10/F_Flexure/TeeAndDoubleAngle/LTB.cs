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

        public double GetFlexuralTorsionalBucklingMomentCapacity(FlexuralCompressionFiberPosition compressionFiberLocation, double L_b)
        {
            double M_n;
            double pi = Math.PI;
            double Iy = Section.Shape.I_y;
            double G = Section.Material.ShearModulus;
            double J = SectionTee.J;
            double B = GetB(compressionFiberLocation,L_b);
            double B2 = Math.Pow(B,2);

            M_n = pi * Math.Sqrt(E * Iy * G * J) / (L_b) * (B + Math.Sqrt(1.0 + B2)); //(F9-4)
            double phiM_n = 0.9 * M_n;
            return phiM_n;

        }

        private double GetB(FlexuralCompressionFiberPosition compressionFiberLocation, double L_b)
        {
            double B;
            double d = SectionTee.d;
            double Iy = SectionTee.I_y;
            double J = SectionTee.J;

            double sign;

            switch (compressionFiberLocation)
            {
                case FlexuralCompressionFiberPosition.Top:
                    //For stems in tension
                    sign = 1;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    //For stems in compression
                    sign = -1;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            B=sign*2.3*(d/L_b)*Math.Sqrt(Iy/J); //(F9-5)

            return B;
        }


    }
}

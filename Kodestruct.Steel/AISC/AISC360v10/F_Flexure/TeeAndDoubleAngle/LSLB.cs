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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamTee : FlexuralMemberTeeBase
    {
        public double GetCompressedStemLocalBucklingCapacity(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Mn;
            if (compressionFiberLocation == FlexuralCompressionFiberPosition.Top)
	            {
		             // stem is in tension
                     return double.PositiveInfinity;
	            }
                        else
	            {
                //stem is in tension
	
                double lambdaStem = GetLambdaStem();
                double Fcr = GetCriticalStressFcr(compressionFiberLocation);

                double Sxc = GetSectionModulusCompressionSxc(compressionFiberLocation);
                double Sxt = GetSectionModulusTensionSxt(compressionFiberLocation);
                double Sx = Math.Min(Sxc, Sxt);

                Mn = Fcr * Sx; //(F9-8)

                return Mn;
                }
        }

        internal double GetCriticalStressFcr(FlexuralCompressionFiberPosition compressionFiberLocation)
        {
            double Fcr;

            Fy = this.Section.Material.YieldStress;

            double lambdaStem = GetLambdaStem();
            if (lambdaStem > 0.84 * SqrtE_Fy())
            {
                if (lambdaStem<1.03*SqrtE_Fy())
                {
                    //(F9-10)
                    Fcr = (2.55 - 1.84 * lambdaStem * Math.Sqrt(Fy / E)) * Fy;
                }
                else
                {
                    //(F9-11)
                    Fcr = 0.69 * E / Math.Pow(lambdaStem, 2);
                }
            }
            else
            {
                Fcr = Fy; //(F9-9)
            }

            return Fcr;
        }




    }
}

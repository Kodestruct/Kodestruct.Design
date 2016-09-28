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
    public abstract partial class FlexuralMemberIBase : FlexuralMember
    {
        public double GetStressFL(double Sxt, double Sxc, double Fy)
        {
            double FL = 0.0;
            if (Sxt / Sxc >= 0.7)
            {
                FL = 0.7 * Fy; //(F4-6a)
            }
            else
            {
                FL = Fy * Sxt / Sxc;
                FL = FL < 0.5 * Fy ? 0.5 * Fy : FL; //(F4-6b)
            }
            return FL;
        }

        public double GetStressFL(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double FL = 0.0;

            double Fy = 0.0;
            double Sxt = 0.0;
            double Sxc = 0.0;


            Fy = Section.Material.YieldStress;

            Sxt = Section.Shape.S_xTop;
            Sxc = Section.Shape.S_xBot;

            FL = GetStressFL(Sxt, Sxc, Fy);

            return FL;
        }
    }
}

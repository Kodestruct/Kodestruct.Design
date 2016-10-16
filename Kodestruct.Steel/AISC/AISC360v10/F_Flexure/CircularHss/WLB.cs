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
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Common.Section;
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamCircularHss : FlexuralMemberChsBase
    {
      public double GetLocalBucklingCapacity()
        {
            double M_n = 0.0;

            if (this.SectionPipe != null)
            {
                CompactnessClassFlexure cClass = GetCompactnessClass();
                if (cClass == CompactnessClassFlexure.Noncompact || cClass == CompactnessClassFlexure.Slender)
                {
                    double Sx = Section.Shape.S_xTop;
                    if (cClass == CompactnessClassFlexure.Noncompact)
                    {
                        //noncompact
                        M_n = (0.021 * E / (D / t) + F_y) * Sx; //(F8-2)
                    }
                    else
                    {
                        //slender
                        double Fcr = 0.33 * E / (D / t); //(F8-4)
                        M_n = Fcr * Sx;  // (F8-3)
                    }
                }
                else
                {
                    M_n = double.PositiveInfinity;
                }
            }
            double phiM_n = 0.9 * M_n;
            return phiM_n;
      }
    }
}

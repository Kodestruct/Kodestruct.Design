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
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {
        public double GetRepetitiveMemberFactor(bool IsRepetitive, 
            double Spacing,
             double Thickness,
            SawnLumberType SawnLumberType)
        {
            //NDS 2015 Section 4.3.9
            if (IsRepetitive == true)
            {
                if (Spacing > 24)
                {
                    return 1.0;
                }
                else
                {
                    //return 1.15;
                    return GetRepetitiveCoefficientFromReferenceTable(Thickness, SawnLumberType);
                }
            }
            else
            {
                return 1.0;
            }
        }

    public double GetRepetitiveCoefficientFromReferenceTable(double Thickness,
    SawnLumberType SawnLumberType)
        {
        
        double C_r;

        switch (SawnLumberType)
        {
            case SawnLumberType.DimensionLumber:
                C_r = GetDimensionalLumberC_r(Thickness); //4A
                break;
            case SawnLumberType.SouthernPineDimensionLumber:
                C_r = GetDimensionalLumberC_r(Thickness); //4B
                break;
            case SawnLumberType.MechanicallyGradedDimensionLumber:
                C_r = GetDimensionalLumberC_r(Thickness); //4C
                break;
            case SawnLumberType.VisuallyGradedTimbers:
                C_r = 1.0;//4D
                break;
            case SawnLumberType.VisuallyGradedDecking:
                 C_r = 1.0;//4E
                break;
            case SawnLumberType.NonNorthAmericanVisuallyGradedDimensionLumber:
                C_r = GetDimensionalLumberC_r(Thickness); //4F
                break;
            default:
                C_r = GetDimensionalLumberC_r(Thickness); //4A
                break;
        }

        return C_r;
        }

        private double GetDimensionalLumberC_r(double Thickness)
        {
            if (Thickness <= 4.0)
            {
                return 1.15;
            }
            return 1.0;
        }
    }
}

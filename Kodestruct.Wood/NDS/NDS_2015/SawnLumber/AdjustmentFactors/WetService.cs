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
using Kodestruct.Wood.NDS.Entities;


namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {

        /// <summary>
        /// When dimension lumber is used where moisture content will exceed 19% for an extended time period, 
        /// design values shall be multiplied by the appropriate wet service factors 
        /// </summary>
        /// <param name="ValueType"> Distiguishes between bending, shear, tension compression reference values</param>
        /// <param name="F">Reference design value for bending or compression</param>
        /// <param name="C_F">Size factor</param>
        /// <param name="SawnLumberType">Defines the type of lumber</param>
        /// <returns></returns>
        public  double GetWetServiceFactor(ReferenceDesignValueType ValueType, double F,
            double C_F, SawnLumberType SawnLumberType)
        {
            double Cm = 1.0;
            switch (SawnLumberType)
            {
                case SawnLumberType.DimensionLumber:
                    return GetDimensionalLumberC_M(ValueType, F, C_F); //4A
                case SawnLumberType.SouthernPineDimensionLumber:
                    return GetDimensionalLumberC_M(ValueType, F, C_F); //4B
                case SawnLumberType.MechanicallyGradedDimensionLumber: //4C
                    return GetDimensionalLumberC_M(ValueType, F, C_F);
                case SawnLumberType.VisuallyGradedTimbers: //4D
                    return GetTimberC_M(ValueType, F, C_F);
                case SawnLumberType.VisuallyGradedDecking: //4E
                    return GetDeckingC_M(ValueType, F, C_F);
                case SawnLumberType.NonNorthAmericanVisuallyGradedDimensionLumber: //4F
                    return GetNonNorthAmericanLumberC_M(ValueType, F, C_F);
                default:
                    return GetDimensionalLumberC_M(ValueType, F, C_F); //4A

            }
        }

        private double GetDeckingC_M(ReferenceDesignValueType ValueType, double F_b, double C_F)
        {
             switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    double Fb = F_b;
                    
                    if (Fb*C_F<=1.500) //when (Fb)(CF) =1,150 psi, CM= 1.0 form footnote of table
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.85;
                    }
                    
                case ReferenceDesignValueType.TensionParallelToGrain:            return 1.0;
                case ReferenceDesignValueType.ShearParallelToGrain:              return 1.0;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:    return 0.67;
                case ReferenceDesignValueType.CompresionParallelToGrain:         return 1.0;
                case ReferenceDesignValueType.ModulusOfElasticity:               return 0.9;
                case ReferenceDesignValueType.ModulusOfElasticityMin:            return 0.9;
                default: return 1.0;
            }
        }

        private double GetTimberC_M(ReferenceDesignValueType ValueType, double DesignReferenceValue, double CF)
        {
             switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:                           return 1.0;
                case ReferenceDesignValueType.TensionParallelToGrain:            return 1.0;
                case ReferenceDesignValueType.ShearParallelToGrain:              return 1.0;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:    return 0.67;
                case ReferenceDesignValueType.CompresionParallelToGrain:         return 0.91;
                case ReferenceDesignValueType.ModulusOfElasticity:               return 1.0;
                case ReferenceDesignValueType.ModulusOfElasticityMin:            return 1.0;
                default: return 1.0;
            }
        }

        double GetDimensionalLumberC_M(ReferenceDesignValueType ValueType, double DesignReferenceValue, double CF)
        {
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    double Fb = DesignReferenceValue;
                    
                    if (Fb*CF<=1.500) //when (Fb)(CF) =1,150 psi, CM= 1.0
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.85;
                    }
                    
                case ReferenceDesignValueType.TensionParallelToGrain:            return 1.0;
                case ReferenceDesignValueType.ShearParallelToGrain:              return 0.97;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:    return 0.67;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    double Fc = DesignReferenceValue;
                    if (Fc*CF <= 0.750) //when (Fb)(CF) =1,150 psi, CM= 1.0
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.8;
                    }
                case ReferenceDesignValueType.ModulusOfElasticity:               return 0.9;
                case ReferenceDesignValueType.ModulusOfElasticityMin:            return 0.9;
                default: return 1.0;
            }
        }

        double GetNonNorthAmericanLumberC_M(ReferenceDesignValueType ValueType, double DesignReferenceValue, double CF)
        {
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    double Fb = DesignReferenceValue;

                    if (Fb * CF <= 1.500) //when (Fb)(CF) =1,150 psi, CM= 1.0
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.85;
                    }

                case ReferenceDesignValueType.TensionParallelToGrain: return 1.0;
                case ReferenceDesignValueType.ShearParallelToGrain: return 0.97;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain: return 0.67;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    double Fc = DesignReferenceValue;
                    if (Fc * CF <= 0.750) //when (Fb)(CF) =750 psi, CM= 1.0
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.8;
                    }
                case ReferenceDesignValueType.ModulusOfElasticity: return 0.9;
                case ReferenceDesignValueType.ModulusOfElasticityMin: return 0.9;
                default: return 1.0;
            }
        }


    }
}

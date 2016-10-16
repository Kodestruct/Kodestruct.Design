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
using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.Entities;
namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {
        public double GetFlatUseFactor( 
            double Depth, double Thickness,
            SawnLumberType SawnLumberType,
            CommercialGrade CommercialGrade, 
            ReferenceDesignValueType ReferenceDesignValueType
            )
        {
            double b = Depth;
            double C_fu = 1.0;
            if (b >= 2 || b <= 4)
            {
                C_fu = GetFlatUseCoefficientFromReferenceTable(
                Depth,  Thickness,
                SawnLumberType, CommercialGrade, 
                ReferenceDesignValueType);
            }
            return C_fu;
        }

        private double GetFlatUseCoefficientFromReferenceTable(
            double Depth, double Thickness,
            SawnLumberType SawnLumberType, 
            CommercialGrade CommercialGrade, 
            ReferenceDesignValueType ReferenceDesignValueType
            )
        {
            double b = Depth;
            double t = Thickness;
            double C_fu = 1.0;
            switch (SawnLumberType)
            {
                case SawnLumberType.DimensionLumber:
                    C_fu = GetDimensionalLumberC_fu(b, t); break;//4A
                case SawnLumberType.SouthernPineDimensionLumber://4B
                    C_fu = GetSPDimensionalLumberC_fu(b, t); break;
                case SawnLumberType.MechanicallyGradedDimensionLumber: //4C
                    C_fu = GetMechanicallyGradedLumberC_fu(b, t);  break;
                case SawnLumberType.VisuallyGradedTimbers: //4D
                    C_fu = GetTimberC_fu(b, t, CommercialGrade, ReferenceDesignValueType); break;
                case SawnLumberType.VisuallyGradedDecking: //4E
                    C_fu = GetDeckingC_fu(); break;
                case SawnLumberType.NonNorthAmericanVisuallyGradedDimensionLumber: //4F
                    C_fu = GetNonNorthAmericanLumberC_fu(b, t); break;
                default://4A
                    C_fu = GetDimensionalLumberC_fu(b, t); break; 

            }
            return C_fu;
        }

        private double GetTimberC_fu(double b, double t, CommercialGrade CommercialGrade, 
            ReferenceDesignValueType ReferenceDesignValueType)
        {
            double C_fu = 1.0;

            if (CommercialGrade == Entities.CommercialGrade.SelectStructural)
            {
                if (ReferenceDesignValueType ==  Entities.ReferenceDesignValueType.Bending)
                {
                    C_fu = 0.86;
                }
                else if (ReferenceDesignValueType ==  ReferenceDesignValueType.ModulusOfElasticityMin || 
                    ReferenceDesignValueType == ReferenceDesignValueType.ModulusOfElasticityMin)
                {
                    C_fu = 1.0;
                }
                else
                {
                    C_fu = 1.0;
                }
            }
           
            if (CommercialGrade == Entities.CommercialGrade.No2)
            {
                if (ReferenceDesignValueType ==  Entities.ReferenceDesignValueType.Bending)
                {
                    C_fu =1.0;
                }
                else if (ReferenceDesignValueType ==  ReferenceDesignValueType.ModulusOfElasticityMin || 
                    ReferenceDesignValueType == ReferenceDesignValueType.ModulusOfElasticityMin)
                {
                    C_fu = 1.0;
                }
                else
                {
                    C_fu = 1.0;
                }
            }

            else
	        {
                     //(CommercialGrade == Entities.CommercialGrade.No1)
                    
                        if (ReferenceDesignValueType ==  Entities.ReferenceDesignValueType.Bending)
                        {
                            C_fu =0.74;
                        }
                        else if (ReferenceDesignValueType ==  ReferenceDesignValueType.ModulusOfElasticityMin || 
                            ReferenceDesignValueType == ReferenceDesignValueType.ModulusOfElasticityMin)
                        {
                            C_fu =  0.9;
                        }
                        else
                        {
                            C_fu = 1.0;
                        }
            }
	        

            return C_fu;
        }

        private double GetNonNorthAmericanLumberC_fu(double Depth, double Thickness)
        {
            double b = Math.Round(Depth);
            double t = Thickness;
            double C_fu = 1.0;

            if (b <= 3) { if (t < 4) { C_fu = 1.0; } else { C_fu = 1.0; } }
            if (b == 4) { if (t < 4) { C_fu = 1.1; } else { C_fu = 1.0; } }
            if (b == 5) { if (t < 4) { C_fu = 1.1; } else { C_fu = 1.05; } }
            if (b == 6) { if (t < 4) { C_fu = 1.15; } else { C_fu = 1.05; } }
            if (b == 8) { if (t < 4) { C_fu = 1.15; } else { C_fu = 1.05; } }
            if (b <= 10) { if (t < 4) { C_fu = 1.2; } else { C_fu = 1.1; } }

            return C_fu;
        }

        private double GetDeckingC_fu()
        {
            return 1.0;
        }


        private double GetMechanicallyGradedLumberC_fu(double Depth, double Thickness)
        {
            double b = Math.Round(Depth);
            double t = Thickness;
            double C_fu = 1.0;

            if (b <= 3) { C_fu = 1.0;  }
            if (b == 4)  { C_fu = 1.1;  }
            if (b == 5)  { C_fu = 1.1;  }
            if (b == 6)  { C_fu = 1.15; }
            if (b == 8)  { C_fu = 1.15; }
            if (b >= 10){ C_fu = 1.2;  }

            return C_fu;
        }

        private double GetSPDimensionalLumberC_fu(double Depth, double Thickness)
        {
            double b = Math.Round(Depth);
            double t = Thickness;
            double C_fu = 1.0;

            if (b <= 3) { if (t < 4) { C_fu = 1.0; } else { C_fu = 1.0; } }
            if (b == 4) { if (t < 4) { C_fu = 1.1; } else { C_fu = 1.0; } }
            if (b == 5) { if (t < 4) { C_fu = 1.1; } else { C_fu = 1.05; } }
            if (b == 6) { if (t < 4) { C_fu = 1.15; } else { C_fu = 1.05; } }
            if (b == 8) { if (t < 4) { C_fu = 1.15; } else { C_fu = 1.05; } }
            if (b <= 10) { if (t < 4) { C_fu = 1.2; } else { C_fu = 1.1; } }

            return C_fu;
        }

        private double GetDimensionalLumberC_fu(double Depth, double Thickness)
        {
           
            double b = Math.Round(Depth);
            double t = Thickness;  
            double C_fu = 1.0;

            if(b<= 3 ) {if (t<4) { C_fu = 1.0 ;} else { C_fu = 1.0  ;}}
            if(b== 4 ) {if (t< 4) { C_fu = 1.1 ;} else { C_fu = 1.0  ;}}
            if(b== 5 ) {if (t< 4) { C_fu = 1.1 ;} else { C_fu = 1.05 ;}}
            if(b== 6 ) {if (t< 4) { C_fu = 1.15;} else { C_fu =  1.05;}}
            if(b== 8 ) {if (t< 4) { C_fu = 1.15;} else { C_fu =  1.05;}}
            if(b<=10) {if (t<4) { C_fu = 1.2 ;} else { C_fu = 1.1  ;}}

            return C_fu;
          }
    }
}

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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.Entities;
using Kodestruct.Wood.Properties;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {
        public  double GetSizeFactor(
            double Depth, double Thickness,
            SawnLumberType SawnLumberType,
            CommercialGrade CommercialGrade,
            ReferenceDesignValueType ReferenceDesignValueType,
            bool LoadAppliedToNarrowFace=true
            )
        {
            double C_F;
            if (Thickness >= 5 && Depth > 12.0)
            {
                // NDS 2015 Section 4.3.6.2
                C_F = Math.Pow(12 / Depth, 1 / 9.0);
                C_F = C_F > 1.0 ? 1.0 : C_F;
            }

            else
            {
                C_F = GetSizeCoefficientFromReferenceTable(Depth, Thickness, SawnLumberType,
                   CommercialGrade, ReferenceDesignValueType, LoadAppliedToNarrowFace);
            }
            return C_F;
        }

        private double GetSizeCoefficientFromReferenceTable(
           double Depth, double Thickness,
           SawnLumberType SawnLumberType,
           CommercialGrade CommercialGrade,
           ReferenceDesignValueType ReferenceDesignValueType,
            bool LoadAppliedToNarrowFace
           )
        {
            double b = Depth;
            double t = Thickness;
            double C_F = 1.0;
            switch (SawnLumberType)
            {
                case SawnLumberType.DimensionLumber:
                    C_F = GetDimensionalLumberC_F(b, t, CommercialGrade,ReferenceDesignValueType); break;//4A
                case SawnLumberType.SouthernPineDimensionLumber://4B
                    C_F = GetSPDimensionalLumberC_F(b, t,CommercialGrade); break;
                case SawnLumberType.MechanicallyGradedDimensionLumber: //4C
                    C_F = 1.0; break;
                case SawnLumberType.VisuallyGradedTimbers: //4D
                    C_F = GetTimberC_F(b, ReferenceDesignValueType, LoadAppliedToNarrowFace); break;
                case SawnLumberType.VisuallyGradedDecking: //4E
                    C_F = GetDeckingC_F(t); break;
                case SawnLumberType.NonNorthAmericanVisuallyGradedDimensionLumber: //4F
                    //Adjustment factors for table 4F are the same as those for 4A
                    C_F = GetDimensionalLumberC_F(b, t, CommercialGrade, ReferenceDesignValueType); break;
                default://4A
                     C_F = GetDimensionalLumberC_F(b, t, CommercialGrade,ReferenceDesignValueType); break;

            }
            return C_F;
        }

        private double GetDeckingC_F(double t)
        {
            //todo:  Redwood  does not have this adjustment
            if (t>3 && t<=3)
            {
                return 1.1;
            }
            else
            {
                return 1.04;
            }
        }

        private double GetTimberC_F(double Depth, ReferenceDesignValueType ReferenceDesignValueType, bool LoadAppliedToNarrowFace)
        {
            double C_F;

            if (LoadAppliedToNarrowFace ==true)
            {
                if (Depth>12.0 && ReferenceDesignValueType == ReferenceDesignValueType.Bending)
                {
                    C_F = Math.Pow(12 / Depth, 1 / 9.0);
                }
                else
                {
                    C_F = 1.0;
                }
                return C_F;
            }
            else
            {
                return 1.0;
            }
        }

        private double GetSPDimensionalLumberC_F(double b, double t, CommercialGrade CommercialGrade)
        {
            double C_F = 1.0;
            if (CommercialGrade != CommercialGrade.DenseStructural65
                && CommercialGrade != CommercialGrade.DenseStructural72
                && CommercialGrade != CommercialGrade.DenseStructural86)
            {
                if (t==4&& b>=8)
                {

                    C_F = 1.1;
                }
                else if (b>12)
                {
                    C_F = 0.9;
                }
            }
            else
            {
                if (b>12)
                {
                    C_F = Math.Pow(12 / d, 1 / 9.0);
                }
            }
            return C_F;
        }

        private double GetDimensionalLumberC_F(double Depth, double Thickness, CommercialGrade Grade, ReferenceDesignValueType ReferenceDesignValueType)
        {
            double SizeFactor;
            string GradeString = "Other";


            if (Grade == CommercialGrade.Standard || Grade == CommercialGrade.Construction)
            {
                SizeFactor = 1.0; 
            }
            else 
            {
                double b = Math.Ceiling(Depth);
                double t = Math.Ceiling(Thickness);

                //Adjust thickness
                if (t <= 3)
                {
                    t = 3.0;
                }
                else if (t > 3 && t <= 5)
                {
                    t = 4.0;
                }
                else
                {
                    throw new Exception("Use timber values for elements having over 5 inches in thickness.");
                }
                
                //adjust the values for lookup (for depth)
                if (Grade == CommercialGrade.Utility)
                {
                    GradeString = Grade.ToString();
                    if (b<=3)
                    {
                        b = 3; 
                    }
                    else
                    {
                        b = 4;
                    }
                }
                else if (Grade == CommercialGrade.Stud)
                {
                    GradeString = Grade.ToString();

                    if (b <= 4)
                    {
                        b = 4;
                    }
                    else if (b >= 7)
                    {
                        b = 8;
                    }

                }
                else
                {
                    
                    GradeString = "Other";


                    //Adjust depth
                    if (b<=4)
                    {
                        b = 4;  
                    }
                    else if (b==7)
                    {
                        b = 8;
                    }
                    else if (b == 9)
                    {
                        b = 10;
                    }
                    else if (b>=10)
                    {
                        b = 14;
                    }

                }
               
                #region Read Table Data

                var Tv11 = new { Grade="",	Depth=0.0,	Thickness=0.0, Fb=0.0,	Ft=0.0,	Fc=0.0}; // sample
                var ResultList = ListFactory.MakeList(Tv11);

                using (StringReader reader = new StringReader(Resources.NDS2015_Table4A_SizeFactor))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 6)
                        {

                            string _Grade = Vals[0];
                            double _Depth     =double.Parse(Vals[1],CultureInfo.InvariantCulture);
                            double _Thickness =double.Parse(Vals[2],CultureInfo.InvariantCulture);
                            double _Fb        =double.Parse(Vals[3],CultureInfo.InvariantCulture);
                            double _Ft        =double.Parse(Vals[4],CultureInfo.InvariantCulture);
                            double _Fc        =double.Parse(Vals[5],CultureInfo.InvariantCulture);

                            ResultList.Add(new
                            {
                                Grade    = _Grade    ,
                                Depth    = _Depth    ,
                                Thickness= _Thickness,
                                Fb       = _Fb       ,
                                Ft       = _Ft       ,
                                Fc       = _Fc       

                            });
                        }
                    }

                }

                #endregion

                var RValues = from v in ResultList
                              where
                                  (v.Grade == GradeString &&
                                  v.Depth == b  &&
                                  v.Thickness ==t)
                              select v;
                var foundValue = (RValues.ToList()).FirstOrDefault();
                if (foundValue == null)
                {
                    throw new Exception("Combination of lumber grade and size was not found. Check input");
                }
                else
                {
                    switch (ReferenceDesignValueType)
                    {
                        case ReferenceDesignValueType.Bending:
                            SizeFactor = foundValue.Fb;
                            break;
                        case ReferenceDesignValueType.TensionParallelToGrain:
                            SizeFactor = foundValue.Ft;
                            break;
                        case ReferenceDesignValueType.CompresionParallelToGrain:
                            SizeFactor = foundValue.Fc;
                            break;
                        default:
                            SizeFactor = 1.0;
                            break;
                    }
                }
                if (SizeFactor == 0)
                {
                    throw new Exception("Combination of lumber grade and size was not found. Check input");
                }
            }

            return SizeFactor;
        }
    }
}

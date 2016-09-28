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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Mathematics;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.CC
{
    public partial class ComponentOrCladding : BuildingDirectionalProcedureElement
    {
        public CCPressureResult GetPressure(WindWallCladdingZone Zone, double theta, double A_trib,double B, double L, double h)
        {
            double CpPos = GetWallPressureCoefficientPositive(Zone, theta, A_trib, h);
            double CpNeg= GetWallPressureCoefficientNegative(Zone, theta, A_trib, h);


            double CpMax = Math.Max(Math.Abs(CpPos), Math.Abs(CpNeg));
            
            #region Cp
            ICalcLogEntry CpEntry = new CalcLogEntry();
            CpEntry.ValueName = "Cp";
            CpEntry.AddDependencyValue("A", Math.Round(A_trib, 3));
            CpEntry.AddDependencyValue("h", Math.Round(h, 3));
            CpEntry.AddDependencyValue("theta", Math.Round(theta, 3));
            CpEntry.AddDependencyValue("Gcp", Math.Round(CpPos, 3));
            CpEntry.AddDependencyValue("GcpNeg", Math.Round(CpNeg, 3));
            CpEntry.Reference = "";
            CpEntry.DescriptionReference = SelectTemplate(h,theta,Zone);
            CpEntry.FormulaID = null; //reference to formula from code
            CpEntry.VariableValue = Math.Round(CpMax, 3).ToString();
            #endregion
            this.AddToLog(CpEntry);

            double a = GetCornerZoneDimension(B, L, h);
            CCPressureResult result = new CCPressureResult(CpPos, CpNeg, a);
            return result;

        }
        private string SelectTemplate(double h, double theta, WindWallCladdingZone Zone)
        {
            string templatePath = null;

            if (h<=60)
            {
                if (theta>10)
                {
                    switch (Zone)
                    {
                        case WindWallCladdingZone.Corner:
                            templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCCornerLowRise.docx";
                            break;
                        case WindWallCladdingZone.Middle:
                            templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCMiddleLowRise.docx";
                            break;
                    }
                }
                else
                {
                    //10% reduction
                    switch (Zone)
                    {
                        case WindWallCladdingZone.Corner:
                            templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCCornerLowRiseFlat.docx";
                            break;
                        case WindWallCladdingZone.Middle:
                            templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCMiddleLowRiseFlat.docx";
                            break;
                    }
                }
            }
            else
            {
                switch (Zone)
                {
                    case WindWallCladdingZone.Corner:
                        templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCCornerHiRise.docx";
                        break;                                                                                
                    case WindWallCladdingZone.Middle:                                                         
                        templatePath = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindGCpWallCCMiddleHiRise.docx";
                        break;
                }

            }
            return templatePath;
        }

        public double GetWallPressureCoefficientPositive(WindWallCladdingZone Zone, double theta, double A_trib, double h)
        {
            //not dependent on zone type
            //Figure 30.4-1
            double CpPos = 1.0;

            double Asmallest;
            double Alargest;
            double CP_smallestA;
            double CP_largesAlargestA;


            Asmallest = 10.0;
            Alargest = 500.0;


            if (h<=60.0) //low-rise
            {
                CP_smallestA = 1.0;
                CP_largesAlargestA = 0.7;
                CpPos = GetCp(A_trib, Asmallest, Alargest, CP_smallestA, CP_largesAlargestA);
                if (theta <= 10)
                {
                    CpPos = CpPos * 0.9; //10% reduction

                }
            }
            else //hi-rise
            {
                //Figure 30.6-1
                CP_smallestA = 0.9;
                CP_largesAlargestA = 0.6;
                CpPos = GetCp(A_trib, Asmallest, Alargest, CP_smallestA, CP_largesAlargestA);

            }
            return CpPos;
        }

        public double GetWallPressureCoefficientNegative(WindWallCladdingZone Zone, double theta, double A_trib, double h)
        {
            double CpNeg = 1.4;
            double Asmallest            =10.0;
            double Alargest             =500.0;

            double CP_smallestA         =1.0;
            double CP_largesAlargestA   =1.0;

            //Figure 30.4-1
            if (h<=60)
            {
                switch (Zone)
                {
                    case WindWallCladdingZone.Corner:
                        CP_smallestA = -1.4;
                        CP_largesAlargestA = -0.8;
                        break;
                    case WindWallCladdingZone.Middle:

                        CP_smallestA = -1.1;
                        CP_largesAlargestA = -0.8;
                        break;

                }
                if (theta <= 10)
                {
                    CP_smallestA = CP_smallestA * 0.9; //10% reduction
                    CP_largesAlargestA = CP_largesAlargestA * 0.9; //10% reduction
                }
            }
            else
            {
                switch (Zone)
                {
                    case WindWallCladdingZone.Corner:
                        CP_smallestA = -1.8;
                        CP_largesAlargestA = -1.0;
                        break;
                    case WindWallCladdingZone.Middle:

                        CP_smallestA = -0.9;
                        CP_largesAlargestA = -0.7;
                        break;

                } 
            }

            CpNeg = GetCp(A_trib, Asmallest, Alargest, CP_smallestA, CP_largesAlargestA);
            return CpNeg;
        }

        private double GetCp(double A_trib,
            double Asmallest     ,
            double Alargest      ,
            double CP_smallestA  ,
            double CP_largestA   )
        {
            double CpPos=1.0;

            if (A_trib < Asmallest)
            {
                CpPos = CP_smallestA;
            }
            else
            {
                if (A_trib < Alargest)
                {
                    CpPos = Interpolation.InterpolateLinear(Asmallest, CP_smallestA, Alargest, CP_largestA, A_trib);
                }
                else
                {
                    CpPos = CP_largestA;
                }
            }
            return CpPos;
        }

        public double GetCornerZoneDimension(double B, double L, double h)
        {
            double Lm = Math.Min(B, L);
            double a01 = 0.1 * Lm;
            double a02 = 0.4 * h;
                    double a1 = Math.Min(a01, a02);
            double a11 = 3.0;
            double a12 = 0.04 * Lm;
                    double a2 = Math.Min(a11, a12);

                    double a = Math.Max(a1, a2);
                    
                    #region a
                    ICalcLogEntry aEntry = new CalcLogEntry();
                    aEntry.ValueName = "a";
                    aEntry.AddDependencyValue("a1", Math.Round(a1, 3));
                    aEntry.AddDependencyValue("a2", Math.Round(a2, 3));
                    aEntry.AddDependencyValue("Lm", Math.Round(Lm, 3));
                    aEntry.Reference = "";
                    aEntry.DescriptionReference = "/Templates/loads/ASCE7_10/Wind/PressureCoefficient/WindCpWallCCCornerDimension.docx";
                    aEntry.FormulaID = null; //reference to formula from code
                    aEntry.VariableValue = Math.Round(a, 3).ToString();
                    #endregion
                    this.AddToLog(aEntry);
                    return a;
        }
    }
}

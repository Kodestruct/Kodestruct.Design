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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using data = Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SnowLoads
{
    public partial class SnowStructure
    {
        public double GetSnowSlopeFactor(double RoofSlopeAngle, data.SnowRoofSurfaceType surface, double ThermalFactor)
        {
            double Ct=ThermalFactor;
            double Cs;

            
            #region Cs
            ICalcLogEntry CsEntry = new CalcLogEntry();
            CsEntry.ValueName = "Cs";
            CsEntry.AddDependencyValue("s", Math.Round(RoofSlopeAngle, 3));
            CsEntry.AddDependencyValue("Ct", Math.Round(ThermalFactor, 3));
            CsEntry.Reference = "";
            CsEntry.FormulaID = null; //reference to formula from code
            
            #endregion
            

            if (Ct<=1.0)
            {
                Cs =GetWarmRoofCs(RoofSlopeAngle, surface, CsEntry);
            }
            else
            {
                if (Ct>=1.2)
                {
                    Cs = GetColdRoofCs(RoofSlopeAngle, surface, CsEntry);
                }
                else
                {
                    Cs = GetIntermediateColdRoofCs(RoofSlopeAngle, surface, CsEntry);
                }
            }
            CsEntry.VariableValue = Math.Round(Cs, 3).ToString();
            this.AddToLog(CsEntry);

            return Cs;
        }

        private double GetWarmRoofCs(double slope, data.SnowRoofSurfaceType surface, ICalcLogEntry CsEntry)
        {
            double Cs = 1.0;

            if (surface == data.SnowRoofSurfaceType.Slippery)
            {
                if (slope <= 5.0)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmSlippery.docx";
                    }
                    else
                    {
                        Cs = 1.0 - (slope - 5.0) / 65.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmSlipperyInterp.docx";

                    }
                }
            }
            else
            {
                if (slope <= 30.0)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmNonSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmNonSlippery.docx";
                    }
                    else
                    {
                        Cs = 1.0 - (slope - 30.0) / 40.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorWarmSlipperyInterp.docx";

                    }
                }
            }
            
            return Cs;
        }

        private double GetIntermediateColdRoofCs(double slope, data.SnowRoofSurfaceType surface, ICalcLogEntry CsEntry)
        {
            double Cs = 1.0;


            if (surface == data.SnowRoofSurfaceType.Slippery)
            {
                if (slope <= 10.0)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdSlippery.docx";
                    }
                    else
                    {

                        Cs = 1.0 - (slope - 10.0) / 60.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdSlipperyInterp.docx";

                    }
                }
            }
            else
            {
                if (slope <= 37.5)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdNonSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdNonSlippery.docx";
                    }
                    else
                    {

                        Cs = 1.0 - (slope - 37.5) / 32.5;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorIntermediateColdNonSlipperyInterp.docx";
                    }
                }
            }
            
            return Cs;
        }

        private double GetColdRoofCs(double slope, data.SnowRoofSurfaceType surface, ICalcLogEntry CsEntry)
        {
            double Cs = 1.0;


            if (surface == data.SnowRoofSurfaceType.Slippery)
            {
                if (slope <= 15.0)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdSlippery.docx";
                    }
                    else
                    {

                        Cs = 1.0 - (slope - 15.0) / 55.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdSlipperyInterp.docx";
                    }
                }
            }
            else
            {
                if (slope <= 45.0)
                {
                    Cs = 1.0;
                    CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdNonSlippery.docx";
                }
                else
                {
                    if (slope > 70)
                    {
                        Cs = 0.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdNonSlippery.docx";
                    }
                    else
                    {

                        Cs = 1.0 - (slope - 45.0) / 25.0;
                        CsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowSlopeFactorColdNonSlipperyInterp.docx";
                    }
                }
            }
            return Cs;
        }
    }
}

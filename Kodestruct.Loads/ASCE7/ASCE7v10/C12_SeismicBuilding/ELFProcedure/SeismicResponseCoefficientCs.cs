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
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicLateralForceResistingStructure : AnalyticalElement
    {

        public double GetResponseCoefficientCs(double T, double SDS, double SD1, double TL, double R, double Ie, double S1)
        {

            //Use section 12.8 EQUIVALENT LATERAL FORCE PROCEDURE

            //Use section 12.8.1.1 Calculation of Seismic Response Coefficient

            double CsMax, CsCurved, CsCutoff, CsCutoffZFourtyFourSds, CsCutoffOnePercent, CsCutoffOnePercentAndZFourtyFourSds, CsCutoffHighSeismic;

            double Cs = 0;
            CsMax = SDS / (R / Ie); // Eq(12.8-2)
             // log gentry added below
            #region ConstantAcceleration
            ICalcLogEntry ConstantAccelerationEntry = new CalcLogEntry();
            ConstantAccelerationEntry.ValueName = "Cs";
            ConstantAccelerationEntry.AddDependencyValue("SDS", Math.Round(SDS, 3));
            ConstantAccelerationEntry.AddDependencyValue("R", Math.Round(R, 3));
            ConstantAccelerationEntry.AddDependencyValue("Ie", Math.Round(Ie, 3));
            ConstantAccelerationEntry.Reference = "";
            ConstantAccelerationEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseFlat.docx";
            ConstantAccelerationEntry.FormulaID = null; //reference to formula from code
            ConstantAccelerationEntry.VariableValue = Math.Round(CsMax, 3).ToString();
            #endregion
            

            if (T <= TL) // most typical case (less than long-period transition period)
            {
                CsCurved = SD1 / (T * (R / Ie)); //Eq (12.8-3)

                #region CurvedShortPeriod
                ICalcLogEntry CurvedShortPeriodEntry = new CalcLogEntry();
                CurvedShortPeriodEntry.ValueName = "Cs";
                CurvedShortPeriodEntry.AddDependencyValue("SD1", Math.Round(SD1, 3));
                CurvedShortPeriodEntry.AddDependencyValue("T", Math.Round(T, 3));
                CurvedShortPeriodEntry.AddDependencyValue("R", Math.Round(R, 2));
                CurvedShortPeriodEntry.AddDependencyValue("Ie", Math.Round(Ie, 2));
                CurvedShortPeriodEntry.Reference = "Seismic response coefficient";
                CurvedShortPeriodEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCurvedShortPeriod.docx";
                CurvedShortPeriodEntry.FormulaID = "12.8-3"; //reference to formula from code
                CurvedShortPeriodEntry.VariableValue = Math.Round(CsCurved, 3).ToString();
                #endregion
                this.AddToLog(CurvedShortPeriodEntry);

                if (CsCurved < CsMax)
                {
                    Cs = CsCurved;
                    this.AddToLog(ConstantAccelerationEntry);
                    AddGoverningCaseMinLogEntry("Cs", CsMax, CsCurved, "12.8-3");
                    
                }
                else
                {
                    Cs = CsMax;
                    this.AddToLog(ConstantAccelerationEntry);
                    AddGoverningCaseMinLogEntry("Cs", CsCurved, CsMax, "12.8-2");
                }

            }
            else // long-period transition period segment
            {
                CsCurved = (SD1 * TL) / (Math.Pow(T, 2) * (R / Ie)); //Eq (12.8-4)

                #region CurvedLongPeriod
                ICalcLogEntry CurvedLongPeriodEntry = new CalcLogEntry();
                CurvedLongPeriodEntry.ValueName = "Cs";
                CurvedLongPeriodEntry.AddDependencyValue("SD1", Math.Round(SD1, 3));
                CurvedLongPeriodEntry.AddDependencyValue("TL", Math.Round(TL, 3));
                CurvedLongPeriodEntry.AddDependencyValue("R", Math.Round(R, 2));
                CurvedLongPeriodEntry.AddDependencyValue("Ie", Math.Round(Ie, 2));
                CurvedLongPeriodEntry.AddDependencyValue("T", Math.Round(T, 3));
                CurvedLongPeriodEntry.Reference = "Seismic response coefficient";
                CurvedLongPeriodEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCurvedLongPeriod.docx";
                CurvedLongPeriodEntry.FormulaID = "12.8-4"; //reference to formula from code
                CurvedLongPeriodEntry.VariableValue = Math.Round(CsCurved, 3).ToString(); 
                #endregion
                this.AddToLog(CurvedLongPeriodEntry);
            }
            // Check for minimum cutoff values

            CsCutoffZFourtyFourSds = 0.044 * SDS * Ie; //Eq (12.8-5)

            #region CsCutoffZFourtyFourCs
            ICalcLogEntry CsCutoffZFourtyFourCsEntry = new CalcLogEntry();
            CsCutoffZFourtyFourCsEntry.ValueName = "Cs";
            CsCutoffZFourtyFourCsEntry.AddDependencyValue("SDS", Math.Round(SDS, 3));
            CsCutoffZFourtyFourCsEntry.AddDependencyValue("Ie", Math.Round(Ie, 3));
            CsCutoffZFourtyFourCsEntry.Reference = "Seismic response coefficient";
            CsCutoffZFourtyFourCsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCutoffZFourtyFourCs.docx";
            CsCutoffZFourtyFourCsEntry.FormulaID = "12.8-5"; //reference to formula from code
            CsCutoffZFourtyFourCsEntry.VariableValue =  Math.Round(CsCutoffZFourtyFourSds, 3).ToString(); ;
            #endregion
            this.AddToLog(CsCutoffZFourtyFourCsEntry);

            CsCutoffOnePercent = 0.01;
            #region CsCutoffOnePercent
            ICalcLogEntry CsCutoffOnePercentEntry = new CalcLogEntry();
            CsCutoffOnePercentEntry.ValueName = "Cs";
            CsCutoffOnePercentEntry.Reference = "Seismic response coefficient";
            CsCutoffOnePercentEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCutoffOnePercent.docx";
            CsCutoffOnePercentEntry.FormulaID = null; //reference to formula from code
            CsCutoffOnePercentEntry.VariableValue = CsCutoffOnePercent.ToString();
            #endregion
            this.AddToLog(CsCutoffOnePercentEntry);

            //Select which cutoff applies both Eq (12.8-5) 
            if (CsCutoffZFourtyFourSds > CsCutoffOnePercent)
            {
                CsCutoffOnePercentAndZFourtyFourSds = CsCutoffZFourtyFourSds;

                #region CsCutoffZFourtyFourSds Governs
                ICalcLogEntry CsCutoffZFourtyFourSdsGov = new CalcLogEntry();
                CsCutoffZFourtyFourSdsGov.ValueName = "Cs";
                CsCutoffZFourtyFourSdsGov.AddDependencyValue("CsCutoffZFourtyFourSds", Math.Round(CsCutoffZFourtyFourSds, 3));
                CsCutoffZFourtyFourSdsGov.AddDependencyValue("CsCutoffOnePercent", Math.Round(CsCutoffOnePercent, 3));
                CsCutoffZFourtyFourSdsGov.Reference = "Seismic response coefficient";
                CsCutoffZFourtyFourSdsGov.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCsCutoffZFourtyFourSdsGoverns.docx";
                CsCutoffZFourtyFourSdsGov.FormulaID = null; //reference to formula from code
                CsCutoffZFourtyFourSdsGov.VariableValue = Math.Round(CsCutoffOnePercentAndZFourtyFourSds, 3).ToString(); 
                #endregion
                this.AddToLog(CsCutoffZFourtyFourSdsGov);
            }
            else
            {
                CsCutoffOnePercentAndZFourtyFourSds = CsCutoffOnePercent;

                #region CsCutoffOnePercent Governs
                ICalcLogEntry CsCutoffOnePercentGovEntry = new CalcLogEntry();
                CsCutoffOnePercentGovEntry.ValueName = "Cs";
                CsCutoffOnePercentGovEntry.AddDependencyValue("CsCutoffZFourtyFourSds", Math.Round(CsCutoffZFourtyFourSds, 3));
                CsCutoffOnePercentGovEntry.AddDependencyValue("CsCutoffOnePercent", Math.Round(CsCutoffOnePercent, 3));
                CsCutoffOnePercentGovEntry.Reference = "";
                CsCutoffOnePercentGovEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCsCutoffOnePercentGoverns.docx";
                CsCutoffOnePercentGovEntry.FormulaID = null; //reference to formula from code
                CsCutoffOnePercentGovEntry.VariableValue = Math.Round(CsCutoffOnePercentAndZFourtyFourSds, 3).ToString(); 
                #endregion
                this.AddToLog(CsCutoffOnePercentGovEntry);

            }



            //Special case for high-seismic regions
            if (S1 >= 0.6)
            {
                CsCutoffHighSeismic = 0.5 * S1 / (R / Ie); // Eq (12.8-6)

                #region CsCutoffHighSeismic
                ICalcLogEntry CsCutoffHighSeismicEntry = new CalcLogEntry();
                CsCutoffHighSeismicEntry.ValueName = "Cs";
                CsCutoffHighSeismicEntry.AddDependencyValue("Sone", Math.Round(S1, 3));
                CsCutoffHighSeismicEntry.AddDependencyValue("R", Math.Round(R, 3));
                CsCutoffHighSeismicEntry.AddDependencyValue("Ie", Math.Round(Ie, 3));
                CsCutoffHighSeismicEntry.Reference = "Seismic response coefficient";
                CsCutoffHighSeismicEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCsCutoffHighSeismic.docx";
                CsCutoffHighSeismicEntry.FormulaID = "12.8-6"; //reference to formula from code
                CsCutoffHighSeismicEntry.VariableValue = CsCutoffHighSeismic.ToString();
                #endregion
                this.AddToLog(CsCutoffHighSeismicEntry);

                //determine if high-seismic cutoff value governs over regular provisions
                if (CsCutoffHighSeismic > CsCutoffOnePercentAndZFourtyFourSds)
                {
                    AddGoverningCaseMaxLogEntry("Cs", CsCutoffHighSeismic, CsCutoffOnePercentAndZFourtyFourSds, "12.8-6");
                    CsCutoff = CsCutoffHighSeismic;
                }
                else
                {
                    AddGoverningCaseMaxLogEntry("Cs", CsCutoffOnePercentAndZFourtyFourSds, CsCutoffHighSeismic, "12.8-5");
                    CsCutoff = CsCutoffOnePercentAndZFourtyFourSds;
                }

            }
            else
            {
                CsCutoff = CsCutoffOnePercentAndZFourtyFourSds;
            }


            //Determine if cutoff governs
            Cs = Cs > CsCutoff ? Cs : CsCutoff;

            #region CsFinal
            ICalcLogEntry CsFinalEntry = new CalcLogEntry();
            CsFinalEntry.ValueName = "Cs";
            CsFinalEntry.Reference = "Seismic Response Coefficient";
            CsFinalEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicResponseCoefficientCsGoverning.docx";
            CsFinalEntry.FormulaID = null; //reference to formula from code
            CsFinalEntry.VariableValue = Math.Round(Cs, 4).ToString();

            #endregion
            this.AddToLog(CsFinalEntry);
            
            
            

            return Cs;
        }
    }
}

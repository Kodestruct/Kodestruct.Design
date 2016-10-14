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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Building : SeismicLateralForceResistingStructure
    {
        public double GetApproximatePeriodShearWall(List<ApproximatePeriodShearWallInfo> Walls, double BaseArea, double Height)
        {
            double Ta = 0.0;
            double Cw = 0.0;

            double hn = Height;
            double A_B = BaseArea;
            double CwSum = 0.0;

            List<List<string>> ReportTableData = new List<List<string>>();

            foreach (ApproximatePeriodShearWallInfo wall in Walls)
            {
                //Summation term in 12.8-10
                double hi = wall.Height;
                double Ai = wall.WebArea;
                double Di = wall.Length;

                double CwThis = Math.Pow(hn / hi, 2.0) * Ai / (1 + 0.83 * Math.Pow(hi / Di, 2.0));
                CwSum += CwThis;
                #region report part

                List<string> row = new List<string>()
                    {
                       Math.Round(hi,3).ToString(),Math.Round(Di,3).ToString(),Math.Round(Ai,2).ToString(),Math.Round(CwThis,3).ToString()
                    };
                ReportTableData.Add(row);

                #endregion
            }
            //12.8-10
            Cw = 100.0 / A_B * CwSum;
            Ta = 0.0019 / Math.Sqrt(Cw) * hn;

            
            #region Ta
            ICalcLogEntry TaEntry = new CalcLogEntry();
            TaEntry.ValueName = "Ta";
            TaEntry.AddDependencyValue("Cw", Math.Round(Cw, 3));
            TaEntry.AddDependencyValue("AeffSum", Math.Round(CwSum, 3));
            TaEntry.AddDependencyValue("hn", Math.Round(hn, 3));
            TaEntry.AddDependencyValue("AB", Math.Round(A_B, 3));
            TaEntry.TableData = ReportTableData;
            TaEntry.TemplateTableTitle = "W";
            TaEntry.Reference = "";
            TaEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicApproximatePeriodShearWallTa.docx";
            TaEntry.FormulaID = null; //reference to formula from code
            TaEntry.VariableValue = Math.Round(Ta, 3).ToString();
            #endregion
            this.AddToLog(TaEntry);

            return Ta;
        }
    }
}

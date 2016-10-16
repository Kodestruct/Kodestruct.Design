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
using System.Globalization;
using System.IO;
using System.Linq;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Building : SeismicLateralForceResistingStructure
    {

        public GeneralProcedureDataInfo GeneralProcedureData { get; set; }

        public double GetApproximatePeriodGeneral(double hn, SeismicSystemTypeForApproximateAnalysis System)
        {
            this.GeneralProcedureData = new GeneralProcedureDataInfo();

            #region Read Table 12.8-2 Values of Approximate Period Parameters Ct and x

            var Tv2 = new { System = SeismicSystemTypeForApproximateAnalysis.OtherStructuralSystem, Ct = 0.0, x = 0.0 }; // sample
            var ApproximatePeriodParameterList = ListFactory.MakeList(Tv2);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T12_8_2))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        SeismicSystemTypeForApproximateAnalysis system =
                            (SeismicSystemTypeForApproximateAnalysis)Enum.Parse(typeof(SeismicSystemTypeForApproximateAnalysis), Vals[0]);
                        double ct = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                        double _x = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                        ApproximatePeriodParameterList.Add(new { System = system, Ct = ct, x = _x });
                    }
                }

            }

            #endregion

            var Params = from p in ApproximatePeriodParameterList where p.System == System select p;
            var ParamResultList = (Params.ToList());
            var thisSysParams = ParamResultList[0];
            

            double Ta;
            double Ct = thisSysParams.Ct; 
            GeneralProcedureData.Ct = Ct; //store off for unit testing
            double x = thisSysParams.x;
            GeneralProcedureData.x = x; //store off for unit testing
            Ta = Ct * Math.Pow(hn, x);

            #region Ta
            ICalcLogEntry TaEntry = new CalcLogEntry();
            TaEntry.ValueName = "Ta";
            TaEntry.AddDependencyValue("Ct", Math.Round(Ct, 3));
            TaEntry.AddDependencyValue("hn", Math.Round(hn, 3));
            TaEntry.AddDependencyValue("x", Math.Round(x, 3));
            TaEntry.Reference = "";
            TaEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicApproximatePeriodGeneralProcedureTa.docx";
            TaEntry.FormulaID = null; //reference to formula from code
            TaEntry.VariableValue = Math.Round(Ta, 3).ToString();
            #endregion
            this.AddToLog(TaEntry);

            return Ta;
        }

        public class GeneralProcedureDataInfo
        {
            public double Ct { get; set; }
            public double x { get; set; }
        }
    }
}

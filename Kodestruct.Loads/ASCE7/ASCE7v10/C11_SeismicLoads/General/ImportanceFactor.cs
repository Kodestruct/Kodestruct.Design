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
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class General
    {
        public double GetImportanceFactor(BuildingRiskCategory RiskCategory)
        {
            double Ie = 1.0;

            switch (RiskCategory)
            {
                case BuildingRiskCategory.I:
                    Ie = 1.0;
                    break;
                case BuildingRiskCategory.II:
                    Ie = 1.0;
                    break;
                case BuildingRiskCategory.III:
                    Ie = 1.25;
                    break;
                case BuildingRiskCategory.IV:
                    Ie = 1.5;
                    break;
            }
            
            #region Ie
            ICalcLogEntry IeEntry = new CalcLogEntry();
            IeEntry.ValueName = "Ie";
            IeEntry.AddDependencyValue("RiskCategory", RiskCategory.ToString());
            IeEntry.Reference = "";
            IeEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicImportanceFactorIe.docx";
            IeEntry.FormulaID = "Table 1.5-2"; //reference to formula from code
            IeEntry.VariableValue = Ie.ToString();
            #endregion
            this.AddToLog(IeEntry);

            return Ie;

        }
    }
}

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
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.IceLoads
{
    public partial class IceStructure : AnalyticalElement
    {
        public double GetImportanceFactor(BuildingRiskCategory RiskCategory)
        {
            double Ii = 1.0;

            switch (RiskCategory)
            {
                case BuildingRiskCategory.I:
                    Ii = 0.8;
                    break;
                case BuildingRiskCategory.II:
                    Ii = 1.0;
                    break;
                case BuildingRiskCategory.III:
                    Ii = 1.25;
                    break;
                case BuildingRiskCategory.IV:
                    Ii = 1.25;
                    break;
            }

            
            #region Ii
            ICalcLogEntry IiEntry = new CalcLogEntry();
            IiEntry.ValueName = "Ii";
            IiEntry.AddDependencyValue("RiskCategory", RiskCategory.ToString());
            IiEntry.Reference = "";
            IiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/IceImportanceFactor.docx";
            IiEntry.FormulaID = null; //reference to formula from code
            IiEntry.VariableValue = Math.Round(Ii, 3).ToString();
            #endregion
            this.AddToLog(IiEntry);

            return Ii;
        }
    }
}

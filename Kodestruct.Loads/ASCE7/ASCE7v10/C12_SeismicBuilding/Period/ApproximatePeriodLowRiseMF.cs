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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Building : SeismicLateralForceResistingStructure
    {
        public double GetApproximatePeriodLowRiseMF(double NFloors)
        {
            double Ta = 0.1 * NFloors;
            
            #region Ta
            ICalcLogEntry TaEntry = new CalcLogEntry();
            TaEntry.ValueName = "Ta";
            TaEntry.AddDependencyValue("NFloors", Math.Round(NFloors, 1));
            TaEntry.Reference = "";
            TaEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicApproximatePeriodLowRiseMFTa.docx";
            TaEntry.FormulaID = null; //reference to formula from code
            TaEntry.VariableValue = Math.Round(Ta, 3).ToString();
            #endregion

            this.AddToLog(TaEntry);
            return Ta;
        }
    }
}

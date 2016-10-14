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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicLateralForceResistingStructure : AnalyticalElement
    {
        public double GetBuildingPeriodExponent_k(double T)
        {
            double k;

            ICalcLogEntry kEntry = new CalcLogEntry();
            kEntry.ValueName = "k";
            kEntry.Reference = "";
            kEntry.FormulaID = null; //reference to formula from code


            this.AddToLog(kEntry);

            if (T<=0.5)
            {
                k = 1.0;
                kEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicPeriodExponent_k_Tless0_5.docx";
                //
                
            }
            else if (T >= 2.5)
            {
                k = 2.0;
                kEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicPeriodExponent_k_Tmore2_5.docx";
            }
            else
            {
                k = 0.5 * T + 0.75;
                kEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicPeriodExponent_k_TInterpolated.docx";
                kEntry.AddDependencyValue("T", Math.Round(T, 3));

            }

            kEntry.VariableValue = Math.Round(k, 3).ToString();
            this.AddToLog(kEntry);
            
            return k;
        }
    }
}

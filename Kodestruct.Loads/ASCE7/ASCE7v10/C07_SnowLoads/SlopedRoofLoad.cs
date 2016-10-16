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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SnowLoads
{
    public partial class SnowStructure
    {

        public  double GetSlopedRoofLoad(double Cs, double pf)
        {
            double ps = Cs * pf;
            
            #region ps
            ICalcLogEntry psEntry = new CalcLogEntry();
            psEntry.ValueName = "ps";
            psEntry.AddDependencyValue("Cs", Math.Round(Cs, 3));
            psEntry.AddDependencyValue("pf", Math.Round(pf, 3));
            psEntry.Reference = "";
            psEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SlopedRoofSnowLoad.docx";
            psEntry.FormulaID = null; //reference to formula from code
            psEntry.VariableValue = Math.Round(ps, 3).ToString();
            #endregion
            this.AddToLog(psEntry);

            double p_snow = ps;

            return p_snow;


        }
    }
}

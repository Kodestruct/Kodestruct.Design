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
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Loads.ASCE.ASCE7_10.SnowLoads
{
    public partial class SnowStructure
    {
        public double GetFlatRoofLoad(double Ce, double Ct, double Is, double pg, double pm)
        {
            double pf;
            pf = 0.7 * Ce * Ct * Is * pg;
            
            #region pf
            ICalcLogEntry pfEntry = new CalcLogEntry();
            pfEntry.ValueName = "pf";
            pfEntry.AddDependencyValue("Ce", Math.Round(Ce, 3));
            pfEntry.AddDependencyValue("Ct", Math.Round(Ct, 3));
            pfEntry.AddDependencyValue("Is", Math.Round(Is, 3));
            pfEntry.AddDependencyValue("pg", Math.Round(pg, 3));
            pfEntry.Reference = "";
            pfEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/FlatRoofSnowLoad.docx";
            pfEntry.FormulaID = null; //reference to formula from code
            pfEntry.VariableValue = Math.Round(pf, 3).ToString();
            #endregion
            this.AddToLog(pfEntry);

            double p_snow = pf;
            if (pm!=0.0)
            {
                if (pf>pm)
                {
                    p_snow = pf;
                }
                else
                {
                    p_snow = pm;
                    
                    #region pm
                    ICalcLogEntry pmEntry = new CalcLogEntry();
                    pmEntry.ValueName = "pf";
                    pmEntry.Reference = "";
                    pmEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/FlatRoofSnowLoadMin.docx";
                    pmEntry.FormulaID = null; //reference to formula from code
                    pmEntry.VariableValue = Math.Round(pm, 3).ToString();
                    #endregion
                    this.AddToLog(pmEntry);
                }
            }
            return p_snow;
        }
    }
}

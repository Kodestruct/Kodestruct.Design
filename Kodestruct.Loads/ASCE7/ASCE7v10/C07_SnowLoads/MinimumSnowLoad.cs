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

        public double GetMinimumSnowLoad(double slope, double pg, double Is)
        {
            double pm = 0.0;

            
            #region pm
            ICalcLogEntry pmEntry = new CalcLogEntry();
            pmEntry.ValueName = "pm";
            pmEntry.AddDependencyValue("pg", Math.Round(pg, 3));
            pmEntry.AddDependencyValue("Is", Math.Round(Is, 3));
            pmEntry.Reference = "";
            pmEntry.FormulaID = null; //reference to formula from code

            #endregion
            
            if (slope<=15.0)
            {
                if (pg <= 20.0)
                {
                    pm = Is * pg; ;
                    pmEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/GroundSnowMinimumLight.docx";
                }
                else
                {
                    pm = 20*(Is);
                    pmEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/GroundSnowMinimumHeavy.docx";
                } 
            }

            pmEntry.VariableValue = Math.Round(pm, 3).ToString();
            this.AddToLog(pmEntry);

            return pm;
        }
    }
}

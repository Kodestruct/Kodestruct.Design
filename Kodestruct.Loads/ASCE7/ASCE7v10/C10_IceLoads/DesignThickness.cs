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

namespace Kodestruct.Loads.ASCE.ASCE7_10.IceLoads
{
    public partial class IceStructure : AnalyticalElement
    {
        
        public double GetDesignThickness(double t, double Ii, double fz, double Kzt)
        {
            double td = 0.0 ;

            td = 2.0 * t * Ii * fz * Math.Pow(Kzt, 0.35);

            
            #region td
            ICalcLogEntry tdEntry = new CalcLogEntry();
            tdEntry.ValueName = "td";
            tdEntry.AddDependencyValue("t", Math.Round(t, 3));
            tdEntry.AddDependencyValue("Ii", Math.Round(Ii, 3));
            tdEntry.AddDependencyValue("fz", Math.Round(fz, 3));
            tdEntry.AddDependencyValue("Kzt", Math.Round(Kzt, 3));
            tdEntry.Reference = "";
            tdEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/DesignThickness.docx";
            tdEntry.FormulaID = null; //reference to formula from code
            tdEntry.VariableValue = Math.Round(td, 3).ToString();
            #endregion
            this.AddToLog(tdEntry);
            return td;
        }
    }
}

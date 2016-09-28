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
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.IceLoads
{
    public partial class IceStructure : AnalyticalElement
    {
        
        public double GetHeightFactor(double z)
        {
            double fz=1.0;
            if (z<=900)
            {
                fz = Math.Pow(z / 33, 0.1);
                
                #region fz
                ICalcLogEntry fzEntry = new CalcLogEntry();
                fzEntry.ValueName = "fz";
                fzEntry.AddDependencyValue("z", Math.Round(z, 3));
                fzEntry.Reference = "";
                fzEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/HeightFactor.docx";
                fzEntry.FormulaID = null; //reference to formula from code
                fzEntry.VariableValue = Math.Round(fz, 3).ToString();
                #endregion
                this.AddToLog(fzEntry);
            }
            else
            {
                fz = 1.4;
                
                #region fz
                ICalcLogEntry fzEntry = new CalcLogEntry();
                fzEntry.ValueName = "fz";
                fzEntry.AddDependencyValue("z", Math.Round(z, 3));
                fzEntry.Reference = "";
                fzEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/HeightFactorHigh.docx";
                fzEntry.FormulaID = null; //reference to formula from code
                fzEntry.VariableValue = Math.Round(fz, 3).ToString();
                #endregion
                this.AddToLog(fzEntry);
            }
            return fz;
        }
    }
}

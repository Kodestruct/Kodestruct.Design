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
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.IceLoads
{
    public partial class IceStructure : AnalyticalElement
    {
        
        public double GetIceCrossSectionArea(IceElementType elementType, double td, double Dc)
        {
            double Ai=0.0;
            if (elementType == IceElementType.Line)
            {
                Ai = Math.PI * td * (Dc + td);
                
                #region Ai
                ICalcLogEntry AiEntry = new CalcLogEntry();
                AiEntry.ValueName = "Ai";
                AiEntry.AddDependencyValue("td", Math.Round(td, 3));
                AiEntry.AddDependencyValue("Dc", Math.Round(Dc, 3));
                AiEntry.Reference = "";
                AiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/CrossSectionAreaLine.docx";
                AiEntry.FormulaID = null; //reference to formula from code
                AiEntry.VariableValue = Math.Round(Ai, 3).ToString();
                #endregion
                this.AddToLog(AiEntry);
            }
            else
            {
                Ai = 0.0;
                
                #region Ai
                ICalcLogEntry AiEntry = new CalcLogEntry();
                AiEntry.ValueName = "Ai";
                AiEntry.Reference = "";
                AiEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Ice/CrossSectionArea2dOr3d.docx";
                AiEntry.FormulaID = null; //reference to formula from code
                AiEntry.VariableValue = Math.Round(Ai, 3).ToString();
                #endregion
                this.AddToLog(AiEntry);
            }
            return Ai;
        }
    }
}

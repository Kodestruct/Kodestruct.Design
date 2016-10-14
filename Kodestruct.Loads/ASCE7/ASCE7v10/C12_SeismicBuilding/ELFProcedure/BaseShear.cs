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
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicLateralForceResistingStructure : AnalyticalElement
    {
        public double GetBaseShearVb(double Cs, double W)
        {

            double Vb = W * Cs;

            #region Vb
            ICalcLogEntry VbEntry = new CalcLogEntry();
            VbEntry.ValueName = "Vb";
            VbEntry.AddDependencyValue("W", Math.Round(W, 3));
            VbEntry.AddDependencyValue("Cs", Math.Round(Cs, 3));
            VbEntry.Reference = "Base shear";
            VbEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicBaseShear.docx";
            VbEntry.FormulaID = "12.8-1"; //reference to formula from code
            VbEntry.VariableValue = Math.Round(Vb,3).ToString();
            #endregion

            this.AddToLog(VbEntry);
            return Vb;
        }
    }
}

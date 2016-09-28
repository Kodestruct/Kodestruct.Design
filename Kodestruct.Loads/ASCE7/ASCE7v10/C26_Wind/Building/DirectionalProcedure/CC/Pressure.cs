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
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.CC
{
    public partial class ComponentOrCladding : BuildingDirectionalProcedureElement
    {

        public double GetDesignPressure(double q, double GCpPos, double GCpNeg, double qi, double GCpi, double h)
        {
            double p1;
            double p2;

            if (h<=60.0)
            {
               p1 = q * (GCpPos + GCpi);
               p2 = q * (GCpNeg - GCpi); 
            }
            else
            {
                p1 = q * GCpPos + qi * GCpi;
                p2 = q * GCpNeg - qi * GCpi; 
            }
            double p = Math.Max(Math.Abs(p1), Math.Abs(p2));

            #region p
            ICalcLogEntry pEntry = new CalcLogEntry();
            pEntry.ValueName = "p";
            pEntry.AddDependencyValue("p1", Math.Round(p1, 3));
            pEntry.AddDependencyValue("p2", Math.Round(p2, 3));
            pEntry.AddDependencyValue("q", Math.Round(q, 3));
            pEntry.AddDependencyValue("qi", Math.Round(qi, 3));
            pEntry.AddDependencyValue("h", Math.Round(h, 3));
            pEntry.AddDependencyValue("GCpPos", Math.Round(GCpPos, 3));
            pEntry.AddDependencyValue("GCpNeg", Math.Round(GCpNeg, 3));
            pEntry.AddDependencyValue("GCpi", Math.Round(GCpi, 3));

            pEntry.Reference = "";
            if (h<=60.0)
            {
                pEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindPressure/WindPressureCCLowRise.docx";
            }
            else
            {
                pEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/WindPressure/WindPressureCCLowRise.docx";
            }
            
            pEntry.FormulaID = null; //reference to formula from code
            pEntry.VariableValue = Math.Round(p, 3).ToString();
            #endregion
            this.AddToLog(pEntry);
            return p;
        }
    }
}

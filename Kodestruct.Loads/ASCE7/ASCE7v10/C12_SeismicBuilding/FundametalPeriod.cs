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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Building : SeismicLateralForceResistingStructure
    {
        public double GetMaximumPeriod(double TApr, double Cu)
        {
           double  Tmax = Cu * TApr;

            #region Tmax
           ICalcLogEntry TmaxEntry = new CalcLogEntry();
           TmaxEntry.ValueName = "Tmax";
           TmaxEntry.AddDependencyValue("Cu", Math.Round(Cu, 3));
           TmaxEntry.AddDependencyValue("Ta", Math.Round(TApr, 3));
           TmaxEntry.Reference = "";
           TmaxEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicFundamentalPeriodTmax.docx";
           TmaxEntry.FormulaID = "12.8.2 Period Determination"; //reference to formula from code
           TmaxEntry.VariableValue = Math.Round(Tmax, 3).ToString();
           #endregion
           this.AddToLog(TmaxEntry);
           return Tmax;
        }

        public double GetFundamentalPeriod(double Tcalculated, double Tmax)
        {
            double T = 0;
            if (Tcalculated < Tmax)
	        {
                T = Tcalculated;

                #region TcalcGoverns
                ICalcLogEntry TcalcGovernsEntry = new CalcLogEntry();
                TcalcGovernsEntry.ValueName = "T";
                TcalcGovernsEntry.AddDependencyValue("Tmax", Math.Round(Tmax, 3));
                TcalcGovernsEntry.AddDependencyValue("Tcalculated", Math.Round(Tcalculated, 3));
                TcalcGovernsEntry.Reference = "";
                TcalcGovernsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicFundamentalPeriodTcalcGoverns.docx";
                TcalcGovernsEntry.FormulaID = null; //reference to formula from code
                TcalcGovernsEntry.VariableValue = Math.Round(T, 3).ToString();
                #endregion
                this.AddToLog(TcalcGovernsEntry);
	        }
                    else
	        {
                T=Tmax;

                #region TmaxGoverns
                ICalcLogEntry TmaxGovernsEntry = new CalcLogEntry();
                TmaxGovernsEntry.ValueName = "T";
                TmaxGovernsEntry.AddDependencyValue("Tmax", Math.Round(Tmax, 3));
                TmaxGovernsEntry.AddDependencyValue("Tcalculated", Math.Round(Tcalculated, 3));
                TmaxGovernsEntry.Reference = "";
                TmaxGovernsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicFundamentalPeriodTmaxGoverns.docx";
                TmaxGovernsEntry.FormulaID = null; //reference to formula from code
                TmaxGovernsEntry.VariableValue = Math.Round(T, 3).ToString();
                #endregion
                this.AddToLog(TmaxGovernsEntry);


	        }

            return T;
        }
    }
}

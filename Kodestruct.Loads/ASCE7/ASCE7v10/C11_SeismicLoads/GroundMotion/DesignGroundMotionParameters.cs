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
    public partial class SeismicLocation : AnalyticalElement
    {

        public double CalculateSDS(double SS, double Fa)
        {
            double SMS = Fa * SS; //(11.4-1)

            double SDS = 2.0 / 3.0 * SMS; //(11.4-3)


            #region SMS
            ICalcLogEntry SMSEntry = new CalcLogEntry();
            SMSEntry.ValueName = "SMS";
            SMSEntry.AddDependencyValue("Fa", Math.Round(Fa, 2));
            SMSEntry.AddDependencyValue("SS", Math.Round(SS, 3));
            SMSEntry.Reference = "The MCER spectral response acceleration parameter for short periods";
            SMSEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSMS.docx";
            SMSEntry.FormulaID = null; //(11.4-1)
            SMSEntry.VariableValue = Math.Round(SMS,3).ToString();
            #endregion
            this.AddToLog(SMSEntry);

            #region SDS
            ICalcLogEntry SDSEntry = new CalcLogEntry();
            SDSEntry.ValueName = "SDS";
            SDSEntry.AddDependencyValue("SMS", Math.Round(SMS, 3));
            SDSEntry.Reference = "Design earthquake spectral response acceleration parameter for short periods";
            SDSEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSDS.docx";
            SDSEntry.FormulaID = null; //(11.4-3)
            SDSEntry.VariableValue = Math.Round(SDS,3).ToString();
            #endregion
            this.AddToLog(SDSEntry);

            return SDS;

        }
        public double CalculateSD1(double S1, double Fv)
        {

            double SM1 = Fv * S1;  //(11.4-2)

            double SD1 = 2.0 / 3.0 * SM1; //(11.4-4)

            #region SM1
            ICalcLogEntry SM1Entry = new CalcLogEntry();
            SM1Entry.ValueName = "SM1";
            SM1Entry.AddDependencyValue("Fv", Math.Round(Fv, 2));
            SM1Entry.AddDependencyValue("Sone", Math.Round(S1, 3));
            SM1Entry.Reference = "The MCER spectral response acceleration parameter at 1 second";
            SM1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSM1.docx";
            SM1Entry.FormulaID = null; //(11.4-2)
            SM1Entry.VariableValue = Math.Round(SM1,3).ToString();
            #endregion


            #region SD1
            ICalcLogEntry SD1Entry = new CalcLogEntry();
            SD1Entry.ValueName = "SD1";
            SD1Entry.AddDependencyValue("SM1", Math.Round(SM1, 3));
            SD1Entry.Reference = "Design earthquake spectral response acceleration parameter at 1 second";
            SD1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSD1.docx";
            SD1Entry.FormulaID = null; //(11.4-4)
            SD1Entry.VariableValue = Math.Round(SD1,3).ToString();
            #endregion

            // set entry in Log

            this.AddToLog(SM1Entry);
            this.AddToLog(SD1Entry);

            return SD1;
        }


    }
}

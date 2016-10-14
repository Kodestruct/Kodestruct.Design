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
using Kodestruct.Loads.ASCE7.Entities;
using data = Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class General : AnalyticalElement
    {


        public data.SeismicDesignCategory Get1secSeismicDesignCategory(double SD1, BuildingRiskCategory RiskCategory)
        {
            data.SeismicDesignCategory CategorySD1 = data.SeismicDesignCategory.A;

            //TODO: see special case when SDC is permitted  to be determined from Table 11.6-1 alone

            # region Table 11.6-2 Seismic Design Category Based on 1-S Period Response Acceleration Parameter

            if (SD1 < 0.067)
            {
                CategorySD1 = data.SeismicDesignCategory.A;
            }
            if (SD1 >= 0.067 && SD1 < 0.133)
            {
                if (RiskCategory == BuildingRiskCategory.IV)
                {
                    CategorySD1 = data.SeismicDesignCategory.C;
                }
                else
                {
                    CategorySD1 = data.SeismicDesignCategory.B;
                }

            }
            if (SD1 >= 0.133 && SD1 < 0.2)
            {
                if (RiskCategory == BuildingRiskCategory.IV)
                {
                    CategorySD1 = data.SeismicDesignCategory.D;
                }
                else
                {
                    CategorySD1 = data.SeismicDesignCategory.C;
                }

            }
            if (SD1 >= 0.2)
            {
                CategorySD1 = data.SeismicDesignCategory.D;
            }
            #endregion

            ICalcLogEntry Sd1Entry = new CalcLogEntry();
            Sd1Entry.ValueName = "CategorySD1";
            Sd1Entry.AddDependencyValue("SD1", Math.Round(SD1, 4));
            Sd1Entry.AddDependencyValue("RiskCategory", RiskCategory.ToString());
            Sd1Entry.Reference = "Seismic design rategory";
            Sd1Entry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSDC_1S.docx";
            Sd1Entry.FormulaID = "Table 11.6-2"; //reference to formula from code
            Sd1Entry.VariableValue = CategorySD1.ToString();
            this.AddToLog(Sd1Entry);

            return CategorySD1;
        }
    }
}

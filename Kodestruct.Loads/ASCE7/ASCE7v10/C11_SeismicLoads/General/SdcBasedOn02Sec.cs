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
using data = Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class General : AnalyticalElement
    {

        public  data.SeismicDesignCategory Get02secSeismicDesignCategory(double SDS, BuildingRiskCategory RiskCategory)
        {
            data.SeismicDesignCategory CategorySDS = data.SeismicDesignCategory.A;

            //TODO: see special case when SDC is permitted  to be determined from Table 11.6-1 alone

            # region Table 11.6-1 Seismic Design Category Based on Short Period Response Acceleration Parameter

            if (SDS < 0.167)
            {
                CategorySDS = data.SeismicDesignCategory.A;
            }
            if (SDS >= 0.167 && SDS < 0.33)
            {
                if (RiskCategory == BuildingRiskCategory.IV)
                {
                    CategorySDS = data.SeismicDesignCategory.C;
                }
                else
                {
                    CategorySDS = data.SeismicDesignCategory.B;
                }

            }
            if (SDS >= 0.33 && SDS < 0.5)
            {
                if (RiskCategory == BuildingRiskCategory.IV)
                {
                    CategorySDS = data.SeismicDesignCategory.D;
                }
                else
                {
                    CategorySDS = data.SeismicDesignCategory.C;
                }

            }
            if (SDS >= 0.5)
            {
                CategorySDS = data.SeismicDesignCategory.D;
            }
            #endregion
            
            #region Sds
            ICalcLogEntry SdsEntry = new CalcLogEntry();
            SdsEntry.ValueName = "CategorySDS";
            SdsEntry.AddDependencyValue("SDS", Math.Round(SDS, 4));
            SdsEntry.AddDependencyValue("RiskCategory", RiskCategory.ToString());
            SdsEntry.Reference = "Seismic design rategory";
            SdsEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSDC_02S.docx";
            SdsEntry.FormulaID = "Table 11.6-1"; //reference to formula from code
            SdsEntry.VariableValue = CategorySDS.ToString();
            #endregion
            this.AddToLog(SdsEntry);

            return CategorySDS;

        }
    }
}

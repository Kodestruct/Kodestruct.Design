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
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.General
{
    public partial class Structure : AnalyticalElement
    {
        public BuildingRiskCategory GetRiskCategory(string structureCategoryByOccupancyId)
        {

            #region Read Category Data (ASCE Table 1.5-1)

            var SampleValue = new { OccupancyId = "", OccupancyDescription = "", RiskCategory = "" }; // sample
            var RiskCategoryList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T1_5_1RiskCategories))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        string OccupId = (string)Vals[0];
                        string OccupancyDescription = (string)Vals[1];
                        string RiskCat = (string)Vals[2];


                        RiskCategoryList.Add(new
                        {
                            OccupancyId = OccupId,
                            OccupancyDescription = OccupancyDescription,
                            RiskCategory = RiskCat
                        });
                    }
                }

            }

            #endregion

            var dataValues = from riskCat in RiskCategoryList where (riskCat.OccupancyId == structureCategoryByOccupancyId) select riskCat;
            var resultCategory = dataValues.ToList().First();

            string parsedCategoryValue = resultCategory.RiskCategory;
            string parsedOccupancyDescription = resultCategory.OccupancyDescription;

            BuildingRiskCategory riskCategory = BuildingRiskCategory.None;
            BuildingRiskCategory tmpParsedVal;
            if (Enum.TryParse<BuildingRiskCategory>(parsedCategoryValue, false, out tmpParsedVal) == true)
            {
                riskCategory = tmpParsedVal;
            }

            
            #region RiskCategory
            ICalcLogEntry riskCategoryEntry = new CalcLogEntry();
            riskCategoryEntry.ValueName = "RiskCategory";
            riskCategoryEntry.AddDependencyValue("OccupancyDescription", parsedOccupancyDescription);
            riskCategoryEntry.Reference = "Risk Category";
            riskCategoryEntry.DescriptionReference = "/Templates/General/RiskCategory.docx";
            riskCategoryEntry.FormulaID = "Table 1.5-1"; //reference to formula from code
            riskCategoryEntry.VariableValue = riskCategory.ToString();
            #endregion
            this.AddToLog(riskCategoryEntry);

            return riskCategory;
        }
    }
}

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
using System.Globalization;
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

namespace Kodestruct.Loads.ASCE.ASCE7_10.SnowLoads
{
    public partial class SnowStructure
    {
        public double GetExposureFactor(WindExposureCategory windExposureCategory,SnowRoofExposure RoofExposure)
        {

            var Tv11 = new { Exposure = WindExposureCategory.B, SnowRoofExposure= SnowRoofExposure.PartiallyExposed, Ce = 0.0}; // sample
            var ValueList = ListFactory.MakeList(Tv11);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T7_2SnowCe))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        WindExposureCategory _exp = (WindExposureCategory)Enum.Parse(typeof(WindExposureCategory), Vals[0]);
                        SnowRoofExposure _SnoExp = (SnowRoofExposure)Enum.Parse(typeof(SnowRoofExposure), Vals[1]);
                        double _Ce = double.Parse(Vals[2], CultureInfo.InvariantCulture);

                        ValueList.Add(new { Exposure = _exp, SnowRoofExposure = _SnoExp, Ce = _Ce });
                    }
                }

            }

            var CeValues = from sc in ValueList where (sc.Exposure == windExposureCategory && sc.SnowRoofExposure == RoofExposure) select sc;
            var Ce = CeValues.ToList().FirstOrDefault().Ce;

            string ExposureCategoryString = GetExposureCategoryString(windExposureCategory);
            string RoofExposureString = GetRoofExposureString(RoofExposure);

            
            #region Ce
            ICalcLogEntry CeEntry = new CalcLogEntry();
            CeEntry.ValueName = "Ce";
            CeEntry.AddDependencyValue("WindExposureCategory", ExposureCategoryString);
            CeEntry.AddDependencyValue("RoofExposure", RoofExposureString);
            CeEntry.Reference = "";
            CeEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Snow/SnowExposureFactor.docx";
            CeEntry.FormulaID = null; //reference to formula from code
            CeEntry.VariableValue = Math.Round(Ce, 3).ToString();
            #endregion
            this.AddToLog(CeEntry);

            return Ce;
        }

        private string GetRoofExposureString(SnowRoofExposure RoofExposure)
        {
            string RoofExposureStr = "";
            switch (RoofExposure)
            {
                case SnowRoofExposure.FullyExposed:
                    RoofExposureStr = "fully exposed";
                    break;
                case SnowRoofExposure.PartiallyExposed:
                    RoofExposureStr = "partially exposed";
                    break;
                case SnowRoofExposure.Sheltered:
                    RoofExposureStr = "sheltered";
                    break;
            }
            return RoofExposureStr;
        }

        private string GetExposureCategoryString(WindExposureCategory windExposureCategory)
        {
            if (windExposureCategory== WindExposureCategory.AboveTreelineMountain)
            {
                return "above the treeline in windswept mountainous areas";
            }
            else
            {
                return windExposureCategory.ToString();
            }
        }
    }

}

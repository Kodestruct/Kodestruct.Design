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
        public data.SeismicDesignCategory GetSeismicDesignCategory(BuildingRiskCategory RiskCategory,double SDS, double SD1, double S1 )
        {
            data.SeismicDesignCategory SDC, CategoryS1, CategorySDS, CategorySD1;
            CategorySDS = Get02secSeismicDesignCategory(SDS, RiskCategory);
            CategorySD1 = Get1secSeismicDesignCategory(SD1, RiskCategory);


            if (S1 >= 0.75)
            {
                if (RiskCategory == BuildingRiskCategory.IV)
                {
                    CategoryS1 = data.SeismicDesignCategory.F;
                }
                else
                {
                    CategoryS1 = data.SeismicDesignCategory.E;
                }

                //High-seismic design category
                SDC = CategoryS1;
            }
            else
            {
                if ((int)CategorySDS >= (int)CategorySD1)
                {
                    SDC = CategorySDS;
                   

                }
                else
                {
                    SDC = CategorySD1;

                }

            }
            return SDC;
        }
    }
}

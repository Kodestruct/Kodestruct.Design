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
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Building : SeismicLateralForceResistingStructure
    {

        public double GetCoefficientForUpperBoundOnCalculatedPeriod(double SD1)
        {
            double UpperLimitCoefficientCU = 0.0;

            #region Read Table 12.8-1 Coefficient for Upper Limit on Calculated Period

            var Tv1 = new { SD1 = 0.0, CU = 0.0 }; // sample
            var UpperLimitCoefficientList = ListFactory.MakeList(Tv1);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T12_8_1))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 2)
                    {
                        double sd1 = double.Parse(Vals[0], CultureInfo.InvariantCulture);
                        double cu = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                        UpperLimitCoefficientList.Add(new { SD1 = sd1, CU = cu });
                    }
                }

            }

            #endregion

            var ResultList = (UpperLimitCoefficientList.ToList());    //create list (LINQ immediate execution)
            var MinSValue = UpperLimitCoefficientList.Min(CuVal => CuVal.SD1);
            var MaxSValue = UpperLimitCoefficientList.Max(CuVal => CuVal.SD1);

            // Check for extreme values
            if (SD1 <= MinSValue)
            {
                var MinEntry = from CU in UpperLimitCoefficientList where (CU.SD1 == MinSValue) select CU;
                UpperLimitCoefficientCU = MinEntry.ElementAt(0).CU;
            }
            if (SD1 >= MaxSValue)
            {
                var MaxEntry = from CU in UpperLimitCoefficientList where (CU.SD1 == MaxSValue) select CU;
                UpperLimitCoefficientCU = MaxEntry.ElementAt(0).CU;
            }

            if (SD1 > MinSValue && SD1 < MaxSValue)
            {

                //Intermediate values
                int NumEntries = ResultList.Count();
                for (int i = 0; i < NumEntries; i++)
                {
                    var thisVal = ResultList[i];
                    if (i > 0 && i < NumEntries - 1)
                    {
                        if (SD1 <= ResultList[i - 1].SD1 && SD1 >= ResultList[i + 1].SD1)
                        {
                            UpperLimitCoefficientCU = Interpolation.InterpolateLinear(ResultList[i - 1].SD1, ResultList[i - 1].CU, ResultList[i + 1].SD1, ResultList[i + 1].CU, SD1);
                        }
                    }
                }

            }


            return UpperLimitCoefficientCU;
        }

    }
}

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
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {

        public double GetK1(WindExposureCategory windExposureCategory, TopographyType TopographyType, double HToLh)
        {
            double K1OverHToLh = GetK1OverHToLh(windExposureCategory, TopographyType, HToLh);
            double K1 = K1OverHToLh * HToLh;
            return K1;
        }

        public double GetK1(double K1OverHToLh, double HToLh)
        {
            double K1 = K1OverHToLh * HToLh;
            return K1;
        }
        public double GetK1OverHToLh(WindExposureCategory windExposureCategory, TopographyType TopographyType, double HToLh)
        {

            #region Read Table

            var SampleValue = new { ExposureCategory = "", TopographyType = "", K1OverHToLh = "" }; // sample
            var K1OverHToLhList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10F26_8_1K1OverHToLh))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        string thisTopographyType = (string)Vals[0];
                        string thisExposureCategory = (string)Vals[1];
                        string thisK1OverHToLh = (string)Vals[2];

                        K1OverHToLhList.Add(new
                        {
                            ExposureCategory = thisExposureCategory,
                            TopographyType = thisTopographyType,
                            K1OverHToLh = thisK1OverHToLh
                        });
                    }
                }

            }

            #endregion

            var tableValues = from K1OverHToLhEntry in K1OverHToLhList
                              where (K1OverHToLhEntry.ExposureCategory == windExposureCategory.ToString()
                                  && K1OverHToLhEntry.TopographyType == TopographyType.ToString())
                              select K1OverHToLhEntry;
            var result = (tableValues.ToList()).FirstOrDefault().K1OverHToLh;

            double K1OverHToLh = 0.0;
            if (result != null)
            {
                K1OverHToLh = Double.Parse(result, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ParameterNotFoundInTableException("K1OverHToLh");
            }
            return K1OverHToLh;
        }
    }
}

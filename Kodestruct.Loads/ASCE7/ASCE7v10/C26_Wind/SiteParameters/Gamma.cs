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
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Loads.Properties;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {
        
        public double Getgamma(TopographyType TopographyType)
        {

            #region Read Table

            var SampleValue = new { TopographyType = "", gamma = "" }; // sample
            var gammaList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10F26_8_1Gamma))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 2)
                    {
                        string thisTopographyType = (string)Vals[0];
                        string thisgamma = (string)Vals[1];

                        gammaList.Add(new
                        {
                            TopographyType = thisTopographyType,
                            gamma = thisgamma
                        });
                    }
                }

            }

            #endregion

            var tableValues = from gammaEntry in gammaList
                              where (gammaEntry.TopographyType == TopographyType.ToString())
                              select gammaEntry;
            var result = (tableValues.ToList()).FirstOrDefault().gamma;

            double gamma = 0.0;
            if (result != null)
            {
                gamma = Double.Parse(result, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ParameterNotFoundInTableException("gamma");
            }

            return gamma;
        }
    }
}

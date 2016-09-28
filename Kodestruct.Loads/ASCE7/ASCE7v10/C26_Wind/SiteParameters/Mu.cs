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
        //µ: Horizontal attenuation factor. 
        public double GetMu(TopographyType TopographyType, TopographicLocation TopographicLocation)
        {

            #region Read Table

            var SampleValue = new { TopographyType = "", TopographicLocation = "", Mu = "" }; // sample
            var MuList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ASCE7_10F26_8_1Mu))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 3)
                    {
                        string thisTopographyType = (string)Vals[0];
                        string thisTopographicLocation = (string)Vals[1];
                        string thisMu = (string)Vals[2];

                        MuList.Add(new
                        {
                            TopographyType = thisTopographyType,
                            TopographicLocation = thisTopographicLocation,
                            Mu = thisMu
                        });
                    }
                }

            }

            #endregion

            var tableValues = from muEntry in MuList where (muEntry.TopographyType == TopographyType.ToString() 
                                  && muEntry.TopographicLocation == TopographicLocation.ToString()) select muEntry;
            var result = (tableValues.ToList()).FirstOrDefault().Mu;
            
            double mu =0.0;
            if (result!=null)
            {
                mu = Double.Parse(result, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ParameterNotFoundInTableException("Mu");
            }

            return mu;
        }
    }
}

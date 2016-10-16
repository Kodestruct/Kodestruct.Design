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
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.Properties;


namespace Kodestruct.Concrete.ACI
{
    public class RebarSectionFactory
    {
        public RebarSection GetRebarSection(RebarDesignation BarDesignation)
        {
            RebarSection Bar = null;

            //remove "No" from BarDesignation
            string DesignationStr = BarDesignation.ToString().Substring(2);

           #region Read Table Data

            var Tv11 = new { Designation = "", Diam = 0.0, Area = 0.0, }; // sample
                var ResultList = ListFactory.MakeList(Tv11);

                using (StringReader reader = new StringReader(Resources.ACI_MildRebarProperties))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 4)
                        {
                            string V1 = Vals[0];
                            double V2 = double.Parse(Vals[1], CultureInfo.InvariantCulture);
                            double V3 = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                            ResultList.Add(new { Designation = V1, Diam = V2, Area= V3});
                        }
                    }

                }

                #endregion

                var RebarValues = from v in ResultList where (v.Designation == DesignationStr) select v;
                var foundValues = (RebarValues.ToList());
                if (foundValues.Count>=1)
                {
                    var thisBar = foundValues.First();
                    Bar = new RebarSection(BarDesignation, thisBar.Diam, thisBar.Area);
                }
                return Bar;

        }
    }
}

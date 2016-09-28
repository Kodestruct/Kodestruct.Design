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
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components
{
    public class ComponentClayBrick : BuildingComponentBase
    {
        public ComponentClayBrick(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {

        }

        protected override void Calculate()
        {

            #region Read clay masonry table data

            var SampleValue = new { WytheThickness = "", Weight = "" }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadClayBrick))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 2)
                    {
                        string WytheThickness = (string)Vals[0];
                        string Weight = (string)Vals[1];
                        ComponentWeightList.Add(new
                        {
                            WytheThickness = WytheThickness,
                            Weight = Weight
                        });
                    }
                }

            }

            #endregion

            string ReferenceWytheThickness = null;

            switch (Option1)
            {
                case 0:
                    ReferenceWytheThickness = "4";
                    break;
                case 1:
                    ReferenceWytheThickness = "8";
                    break;
                case 2:
                    ReferenceWytheThickness = "12";
                    break;
                case 3:
                    ReferenceWytheThickness = "16";
                    break;
                default:
                    ReferenceWytheThickness = "4";
                    break;
            }

            double q_masonry = 0.0;

            var DataValues = from weightEntry in ComponentWeightList where (weightEntry.WytheThickness == ReferenceWytheThickness) select weightEntry;
            var ResultList = (DataValues.ToList());

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    double LoadVal;
                    if (Double.TryParse(ResultList.FirstOrDefault().Weight, out LoadVal))
                    {
                        q_masonry = LoadVal;
                    }
                }
            }
            catch
            {

            }
            
            base.Weight= q_masonry;
            base.Notes = string.Format("wythe thickness {0} in. thick", ReferenceWytheThickness);

        }
    }
}

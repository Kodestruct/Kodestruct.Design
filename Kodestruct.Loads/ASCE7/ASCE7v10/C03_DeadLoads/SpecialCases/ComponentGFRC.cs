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
using System.IO;
using System.Linq;
using System.Text;
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components
{
    public class ComponentGFRC : BuildingComponentBase
    {
        public ComponentGFRC(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {
            switch (Option1)
            {
                case 0:
                    Density = GfrcMaterialDensity.d120pcf;
                    break;
                default:
                    Density = GfrcMaterialDensity.d140pcf;
                    break;
            }

            switch (Option2)
            {
                case 0: Backing = BackingType.Backing1_2;         break;
                case 1: Backing = BackingType.Backing5_8;         break;
                case 2: Backing = BackingType.ExposedAggregate3_8;break;
            }
        }

        BackingType Backing { get; set; }
        GfrcMaterialDensity Density { get; set; }

        protected override void Calculate()
        {

            #region Read clay masonry table data

            var SampleValue = new { Density = "", BackingDescription = "", BackingId = "", Weight ="" }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadGFRC))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 4)
                    {
                        string Density            = (string)Vals[0];
                        string BackingDescription = (string)Vals[1];
                        string BackingId          = (string)Vals[2];
                        string Weight             = (string)Vals[3];

                        ComponentWeightList.Add(new
                        {
                            Density            = Density           ,
                            BackingDescription = BackingDescription,
                            BackingId          = BackingId         ,
                            Weight             = Weight            
                        });
                    }
                }

            }

            #endregion

            double q_Gfrc = 0.0;
            string Description = null;

            var DataValues = from weightEntry in ComponentWeightList where (weightEntry.Density == Density.ToString() && weightEntry.BackingId == Backing.ToString()) select weightEntry;
            var ResultList = (DataValues.ToList());

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    double LoadVal;
                    if (Double.TryParse(ResultList.FirstOrDefault().Weight, out LoadVal))
                    {
                        q_Gfrc = LoadVal;
                        Description = ResultList.FirstOrDefault().BackingDescription;
                    }
                }
            }
            catch
            {

            }

            string DensityString = null;
            switch (Density)
            {
                case GfrcMaterialDensity.d120pcf:
                    DensityString = "120 pcf";
                    break;
                case GfrcMaterialDensity.d140pcf:
                    DensityString = "140 pcf";
                    break;
                default:
                    break;
            }

            base.Weight = q_Gfrc;
            base.Notes = string.Format("GFRC with {0} density = {1} ", Description, DensityString);

        }

        enum BackingType
        {
            Backing1_2,
            Backing5_8,
            ExposedAggregate3_8,
        }
        enum GfrcMaterialDensity
        {
            d120pcf,
            d140pcf
        }
    }
}

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
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components
{
    public class ComponentRoofDeck : BuildingComponentBase
    {

        public ComponentRoofDeck(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {
            switch (Option2)
            {
                 case 0:  DepthType = DeckDepthType.d1_5; Profile = DeckProfileType.B; break;
                 case 1:  DepthType = DeckDepthType.d1_5; Profile = DeckProfileType.F; break;
                 case 2:  DepthType = DeckDepthType.d1_5; Profile = DeckProfileType.A; break;
                 case 3:  DepthType = DeckDepthType.d3;   Profile = DeckProfileType.N; break;
                 case 4:  DepthType = DeckDepthType.d1;   Profile = DeckProfileType.E; break;
                 default: DepthType = DeckDepthType.d1_5; Profile = DeckProfileType.B; break; 

            }

            GetGage();

        }

        DeckDepthType DepthType { get; set; }
        DeckProfileType Profile { get; set; }
        DeckGage Gage { get; set; }

        protected override void Calculate()
        {
            double q_deck = 0;
            string ProfileDescription = null;

            #region Read table data

            var SampleValue = new
            {
                DepthType = "",
                ProfileType = "",
                Description ="",
                Gage = "",
                Weight = ""
            }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadRoofDeck))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 5)
                    {

                        string DepthType = (string)Vals[0];
                        string ProfileType = (string)Vals[1];
                        string Description = (string)Vals[2];
                        string Gage = (string)Vals[3];
                        string Weight = (string)Vals[4];

                        ComponentWeightList.Add(new
                        {
                            DepthType = DepthType,
                            ProfileType = ProfileType,
                            Description = Description,
                            Gage = Gage,
                            Weight = Weight

                        });
                    }
                }

                            }

            #endregion


            var DataValues = from weightEntry in ComponentWeightList
                             where
                                 (weightEntry.DepthType == DepthType.ToString() &&
                                 weightEntry.ProfileType== Profile.ToString() &&
                                 weightEntry.Gage == Gage.ToString())
                             select weightEntry;
            var ResultList = (DataValues.ToList());
            double LoadVal;

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    if (Double.TryParse(ResultList.FirstOrDefault().Weight, out LoadVal)) q_deck = LoadVal;
                    ProfileDescription = ResultList.FirstOrDefault().Description;
                }
            }
            catch
            {

            }

            //define strings for the report

            string GageString = null;
            switch (Gage)
            {
                case DeckGage.Gage26: GageString ="26";
                    break;            
                case DeckGage.Gage24: GageString ="24";
                    break;            
                case DeckGage.Gage22: GageString ="22";
                    break;           
                case DeckGage.Gage20: GageString ="20";
                    break;            ;
                case DeckGage.Gage18: GageString ="18";
                    break;            
                case DeckGage.Gage16: GageString ="16"; 
                    break;
                default:
                    break;
            }

            base.Weight = q_deck;
            //update
            base.Notes = string.Format
                ("{0}profile (gage {1})",
                ProfileDescription, GageString);

        }

        protected void GetGage()
        {
            switch (DepthType)
            {
                case DeckDepthType.d1_5:
                    switch (Profile)
	                {
                        case DeckProfileType.B:
                            switch (Option1)
	                        {
		                        case 0: Gage = DeckGage.Gage22; break;
                                case 1: Gage = DeckGage.Gage20; break;
                                case 2: Gage = DeckGage.Gage18; break;
                                case 3: Gage = DeckGage.Gage16; break;

	                        }
                            break;
                        case DeckProfileType.F:
                            switch (Option1)
	                        {
                                case 0: Gage = DeckGage.Gage22; break;
                                case 1: Gage = DeckGage.Gage20; break;
                                case 2: Gage = DeckGage.Gage18; break;

	                        }
                            break;
                        case DeckProfileType.A:
                            switch (Option1)
	                        {
                                case 0: Gage=DeckGage.Gage22; break;
                                case 1: Gage=DeckGage.Gage20; break;
                                case 2: Gage=DeckGage.Gage18; break;

	                        }
                            break;
	                }
                    break;
                case DeckDepthType.d3:
                            switch (Option1)
	                        {
                               case 0:Gage=DeckGage.Gage22; break;
                               case 1:Gage=DeckGage.Gage20; break;
                               case 2:Gage=DeckGage.Gage18; break;
                               case 3:Gage=DeckGage.Gage16; break;

	                        }
                    break;
                case DeckDepthType.d1:
                            switch (Option1)
	                        {
                                case 0: Gage=DeckGage.Gage26; break;
                                case 1: Gage=DeckGage.Gage24; break;
                                case 2: Gage=DeckGage.Gage22; break;
                                case 3: Gage=DeckGage.Gage20; break; 
	                        }

                    break;
                default:
                    break;
            }
        }

        enum DeckDepthType
        {
            d1_5,
            d3,
            d1
        }

        enum DeckProfileType
        {
            B,
            F,
            A,
            N,
            E
        }
        enum DeckGage
        {
            Gage26,
            Gage24,
            Gage22,
            Gage20,
            Gage18,
            Gage16
        }
    }
}

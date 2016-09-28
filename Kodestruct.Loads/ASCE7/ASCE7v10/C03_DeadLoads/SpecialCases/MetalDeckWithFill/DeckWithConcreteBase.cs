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
    public abstract partial class DeckWithConcreteBase: BuildingComponentBase
    {
        public DeckWithConcreteBase(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {

        }


        public override double Weight
        {
            get 
            {
                this.Calculate();
                return base.Weight; 
            }
            set { base.Weight = value; }
        }
        
        protected DeckDepthType DepthType { get; set; }
        protected DeckProfileType ProfileType { get; set; }
        protected ConcreteWeightType WeightType { get; set; }
        protected double TotalSlabDepth { get; set; }

        protected abstract double GetTotalGepth();

        protected override void Calculate()
        {
            TotalSlabDepth = GetTotalGepth();


            #region Read table data

            var SampleValue = new
            {
                DepthType = "",
                TotalDepth = "",
                ProfileType = "",
                NwWeight = "",
                LwWeight = ""

            }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadDeckWithConcrete))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 5)
                    {

                        string DepthType =   (string)Vals[0];
                        string TotalDepth = (string)Vals[1];
                        string ProfileType = (string)Vals[2];
                        string NwWeight =    (string)Vals[3];
                        string LwWeight =    (string)Vals[4];

                        ComponentWeightList.Add(new
                        {
                            DepthType =   DepthType  ,
                            TotalDepth = TotalDepth,
                            ProfileType = ProfileType,
                            NwWeight =    NwWeight   ,
                            LwWeight =    LwWeight   

                        });
                    }
                }

            }

            #endregion



            double q_deckAndConcrete = 0.0;

            var DataValues = from weightEntry in ComponentWeightList
                             where
                                 (weightEntry.DepthType == DepthType.ToString() &&
                                 weightEntry.TotalDepth == TotalSlabDepth.ToString() &&
                                 weightEntry.ProfileType == ProfileType.ToString())
                             select weightEntry;
            var ResultList = (DataValues.ToList());
            double LoadVal;
            string WeightString = null;

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    switch (WeightType)
                    {
                        case ConcreteWeightType.LightWeight:
                            if (Double.TryParse(ResultList.FirstOrDefault().LwWeight, out LoadVal)) q_deckAndConcrete = LoadVal;
                            WeightString = "light weight";
                            break;
                        case ConcreteWeightType.NormalWeight:
                            if (Double.TryParse(ResultList.FirstOrDefault().NwWeight, out LoadVal)) q_deckAndConcrete = LoadVal;
                            WeightString = "normal weight";
                            break;
                        default:
                            WeightString = "normal weight";
                            break;
                    }
                    
                }
            }
            catch
            {

            }

            //define strings for the report
            string ProfileString = null;
            switch (ProfileType)
            {
                case DeckProfileType.p1_5x6:
                    ProfileString = "1 1/2 x 6 in";
                    break;
                case DeckProfileType.p1_5x6INV:
                    ProfileString = "1 1/2 x 6 in. (inverted)";
                    break;
                case DeckProfileType.p2x12:
                    ProfileString = "2 x 12 in.";
                    break;
                case DeckProfileType.p3x12:
                    ProfileString = "3 x 12 in.";
                    break;
                default:
                    break;
            }
            string DeckDepthString = null;
            switch (DepthType)
            {
                case DeckDepthType.d1_5:
                    DeckDepthString = "1 1/2";
                    break;
                case DeckDepthType.d2:
                    DeckDepthString = "2";
                    break;
                case DeckDepthType.d3:
                    DeckDepthString = "3";
                    break;
                default:
                    DeckDepthString = "3";
                    break;
            }


            base.Weight = q_deckAndConcrete;
            base.Notes = string.Format
                ("{0} deep composite deck (having {1} profile) with {2} concrete fill. Total slab thickness ={3}",
                DeckDepthString, ProfileString, WeightString, TotalSlabDepth.ToString());

        }
    }
}

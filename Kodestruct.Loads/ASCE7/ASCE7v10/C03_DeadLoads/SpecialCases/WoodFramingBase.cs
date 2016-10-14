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
    public abstract class WoodFramingBase : BuildingComponentBase
    {
        public WoodFramingBase(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {
            switch (Option1)
            {
                case 0: Size = DimensionalSize.cs2X6; break;
                case 1: Size = DimensionalSize.cs2X8; break;
                case 2: Size = DimensionalSize.cs2X10; break;
                case 3: Size = DimensionalSize.cs2X12; break;
                default: Size = DimensionalSize.cs2X6; break;
            }

            switch (Option2)
            {
                case 0: SpacingOC = Spacing.at12; break;
                case 1: SpacingOC = Spacing.at16; break;
                case 2: SpacingOC = Spacing.at24; break;
                default: SpacingOC = Spacing.at12; break;
            }
        }

        protected DimensionalSize Size { get; set; }
        protected Spacing SpacingOC { get; set; }
        protected FramingType Framing { get; set; }

        protected override void Calculate()
        {

            #region Read table data

            var SampleValue = new
            {
                DimensionalSize = "",
                ElementSpacing = "",
                RegularFraming = "",
                DoubleFloor = ""

            }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadWoodFraming))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 4)
                    {

                        string dimsizeSize = (string)Vals[0];
                        string elSpacing = (string)Vals[1];
                        string RegularFraming = (string)Vals[2];
                        string DoubleFloor = (string)Vals[3];

                        ComponentWeightList.Add(new
                        {
                            DimensionalSize = dimsizeSize,
                            ElementSpacing = elSpacing,
                            RegularFraming = RegularFraming,
                            DoubleFloor = DoubleFloor

                        });
                    }
                }

            }

            #endregion

            double q_wood = 0.0;

            var DataValues = from weightEntry in ComponentWeightList
                             where
                                 (weightEntry.DimensionalSize == Size.ToString() &&
                                 weightEntry.ElementSpacing == SpacingOC.ToString())
                             select weightEntry;
            var ResultList = (DataValues.ToList());
            double LoadVal;

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    switch (Framing)
	                    {
		                    case FramingType.Regular:
                                if (Double.TryParse(ResultList.FirstOrDefault().RegularFraming, out LoadVal)) q_wood = LoadVal;
                             break;
                            case FramingType.DoubleFloor:
                                if (Double.TryParse(ResultList.FirstOrDefault().DoubleFloor, out LoadVal)) q_wood = LoadVal;
                             break;
	                    }
                }
            }
            catch
            {

            }

            //define strings for the report
            string SizeString = null;
            switch (Size)
            {
                case DimensionalSize.cs2X6:
                    SizeString = "2X6";
                    break;
                case DimensionalSize.cs2X8:
                    SizeString = "2X8";
                    break;
                case DimensionalSize.cs2X10:
                    SizeString = "2X10";
                    break;
                case DimensionalSize.cs2X12:
                    SizeString = "2X12";
                    break;
                default:
                    break;
            }

            string SpacingString = null;
            switch (SpacingOC)
            {
                case Spacing.at12:
                    SpacingString = "at 12 in. o.c";
                    break;
                case Spacing.at16:
                    SpacingString = "at 16 in. o.c";
                    break;
                case Spacing.at24:
                    SpacingString = "at 24 in. o.c";
                    break;
                default:
                    break;
            }

            string FramingTypeString = null;

            switch (Framing)
            {
                case FramingType.Regular:
                    FramingTypeString = "wood framing";
                    break;
                case FramingType.DoubleFloor:
                    FramingTypeString = "double wood floor";
                    break;
                default:
                    break;
            }

            base.Weight = q_wood;
            base.Notes = string.Format
                ("{0} member {1}, {2} ",
                SizeString, SpacingString, FramingTypeString);

        }

        protected enum DimensionalSize
        {
            cs2X6,
            cs2X8,
            cs2X10,
            cs2X12
        }
        protected enum Spacing
        {
            at12,
            at16,
            at24
        }
        protected enum FramingType
        {
            Regular,
            DoubleFloor
        }
    }
}

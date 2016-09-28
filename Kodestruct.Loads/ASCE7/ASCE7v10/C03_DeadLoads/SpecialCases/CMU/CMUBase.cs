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
    public abstract partial class CMUBase : BuildingComponentBase
    {
        public CMUBase(int Option1, int Option2, double NumericValue )
            : base(Option1, Option2, NumericValue)
        {
            SetValues();
        }

        protected abstract void SetValues();

        protected double Density { get; set; }
        protected BlockGroutingType GroutingType { get; set; }
        protected double GroutSpacing { get; set; }
        protected double UnitThickness { get; set; }

        protected override void Calculate()
        {

            #region Read CMU table data

            var SampleValue = new 
            { 
                BlockGroutingType=  "",
                Density=	        "",
                GroutSpacing=       "",
                Block4in=           "",
                Block6in =          "",
                Block8in=           "",
                Block10in=          "",
                Block12in=          ""

            }; // sample
            var ComponentWeightList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadCMU))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 8)
                    {

                        string  BlockGroutingType=(string)Vals[0];
                        string  Density=	      (string)Vals[1];
                        string  GroutSpacing=     (string)Vals[2];
                        string  Block4in=         (string)Vals[3];
                        string  Block6in =        (string)Vals[4];
                        string  Block8in=         (string)Vals[5];
                        string  Block10in=        (string)Vals[6];
                        string  Block12in =       (string)Vals[7];

                        ComponentWeightList.Add(new
                        {
                            BlockGroutingType= BlockGroutingType,
                            Density=	       Density,   
                            GroutSpacing=      GroutSpacing,    
                            Block4in=          Block4in,
                            Block6in =         Block6in,
                            Block8in=          Block8in,
                            Block10in=         Block10in,
                            Block12in =        Block12in

                        });
                    }
                }

            }

            #endregion


            double q_masonry = 0.0;

            var DataValues = from weightEntry in ComponentWeightList where
                                 (weightEntry.Density == Density.ToString()+" pcf" &&
                                 weightEntry.BlockGroutingType == GroutingType.ToString() &&
                                 weightEntry.GroutSpacing == GroutSpacing.ToString()) 
                             select weightEntry;
            var ResultList = (DataValues.ToList());
            string ReferenceWytheThickness = null;

            try
            {
                if (ResultList.FirstOrDefault() != null)
                {
                    double LoadVal;

                    switch (UnitThickness.ToString())
                    {
                        case "4":
                            ReferenceWytheThickness = "4";
                            if (Double.TryParse(ResultList.FirstOrDefault().Block4in, out LoadVal)) q_masonry = LoadVal;
                            break;
                        case "6":
                            ReferenceWytheThickness = "6";
                            if (Double.TryParse(ResultList.FirstOrDefault().Block6in, out LoadVal)) q_masonry = LoadVal;
                            break;
                        case "8":
                            ReferenceWytheThickness = "8";
                            if (Double.TryParse(ResultList.FirstOrDefault().Block8in, out LoadVal)) q_masonry = LoadVal;
                            break;
                        case "10":
                            ReferenceWytheThickness = "10";
                            if (Double.TryParse(ResultList.FirstOrDefault().Block10in, out LoadVal)) q_masonry = LoadVal;
                            break;
                        case "12":
                            ReferenceWytheThickness = "12";
                            if (Double.TryParse(ResultList.FirstOrDefault().Block12in, out LoadVal)) q_masonry = LoadVal;
                            break;
                    }
                }
            }
            catch
            {

            }
            string GroutTypeString = null;
            switch (GroutingType)
            {
                case BlockGroutingType.Hollow:
                    GroutTypeString = "hollow, ungrouted units";
                    break;
                case BlockGroutingType.FullGrout:
                    GroutTypeString = "fully grouted units";
                    break;
                case BlockGroutingType.PartialGrout:
                    GroutTypeString = string.Format("partially grouted units, grout at {0} in. o.c.", GroutSpacing);
                    break;
                case BlockGroutingType.Solid:
                    GroutTypeString = "solid units";
                    break;
                default:
                    break;
            }

            base.Weight = q_masonry;
            base.Notes = string.Format("{0}, thickness = {1} in.", GroutTypeString, ReferenceWytheThickness);

        }
    }
}

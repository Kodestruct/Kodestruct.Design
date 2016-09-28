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
using System.Globalization;
using System.IO;
using System.Linq;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;
using Kodestruct.Loads.Properties;
using Kodestruct.Common.Mathematics;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class Site : AnalyticalElement
    {
        public SeismicSiteClass SiteClass { get; set; }

        public Site(SeismicSiteClass Class, ICalcLog CalcLog)
            : base(CalcLog)
        {
            this.SiteClass = Class;
        }

        public double SiteCoefficientFa(double SS)
        {
           double Fa = this.GetSiteCoefficientF(SS, SiteClass, SeismicSiteCoefficient.Fa);
           return Fa;
        }

        public double SiteCoefficientFv(double S1)
        {
            double Fv = this.GetSiteCoefficientF(S1, SiteClass, SeismicSiteCoefficient.Fv);
            return Fv;
        }

        private double GetSiteCoefficientF(double S, SeismicSiteClass SiteClass, SeismicSiteCoefficient SiteCoefficient)
        {
            double SiteCoefficientF = 0;

            #region Read Coefficient Data

            var Tv11 = new { S = 0.0, Site = SeismicSiteClass.A, F = 0.0, Coefficient = SeismicSiteCoefficient.Fa }; // sample
            //var SiteCoefficientList = (new[] { Tv11 }).ToList();    //create list using casting by example
            var SiteCoefficientList = ListFactory.MakeList(Tv11);

            using (StringReader reader = new StringReader(Resources.ASCE7_10T11_4_1AND2))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 4)
                    {
                        double s = double.Parse(Vals[0], CultureInfo.InvariantCulture);
                        SeismicSiteClass Class = (SeismicSiteClass)Enum.Parse(typeof(SeismicSiteClass), Vals[1]);
                        double f = double.Parse(Vals[2], CultureInfo.InvariantCulture);
                        SeismicSiteCoefficient Coeff = (SeismicSiteCoefficient)Enum.Parse(typeof(SeismicSiteCoefficient), Vals[3]);
                        SiteCoefficientList.Add(new { S = s, Site = Class, F = f, Coefficient = Coeff });
                    }
                }

            }

            #endregion

            var FValues = from fa in SiteCoefficientList where (fa.Site == SiteClass && fa.Coefficient == SiteCoefficient) orderby fa.S select fa;
            var ResultList = (FValues.ToList());    //create list (LINQ immediate execution)
            var MinSValue = FValues.Min(fVal => fVal.S);
            var MaxSValue = FValues.Max(fVal => fVal.S);

            // Check for extreme values
            if (S <= MinSValue)
            {
                var MinEntry = from fa in FValues where (fa.Site == SiteClass && fa.S == MinSValue) select fa;
                SiteCoefficientF = MinEntry.ElementAt(0).F;
            }
            if (S >= MaxSValue)
            {
                var MaxEntry = from fa in FValues where (fa.Site == SiteClass && fa.S == MaxSValue) select fa;
                SiteCoefficientF = MaxEntry.ElementAt(0).F;
            }

            if (S > MinSValue && S < MaxSValue)
            {

                //Intermediate values
                int NumEntries = ResultList.Count();
                for (int i = 0; i < NumEntries; i++)
                {
                    var thisVal = ResultList[i];
                    if (i > 0 && i < NumEntries - 1)
                    {
                        if (thisVal.S >= ResultList[i - 1].S && thisVal.S <= ResultList[i + 1].S)
                        {
                            SiteCoefficientF = Interpolation.InterpolateLinear(ResultList[i - 1].S, ResultList[i - 1].F, ResultList[i + 1].S, ResultList[i + 1].F, S);
                        }
                    }
                }

            }

            //output 
            switch (SiteCoefficient)
            {
                case SeismicSiteCoefficient.Fa:

                    double Fa = SiteCoefficientF;
                    #region Fa
                    ICalcLogEntry FaEntry = new CalcLogEntry();
                    FaEntry.ValueName = "Fa";
                    FaEntry.AddDependencyValue("SS", Math.Round(S, 3));
                    FaEntry.AddDependencyValue("SiteClass", SiteClass.ToString());
                    FaEntry.Reference = "Short-period site coefficient";
                    FaEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSiteCoefficientFa.docx";
                    FaEntry.FormulaID = "Table 11.4-1"; //reference to formula from code
                    FaEntry.VariableValue = Fa.ToString();
                    #endregion
                    this.AddToLog(FaEntry);

                    break;
                case SeismicSiteCoefficient.Fv:
                    double Fv = SiteCoefficientF;
                    
                    #region Fv
                    ICalcLogEntry FvEntry = new CalcLogEntry();
                    FvEntry.ValueName = "Fv";
                    FvEntry.AddDependencyValue("Sone", Math.Round(S, 3));
                    FvEntry.AddDependencyValue("SiteClass", SiteClass.ToString());
                    FvEntry.Reference = "Long-period site coefficient";
                    FvEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Seismic/SeismicSiteCoefficientFv.docx";
                    FvEntry.FormulaID = null; //reference to formula from code
                    FvEntry.VariableValue = Fv.ToString();
                    #endregion
                    this.AddToLog(FvEntry);
                    break;
            }

            return SiteCoefficientF;

        }
    }
}

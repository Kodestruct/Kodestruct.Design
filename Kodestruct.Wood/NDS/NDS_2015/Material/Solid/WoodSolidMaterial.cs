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
using System.Reflection;
using System.Text;
using Kodestruct.Analytics.Wood.NDS;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Data;
using Kodestruct.Common.Entities;


namespace Kodestruct.Wood.NDS.NDS2015.Material
{
    public abstract class WoodSolidMaterial :AnalyticalElement, IWoodSolidMaterial
    {

        public WoodSolidMaterial( string Species, string CommercialGrade, string SizeClass, ICalcLog CalcLog)
            : base(CalcLog)
        {
            this.Species = Species;
            this.CommercialGrade = CommercialGrade;
            this.SizeClass = SizeClass;
        }
        public bool ValuesWereCalculated { get; set; }

        protected abstract string GetResource();

        public string Species { get; set; }
        public string CommercialGrade { get; set; }
        public string SizeClass { get; set; }

        private string resourceString;

        public string ResourceString
        {
            get {
                resourceString = GetResource();
                return resourceString; }
            set { resourceString = value; }
        }

        protected void CalculateValues()
        {
            this.Species = Species;
            this.CommercialGrade = CommercialGrade;

            if (ValuesWereCalculated == false)
            {
                
                #region Read Table Data

                var Tv11 = new { Species = "", Grade = "", SizeClass = "", Fb = 0.0, Ft = 0.0, Fv = 0.0, FcPerp = 0.0, Fc = 0.0, E = 0.0, Emin = 0.0, G=0.0 }; // sample
                var ResultList = ListFactory.MakeList(Tv11);


                string resourceName = string.Format("Kodestruct.Wood.Resources.{0}.txt", ResourceString);
                var assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(resourceName);

                using (TextReader tr = new StreamReader(stream))
                {
                    string line;
                    while ((line = tr.ReadLine()) != null)
                    {
                        string[] Vals = line.Split(',');
                        if (Vals.Count() == 11)
                        {
                            string _Species = Vals[0];
                            string _Grade = Vals[1];
                            string _SizeClass = Vals[2];
                            double _Fb = double.Parse(Vals[3], CultureInfo.InvariantCulture);
                            double _Ft = double.Parse(Vals[4], CultureInfo.InvariantCulture);
                            double _Fv = double.Parse(Vals[5], CultureInfo.InvariantCulture);
                            double _FcPerp = double.Parse(Vals[6], CultureInfo.InvariantCulture);
                            double _Fc = double.Parse(Vals[7], CultureInfo.InvariantCulture);
                            double _E = double.Parse(Vals[8], CultureInfo.InvariantCulture);
                            double _Emin = double.Parse(Vals[9], CultureInfo.InvariantCulture);
                            double _G =  double.Parse(Vals[10], CultureInfo.InvariantCulture);
                            ResultList.Add(new
                            {
                                Species = _Species,
                                Grade = _Grade,
                                SizeClass = _SizeClass,
                                Fb = _Fb,
                                Ft = _Ft,
                                Fv = _Fv,
                                FcPerp = _FcPerp,
                                Fc = _Fc,
                                E = _E,
                                Emin = _Emin,
                                G = _G
                            });
                        }
                    }

                }

                #endregion

                var RValues = from v in ResultList
                              where
                                  (v.Species == Species &&
                                  v.Grade == CommercialGrade &&
                                  v.SizeClass == SizeClass)
                              select v;
                var foundValues = (RValues.ToList());
                if (foundValues.Count > 0)
                {
                    var ThisMaterialProps = foundValues.FirstOrDefault();
                    this.F_b = ThisMaterialProps.Fb;
                    this.F_t = ThisMaterialProps.Ft;
                    this.F_v = ThisMaterialProps.Fv;
                    this.F_cPerp = ThisMaterialProps.FcPerp;
                    this.F_cParal = ThisMaterialProps.Fc;
                    this.E = ThisMaterialProps.E;
                    this.E_min = ThisMaterialProps.Emin;
                    this.G = ThisMaterialProps.G;
                }
                
                ValuesWereCalculated = true;
            }
        }


               double _F_b;
        public double F_b
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _F_b;
            }

            set { _F_b = value; }

        }

        

        double _F_cParal;
        public double F_cParal
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _F_cParal;
            }
            set { _F_cParal = value; }
        }

        double _F_cPerp;
        public double F_cPerp
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _F_cPerp;
            }
            set { _F_cPerp = value; }
        }


        double _F_t;
        public double F_t
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _F_t;
            }
            set { _F_t = value; }
        }

        double _F_v;
        public double F_v
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _F_v;
            }
            set { _F_v = value; }
        }

        double _E;
        public double E
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _E;
            }
            set { _E = value; }
        }

        double Emin;
        public double E_min
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return Emin;
            }
            set { Emin = value; }
        }

        double _G;
        public double G
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalculateValues();
                }
                return _G;
            }
            set { _G = value; }
        }


    }
}

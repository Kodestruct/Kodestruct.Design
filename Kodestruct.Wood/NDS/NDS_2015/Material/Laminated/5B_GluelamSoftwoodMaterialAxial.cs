using Kodestruct.Common.Data;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.Material.Laminated
{
    public class GluelamSoftwoodMaterialAxial
    {
 

        public GlulamSoftWoodAxialCombinationSymbol CombinationSymbol { get; set; }
        public int NumberOfLaminations { get; set; }
        public bool ValuesWereCalculated { get; set; }
        public double d { get; set; }

        #region E
        private double _E;
        public double E
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E;
            }
        }
        #endregion

        #region E_min
        private double _E_min;
        public double E_min
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E_min;
            }
        }
        #endregion

        #region F_c_perp
        private double _F_c_perp;
        public double F_c_perp
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c_perp;
            }
        }
        #endregion

        #region F_t
        private double _F_t;
        public double F_t
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_t;
            }
        }
        #endregion

        #region F_c4
        private double _F_c4;
        public double F_c4
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c4;
            }
        }
        #endregion

        #region F_c23
        private double _F_c23;
        public double Fc_23
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c23;
            }
        }
        #endregion

        #region F_by4
        private double _F_by4;
        public double F_by4
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_by4;
            }
        }
        #endregion

        #region F_by3
        private double _F_by3;
        public double F_by3
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_by3;
            }
        }
        #endregion

        #region F_by2
        private double _F_by2;
        public double F_by2
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_by2;
            }
        }
        #endregion

        #region F_vy
        private double _F_vy;
        public double F_vy
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_vy;
            }
        }
        #endregion

        #region F_bx
        private double _F_bx;
        public double F_bx
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_bx;
            }
        }
        #endregion

        #region F_vx
        private double _F_vx;
        public double F_vx
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_vx;
            }
        }
        #endregion

        #region G
        private double _G;
        public double G
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _G;
            }
        }
        #endregion

        #region CombinationSymbolId
        private string _CombinationSymbolId;
        public string CombinationSymbolId
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _CombinationSymbolId;
            }
        }
        #endregion

        #region Species
        private string _Species;
        public string Species
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _Species;
            }
        }
        #endregion

        #region Grade
        private string _Grade;
        public string Grade
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _Grade;
            }
        }
        #endregion

        public GluelamSoftwoodMaterialAxial(GlulamSoftWoodAxialCombinationSymbol CombinationSymbol, double d,
            int NumberOfLaminations)
        {
            this.NumberOfLaminations = NumberOfLaminations;
            this.d = d;
            this.CombinationSymbol = CombinationSymbol;
       }

        void CalulateValues()
        {

            ValuesWereCalculated = true;

            #region Read Occupancy Data

            var SampleValue = new
            {
                CombinationSymbol = "",
                CombinationSymbolId = "",
                Species = "",
                Grade = "",
                E = 0.0,
                E_min = 0.0,
                F_c_perp = 0.0,
                F_t = 0.0,
                F_c4 = 0.0,
                F_c23 = 0.0,
                F_by4 = 0.0,
                F_by3 = 0.0,
                F_by2 = 0.0,
                F_vy = 0.0,
                F_bx = 0.0,
                F_vx = 0.0,
                G = 0.0,
            }; // sample
            var glulamVals = ListFactory.MakeList(SampleValue);

            string lookupResource = Resources.NDS2015_Table5BSoftwoodAxialMembers;
 
            using (StringReader reader = new StringReader(lookupResource))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 17)
                    {
                        string _CombinationSymbol = Vals[0];
                        string _CombinationSymbolId = Vals[1];
                        string _Species = Vals[2];
                        string _Grade = Vals[3];
                        double _E = Double.Parse(Vals[4]);
                        double _E_min = Double.Parse(Vals[5]);
                        double _F_c_perp = Double.Parse(Vals[6]);
                        double _F_t = Double.Parse(Vals[7]);
                        double _F_c4 = Double.Parse(Vals[8]);
                        double _F_c23 = Double.Parse(Vals[9]);
                        double _F_by4 = Double.Parse(Vals[10]);
                        double _F_by3 = Double.Parse(Vals[11]);
                        double _F_by2 = Double.Parse(Vals[12]);
                        double _F_vy = Double.Parse(Vals[13]);
                        double _F_bx = Double.Parse(Vals[14]);
                        double _F_vx = Double.Parse(Vals[15]);
                        double _G = Double.Parse(Vals[16]);



                        glulamVals.Add
                        (new
                        {
                            CombinationSymbol = _CombinationSymbol,
                            CombinationSymbolId = _CombinationSymbolId,
                            Species = _Species,
                            Grade = _Grade,
                            E = _E * Math.Pow(10, 6),
                            E_min = _E_min * Math.Pow(10, 6),
                            F_c_perp = _F_c_perp,
                            F_t = _F_t,
                            F_c4 = _F_c4,
                            F_c23 = _F_c23,
                            F_by4 = _F_by4,
                            F_by3 = _F_by3,
                            F_by2 = _F_by2,
                            F_vy = _F_vy,
                            F_bx = _F_bx,
                            F_vx = _F_vx,
                            G = _G
                        });
                    }
                }

            }

            #endregion


            var thisCombinationVals = glulamVals.Where(g => g.CombinationSymbol == this.CombinationSymbol.ToString()).FirstOrDefault();
            this._CombinationSymbolId = thisCombinationVals.CombinationSymbolId;
            this._Species = thisCombinationVals.Species;
            this._Grade = thisCombinationVals.Grade;
            this._E = thisCombinationVals.E;
            this._E_min = thisCombinationVals.E_min;
            this._F_c_perp = thisCombinationVals.F_c_perp;
            this._F_t = thisCombinationVals.F_t;
            this._F_c4 = thisCombinationVals.F_c4;
            this._F_c23 = thisCombinationVals.F_c23;
            this._F_by4 = thisCombinationVals.F_by4;
            this._F_by3 = thisCombinationVals.F_by3;
            this._F_by2 = thisCombinationVals.F_by2;
            this._F_vy = thisCombinationVals.F_vy;
            this._F_bx = thisCombinationVals.F_bx;
            this._F_vx = thisCombinationVals.F_vx;
            this._G = thisCombinationVals.G;


            ////Special cases

            if (d>15.0)
            {
                _F_bx = (_F_bx * 0.88).Floor(1);
            }
            if (NumberOfLaminations ==2)
            {
                _F_vy = (_F_vy * 0.84).Floor(1);
            }
            if (NumberOfLaminations ==3)
            {
                _F_vy = (_F_vy * 0.95).Floor(1);
            }


        }

    }
}

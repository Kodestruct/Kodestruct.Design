using Kodestruct.Common.Data;
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
    public class GluelamSoftwoodMaterialFlexureSimple
    {
        public GlulamSimpleFlexuralStressClass StressClass { get; set; }
        public GlulamWoodSpeciesSimple WoodSpecies { get; set; }
        public int NumberOfLaminations { get; set; }
        public bool ValuesWereCalculated { get; set; }


        #region F_bx_p
        private double _F_bx_p;
        public double F_bx_p
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_bx_p;
            }
        }
        #endregion

        #region F_bx_m
        private double _F_bx_m;
        public double F_bx_m
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_bx_m;
            }
        }
        #endregion

        #region F_c_perp_x
        private double _F_c_perp_x;
        public double F_c_perp_x
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c_perp_x;
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

        #region E_x
        private double _E_x;
        public double E_x
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E_x;
            }
        }
        #endregion

        #region E_x_min
        private double _E_x_min;
        public double E_x_min
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E_x_min;
            }
        }
        #endregion

        #region F_by
        private double _F_by;
        public double F_by
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_by;
            }
        }
        #endregion

        #region F_c_perp_y
        private double _F_c_perp_y;
        public double F_c_perp_y
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c_perp_y;
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

        #region E_y
        private double _E_y;
        public double E_y
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E_y;
            }
        }
        #endregion

        #region E_y_min
        private double _E_y_min;
        public double E_y_min
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _E_y_min;
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

        #region F_c
        private double _F_c;
        public double F_c
        {
            get
            {
                if (ValuesWereCalculated == false)
                {
                    CalulateValues();
                }
                return _F_c;
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
 

        public GluelamSoftwoodMaterialFlexureSimple( GlulamSimpleFlexuralStressClass StressClass, GlulamWoodSpeciesSimple WoodSpecies,
            int NumberOfLaminations)
        {
 
            this.NumberOfLaminations = NumberOfLaminations;
            this.StressClass = StressClass;
            this.WoodSpecies = WoodSpecies;

        }

        void CalulateValues()
        {

            ValuesWereCalculated = true;

            #region Read Occupancy Data

            var SampleValue = new
            {
                StressClass = "",
                F_bx_p = 0.0,
                F_bx_m = 0.0,
                F_c_perp_x = 0.0,
                F_vx = 0.0,
                E_x = 0.0,
                E_x_min = 0.0,
                F_by = 0.0,
                F_c_perp_y = 0.0,
                F_vy = 0.0,
                E_y = 0.0,
                E_y_min = 0.0,
                F_t = 0.0,
                F_c = 0.0,
                G = 0.0
            }; // sample
            var glulamVals = ListFactory.MakeList(SampleValue);

            string lookupResource = null;
            switch (WoodSpecies)
            {
                case GlulamWoodSpeciesSimple.DouglasFir:
                    lookupResource = Resources.NDS2015_Table5ASimple_DougFir;
                    break;
                case GlulamWoodSpeciesSimple.SouthernPineNoWanes:
                    lookupResource = Resources.NDS2015_Table5ASimple_SP_NoWane;
                    break;
                case GlulamWoodSpeciesSimple.SouthernPineOneSide:
                    lookupResource = Resources.NDS2015_Table5ASimple_SP_Wane1s;
                    break;
                case GlulamWoodSpeciesSimple.SouthernPineBothSides:
                    lookupResource = Resources.NDS2015_Table5ASimple_SP_Wane2s;
                    break;
                case GlulamWoodSpeciesSimple.Other:
                    lookupResource = Resources.NDS2015_Table5ASimple_Other;
                    break;
                default:
                    lookupResource = Resources.NDS2015_Table5ASimple_Other;
                    break;
            }
            using (StringReader reader = new StringReader(lookupResource))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 21)
                    {
                        string _StressClass = Vals[0];
                        double _F_bx_p = Double.Parse(Vals[1]);
                        double _F_bx_m = Double.Parse(Vals[2]);
                        double _F_c_perp_x = Double.Parse(Vals[3]);
                        double _F_vx = Double.Parse(Vals[4]);
                        double _E_x = Double.Parse(Vals[5]);
                        double _E_x_min = Double.Parse(Vals[6]);
                        double _F_by = Double.Parse(Vals[7]);
                        double _F_c_perp_y = Double.Parse(Vals[8]);
                        double _F_vy = Double.Parse(Vals[9]);
                        double _E_y = Double.Parse(Vals[10]);
                        double _E_y_min = Double.Parse(Vals[11]);
                        double _F_t = Double.Parse(Vals[12]);
                        double _F_c = Double.Parse(Vals[13]);
                        double _G = Double.Parse(Vals[14]);


                        glulamVals.Add
                        (new
                        {
                            StressClass = _StressClass,
                            F_bx_p = _F_bx_p,
                            F_bx_m = _F_bx_m,
                            F_c_perp_x = _F_c_perp_x,
                            F_vx = _F_vx,
                            E_x = _E_x * Math.Pow(10, 6),
                            E_x_min = _E_x_min * Math.Pow(10, 6),
                            F_by = _F_by,
                            F_c_perp_y = _F_c_perp_y,
                            F_vy = _F_vy,
                            E_y = _E_y * Math.Pow(10, 6),
                            E_y_min = _E_y_min * Math.Pow(10, 6),
                            F_t = _F_t,
                            F_c = _F_c,
                            G = _G
                        });
                    }
                }

            }

            #endregion


            var thisStressVals = glulamVals.Where(g => g.StressClass == this.StressClass.ToString()).FirstOrDefault();
            this._F_bx_p = thisStressVals.F_bx_p;
            this._F_bx_m = thisStressVals.F_bx_m;
            this._F_c_perp_x = thisStressVals.F_c_perp_x;
            this._F_vx = thisStressVals.F_vx;
            this._E_x = thisStressVals.E_x;
            this._E_x_min = thisStressVals.E_x_min;
            this._F_by = thisStressVals.F_by;
            this._F_c_perp_y = thisStressVals.F_c_perp_y;
            this._F_vy = thisStressVals.F_vy;
            this._E_y = thisStressVals.E_y;
            this._E_y_min = thisStressVals.E_y_min;
            this._F_t = thisStressVals.F_t;
            this._F_c = thisStressVals.F_c;
            this._G = thisStressVals.G;

            //Special cases
            if (WoodSpecies.ToString().StartsWith("28") || WoodSpecies.ToString().StartsWith("30"))
            {
                if (NumberOfLaminations > 15)
                {
                    _E_x = 2.0 * Math.Pow(10, 6);
                    _E_x_min = 1.06 * Math.Pow(10, 6);
                }
            }

        }

    }
}

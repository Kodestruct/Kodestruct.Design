using Kodestruct.Common.Data;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Wood.NDS.Entities.Glulam;
using Kodestruct.Wood.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public class GlulamSoftwoodFlexuralMemberSimple: GluelamMember
    {
        public GlulamSimpleFlexuralStressClass StressClass { get; set; }
        public GlulamWoodSpeciesSimple WoodSpecies { get; set; }
        public ISectionRectangular Section { get; set; }
        public double b { get; set; }
        public double h { get; set; }
        public int NumberOfLaminations { get; set; }
        double F_bx_p;
        double F_bx_m;
        double F_c_perp_x;
        double F_vx;
        double E_x;
        double E_x_min;
        double F_by;
        double F_c_perp_y;
        double F_vy;
        double E_y;
        double E_y_min;
        double F_t;
        double F_c;
        double G;

        public GlulamSoftwoodFlexuralMemberSimple(double b, double h, int NumberLaminations, 
            GlulamSimpleFlexuralStressClass StressClass, GlulamWoodSpeciesSimple WoodSpecies)
        {
            this.b = b;
            this.h = h;
            this.NumberOfLaminations = NumberOfLaminations;
            this.StressClass = StressClass;
            this.WoodSpecies = WoodSpecies;
            GetReferenceValues();
        }
        void GetReferenceValues()
        {
 
            #region Read Occupancy Data

            var SampleValue = new
            {
                StressClass="",
                F_bx_p =0.0,
                F_bx_m =0.0,
                F_c_perp_x =0.0,
                F_vx =0.0,
                E_x =0.0,
                E_x_min =0.0,
                F_by =0.0,
                F_c_perp_y =0.0,
                F_vy =0.0,
                E_y =0.0,
                E_y_min =0.0,
                F_t =0.0,
                F_c =0.0,
                G =0.0
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
                            E_x = _E_x*Math.Pow(10,6),
                            E_x_min = _E_x_min * Math.Pow(10, 6),
                            F_by = _F_by,
                            F_c_perp_y = _F_c_perp_y,
                            F_vy = _F_vy,
                            E_y = _E_y * Math.Pow(10, 6),
                            E_y_min = _E_y_min * Math.Pow(10, 6),
                            F_t = _F_t,
                            F_c = _F_c,
                            G = _G
                        }  );
                    }
                }

            }

            #endregion

 
            var thisStressVals = glulamVals.Where(g => g.StressClass == this.StressClass.ToString()).FirstOrDefault();
            this.F_bx_p = thisStressVals.F_bx_p;
            this.F_bx_m = thisStressVals.F_bx_m;
            this.F_c_perp_x = thisStressVals.F_c_perp_x;
            this.F_vx = thisStressVals.F_vx;
            this.E_x = thisStressVals.E_x;
            this.E_x_min = thisStressVals.E_x_min;
            this.F_by = thisStressVals.F_by;
            this.F_c_perp_y = thisStressVals.F_c_perp_y;
            this.F_vy = thisStressVals.F_vy;
            this.E_y = thisStressVals.E_y;
            this.E_y_min = thisStressVals.E_y_min;
            this.F_t = thisStressVals.F_t;
            this.F_c = thisStressVals.F_c;
            this.G = thisStressVals.G;

            //Special cases
            if (WoodSpecies.ToString().StartsWith("28") || WoodSpecies.ToString().StartsWith("30"))
            {
                if (NumberOfLaminations > 15)
                {
                    E_x = 2.0 * Math.Pow(10, 6);
                    E_x_min = 1.06 * Math.Pow(10, 6);
                }
            }
 
        }
    }
}

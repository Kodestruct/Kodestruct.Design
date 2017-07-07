using Kodestruct.Concrete.ACI.ACI318_14.SectionalStrength.Shear.TwoWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public class ReinforcedSectionTwoWayShear
    {
        public ReinforcedSectionTwoWayShear(IConcreteMaterial Material, IRebarMaterial Rebar,
            PunchingPerimeterData Perimeter, double A_v, double s, PunchingReinforcementType PunchingReinforcementType)
        {
            this.Material = Material;
            this.Perimeter = Perimeter;
            this.Rebar = Rebar;
            this.A_v                       =A_v                       ;
            this.s                         =s                         ;
            this.PunchingReinforcementType = PunchingReinforcementType;
        }

        IConcreteMaterial Material { get; set; }
        PunchingPerimeterData Perimeter { get; set; }
        IRebarMaterial Rebar { get; set; }
        double A_v {get; set;}
        double s { get; set; }
        PunchingReinforcementType PunchingReinforcementType { get; set; }

        public double GetTwoWayStrength()
        {

            double f_c = Material.SpecifiedCompressiveStrength;
            double f_y = Rebar.YieldStress;
            double lambda = Material.lambda;

            double b_o = this.Perimeter.Segments.Sum(s => s.Length);
            double v_c;
            if (PunchingReinforcementType== PunchingReinforcementType.HeadedShearStuds)
            {
                v_c = lambda * 3.0 * Math.Sqrt(f_c);
            }
            else
            {
                v_c = lambda * 2.0 * Math.Sqrt(f_c);
            }

            double v_s = A_v * f_y / (b_o * this.s); // (4-13) per ACI 421R-08

            double v_n_Max= lambda * 6.0 * Math.Sqrt(f_c);

            double v_n = v_c + v_s;

            v_n = v_n > v_n_Max ? v_n_Max : v_n;

            return v_n;
        }
    }
}

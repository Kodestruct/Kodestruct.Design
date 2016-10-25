using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public class TorsionSectionGeneric : IConcreteTorsionalShape
    {
        public TorsionSectionGeneric(double A_oh, double A_cp, double p_h, double p_cp, double T_th, IConcreteMaterial Material)
        {

           this.T_th = T_th;
           this.A_oh =A_oh;
           this.A_cp =A_cp;
           this.p_h  =p_h ;
           this.p_cp = p_cp;
           this.Material = Material;
        }

        double A_oh ;
        double A_cp;
        double p_h  ;
        double p_cp;

        double T_th;
        public double GetA_oh()
        {
            return A_oh;
        }

        public double Get_p_h()
        {
            return p_h;
        }

        public double Get_T_th(double N_u)
        {
            return T_th;
        }


        public double GetA_cp()
        {
            throw new NotImplementedException();
        }

        public double Get_p_cp()
        {
            throw new NotImplementedException();
        }

        public IConcreteMaterial Material { get; set; }
    }
}

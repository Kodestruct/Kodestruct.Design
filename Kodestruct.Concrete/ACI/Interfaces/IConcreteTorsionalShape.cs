using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kodestruct.Concrete.ACI
{
    public interface IConcreteTorsionalShape
    {
        //Area within centerline of closed stirrups
        double GetA_oh();
        //area enclosed by outside perimeter of concrete
        double GetA_cp();

        //perimeter of centerline of outermost closed transverse torsional reinforcement
        double Get_p_h();

        //outside perimeter of concrete cross section
        double Get_p_cp();

        //Threshold torsion
        double Get_T_th(double N_u);

        IConcreteMaterial Material { get; set; }
    }
}

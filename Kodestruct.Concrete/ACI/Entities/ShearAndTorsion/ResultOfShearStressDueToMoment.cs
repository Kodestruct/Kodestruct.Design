using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.Entities
{
    public class ResultOfShearStressDueToMoment
    {
        public double v_max { get; set; }
        public double v_min { get; set; }

        public double J_x { get; set; }
        public double J_y { get; set; }
        public double theta { get; set; }

        public ResultOfShearStressDueToMoment(double v_max, double v_min, double J_x, double J_y, double theta)
        {
            this.v_max =v_max ;
            this.v_min =v_min ;
            this.J_x   =J_x   ;
            this.J_y   =J_y   ;
            this.theta = theta;
        }

        public ResultOfShearStressDueToMoment(double v_max, double v_min)
        {
            this.v_max = v_max;
            this.v_min = v_min;
        }
    }
}

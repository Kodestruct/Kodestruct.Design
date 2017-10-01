using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.Entities
{
    public class PMPair
    {
        public double P { get; set; }
        public double M { get; set; }

        public PMPair(double P, double M)
        {
            this.P = P;
            this.M = M;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public interface IConcreteCompressionMaterial
    {
         double CompressiveStrength { get; set; }
         double beta { get; set; }
    }
}

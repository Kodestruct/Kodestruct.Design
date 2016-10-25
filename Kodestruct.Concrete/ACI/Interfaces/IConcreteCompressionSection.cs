using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public interface IConcreteCompressionSection
    {

        IConcreteCompressionMaterial Material { get; set; }
        ISliceableSection SliceableShape { get; }
    }
}

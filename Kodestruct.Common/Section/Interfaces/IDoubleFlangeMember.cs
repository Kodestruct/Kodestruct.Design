using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Section.Interfaces
{

    public interface IDoubleFlangeMember : ISection
    {
        double d { get; }
        double t_f { get; }
        double t_w { get; }
    }
}

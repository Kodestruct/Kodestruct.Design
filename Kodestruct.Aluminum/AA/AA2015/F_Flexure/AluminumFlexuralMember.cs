using Kodestruct.Aluminum.AA.Entities;
using Kodestruct.Aluminum.AA.Entities.Exceptions;
using Kodestruct.Aluminum.AA.Entities.Interfaces;
using Kodestruct.Aluminum.AA.Entities.Section;
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.Flexure
{
    public partial class AluminumFlexuralMember : IAluminumBeamFlexure
    {

        public IAluminumSection Section { get; set; }
        public AluminumFlexuralMember()
        {

        }

        public AluminumFlexuralMember(IAluminumSection Section)
        {
            this.Section = Section;
        }
    }
}

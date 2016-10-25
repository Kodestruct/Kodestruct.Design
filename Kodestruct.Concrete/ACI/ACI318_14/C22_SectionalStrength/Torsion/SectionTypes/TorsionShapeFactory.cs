using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public class TorsionShapeFactory
    {
        public IConcreteTorsionalShape GetShape(ISection Shape, IConcreteMaterial Material, double c_transv_ctr)
        {
            if (Shape is ISectionRectangular)
            {
                SectionRectangular RectangularShape = Shape as SectionRectangular;
                return new TorsionSectionRectangularNonPrestressed(RectangularShape,Material, c_transv_ctr);
            }
            else if (Shape is TorsionSectionGeneric)
	        {
                return Shape as TorsionSectionGeneric;
	        }
            else
            {
                throw new Exception("Shape type not supported for torsional analysis.");
            }
        }
    }
}

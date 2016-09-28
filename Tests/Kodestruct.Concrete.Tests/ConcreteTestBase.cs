using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
    public class ConcreteTestBase: ToleranceTestBase
    {
        public IConcreteMaterial GetConcreteMaterial(double fc)
        {
            CalcLog log = new CalcLog();
            ConcreteMaterial concrete = new ConcreteMaterial(fc, ConcreteTypeByWeight.Normalweight, log);
            return concrete;
        }

        public IConcreteSection GetRectangularSection(double Width, double Height, double fc)
        {
            IConcreteMaterial mat = GetConcreteMaterial(fc);
            CrossSectionRectangularShape section = new CrossSectionRectangularShape(mat, null, Width, Height);
            return section;
        }


    }
}

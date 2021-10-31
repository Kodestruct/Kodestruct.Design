 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI318_14.Materials;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI318_14.Tests.Prestressed
{
    public class PrestressedBeamTestBase
    {
        private ICalcLog log;

        public ICalcLog Log
        {
            get { return log; }
            set { log = value; }
        }
        public PrestressedBeamTestBase()
        {
 
        }

        protected IPrestressedConcreteMaterial GetPrestressedConcreteMaterial(double fc, double fci)
        {
            ConcretePrestressed concrete = new ConcretePrestressed(fc, fci, ConcreteTypeByWeight.Normalweight, log);
            return concrete;
        }
    }
}

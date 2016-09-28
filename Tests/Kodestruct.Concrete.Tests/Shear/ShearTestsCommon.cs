using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using NUnit.Framework;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI318_14.Materials;
using Kodestruct.Concrete.ACI.Entities;



namespace Kodestruct.Concrete.ACI318_14.Tests.Shear
{
    [TestFixture]
    public partial class AciConcreteShearTestsBase : ToleranceTestBase
    {
        private ICalcLog log;

        public ICalcLog Log
        {
            get { return log; }
            set { log = value; }
        }

        private IConcreteMaterial mat;

        public IConcreteMaterial Material
        {
            get
            {
                if (mat == null)
                {
                    mat = new ConcreteMaterial(4000, ConcreteTypeByWeight.Normalweight, log);
                }
                return mat;
            }
            set { mat = value; }
        }




        protected double tolerance;

        public AciConcreteShearTestsBase()
        {
            //ICalcLogEntry entryStub = mocks.Stub<ICalcLogEntry>();
            MockRepository mocks = new MockRepository();
            log = mocks.Stub<ICalcLog>();
            tolerance = 0.02; //2% can differ from rounding
        }

        public IConcreteMaterial GetConcreteMaterial(double fc, bool IsLightWeight)
        {
            ConcreteMaterial concrete;
            if (IsLightWeight == true)
            {
                concrete = new ConcreteMaterial(fc, ConcreteTypeByWeight.Lightweight, log);
            }
            else
            {
                concrete = new ConcreteMaterial(fc, ConcreteTypeByWeight.Normalweight, log);
            }

            return concrete;
        }

        public IConcreteSection GetRectangularSection(double Width, double Height, double fc, bool IsLightWeight)
        {
            IConcreteMaterial mat = GetConcreteMaterial(fc, IsLightWeight);
            CrossSectionRectangularShape section = new CrossSectionRectangularShape(mat, null, Width, Height);
            return section;
        }

        public ConcreteSectionOneWayShearNonPrestressed GetConcreteOneWayShearBeam(double Width, double Height, double fc, double d, bool IsLightWeight)
        {

            IConcreteSection Section = GetRectangularSection(Width, Height, fc,IsLightWeight);

            ConcreteSectionOneWayShearNonPrestressed beam = new ConcreteSectionOneWayShearNonPrestressed(d,Section);
            return beam;
        }
    }


}

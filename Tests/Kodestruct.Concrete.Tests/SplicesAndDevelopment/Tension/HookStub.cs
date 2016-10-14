//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI318_14.Materials;



namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class StandardHookTests
    {

        private StandardHookInTension CreateHookObject(double ConcStrength, double RebarDiameter)
        {
            bool IsEpoxyCoated = false;
            ConcreteTypeByWeight typeByWeight = ConcreteTypeByWeight.Normalweight;
            double ExcessFlexureReinforcementRatio = 1.0;

            return this.CreateHookObject(ConcStrength, RebarDiameter, IsEpoxyCoated, typeByWeight, ExcessFlexureReinforcementRatio);
        }


        private StandardHookInTension CreateHookObject(double ConcStrength, double RebarDiameter, bool IsEpoxyCoated,
    ConcreteTypeByWeight typeByWeight, double ExcessFlexureReinforcementRatio)
        {
            MockRepository mocks = new MockRepository();

            IRebarMaterial rebarMat = mocks.Stub<IRebarMaterial>();
            Expect.Call(rebarMat.YieldStress).Return(60000);
            Rebar rebar = new Rebar(RebarDiameter,IsEpoxyCoated, rebarMat);

            ICalcLogEntry entryStub = mocks.Stub<ICalcLogEntry>();
            ICalcLog logStub = mocks.Stub<ICalcLog>();

            //IConcreteMaterial ConcStub = mocks.Stub<IConcreteMaterial>();
            IConcreteMaterial ConcStub = new ConcreteMaterial(ConcStrength, typeByWeight, logStub) as IConcreteMaterial;
            ConcStub.SpecifiedCompressiveStrength = ConcStrength;
            ConcStub.TypeByWeight = typeByWeight;


            using (mocks.Record())
            {
                logStub.CreateNewEntry();
                LastCall.Return(entryStub);
            }

            StandardHookInTension tensHook = new StandardHookInTension(ConcStub, rebar, logStub, ExcessFlexureReinforcementRatio);

            return tensHook;
        }


    }



}

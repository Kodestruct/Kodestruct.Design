//Sample license text.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;


namespace Kodestruct.Concrete.ACI318_14.Tests
{
    [TestFixture]
    public partial class AciTensionDevelopmentTests : ToleranceTestBase
    {

        private DevelopmentTension CreateDevelopmentObject(double ConcStrength, double ClearSpacing, double ClearCover, bool IsTopRebar,
            double ExcessRebarRatio, bool checkMinLength)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject(ConcStrength, ConcreteTypeByWeight.Lightweight,
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength);

            return tensDev;
        }

        private DevelopmentTension CreateDevelopmentObject(double ConcStrength, ConcreteTypeByWeight typeByWeight, double ClearSpacing, double ClearCover, bool IsTopRebar,
        double ExcessRebarRatio, bool checkMinLength)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject(ConcStrength, ConcreteTypeByWeight.Lightweight, 0.0,
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength);

            return tensDev;
        }

        private DevelopmentTension CreateDevelopmentObject(double RebarDiameter, double ClearSpacing, double ClearCover)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject(4000.0, RebarDiameter, true, ConcreteTypeByWeight.Lightweight, 0.0,
                            ClearSpacing, ClearCover, false, 1.0, false);

            return tensDev;
        }

        private DevelopmentTension CreateDevelopmentObject(double RebarDiameter, bool IsEpoxyCoated, double ClearSpacing, double ClearCover)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject(4000.0, RebarDiameter, IsEpoxyCoated, ConcreteTypeByWeight.Lightweight, 0.0,
                            ClearSpacing, ClearCover, false, 1.0, false);

            return tensDev;
        }

        private DevelopmentTension CreateDevelopmentObject(double ConcStrength, ConcreteTypeByWeight typeByWeight, double AverageSplitStrength,
            double ClearSpacing, double ClearCover, bool IsTopRebar, double ExcessRebarRatio, bool checkMinLength)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject(ConcStrength, 1.0, false, typeByWeight, AverageSplitStrength,
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength);

            return tensDev;

        }



        private DevelopmentTension CreateDevelopmentObject(double ConcStrength, double RebarDiameter, bool IsEpoxyCoated,
        ConcreteTypeByWeight typeByWeight, double AverageSplitStrength,
        double ClearSpacing, double ClearCover, bool IsTopRebar, double ExcessRebarRatio, bool checkMinLength)
        {
            DevelopmentTension tensDev = this.CreateDevelopmentObject
                (ConcStrength, RebarDiameter, IsEpoxyCoated, typeByWeight, TypeOfLightweightConcrete.AllLightweightConcrete, AverageSplitStrength,
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength);

            return tensDev;

        }



        private DevelopmentTension CreateDevelopmentObject(double ConcStrength, double RebarDiameter, bool IsEpoxyCoated,
    ConcreteTypeByWeight typeByWeight, TypeOfLightweightConcrete lightWeightType, double AverageSplitStrength,
    double ClearSpacing, double ClearCover, bool IsTopRebar, double ExcessRebarRatio, bool checkMinLength)
        {
            MockRepository mocks = new MockRepository();

            IRebarMaterial rebarMat = mocks.Stub<IRebarMaterial>();
            Expect.Call(rebarMat.YieldStress).Return(60000);
            //IRebarMaterial rebarMat = new MaterialAstmA706() as IRebarMaterial;

            ICalcLogEntry entryStub = mocks.Stub<ICalcLogEntry>();
            //entryStub.DependencyValues = new Dictionary<string, double>();
            ICalcLog logStub = mocks.Stub<ICalcLog>();

            //IConcreteMaterial ConcStub = mocks.Stub<IConcreteMaterial>();
            IConcreteMaterial ConcStub = new ConcreteMaterial(ConcStrength, typeByWeight, lightWeightType,
                logStub) as IConcreteMaterial;
            ConcStub.SpecifiedCompressiveStrength = ConcStrength;
            ConcStub.TypeByWeight = typeByWeight;
            ConcStub.AverageSplittingTensileStrength = AverageSplitStrength;


            using (mocks.Record())
            {
                logStub.CreateNewEntry();
                LastCall.Return(entryStub);
            }

            DevelopmentTension tensDev = new DevelopmentTension(ConcStub,
                            new Rebar(RebarDiameter, IsEpoxyCoated, rebarMat),
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength, logStub);

            return tensDev;
        }

    }

}

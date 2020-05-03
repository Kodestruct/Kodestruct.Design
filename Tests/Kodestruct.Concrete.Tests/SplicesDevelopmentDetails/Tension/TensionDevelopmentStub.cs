 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Materials;


namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
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
 
            IRebarMaterial rebarMat = new RebarMaterialGeneral(60000);
 
 
            //IConcreteMaterial ConcStub = mocks.Stub<IConcreteMaterial>();
            IConcreteMaterial ConcStub = new ConcreteMaterial(ConcStrength, typeByWeight, lightWeightType,
                null) as IConcreteMaterial;
            ConcStub.SpecifiedCompressiveStrength = ConcStrength;
            ConcStub.TypeByWeight = typeByWeight;
            ConcStub.AverageSplittingTensileStrength = AverageSplitStrength;

 

            DevelopmentTension tensDev = new DevelopmentTension(ConcStub,
                            new Rebar(RebarDiameter, IsEpoxyCoated, rebarMat),
                            ClearSpacing, ClearCover, IsTopRebar, ExcessRebarRatio, checkMinLength, null);

            return tensDev;
        }

    }

}

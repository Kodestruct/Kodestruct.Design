 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI318_14.Materials;



namespace Kodestruct.Concrete.ACI318_14.Tests
{
     
    public partial class AciStandardHookTests
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


            IRebarMaterial rebarMat = new RebarMaterialGeneral(60000);
            Rebar rebar = new Rebar(RebarDiameter,IsEpoxyCoated, rebarMat);

 
            IConcreteMaterial ConcStub = new ConcreteMaterial(ConcStrength, typeByWeight, null) as IConcreteMaterial;
            ConcStub.SpecifiedCompressiveStrength = ConcStrength;
            ConcStub.TypeByWeight = typeByWeight;



            StandardHookInTension tensHook = new StandardHookInTension(ConcStub, rebar, null, ExcessFlexureReinforcementRatio);

            return tensHook;
        }


    }



}

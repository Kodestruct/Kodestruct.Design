#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;



namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteFlexuralSectionBase : ConcreteSectionLongitudinalReinforcedBase, IConcreteFlexuralMember
    {
        public virtual double GetCrackedMomentOfInertia(FlexuralCompressionFiberPosition compFiberPosition)
        {
            //Get LinearStrainDistribution from flexural result
            double kd = FindCrackedSectionNeutralAxis(compFiberPosition);

            IMoveableSection concreteShape = GetConcreteShape();
            List<IMoveableSection> allShapes = GetRebarShapes();
            //allShapes.Add(concreteShape);

            double Ix = 0.0;
            //Find centroid
            double SumAreaDistance = allShapes.Sum(s => s.A*s.GetElasticCentroidCoordinate().Y);
            double SumArea = allShapes.Sum(s => s.A );
            double YCentr = SumAreaDistance / SumArea;

            //Find moment of inertia
            double I_cr = allShapes.Sum(s => s.A * Math.Pow(YCentr-s.GetElasticCentroidCoordinate().Y,2.0) + s.I_x);
            return I_cr;
        }

        private List<IMoveableSection> GetRebarShapes()
        {
            List<IMoveableSection> AllBars = new List<IMoveableSection>();
            AllBars.AddRange(CompShapeTransformed);
            AllBars.AddRange(TensBarsTransformed);

            return AllBars;
        }

        private IMoveableSection GetConcreteShape()
        {
            return compressedConcretePortion;
        }

        private double FindCrackedSectionNeutralAxis(FlexuralCompressionFiberPosition compFiberPosition)
        {
            double targetDelta = 0.0;
            double CompressedBlockMin  = 0.0;
            double CompressedBlockMax = this.Section.SliceableShape.YMax - this.Section.SliceableShape.YMin;
            currentCompressionFiberPosition = compFiberPosition; //store this off because it will be necessary during iteration
            double ConvergenceTolerance = 0.0001;

            double kd = RootFinding.Brent(new FunctionOfOneVariable(DeltaA_times_Y), CompressedBlockMin, CompressedBlockMax, ConvergenceTolerance, targetDelta);
            return kd;
        }

        private double DeltaA_times_Y(double kd)
        {
            CalculateCrackedSectionTransformedShapes(kd, currentCompressionFiberPosition);

            double YNeutral = GetYNeutral(kd);

            double A_times_y_Compression = CrackedMomentOfInertiaCompressedShapes.Sum(sh => 
               { 
                    if (sh.A ==0.0)
	                {
                        return 0.0;
	                }
                            else
	                {
                            return sh.A * Math.Abs(sh.GetElasticCentroidCoordinate().Y-YNeutral);
	                }
                   
               }
                );
            //double A_times_y_Tension = CrackedMomentOfInertiaTensionShapes.Sum(sh => 
            //    {
            //       return sh.A * Math.Abs(sh.GetElasticCentroidCoordinate().Y - YNeutral);
            //    })
            //    ;

            double A_times_y_Tension = 0.0;
            foreach (var tensionBar in CrackedMomentOfInertiaTensionShapes)
            {
                double bar_d = Math.Abs(tensionBar.GetElasticCentroidCoordinate().Y - YNeutral);
                double thisBarATimesY = tensionBar.A * bar_d;
                A_times_y_Tension = A_times_y_Tension + thisBarATimesY;
            }

            return A_times_y_Compression - A_times_y_Tension;
        }

        double GetYNeutral(double kd)
        {
            double YNeutral = 0.0;

            switch (currentCompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    YNeutral = this.Section.SliceableShape.YMax - kd;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    YNeutral = this.Section.SliceableShape.YMin + kd;
                    break;
                default:
                    throw new CompressionFiberPositionException();

            }

            return YNeutral;
        }

        // variables to store shapes during iteration
        List<IMoveableSection> CrackedMomentOfInertiaCompressedShapes;
        List<IMoveableSection> CrackedMomentOfInertiaTensionShapes;
         List<IMoveableSection> CompShapeTransformed;
         List<IMoveableSection> TensBarsTransformed;
         IMoveableSection compressedConcretePortion;

        private void CalculateCrackedSectionTransformedShapes(double kd, FlexuralCompressionFiberPosition compFiber)
        {
            compressedConcretePortion = null;
            CompShapeTransformed = new List<IMoveableSection>();
            TensBarsTransformed = new List<IMoveableSection>();
            CrackedMomentOfInertiaCompressedShapes = new List<IMoveableSection>();
            CrackedMomentOfInertiaTensionShapes = new List<IMoveableSection>();

            double YNeutral = GetYNeutral(kd);

            switch (compFiber)
            {
                case FlexuralCompressionFiberPosition.Top:

                    //1. Concrete Shape (compression)
                    compressedConcretePortion = this.Section.SliceableShape.GetTopSliceSection(kd, SlicingPlaneOffsetType.Top);
                    //2. Steel bars (compression)
                    CompShapeTransformed= GetTransformedRebarShapes( this.Section.SliceableShape.YMax,  YNeutral);
                    CompShapeTransformed.Add(compressedConcretePortion);
                    CrackedMomentOfInertiaCompressedShapes = CompShapeTransformed;
                    //3 Steel bars (tension)
                    TensBarsTransformed = GetTransformedRebarShapes(YNeutral, this.Section.SliceableShape.YMin);
                    CrackedMomentOfInertiaTensionShapes = TensBarsTransformed;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:

                    //1. Concrete Shape (compression)
                    compressedConcretePortion = this.Section.SliceableShape.GetBottomSliceSection(kd, SlicingPlaneOffsetType.Bottom);
                    //2. Steel bars (compression)
                    CompShapeTransformed = GetTransformedRebarShapes(YNeutral, this.Section.SliceableShape.YMin);
                    CompShapeTransformed.Add(compressedConcretePortion);
                    CrackedMomentOfInertiaCompressedShapes = CompShapeTransformed;
                    //3 Steel bars (tension)
                    TensBarsTransformed = GetTransformedRebarShapes(this.Section.SliceableShape.YMax, YNeutral);
                    CrackedMomentOfInertiaTensionShapes = TensBarsTransformed;
                    break;

                    break;
                default:
                    throw new CompressionFiberPositionException();
            }


        }





    }
}

#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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

 

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteFlexuralSectionBase : ConcreteSectionLongitudinalReinforcedBase, IConcreteFlexuralMember
    {

        protected double MaxSteelStrain { get; set; }
        private double maxConcreteStrain;




        protected virtual double CalculateMaximumSteelStrain(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            double MaxStrain = 0.0;

            if (CompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                var LowestPointY = LongitudinalBars.Min(yVal => yVal.Coordinate.Y);
                var PointsAtLowestY = LongitudinalBars.Where(point => point.Coordinate.Y == LowestPointY).Select(point => point);
                var LimitStrain = PointsAtLowestY.Min(point => point.Rebar.Material.GetUltimateStrain(point.Rebar.Diameter));
                MaxStrain = -LimitStrain;
            }
            else
            {
                var HighestPointY = LongitudinalBars.Max(yVal => yVal.Coordinate.Y);
                var PointsAtHighestY = LongitudinalBars.Where(point => point.Coordinate.Y == HighestPointY).Select(point => point);
                var LimitStrain = PointsAtHighestY.Min(point => point.Rebar.Material.GetUltimateStrain(point.Rebar.Diameter));
                MaxStrain = -LimitStrain;
            }
            return MaxStrain;
        }

        private LinearStrainDistribution GetStrainMaxTensionMaxCompression(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {

            //Calulate limit strain for the lowest rebar point
            //Distinguish between top and bottom tension cases
            double StrainDistributionHeight = 0.0;
            //double MaxStrain = CalculateMaximumSteelStrain(CompressionFiberPosition);
            double MaxStrain = this.MaxSteelStrain;

            LinearStrainDistribution MaxMaxDistribution;
            StrainDistributionHeight = GetStrainDistributionHeight(CompressionFiberPosition);

            if (CompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                var LowestPointY = LongitudinalBars.Min(yVal => yVal.Coordinate.Y);
                var PointsAtLowestY = LongitudinalBars.Where(point => point.Coordinate.Y == LowestPointY).Select(point => point);
                var LimitStrain = PointsAtLowestY.Min(point => point.Rebar.Material.GetUltimateStrain(point.Rebar.Diameter));
                MaxStrain = LimitStrain;
                MaxMaxDistribution = new LinearStrainDistribution
                (StrainDistributionHeight, this.MaxConcreteStrain, -MaxStrain);
            }
            else
            {
                var HighestPointY = LongitudinalBars.Max(yVal => yVal.Coordinate.Y);
                var PointsAtHighestY = LongitudinalBars.Where(point => point.Coordinate.Y == HighestPointY).Select(point => point);
                var LimitStrain = PointsAtHighestY.Min(point => point.Rebar.Material.GetUltimateStrain(point.Rebar.Diameter));
                MaxStrain = LimitStrain;
                MaxMaxDistribution = new LinearStrainDistribution
                (StrainDistributionHeight, -MaxStrain, this.MaxConcreteStrain);
            }

            return MaxMaxDistribution;
        }

        protected virtual LinearStrainDistribution GetInitialStrainEstimate(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {
            //Find rebar resultant force
            // for initial estimate include only rebar below the centroid line 
            // as a reasonable estimate for most regular beams
            double centroidY = Section.SliceableShape.YMin + Section.SliceableShape.y_Bar;
            ForceMomentContribution rebarResultant = null;

            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    rebarResultant = GetApproximateRebarResultant(BarCoordinateFilter.Y, BarCoordinateLimitFilterType.Maximum, centroidY);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    rebarResultant = GetApproximateRebarResultant(BarCoordinateFilter.Y, BarCoordinateLimitFilterType.Minimum, centroidY);
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

         //Get corresponding strain
            double a = GetCompressionBlockDepth(rebarResultant.Force, CompressionFiberPosition);
            LinearStrainDistribution strainDistribution = GetStrainDistributionBasedOn_a(a, CompressionFiberPosition);

            return strainDistribution;
        }

        protected virtual SectionAnalysisResult FindMomentResultInPresenceOfAxialForce(FlexuralCompressionFiberPosition CompressionFiberPosition,
          double P,   double MaxSteelStrain,  double StrainConvergenceTolerance = 0.00001)
        {
            currentCompressionFiberPosition = CompressionFiberPosition; //store this off because it will be necessary during iteration
            StrainHeight = GetStrainDistributionHeight(CompressionFiberPosition);//store this off because it will be necessary during iteration

            double StrainMin = MaxSteelStrain;
            double StrainMax = StrainUltimateConcrete.Value;
                 
            double targetP = P;
            LinearStrainDistribution finalStrainDistribution = null;

            double SteelStrain = RootFinding.Brent(new FunctionOfOneVariable(SectionAxialForceResultantFunction), StrainMin, StrainMax, 
            StrainConvergenceTolerance, targetP);
            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    
                        finalStrainDistribution = new LinearStrainDistribution(StrainHeight, this.MaxConcreteStrain, SteelStrain);

                    break;
                case FlexuralCompressionFiberPosition.Bottom:

                        finalStrainDistribution = new LinearStrainDistribution(StrainHeight, SteelStrain, this.MaxConcreteStrain);

                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            SectionAnalysisResult finalResult = GetSectionResult(finalStrainDistribution, CompressionFiberPosition);
            return finalResult;
        }


        protected virtual SectionAnalysisResult FindPureMomentResult(FlexuralCompressionFiberPosition CompressionFiberPosition,
             TCIterationBound bound,  double StrainConvergenceTolerance = 0.00001)
        {
            currentCompressionFiberPosition = CompressionFiberPosition; //store this off because it will be necessary during iteration
            StrainHeight = GetStrainDistributionHeight(CompressionFiberPosition);//store this off because it will be necessary during iteration


            double SteelStrain = 0;
            double StrainMax = bound.MaxStrain;
            double StrainMin = bound.MinStrain;

            double targetTCDelta = 0;
            LinearStrainDistribution finalStrainDistribution = null;

            //SteelStrain = RootFinding.Brent(new FunctionOfOneVariable(GetTandCDeltaForSteelStrainIteration), TCDeltaMin, TCDeltaMax, StrainConvergenceTolerance, targetTCDelta);
            SteelStrain = RootFinding.Brent(new FunctionOfOneVariable(DeltaTCCalculationFunction), StrainMin, StrainMax, StrainConvergenceTolerance, targetTCDelta);
            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    
                        finalStrainDistribution = new LinearStrainDistribution(StrainHeight, this.MaxConcreteStrain, SteelStrain);

                    break;
                case FlexuralCompressionFiberPosition.Bottom:

                        finalStrainDistribution = new LinearStrainDistribution(StrainHeight, SteelStrain, this.MaxConcreteStrain);

                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            SectionAnalysisResult finalResult = GetSectionResult(finalStrainDistribution, CompressionFiberPosition);
            return finalResult;
        }

        FlexuralCompressionFiberPosition currentCompressionFiberPosition = FlexuralCompressionFiberPosition.Top;

        double StrainHeight;

        private double DeltaTCCalculationFunction(double SteelStrain)
        {
            //function of CurrentCompressionFiberPosition
            //1. create trial strain
            LinearStrainDistribution iteratedStrainDistribution = null;
            switch (currentCompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                        iteratedStrainDistribution = new LinearStrainDistribution(StrainHeight, this.MaxConcreteStrain, SteelStrain);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                        iteratedStrainDistribution = new LinearStrainDistribution(StrainHeight, SteelStrain, this.MaxConcreteStrain);
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }
            //2. Calculate result
            SectionAnalysisResult iteratedResult = GetSectionResult(iteratedStrainDistribution, currentCompressionFiberPosition);
            //3. calculate difference between T and C
                double T = iteratedResult.TForce;
                double C = iteratedResult.CForce;

            return Math.Abs(T)-Math.Abs(C);
        }



        private double SectionAxialForceResultantFunction(double SteelStrain)
        {
            //Create trial strain
            LinearStrainDistribution iteratedStrainDistribution = null;
            switch (currentCompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    iteratedStrainDistribution = new LinearStrainDistribution(StrainHeight, this.MaxConcreteStrain, SteelStrain);
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    iteratedStrainDistribution = new LinearStrainDistribution(StrainHeight, SteelStrain, this.MaxConcreteStrain);
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }
            //Calculate result
            SectionAnalysisResult iteratedResult = GetSectionResult(iteratedStrainDistribution, currentCompressionFiberPosition);

            return iteratedResult.AxialForce;
        }

        protected virtual TCIterationBound GetSolutionBoundaries(SectionAnalysisResult result, LinearStrainDistribution ApproximationStrainDistribution)
        {
            double t = Math.Abs(result.TForce);
            double c = Math.Abs(result.CForce);

            SectionAnalysisResult secondResult = new SectionAnalysisResult();
            TCIterationBound data = new TCIterationBound();
            LinearStrainDistribution secondApproximationStrainDistribution = null;
            double MaxSteelStrain = CalculateMaximumSteelStrain(currentCompressionFiberPosition);

            //Step 1: create adjusted strain distribution
            if (t>c)
            {
                if (currentCompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
                {
                    secondApproximationStrainDistribution = new LinearStrainDistribution(ApproximationStrainDistribution.Height,
                        ApproximationStrainDistribution.TopFiberStrain, this.MaxConcreteStrain);
                }
                else
                {
                    secondApproximationStrainDistribution = new LinearStrainDistribution(ApproximationStrainDistribution.Height,
                         this.MaxConcreteStrain, ApproximationStrainDistribution.BottomFiberStrain);
                }
                
            }
            else
            {
                if (currentCompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
                {
                    secondApproximationStrainDistribution = new LinearStrainDistribution(ApproximationStrainDistribution.Height,
                        ApproximationStrainDistribution.TopFiberStrain, MaxSteelStrain);
                }
                else
                {
                    secondApproximationStrainDistribution = new LinearStrainDistribution(ApproximationStrainDistribution.Height,
                     MaxSteelStrain, ApproximationStrainDistribution.BottomFiberStrain);
                }
                
            }

            //Step 2: Fill in data for output
            if (currentCompressionFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                data.MinStrain = -Math.Max(Math.Abs(ApproximationStrainDistribution.BottomFiberStrain), Math.Abs(secondApproximationStrainDistribution.BottomFiberStrain));
                data.MaxStrain = -Math.Min(Math.Abs(ApproximationStrainDistribution.BottomFiberStrain), Math.Abs(secondApproximationStrainDistribution.BottomFiberStrain));    
            }
            else
            {
                data.MinStrain = -Math.Max(Math.Abs(ApproximationStrainDistribution.TopFiberStrain), Math.Abs(secondApproximationStrainDistribution.TopFiberStrain));
                data.MaxStrain = -Math.Min(Math.Abs(ApproximationStrainDistribution.TopFiberStrain), Math.Abs(secondApproximationStrainDistribution.TopFiberStrain));   
            }

            return data;
        }


    }
}

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
using System.Text;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteSectionLongitudinalReinforcedBase : ConcreteSectionBase, IConcreteSectionWithLongitudinalRebar 
    {

        public ConcreteSectionLongitudinalReinforcedBase(IConcreteSection Section, List<RebarPoint> LongitudinalBars, ICalcLog CalcLog)
			: base(Section,CalcLog)
		{
			this.Section = Section;
			this.longitBars = LongitudinalBars;
		}

        private  List<RebarPoint>  longitBars;

        public  List<RebarPoint>  LongitudinalBars
        {
            get { return longitBars; }
            set { longitBars = value; }
        }

       
 

        protected virtual  List<RebarPointResult> CalculateRebarResults(LinearStrainDistribution StrainDistribution )
        {
            List<RebarPointResult> ResultList = new List<RebarPointResult>();

            double c = StrainDistribution.NeutralAxisTopDistance;
            double h = StrainDistribution.Height;
            double YMax = Section.SliceableShape.YMax;
            double YMin = Section.SliceableShape.YMin;
            double XMax = Section.SliceableShape.XMax;
            double XMin = Section.SliceableShape.XMin;

            double sectionHeight = Section.SliceableShape.YMax - Section.SliceableShape.YMin;
            double distTopToSecNeutralAxis = sectionHeight - Section.SliceableShape.y_Bar;

                foreach (RebarPoint rbrPnt in LongitudinalBars)
                {
                    double BarDistanceToTop = YMax - rbrPnt.Coordinate.Y;
                    double BarDistanceToNa = BarDistanceToTop - distTopToSecNeutralAxis;
                    double Strain = StrainDistribution.GetStrainAtPointOffsetFromTop(BarDistanceToTop);
                    double Force;
                    double Stress;


                            Force = rbrPnt.Rebar.GetForce(Strain);
                            Stress = rbrPnt.Rebar.GetStress(Strain);
                            ResultList.Add(new RebarPointResult(Stress, Strain, Force, BarDistanceToNa, rbrPnt));


                } 


            return ResultList;
        }


       protected Point2D FindRebarGeometricCentroid(List<RebarPoint> bars)
       {
           double totalArea=0;
           double SumAreaMomentsX=0;
           double SumAreaMomentsY=0;

           if (bars.Count == 0)
           {
               throw new NoRebarException();
           }

           foreach (var bar in bars)
           {
               double Area = bar.Rebar.Area;
               double MomentX = Area * bar.Coordinate.Y;
               double MomentY = Area * bar.Coordinate.X;
               //RebarAreaMoment fm = new RebarAreaMoment() { Area = Area, MomentX = MomentX, MomentY = MomentY };
               totalArea += Area;
               SumAreaMomentsX += MomentX;
               SumAreaMomentsY += MomentY;
           }
           double LocationX = SumAreaMomentsY / totalArea;
           double LocationY = SumAreaMomentsX / totalArea;
           return new Point2D(LocationX, LocationY);
       }

    protected RebarPoint FindRebarWithExtremeCoordinate(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilter, double CutoffCoordinate)
    {
        RebarPoint returnBar = null;

        RebarPoint maxXRebar = null;
        RebarPoint maxYRebar = null;
        RebarPoint minXRebar = null;
        RebarPoint minYRebar = null;

        double MaxY = double.NegativeInfinity;
        double MaxX = double.NegativeInfinity;
        double MinY = double.PositiveInfinity;
        double MinX = double.PositiveInfinity;

        if (longitBars.Count == 0)
        {
            throw new NoRebarException();
        }

        foreach (var bar in longitBars)
        {
            if (bar.Coordinate.X > MaxX)
            {
                if (bar.Coordinate.X<CutoffCoordinate)
                {
                    maxXRebar = bar;
                    MaxX = bar.Coordinate.X; 
                }
            }

            if (bar.Coordinate.Y > MaxY)
            {
                if (bar.Coordinate.Y < CutoffCoordinate)
                {
                    maxYRebar = bar;
                    MaxY = bar.Coordinate.Y;
                }
            }
            if (bar.Coordinate.X < MinX)
            {
                if (bar.Coordinate.X > CutoffCoordinate)
                {
                    minXRebar = bar;
                    MinX = bar.Coordinate.X;
                }
            }

            if (bar.Coordinate.Y < MinY)
            {
                if (bar.Coordinate.Y > CutoffCoordinate)
                {
                    minYRebar = bar;
                    MinY = bar.Coordinate.Y;
                }
            }

        }

        if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Maximum)
        {
            returnBar = maxXRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Minimum)
        {
            returnBar = minXRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Maximum)
        {
            returnBar = maxYRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Minimum)
        {
            returnBar = minYRebar;
        }

        return returnBar;

    }

    protected ForceMomentContribution GetRebarResultant(LinearStrainDistribution StrainDistribution, ResultantType resType )
    {
        ForceMomentContribution resultant = new ForceMomentContribution();
        //tension is negative
        List<RebarPointResult> RebarResults = CalculateRebarResults(StrainDistribution);
        foreach (var barResult in RebarResults)
        {
            if (resType == ResultantType.Tension)
            {
                if (barResult.Strain < 0)
                {
                    resultant.Force += barResult.Force;
                    resultant.Moment += barResult.Force * barResult.DistanceToNeutralAxis;
                }
            }
            else
            {
                if (barResult.Strain > 0)
                {
                    resultant.Force += barResult.Force;
                    resultant.Moment += barResult.Force * barResult.DistanceToNeutralAxis;
                }
            }
        }
        resultant.RebarResults = RebarResults;
        return resultant;
    }
    protected ForceMomentContribution GetRebarResultant(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilter, double CutoffCoordinate)
    {
        ForceMomentContribution resultant = new ForceMomentContribution();

            foreach (var bar in longitBars)
            {
                double barLimitForce = 0;
                double barLimitForceMoment = 0;

                if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Maximum)
                {
                    if (bar.Coordinate.X <= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Maximum)
                {
                    if (bar.Coordinate.Y <= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Minimum)
                {
                    if (bar.Coordinate.X >= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Minimum)
                {
                    if (bar.Coordinate.Y >= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                    }
                }

                ForceMomentContribution barResultant = new ForceMomentContribution(){ Force = barLimitForce, Moment=barLimitForceMoment};
                resultant += barResultant;
            }

            return resultant;

    }

    protected ForceMomentContribution GetApproximateRebarResultant(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilterType,
    double CutoffCoordinate)
    {
        ForceMomentContribution resultant = new ForceMomentContribution();

            double barLimitForce = 0;
            double barLimitForceMoment = 0;
            List<RebarPoint> filteredBars = GetFilteredBars(CutoffCoordinate, CoordinateFilter, LimitFilterType);

            foreach (var bar in filteredBars)
            {

                if (CoordinateFilter == BarCoordinateFilter.X) // && LimitFilterType == BarCoordinateLimitFilterType.Maximum)
                {

                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.X;

                }

                else
                {

                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.Y;

                }
            }


            ForceMomentContribution barResultant = new ForceMomentContribution() { Force = barLimitForce, Moment = barLimitForceMoment };
            resultant += barResultant;
        

        return resultant;

    }



        public enum BarCoordinateFilter
        {
            X,
            Y
        }

        public enum BarCoordinateLimitFilterType
        {
            Maximum,
            Minimum
        }

       private class RebarAreaMoment
       {
           public double Area { get; set; }
           public double MomentX { get; set; }
           public double MomentY { get; set; }
       }


       public double Get_d(double c, double h, FlexuralCompressionFiberPosition CompressionFiber)
       {

           //Find centroid of longitudinal rebar, calculate d
           //compare d with h
           double CutoffCoordinate;
           if (CompressionFiber == FlexuralCompressionFiberPosition.Top)
           {
               CutoffCoordinate = Section.SliceableShape.YMax - c;
               List<RebarPoint> filteredBars = GetFilteredBars(CutoffCoordinate, BarCoordinateFilter.Y, BarCoordinateLimitFilterType.Maximum);
           }
           else
           {
               CutoffCoordinate = Section.SliceableShape.YMin + c;
               List<RebarPoint> filteredBars = GetFilteredBars(CutoffCoordinate, BarCoordinateFilter.Y, BarCoordinateLimitFilterType.Minimum);
           }

           double A_total=0;
           double M_total = 0;

           
           foreach (var bar in longitBars)
           {

               A_total = A_total + bar.Rebar.Area;
               M_total = M_total + bar.Rebar.Area * bar.Coordinate.Y;
           }
  
           double YCentroid = M_total / A_total;

           double d;
           double d_rebar;

           if (CompressionFiber == FlexuralCompressionFiberPosition.Top)
           {
               d_rebar = Section.SliceableShape.YMax - YCentroid;
           }
           else
           {
               d_rebar =  YCentroid - Section.SliceableShape.YMin;
           }
           d = Math.Max(0.8 * h, d_rebar);

           return d;
       }


       public  List<RebarPoint> GetFilteredBars(double CutoffCoordinate, BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilterType)
       {

           List<RebarPoint> barPoints = new List<RebarPoint>();

           foreach (var bar in longitBars)
           {


               if (CoordinateFilter == BarCoordinateFilter.X && LimitFilterType == BarCoordinateLimitFilterType.Maximum)
               {
                   if (bar.Coordinate.X <= CutoffCoordinate)
                   {
                       barPoints.Add(bar);
                   }
               }

               if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilterType == BarCoordinateLimitFilterType.Maximum)
               {
                   if (bar.Coordinate.Y <= CutoffCoordinate)
                   {
                       barPoints.Add(bar);
                   }
               }

               if (CoordinateFilter == BarCoordinateFilter.X && LimitFilterType == BarCoordinateLimitFilterType.Minimum)
               {
                   if (bar.Coordinate.X >= CutoffCoordinate)
                   {
                       barPoints.Add(bar);
                   }
               }

               if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilterType == BarCoordinateLimitFilterType.Minimum)
               {
                   if (bar.Coordinate.Y >= CutoffCoordinate)
                   {
                       barPoints.Add(bar);
                   }
               }
           }

           return barPoints;
       }
    }
}

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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;

 

namespace Kodestruct.Concrete.ACI
{
    //public class ConcreteFlexuralSectionSinglyReinforcedBase : ConcreteFlexuralSectionBase, IConcreteFlexuralMember
    //{
    //    public ConcreteFlexuralSectionSinglyReinforcedBase(IConcreteSectionRectangular Section, 
    //        List<RebarPoint> LongitudinalBars, ICalcLog log)
    //        : base(Section,LongitudinalBars,log)
    //    {
    //        RectangularSection = Section;
    //    }

    //    IConcreteSectionRectangular RectangularSection;

    //    public ConcreteSectionFlexuralAnalysisResult GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition CompressionFiberPosition)
    //    {

    //        double Tforce = this.GetTForce();
    //        double DepthOfCompressionBlock_a = GetCompressionBlockDepth(Tforce, CompressionFiberPosition);
    //        double h = RectangularSection.Height;

    //        double d = this.Get_d();
    //        double Mn = Tforce * (d - DepthOfCompressionBlock_a / 2.0);
    //        double M = Mn / 12.0; //kip*ft 

            

    //        LinearStrainDistribution strainDistribution = GetStrainDistributionBasedOn_a(DepthOfCompressionBlock_a, CompressionFiberPosition);
    //        ConcreteSectionFlexuralAnalysisResult Mn_result = new ConcreteSectionFlexuralAnalysisResult(Mn, strainDistribution,null);
    //        return Mn_result;
    //    }

    //    double d;

        //double Get_d()
        //{
        //    double d = 0.0;
        //    double YMax = Section.SliceableShape.YMax;

        //    if (LongitudinalBars.Count > 1)
        //    {
        //        double As = LongitudinalBars.Sum(r => r.Rebar.Area);
                

        //        double SumAreaTimesDepth = LongitudinalBars.Sum(r => 
        //        {
        //            double d_c = YMax - r.Coordinate.Y;
        //            double Abar = r.Rebar.Area;
        //            return Abar * d_c;
        //        });

        //        d = SumAreaTimesDepth / As;
        //    }
        //    else
        //    {
        //        RebarPoint rebar = LongitudinalBars.First();
        //        d = this.Section.SliceableShape.YMax-rebar.Coordinate.Y;
        //    }

        //    return d;
        //}
        //double GetTForce()
        //{
        //    double Tforce = 0.0;
        //    if (LongitudinalBars.Count > 1)
        //    {
        //        double As = LongitudinalBars.Sum(a => a.Rebar.Area);
        //        Tforce = LongitudinalBars.Sum(a => a.Rebar.Area * a.Rebar.Material.YieldStress);
        //    }
        //    else
        //    {
        //        RebarPoint rebar = LongitudinalBars.First();
        //        double As = rebar.Rebar.Area;
        //        double Fy = rebar.Rebar.Material.YieldStress;

        //        Tforce = As * Fy;
        //    }

        //    return Tforce;
        //}
   // }
}

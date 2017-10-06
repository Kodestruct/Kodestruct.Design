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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class ConcreteSectionLongitudinalReinforcedBase : ConcreteSectionBase, IConcreteSectionWithLongitudinalRebar
    {

        ISliceableSection TransformedSection { get; set; }


        private ISliceableSection CalculateTransformedSection()
        {

            var TransformedRebarShapes = GetTransformedRebarShapes(Section.SliceableShape.YMax, Section.SliceableShape.YMin);
            CompoundShape compoundShape = this.Section.SliceableShape as CompoundShape;
            
            if (compoundShape!=null)
            {
               
                List<CompoundShapePart> parts = compoundShape.RectanglesXAxis.Select(cs => cs).ToList();
                var UniqueYPointsForBars = this.LongitudinalBars.Select(b => b.Coordinate.Y).Distinct();
                foreach (var YPoint in UniqueYPointsForBars)
                {
                    var pointsAtThisY = TransformedRebarShapes.Where(b => b.GetElasticCentroidCoordinate().Y == YPoint);
                    if (pointsAtThisY !=null)
                    {
                        double A_total = pointsAtThisY.Sum(p => p.A);
                        double b_total = pointsAtThisY.Sum(p => p.XMax - p.XMin);
                        double h_average = A_total / b_total;
                        CompoundShapePart part = new CompoundShapePart(b_total, h_average, new Point2D(0, YPoint));
                        parts.Add(part);
                    }

                }
                ArbitraryCompoundShape combinedShape = new ArbitraryCompoundShape(parts, null);
                
                return combinedShape;
            }
            else
            {
                return Section.SliceableShape as ISliceableSection;
            }

           
        }

        public List<IMoveableSection> GetTransformedRebarShapes(double YMax, double YMin)
        {

            List<IMoveableSection> barSections = new List<IMoveableSection>();
            double E_c = this.Section.Material.ModulusOfElasticity;
            double E_s = 29000000.0; //Steel modulus of elasticity
            double n = E_s / E_c;

            List<RebarPoint> filteredBars = LongitudinalBars.Where
                (
                b =>

                        b.Coordinate.Y >= YMin && b.Coordinate.Y <= YMax

                ).ToList();
            foreach (RebarPoint rbrPnt in filteredBars)
            {

                double A_bar = rbrPnt.Rebar.Area * n;
                double d_bar = rbrPnt.Rebar.Diameter;
                if (d_bar == 0)
                {
                    d_bar = (YMax - YMin) / 100.0;
                }
                Point2D thisCentroid = new Point2D(rbrPnt.Coordinate.X, rbrPnt.Coordinate.Y);

                SectionRectangular rect = new SectionRectangular(null, A_bar / d_bar, d_bar, thisCentroid);
                //rect.Centroid = new Point2D(rbrPnt.Coordinate.X, rbrPnt.Coordinate.Y);
                barSections.Add(rect);
            }

            return barSections;
        }
    }
}

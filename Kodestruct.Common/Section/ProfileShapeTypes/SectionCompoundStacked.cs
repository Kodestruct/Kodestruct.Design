using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Section.SectionTypes
{
    public class SectionCompoundStacked:  CompoundShape
    {
        /// <summary>
        /// Creates custom shape from vertically stacked rectangles
        /// </summary>
        /// <param name="Rectangles"></param>
        public SectionCompoundStacked(List<SectionRectangular>Rectangles)
        {
            rectangles = Rectangles;
        }

        List<SectionRectangular> rectangles;

        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            List<CompoundShapePart> parts = new List<CompoundShapePart>();
            double YCoord = 0;
            double HPrev = 0;

            foreach (var r in rectangles)
            {
                YCoord = HPrev + r.H / 2.0;
                HPrev = HPrev+r.H;
                parts.Add(new CompoundShapePart(r.B, r.H, new Mathematics.Point2D(0, YCoord)));
            }
            return parts;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {

            List<CompoundShapePart>  allParts = new List<CompoundShapePart>();

            var UniqueWidths = rectangles.Select(r => r.B).Distinct().OrderByDescending(n =>n).ToList();

            List<CompoundShapePart> halfOfParts = new List<CompoundShapePart>();
            double YCoord = 0;
            double HPrev = 0;
            double BPrev = 0;

            for (int i = 0; i < UniqueWidths.Count(); i++)
            {

                double thisSegmentHeight=0;

                if (i < UniqueWidths.Count()-1)
                {
                    thisSegmentHeight = (UniqueWidths[i] - UniqueWidths[i+1]) / 2.0;
                }
                else
                {
                    thisSegmentHeight = UniqueWidths[i]/2.0;
                }
                double thisSegmentWidth = BPrev + rectangles.Where(r => r.B == UniqueWidths[i]).Sum(rect => rect.H);
                YCoord = HPrev + thisSegmentHeight / 2.0;




                CompoundShapePart thisPart = new CompoundShapePart(thisSegmentWidth, thisSegmentHeight,new Mathematics.Point2D(0,YCoord));
                halfOfParts.Add(thisPart);
                
                
                //save height and width for previous iterations
                HPrev = HPrev + thisSegmentHeight;
                BPrev = thisSegmentWidth;
            }

            //mirror rectangles
            double LeftFaceOffset = halfOfParts.Select(p => p.Ymax).Max();

            foreach (var part in halfOfParts)
            {

                allParts.Add(new CompoundShapePart(part.b, part.h, new Mathematics.Point2D(0, LeftFaceOffset-part.InsertionPoint.Y )));
                allParts.Add(new CompoundShapePart(part.b, part.h, new Mathematics.Point2D(0, -(LeftFaceOffset - part.InsertionPoint.Y))));
            }
            return allParts;

        }

        protected override void CalculateWarpingConstant()
        {
           
        }
    }
}

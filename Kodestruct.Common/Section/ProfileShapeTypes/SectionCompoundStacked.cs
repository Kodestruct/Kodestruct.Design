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

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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;


namespace Kodestruct.Common.Section.SectionTypes
{
    /// <summary>
    /// Generic rectangle shape with geometric parameters provided in a constructor.
    /// </summary>
    public class SectionRectangular : CompoundShape, ISectionRectangular, ISliceableSection, IFirstMomentOfAreaCalculatable //SectionBaseClass,
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="b">Width</param>
        /// <param name="h">Height</param>
        /// <param name="Centroid">Coordinates of centroid.</param>
        public SectionRectangular(string Name, double b, double h, Point2D Centroid)
        {
            this.b = b;
            this.h = h;
            this.Centroid = Centroid;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="b">Width</param>
        /// <param name="h">Height</param>
        public SectionRectangular(double b, double h):this(null, b, h, new Point2D(0,0))
        {
        }


        private Point2D centroid;

        public Point2D Centroid
        {
            get {
                if (centroid == null)
                {
                    centroid = new Point2D(0.0, 0.0);
                }
                return centroid; }
            set { centroid = value; }
        }
        

        private double h;
        public double H
        {
            get { return h; }
            set { h = value; }
        }
        


        private double b;
        /// <summary>
        /// Width
        /// </summary>
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        

        public ISection GetWeakAxisClone()
        {
            throw new NotImplementedException();
        }

        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            List<CompoundShapePart> rectangles = new List<CompoundShapePart>();
            rectangles.Add(new CompoundShapePart(B, H, centroid));
            return rectangles;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            List<CompoundShapePart> rectangles = new List<CompoundShapePart>();
            rectangles.Add(new CompoundShapePart(B, H, centroid));
            return rectangles;
        }

        protected override void CalculateWarpingConstant()
        {
            _C_w = 0.0;
        }

        protected override void CalculateTorsionalConstant()
        {
            //From Boresi, Schmidt; Advanced Mechanics of Materials
            //Table B.1
            _J = (((b * Math.Pow(h, 3) + h * Math.Pow(b, 3))) / (12));
        }


        public double GetFirstMomentOfAreaX(double TopOffset)
        {
            double Q = 0.0;
            double h = YMax - YMin;
            if (TopOffset>h || TopOffset<0)
            {
                throw new Exception("Top offset cannot be greater than section height or be a negative number");
            }
            double topOffsetNA = YMax - this.GetElasticCentroidCoordinate().Y;
            if (TopOffset<=topOffsetNA) //specified slice is above N.A.
            {
                var topSlice = this.GetTopSliceSection(TopOffset, SlicingPlaneOffsetType.Top);
                double Y = topSlice.GetElasticCentroidCoordinate().Y - this.GetElasticCentroidCoordinate().Y;
                Q = topSlice.A * Y;
            }
            else
            {
                var botSlice = this.GetTopSliceSection(TopOffset, SlicingPlaneOffsetType.Bottom);
                double Y = this.GetElasticCentroidCoordinate().Y - botSlice.GetElasticCentroidCoordinate().Y;
                Q = botSlice.A * Y;
            }

            return Q;
        }

    }
}

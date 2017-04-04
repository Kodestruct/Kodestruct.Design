using Kodestruct.Common.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Section
{
    public class ThinWallSegment
    {
        public double WallThickness { get; set; }
        public Line2D Line { get; set; }

        /// <summary>
        /// Vertical thin wall segment
        /// </summary>
        /// <param name="YMin">Y coordinate for lowest point</param>
        /// <param name="YMax">Y coordinate for highest point</param>
        /// <param name="WallThickness">Wall thickness</param>
        public ThinWallSegment(double YMin, double YMax, double WallThickness)
        {
            Line = new Line2D(new Point2D(0, YMin), new Point2D(0, YMax));
            this.WallThickness = WallThickness;
        }

        public ThinWallSegment(Line2D Line, double WallThickness)
        {
            this.Line = Line;
            this.WallThickness = WallThickness;
        }
        
    }
}

using Kodestruct.Common.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public class PunchingPerimeterData
    {
        public List<PerimeterLineSegment> Segments { get; set; }
        public Point2D ColumnCentroid { get; set; }

        public PunchingPerimeterData(List<PerimeterLineSegment> Segments, Point2D ColumnCentroid)
        {
            this.Segments = Segments;
            this.ColumnCentroid = ColumnCentroid;
        }
    }
}

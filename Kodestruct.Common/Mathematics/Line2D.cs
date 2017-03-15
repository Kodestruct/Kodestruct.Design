using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Mathematics
{
    public class Line2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        private double _YMax;

        public double YMax
        {
            get { return _YMax; }
            set { _YMax = value; }
        }

        private double _YMin;

        public double YMin
        {
            get { return _YMin; }
            set { _YMin = value; }
        }
        
    }
}

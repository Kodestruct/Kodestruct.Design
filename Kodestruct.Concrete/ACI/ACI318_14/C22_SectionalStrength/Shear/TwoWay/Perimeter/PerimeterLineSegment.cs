using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public class PerimeterLineSegment
    {
       public Point2D PointI { get; set; }
       public Point2D PointJ { get; set; }

        public PerimeterLineSegment(Point2D PointI, Point2D PointJ)
        {
            this.PointI = PointI;
            this.PointJ = PointJ;
        }

        private Point2D centroid;

        public Point2D Centroid
        {
            get { 
                
                centroid = new Point2D((PointJ.X - PointI.X)/2.0, (PointJ.Y - PointI.Y)/2.0);
                return centroid; }
        }
        
        private double length;

        public double Length
        {
            get {
                length = Math.Sqrt(Math.Pow(PointJ.X - PointI.X, 2.0) + Math.Pow(PointJ.Y - PointI.Y, 2.0));
                return length; }
            set { length = value; }
        }

        #region Y exteme coodinates
        private double yMax;

        public double YMax
        {
            get { return Math.Max(PointI.Y, PointJ.Y); }

        }

        private double yMin;

        public double YMin
        {
            get { return Math.Min(PointI.Y, PointJ.Y); }
        } 
        #endregion

        #region X exteme coodinates
        private double xMax;

        public double XMax
        {
            get { return Math.Max(PointI.X, PointJ.X); }

        }

        private double xMin;

        public double XMin
        {
            get { return Math.Min(PointI.X, PointJ.X); }
        }
        #endregion
        
                


    }
}

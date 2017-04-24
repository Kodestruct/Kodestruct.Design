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

namespace Kodestruct.Common.Mathematics
{
    public class Line2D
    {
        public Line2D()
        {

        }

        public Line2D(Point2D StartPoint, Point2D EndPoint)
        {
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
        }
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        private double _YMax;

        public double YMax
        {
            get {
                _YMax = Math.Max(StartPoint.Y, EndPoint.Y);
                return _YMax; }

        }

        private double _YMin;

        public double YMin
        {
            get {
                _YMin = Math.Min(StartPoint.Y, EndPoint.Y);
                return _YMin; }

        }

        private double _XMax;

        public double XMax
        {
            get
            {
                _XMax = Math.Max(StartPoint.X, EndPoint.X);
                return _XMax;
            }

        }

        private double _XMin;

        public double XMin
        {
            get
            {
                _XMin = Math.Min(StartPoint.X, EndPoint.X);
                return _XMin;
            }

        }

        private double _angleToHorizontal;

        public double AngleToHorizontal
        {
            get {

                _angleToHorizontal = GetAngleToHorizontal();
                return _angleToHorizontal; }

        }

        private double GetAngleToHorizontal()
        {
            return Math.Atan((this.YMax - this.YMin) / (this.XMax - this.XMin));
        }

        private double _angleToVertical;

        public double AngleToVertical
        {
            get
            {

                _angleToVertical = GetAngleToVertical();
                return _angleToVertical;
            }

        }

        private double GetAngleToVertical()
        {
            return Math.Atan((this.XMax - this.XMin) / (this.YMax - this.YMin));
        }
        

        public Line2D GetSubSegment(double YMax, double YMin)
        {
            if (this.XMax == this.XMin)
            {
                return new Line2D(new Point2D(this.XMax, YMin), new Point2D(this.XMax, YMax));
            }
            else if (this.YMax ==this.YMin)
            {
                return new Line2D(new Point2D(this.XMin, this.YMax), new Point2D(this.XMax, this.YMax));
            }

            else
            {
                double dy1 = YMin - this.YMin;
                double dy2 = YMax - this.YMin;
                double alpha = this.AngleToVertical;

                //double dxMin = YMin - this.YMin;
                double xmin =this.XMin + dy1 * Math.Tan(alpha);
                double xmax =this.XMin + dy2 * Math.Tan(alpha);

                return new Line2D(new Point2D(xmin, YMin), new Point2D(xmax, YMax));
            }


        }

        private double length;

        public double Length
        {
            get {
                length = Math.Sqrt(Math.Pow(XMax - XMin, 2) + Math.Pow(YMax - YMin, 2));
                return length; }

        }
        
    }
}

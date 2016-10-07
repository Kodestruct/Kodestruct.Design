#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section
{
    /// <summary>
    /// Shape comprised of full-width rectangles,
    /// providing default implementation of ISliceableSection
    /// for calculation of elastic properties,
    /// location of PNA (Plastic Neutral Axis)
    /// and Plastic Properties
    /// </summary>
    public abstract partial class CompoundShape : ISliceableSection, ISection //SectionBaseClass, 
    {

        bool momentsOfInertiaCalculated;
        bool elasticPropertiesCalculated;
        bool areaCalculated;
        protected bool torsionConstantCalculated {get; set;}
        bool warpingConstantCalculated;
        bool plasticPropertiesCalculated;

        public string Name { get; set; }

        double _Ix;
        public double I_x
        {
            get 
            {
                if (momentsOfInertiaCalculated == false)
                {
                    CalculateMomentsOfInertia();
                }
                return _Ix;
            }
        }



        double _Iy;
        public double I_y
        {
            get 
            {
                if (momentsOfInertiaCalculated == false)
                {
                    CalculateMomentsOfInertia();
                }
                return _Iy; 
            }
        }

        private void CalculateMomentsOfInertia()
        {
            
            foreach (var r in RectanglesXAxis)
            {
                double thisA = r.GetArea();
                double thisYbar = (r.Centroid.Y - this.Centroid.Y);
                double thisIx =  r.GetMomentOfInertia() + thisA * Math.Pow(thisYbar, 2);
                _Ix = _Ix + thisIx;
            }

            //iternally the RectanglesYAxis collection must provide rotated rectangles
            //therefore even though we calculate Iy,
            //we follow the procedures for calculation of Ix since the provided rectanges are not the same.
            //except for the centroid we need to use X coordinate (again because the shape is internally rotated)

            foreach (var r in RectanglesYAxis)
            {
                double thisA = r.GetArea();
                double thisYbar = (r.Centroid.Y - this.CentroidYAxisRect);
                double thisIy = r.GetMomentOfInertia() + thisA * Math.Pow(thisYbar, 2);
                _Iy = _Iy + thisIy;
            }

            momentsOfInertiaCalculated = true;
        }

        double GetQ_xTop(double y_top)
        {

            if (y_top>this.YMax-Centroid.Y)
	        {
		         throw new Exception("Error. dimension y_top needs to be less or equal to the distance from top of shape to the neutral axis.");
	        }
            double _Q_x=0.0;
            double Y_minLimit = this.YMax - y_top;

            foreach (var r in RectanglesYAxis)
            {
                if (r.Ymin>=Y_minLimit)
                {
                    double A_this = r.GetArea();
                    _Q_x = _Q_x + (r.Centroid.Y - this.Centroid.Y) * A_this;
                }
                else if (r.Ymin<Y_minLimit&& r.Ymax >Y_minLimit)
                {
                    double h1 = r.Ymax - Y_minLimit;
                    double A = h1 * r.b;
                    double y1 =( r.Ymin - Centroid.Y) + h1 / 2.0;
                    _Q_x = _Q_x + A * y1;
                }
            }

            return _Q_x;
        }

        double GetQ_xBottom(double y_bottom)
        {
            if (y_bottom >  Centroid.Y - this.YMin)
            {
                throw new Exception("Error. dimension y_bottom needs to be less or equal to the distance from bottom of shape to the neutral axis.");
            }
            double _Q_x=0.0;
            double Y_maxLimit = this.YMin + y_bottom;

            foreach (var r in RectanglesYAxis)
            {
                if (r.Ymax<= Y_maxLimit)
                {
                    double A_this = r.GetArea();
                    _Q_x = _Q_x + (r.Centroid.Y - this.Centroid.Y) * A_this;
                }
                else if (r.Ymax > Y_maxLimit && r.Ymin < Y_maxLimit)
                {
                    double h1 = Y_maxLimit - r.Ymin;
                    double A = h1 * r.b;
                    double y1 = (Centroid.Y-r.Ymin) + h1 / 2.0;
                    _Q_x = _Q_x + A * y1;
                }
            }

            return _Q_x;

        }

        double _S_xTop;
        public double S_xTop
        {
            get {

                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return _S_xTop;
            }
        }



        double _S_xBot;
        public double S_xBot
        {
            get {

                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return _S_xBot;
            }
        }

        double _S_yLeft;
        public double S_yLeft
        {
            get {
                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return _S_yLeft;
            }
        }

        double _S_yRight;
        public double S_yRight
        {
            get {

                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return _S_yRight;
            }
        }

        double _Z_x;
        public double Z_x
        {
            get 
            {
                if (plasticPropertiesCalculated==false)
                {
                    CalculatePlasticProperties();
                }
                return _Z_x;
            }
        }

        double _Z_y;
        public double Z_y
        {
            get {

                if (plasticPropertiesCalculated == false)
                {
                    CalculatePlasticProperties();
                }
                return _Z_y;
            }
        }

        private class plasticRectangle: CompoundShapePart
        {
            public plasticRectangle(double b, double h, Point2D Centroid):
                base(b,h, Centroid)
            {

            }
            public double An { get; set; }
            public double Y_n_tilda { get; set; }
            public double h_n_tilda { get; set; }
        }
        private void CalculatePlasticProperties()
        {
            //sort rectangles collection to make sure that they go from top to bottom
            var sortedRectanglesX = RectanglesXAxis.OrderByDescending(r => r.InsertionPoint.Y).ToList();
            CalculatePlasticSectionModulus(AnalysisAxis.X, sortedRectanglesX);

            //sort rectangles collection to make sure that they go from left to right
            if (RectanglesYAxis!=null)
            {
                var sortedRectanglesY = RectanglesYAxis.OrderBy(r => r.InsertionPoint.X).ToList();
                CalculatePlasticSectionModulus(AnalysisAxis.Y, sortedRectanglesY); 
            }

        }

        /// <summary>
        /// Calculates plastic neutral axis (PNA) and plastic section modulus
        /// in accordance with procedure in the foloowing paper:
        /// "CALCULATION OF THE PLASTIC SECTION MODULUS USING THE COMPUTER" 
        /// DOMINIQUE BERNARD BAUER 
        /// AISC ENGINEERING JOURNAL / THIRD QUARTER /1997 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="rects"></param>
        private void CalculatePlasticSectionModulus(AnalysisAxis axis, 
            List<CompoundShapePart> rects)
        {
        
            double Z = 0.0;
            double Atar = A / 2;
            double Sum_hi=0;  //summation of height of previous rectangles
            double Sum_Ai =0; //summation of areas of previous rectangles
            double Sum_AiPreviousStep = 0;//summation of areas of previous rectangles in the step before this one

            //find location of PNA
            //and store the information in a list
            List<plasticRectangle> pRects = new List<plasticRectangle>();
            double PNACoordinate= 0;

            #region Find PNA
            foreach (var r in rects)
            {

                double bn = 0;
                double hn = 0;
                double hn_actual = 0; //actual height used for fillet areas
                double yn = 0;
                plasticRectangle pr = null;

                switch (axis)
                {
                    case AnalysisAxis.X:
                        pr = new plasticRectangle(r.b, r.h, r.InsertionPoint);
                        bn = pr.b;
                        hn = pr.h;
                        hn_actual = r.h_a;
                        break;
                    case AnalysisAxis.Y:
                        //pr = new plasticRectangle(r.h,r.b,new Point2D(r.InsertionPoint.Y,r.InsertionPoint.X));
                        pr = new plasticRectangle(r.b, r.h, r.InsertionPoint);
                        bn = pr.b;
                        hn = pr.h;
                        hn_actual = r.h_a;
                        break;
                }


                yn = pr.InsertionPoint.Y; //centroid of this rectangle
                double An = bn * hn;
                pr.An = An;
                //distance from top of the rectangle to the PNA
                //this number is meaningful only for one rectangle
                double h_n_tilda;

                if (An == 0 && Sum_Ai == Atar) // case when the rectangle represents a hole at the center of the section
                {
                    h_n_tilda = hn / 2;
                }
                else //all other cases when rectangles are "stacked" on top of each other
                {
                    h_n_tilda = (Atar - Sum_Ai) / bn;
                }

                double Y_n_tilda = 0;

                //check if this rectangle is the one where
                //PNA is located
                if (h_n_tilda > 0 && h_n_tilda < hn) // remove equal?
                {
                    //this condition is met only for one rectangle
                    Y_n_tilda = Sum_hi + h_n_tilda;
                    if (axis == AnalysisAxis.X)
                    {
                        PNACoordinate = Y_n_tilda - this.YMin; //PNA coordinate is meeasured from top
                    }
                    else
                    {
                        PNACoordinate = Y_n_tilda; //PNA coordinate is meeasured from left
                    }

                    pr.h_n_tilda = h_n_tilda;
                }
                else
                {
                    pr.h_n_tilda = 0;
                }
                pr.Y_n_tilda = Y_n_tilda;

                pRects.Add(pr);
                Sum_AiPreviousStep = Sum_Ai;
                Sum_Ai += An;
                //Sum_hi +=hn;
                Sum_hi += hn_actual;
            } 




                double sectionHeight;
                if (axis == AnalysisAxis.X)
	            {
		                sectionHeight= YMax - YMin;
	            }
                else
	            {
                        sectionHeight= XMax - XMin;
                }

double distanceFromBottomToPNA = sectionHeight - PNACoordinate;

            #endregion
                
            foreach (var pr in pRects)
            {
             double Zn;
             if (pr.Y_n_tilda!=0) //special case when rectangle is cut by PNA
	            {
		            double ZnTop = pr.b*Math.Pow(pr.h_n_tilda,2)/2 ;
                    double ZnBot = pr.b * Math.Pow((pr.h - pr.h_n_tilda), 2) / 2;
                    Zn = ZnTop + ZnBot;
	            }
            else
	            {
                    double dn = pr.InsertionPoint.Y - distanceFromBottomToPNA; //PNACoordinate ;
                    Zn = Math.Abs(dn) * pr.An;

                } 
                Z += Zn;

            }

            switch (axis)
            {
                case AnalysisAxis.X: 
                    this._Z_x = Z;
                    this.ypb = distanceFromBottomToPNA; 
                    break;
                case AnalysisAxis.Y: 
                    this._Z_y = Z;
                    this.xpl = distanceFromBottomToPNA; 
                    break;
            }
        }

        double rx;
        public double r_x
        {
            get {
                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return rx;
            }
        }

        double ry;
        public double r_y
        {
            get {

                if (elasticPropertiesCalculated == false)
                {
                    CalculateElasticProperies();
                }
                return ry;
            }
        }
        /// <summary>
        /// Calculates section moduli and radii of gyration
        /// </summary>
        private void CalculateElasticProperies()
        {
            double yt = YMax - Centroid.Y;
            //double Ix = I_x;
            _S_xTop = I_x / yt;
            double ybot = y_Bar;
            _S_xBot = I_x / ybot;
            double xl = x_Bar;
            _S_yLeft = I_y / xl;
            double xr = XMax - Centroid.X;
            _S_yRight = I_y / xr;
            //double A = this._A;
            rx = Math.Sqrt(I_x / A);
            ry = Math.Sqrt(I_y / A);
           
        }

        double xleft;
        public double x_Bar
        {
            get 
            {
                xleft = Centroid.X - XMin;
                return xleft;
            }
        }
        double yb;
        public double y_Bar
        {
            get 
            {
                yb = Centroid.Y - YMin;
                return yb;
            }
        }

        double xpl;
        public double x_pBar
        {
            get { 
                
                return xpl;
            }
        }

        double ypb;
        /// <summary>
        /// Plastic neutral axis location 
        /// THE REPORTED PLASTIC NEUTRAL LOCATION IS GIVEN AS THE DISTANCE FROM BOTTOM FIBER
        /// </summary>
        public double y_pBar
        {
            get {
                if (plasticPropertiesCalculated == false)
                {
                    CalculatePlasticProperties();
                }
                return ypb;
            }
        }

        protected double _C_w;
        public double C_w
        {
            get {

                if (warpingConstantCalculated == false)
                {
                    CalculateWarpingConstant();
                }
                return _C_w; 
            }
        }

        protected abstract void CalculateWarpingConstant();


        protected double _J;
        public double J
        {
            get {

                if (torsionConstantCalculated == false)
                {
                    CalculateTorsionalConstant();
                }
                return _J; 
            }
        }

        protected virtual void CalculateTorsionalConstant()
        {
            foreach (var r in RectanglesXAxis)
            {
                double t = Math.Min(r.b, r.h);
                double b= Math.Max(r.b, r.h);

                _J = _J + b * Math.Pow(t, 3.0) / 3.0;
            }


            torsionConstantCalculated = true;
        }

        public ISection Clone()
        {
            throw new NotImplementedException();
        }

        double _A;
        public double A
        {
            get {
                if (areaCalculated == false)
                {
                    CalculateArea();
                }
                return _A; 
            }
        }

        private void CalculateArea()
        {
          
            foreach (var r in RectanglesXAxis)
            {
                _A = _A + r.GetArea();
            }
            areaCalculated = true;
        }

        enum AnalysisAxis
        {
            X,
            Y
        }
    }
}

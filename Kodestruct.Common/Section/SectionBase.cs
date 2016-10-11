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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section
{
    public abstract class SectionBase: AnalyticalElement, ISection
    {

        public SectionBase():this(null)
        {

        }

        public SectionBase(string Name): this(Name,null)
        {
        }
        public SectionBase(string Name, ICalcLog Log)
        {
            this.name = Name;
            plasticCentroidCoordinate = new Point2D(0,0);
            elasticCentroidCoordinate = new Point2D(0,0);
        }

        private string  name;

        public virtual string  Name
        {
            get { return name; }
        }

        protected double _B;

        public double B
        {
            get { return _B; }
            set { _B = value; }
        }

        protected double _H;

        public double H
        {
            get { return _H; }
            set { _H = value; }
        }
        
        

        protected double _A;

        public virtual double A
        {
            get { return _A; }
            set { _A = value; }
        }

        protected double _I_x;

        public virtual double I_x
        {
            get { return _I_x; }
        }

        protected double _I_y;

        public virtual double I_y
        {
            get { return _I_y; }

        }

        protected double _S_x_Top;

        public virtual double S_xTop
        {
            get 
            {
                if (_S_x_Top==0)
                {
                    _S_x_Top = _I_x / (_H - _y_Bar);   
                }

                return _S_x_Top; 
            }

        }

        protected double _S_xBot;

        public virtual double S_xBot
        {
            get 
            {
                if (_S_xBot == 0)
                {
                    _S_xBot = _I_x / _y_Bar;
                }
                return _S_xBot; 
            }
        }

        protected double _S_yLeft;

        public virtual double S_yLeft
        {
            get 
            {
                if (_S_yLeft == 0)
                {
                    _S_yLeft = _I_y / _x_Bar;
                }
                return _S_yLeft; 
            }

        }

        protected double _S_yRight;

        public virtual double S_yRight
        {
            get
            {
                if (_S_yRight == 0)
                {
                    _S_yRight = _I_y / (_B - _x_Bar);
                }
                return _S_yRight;
            }
        }

        protected double _Z_x;

        public virtual double Z_x
        {
            get { return _Z_x; }
        }

        protected double _Z_y;

        public virtual double Z_y
        {
            get { return _Z_y; }
        }

        protected double _r_x;

        public virtual double r_x
        {
            get {
                _r_x = Math.Sqrt(_I_x / _A);
                return _r_x; }
        }

        protected double _r_y;

        public virtual double r_y
        {
            get {
                _r_y = Math.Sqrt(_I_y / _A);
                return _r_y; }
        }

        protected double _x_Bar;

        public virtual double x_Bar
        {
            get { return _x_Bar; }
        }

        protected double _y_Bar;

        public virtual double y_Bar
        {
            get { 
                return _y_Bar; }
        }

        protected double _x_pBar;

        public virtual double x_pBar
        {
            get { return _x_pBar; }
        }

        protected double _y_pBar;

        public virtual double y_pBar
        {
            get { return _y_pBar; }

        }

        protected double _C_w;

        public virtual double C_w
        {
            get { return _C_w; }

        }

        protected double _J;

        public virtual double J
        {
            get { return _J; }

        }


        protected Point2D elasticCentroidCoordinate;


        public Point2D ElasticCentroidCoordinate
        {
            get 
            { 
                return elasticCentroidCoordinate; 
            }
            set 
            { 
                elasticCentroidCoordinate = value; 
            }
        }

        protected Point2D plasticCentroidCoordinate;

        public Point2D PlasticCentroidCoordinate
        {
            get
            {
                return plasticCentroidCoordinate; 
            }
            set
            {
                plasticCentroidCoordinate = value; 
            }
        }

        //public abstract ISection Clone();



    }
}

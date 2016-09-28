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
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section.General
{
    public class SectionGeneralProperties: ISection
    {

        double _A;
        double _I_x;
        double _I_y;
        double _S_xTop;
        double _S_xbot;
        double _S_yLeft;
        double _S_yRight;
        double _Z_x;
        double _Z_y;
        double _r_x;
        double _r_y;
        double _x_Left;
        double _y_Bot;
        double _X_pLeft;
        double _y_pBot;
        double _C_w;
        double _J;

        public SectionGeneralProperties(string Name, double Area, double MomentOfInertiaX, double MomentOfInertiaY,  double SectionModulusXTop, double SectionModulusXBot, double SectionModulusYLeft, double SectionModulusYRight, double PlasticSectionModulusX, double PlasticSectionModulusY, double RadiusOfGyrationX, double RadiusOfGyrationY, double CentroidXtoLeftEdge, double CentroidYtoBottomEdge, double PlasticCentroidXtoLeftEdge, double PlasticCentroidYtoBottomEdge, double WarpingConstant, double TorsionalConstant)
        {
            //this.Name = Name;
            this._A = Area;
            this._I_x = MomentOfInertiaX;
            this._I_y = MomentOfInertiaY;
            this._S_xTop = SectionModulusXTop;
            this._S_xbot = SectionModulusXBot;
            this._S_yLeft = SectionModulusYLeft;
            this._S_yRight = SectionModulusYRight;
            this._Z_x = PlasticSectionModulusX;
            this._Z_y = PlasticSectionModulusY;
            this._r_x = RadiusOfGyrationX;
            this._r_y = RadiusOfGyrationY;
            this._x_Left = CentroidXtoLeftEdge;
            this._y_Bot = CentroidYtoBottomEdge;
            this._X_pLeft = PlasticCentroidXtoLeftEdge;
            this._y_pBot = PlasticCentroidYtoBottomEdge;
            this._C_w = WarpingConstant;
            this._J = TorsionalConstant;
        }
        string name;
        public string Name
        {
            get { return name; }
        }

        public double A
        {
            get { return _A; }
        }

        public double I_x
        {
            get { return _I_x; }
        }

        public double I_y
        {
            get { return _I_y; }
        }


        public double S_xTop
        {
            get { return _S_xTop; }
        }

        public double S_xBot
        {
            get { return _S_xbot; }
        }

        public double S_yLeft
        {
            get {return _S_yLeft;  }
        }

        public double S_yRight
        {
            get { return _S_yRight; }
        }

        public double Z_x
        {
            get { return _Z_x; }
        }

        public double Z_y
        {
            get { return _Z_y; }
        }

        public double r_x
        {
            get { return _r_x; }
        }

        public double r_y
        {
            get { return _r_y; }
        }

        public double x_Bar
        {
            get { return _x_Left; }
        }

        public double y_Bar
        {
            get { return _y_Bot; }
        }

        public double x_pBar
        {
            get { return _X_pLeft; }
        }

        public double y_pBar
        {
            get {return _y_pBot;  }
        }

        public double C_w
        {
            get { return _C_w; }
        }

        public double J
        {
            get { return _J; }
        }

        public ISection Clone()
        {
            throw new NotImplementedException();
        }


        public double Angle_alpha
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double I_z
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double r_z
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double beta_w
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

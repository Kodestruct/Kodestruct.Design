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
using Kodestruct.Common.Section.Interfaces;


namespace Kodestruct.Common.Section.SectionTypes
{
    /// <summary>
    /// Generic rectangular tube shape with geometric parameters provided in a constructor.
    /// </summary>
    public class SectionTube : SectionBox , ISectionTube
    {


        public SectionTube(string Name, double H, double B, double t, double t_des,
            double CornerRadiusOutside = -1)
            : base(Name, H, B, t_des, t_des)
        {
            this.H = H;
            this.B = B;
            this.t_nom = t;
            this._t_des = t_des;
            this._t_f = t_des;
            this._t_w = t_des;

            if (CornerRadiusOutside == -1)
            {
                this.r_c = 1.5 * t;
            }
            else
            {
                this.r_c = CornerRadiusOutside;
            }

        }
        #region Properties specific to HSS tube


        private double r_c;

        public double CornerRadiusOutside
        {
            get { return r_c; }
            set { r_c = value; }
        }

        //private const double CornerRadiusOutside = 1.5;

        private double h;

        public double H
        {
            get { return h; }
            set { h = value; }
        }

        private double b;

        public double B
        {
            get { return b; }
            set { b = value; }
        }

        private double _t_nom;

        public double t_nom
        {
            get { return _t_nom; }
            set { _t_nom = value; }
        }


        private double _t_des;

        public double t_des
        {
            get {
                if (_t_des ==-1)
                {
                    _t_des = 0.93 * t_nom;
                }

                return _t_des; }
            set { _t_des = value; }
        }
        
        #endregion

    }
}

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
    /// Generic pipe shape with geometric parameters provided in a constructor.
    /// </summary>
    public class SectionPipe: SectionBase,  ISectionHollow, ISectionPipe
    {
        public double D { get; set; }
        public double t { get; set; }

        private double _t_des;

        public double t_des
        {
            get { return _t_des; }
            set { _t_des = value; }
        }
        


        public SectionPipe(string Name, double D, double t_w): this(Name, D,t_w,t_w)
        {
        }

        public SectionPipe(string Name, double D, double t_nom, double t_des)
            : base(Name)
        {
            this.D = D;
            this.t = t_nom;
            this._t_des = t_des;
            CalculateProperties();

        }

        /// <summary>
        /// Overrides default fields for properties
        /// </summary>
        private void CalculateProperties()
        {
 
            double R = D/2.0;
            double d = D;
            double t = t_des;

            double R_i = (D-2*t_des)/2;
            _H = D;
            B = D;
            _A =Math.PI * (R * R - R_i*R_i);
            _I_x = ((Math.PI) / (4)) * (Math.Pow(R, 4) - Math.Pow(R_i, 4));
            _I_y = _I_x;
            _Z_x = ((4) / (3))*(((Math.Pow(R, 4)-Math.Pow(R_i, 3)*R) / (Math.Pow(R, 4)-Math.Pow(R_i, 4)))) ;
            _Z_y = _Z_x;
            _C_w = 0.0;
            _J = ((Math.PI) / (32)) * (Math.Pow(d, 4) - Math.Pow((d - 2 * t), 4));
            _x_Bar = R;
            _x_pBar = R;
            _y_Bar = R;
            _y_pBar = R;
        }

    


        public ISection GetWeakAxisClone()
        {
            string cloneName = this.Name + "_clone";
            return new SectionPipe (cloneName, D,t);
        }

        //public override ISection Clone()
        //{
        //    return new SectionPipe(Name, D, t);
        //}
    }
}

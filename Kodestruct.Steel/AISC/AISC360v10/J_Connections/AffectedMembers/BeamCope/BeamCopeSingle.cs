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
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public class BeamCopeSingle: BeamCopeBase
    {
        public BeamCopeSingle(double c, double d_c, ISectionI Section, ISteelMaterial Material):
            base(c,d_c,Section,Material)
        {

        }

        private SectionTee _tee;

        public SectionTee tee
        {
            get {
                if (_tee == null)
                {
                    this.GetCopeSection();
                 }
                return _tee; }
 
        }
        

        private void GetCopeSection()
        {
             _tee = new SectionTee(null, d - d_c, Section.b_fTop, Section.t_fBot, Section.t_w);
        }

        public double GetPlateBucklingModelAdjustmentFactor()
        {
            double f;
            if (this.c/d <=1)
            {
                f = (2 * c) / d;
            }
            else
            {
                f = 1.0 + c / d;
            }
            return f;
        }


        public double GetPlateBucklingCoefficient()
        {
            double k;
            if (c/h_o<=1)
            {
                k = 2.2 * Math.Pow((((h_o) / (c))), 1.65);
            }
            else
            {
                k = ((2.2 * h_o) / (c));
            }
            return k;
        }
        protected override double GetS_net()
        {
            double S_top = tee.S_xTop;
            double S_bot = tee.S_xBot;
            return Math.Min(S_top, S_bot);
        }

        public override double GetF_cr()
        {
            double f = GetPlateBucklingModelAdjustmentFactor();
            double k = GetPlateBucklingCoefficient();

            double F_cr ;
            bool PermissibleCopeGeometry = CheckCopeGeometry();
            if (PermissibleCopeGeometry == true)
            {
                F_cr = 26210 * Math.Pow((((t_w) / (h_o))), 2) * f * k; //Manual Eq. 9-7
            }
            else
            {
                F_cr = GetFcrGeneral();
            }
            double F;
            double F_y = Material.YieldStress;
            F = Math.Abs(F_cr) > F_y ? F_y : Math.Abs(F_cr);
            return F;
        }

        protected override double GetZ_net()
        {
            return tee.Z_x;
        }

        protected override double Get_h_o()
        {
            return tee.d - d_c;
        }

        public override double Get_t_w()
        {
            return tee.t_w;
        }
    }
}

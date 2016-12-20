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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Composite;
using Kodestruct.Steel.AISC.AISC360v10.Shear;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.WebOpenings
{
    public class SteelIBeamWebOpening : WebOpeningBase
    {

        public SteelIBeamWebOpening(ISectionI Section, double F_y,
            double a_o, double h_o, double e, double t_r, double b_r, bool IsSingleSideReinforcement = false, double PlateOffset = 0) 
            :base(Section,a_o,h_o,e,F_y,t_r,b_r,IsSingleSideReinforcement, PlateOffset)
        {

        }

        public override double Get_phi()
        {
            return 0.9;
        }

        public override double Get_nu_Top()
        {
            return a_o / s_t;
        }

        public override double Get_mu_Top()
        {
            if (A_r == 0)
            {
                return 0;
            }
            else
            {
                double V_pt = GetV_pt();
                double d_rT = s_t - PlateOffset - t_r / 2.0;
                double mu_tp = P_r * d_rT / (V_pt * s_t);
                return mu_tp;
            }
        }

        public override double Get_nu_Bottom()
        {
            return a_o / s_b;
        }

        public override double Get_mu_Bottom()
        {
            if (A_r == 0)
            {
                return 0;
            }
            else
            {
                double V_pb = GetV_pb();
                double d_rB = s_b - PlateOffset - t_r / 2.0;
                double mu_tp = P_r * d_rB / (V_pb * s_t);
                return mu_tp;
            }
        }

        protected override double Get_alphaTop()
        {
            double mu = Get_mu_Top();
            double nu = Get_nu_Top();
           
                double alphaTop = Get_alpha(mu, nu);
                return alphaTop;
            
        }
    }
}

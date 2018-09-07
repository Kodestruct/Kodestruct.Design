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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v16.Connections
{
    public class BeamCopeSingle: BeamCopeBase
    {
        public BeamCopeSingle(double c, double d_c, ISectionI Section, ISteelMaterial Material):
            base(c,d_c,Section,Material)
        {

        }

        public override double GetF_cr()
        {
            return GetElasticF_cr();
        }

        double GetElasticF_cr()
        {
            double lambda = h_o / t_w; //Eq. 9-1
            double E = Material.ModulusOfElasticity;
            double k1 = Get_k1();
            double F_cr = (0.903 * E * k1) / Math.Pow(lambda, 2); //Eq. 9-9
            return F_cr;
        }


        protected override double Get_phiM_n()
        {
            double lambda = GetLambda_p();
            double lambda_p = GetLambda_p();
            double F_y = Material.YieldStress;
            double S_net = GetS_net();

            if (lambda<= lambda_p)
            {
                double Z_net = GetZ_net();
                return 0.9 * F_y * Z_net; //Eq. 9-8
            }
            else if (lambda>2.0*lambda_p)
            {
                double F_cr = this.GetElasticF_cr();
                return 0.9 * F_cr * S_net; //Eq. 9-8
            }
            else
            {
                double M_y = F_y * S_net;
                double M_p = Get_M_p();
                double M_n = M_p-(M_p-M_y)*(lambda/lambda_p-1.0); //Eq. 9-7
                return 0.9 * M_n;
            }
        }

        double Get_M_p()
        {
            double Z_net = GetZ_net();
            double F_y = Material.YieldStress;
            return 0.9 * F_y * Z_net; //Eq. 9-8
        }

        private double GetLambda_p()
        {
            double F_y = Material.YieldStress;
            double E = Material.ModulusOfElasticity;
            double k1 = Get_k1();
            double lambda_p = 0.475 * Math.Sqrt((k1 * E) / F_y);
            return lambda_p;

        }

        private double Get_k1()
        {
            double f = GetPlateBucklingModelAdjustmentFactor();
            double k = GetPlateBucklingCoefficient();
            double k1 = Math.Max(k * f, 1.61); //Eq 9-1
            return k1;
        }
        private SectionTee _tee;

        public SectionTee tee
        {
            get
            {
                if (_tee == null)
                {
                    this.GetCopeSection();
                }
                return _tee;
            }

        }


        private void GetCopeSection()
        {
            _tee = new SectionTee(null, d - d_c, Section.b_fTop, Section.t_fBot, Section.t_w);
        }

        public double GetPlateBucklingModelAdjustmentFactor()
        {
            double f;
            if (this.c / d <= 1)
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
            if (c / h_o <= 1)
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

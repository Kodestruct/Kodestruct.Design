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
using Kodestruct.Steel.AISC.AISC360v10.Composite;
using Kodestruct.Steel.AISC.AISC360v10.Shear;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.WebOpenings
{
    public partial class CompositeIBeamWebOpening : WebOpeningBase
    {

        bool ValuesNeedRecalculation;
        bool ValuesCalculated;


        private void CalculateAllIterationValues()
        {
            double nu = Get_nu_Top();
            double P_r = GetP_r();
            double V_pt = GetV_pt();
            double d_r = Get_d_rTop(PlateOffset);
            double mu_TopItr =0;

            while (ValuesNeedRecalculation==true)
            {
                double P_ch;
                double muPrevious = mu_TopItr;
                if (mu_TopItr> nu)
                {
                     P_ch = GetP_ch(true);
                }
                else
	            {
                    P_ch = GetP_ch(false);
	            }

                double d_h = Get_d_h(P_ch);
                double P_cl = GetP_cl(P_ch);
                double d_l = Get_d_l(P_cl, this.DeckAtBeamCondition, this.w_rMin, this.s_r);

                mu_TopItr = Calculate_mu_Top(P_r, P_ch, P_cl, d_r, d_h, d_l, V_pt);

                if (muPrevious != 0)
                {
                    if (mu_TopItr>nu && muPrevious>nu)
                    {
                        ValuesNeedRecalculation = false;
                    }
                    else if (mu_TopItr <= nu && muPrevious <= nu)
                    {
                        ValuesNeedRecalculation = false;
                    }
                    else
                    {
                        ValuesNeedRecalculation = true;
                    }

                }
                else
                {
                    ValuesNeedRecalculation = true;
                }

                mu_Top = mu_TopItr;
            }

            ValuesCalculated = true;
        }


        #region Variables

        double alpha_top;

        //private double Calculate_alpha_top()
        //{

        //}

        #region mu_top

        double mu_Top;
        public override double Get_mu_Top()
        {
            if (ValuesCalculated == false)
            {
                CalculateAllIterationValues();
            }
            return mu_Top;
        }

        private double Calculate_mu_Top(double P_r,double P_ch,double P_cl, double d_r, double d_h,double d_l,double V_pt)
        {

            double mu = (2.0 * P_r * d_r + P_ch * d_h - P_cl * d_l) / (V_pt * s_t);
            return mu;

        } 
        #endregion

        /// <summary>
        /// Concrete axial force at low moment end
        /// </summary>
        /// <returns></returns>
        double GetP_ch(bool overrideP_ch)
        {
            double t_e = Get_t_e();
            double P_ch1 = 0.85 * f_cPrime * SlabEffectiveWidth * t_e; //(3-15a)
            double P_ch2 = SumQ_n; //(3-15b)
            double d = Section.d;
            double P_ch3 = double.PositiveInfinity;

            double A_st = Section.GetTopSliceSection(d / 2 - e - h_o / 2.0, SlicingPlaneOffsetType.Top).A; //slice of steel section from opening centerline up
            if (overrideP_ch == false)
            {
                P_ch3 = F_y * A_st; //(3-15c)
            }
            else
            {
                double b_f = Section.b_fTop;
                double t_f = Section.t_fTop;
                double t_w = Section.t_w;

                P_ch3 = F_y * (t_f * (b_f - t_w) + A_r); //(3-20)
            }


            List<double> P_chList = new List<double>()
            {
                P_ch1, P_ch2, P_ch3
            };
            double P_ch = P_chList.Min();
            return P_ch;

        }

        /// <summary>
        /// Concrete force at low moment end
        /// </summary>
        /// <param name="P_ch"></param>
        /// <returns></returns>
        double GetP_cl(double P_ch)
        {
            double P_cl = P_ch - N_o * Q_n;
            P_cl = P_cl < 0 ? 0 : P_cl;  //(3-16)
            return P_cl;
        }

        double Get_d_h(double P_ch)
        {
            double b_e = SlabEffectiveWidth;
            double t_s = SlabDeckThickness + SlabSolidThickness;
            double d_h = t_s - P_ch / (1.7 * f_cPrime * b_e);  //(3-17)
            return d_h;
        }
        double Get_d_l(double P_cl, DeckAtBeamCondition DeckAtBeamCondition, double w_rMin, double s_r)
        {
            double d_l;
            double t_s;
            double b_e = SlabEffectiveWidth;

            switch (DeckAtBeamCondition)
            {
                case DeckAtBeamCondition.NoDeck:
                    d_l = P_cl / (1.7 * f_cPrime * b_e); //(3-18a)
                    break;
                case DeckAtBeamCondition.Parallel:
                    double b_em = b_e / s_r * w_rMin;
                    d_l = P_cl / (1.7 * f_cPrime * b_em); //(3-18a)
                    break;
                case DeckAtBeamCondition.Perpendicular:

                    t_s = SlabDeckThickness + SlabSolidThickness;
                    d_l = t_s - SlabSolidThickness + P_cl / (1.7 * f_cPrime * b_e); //(3-18b)
                    break;
                default:
                    t_s = SlabDeckThickness + SlabSolidThickness;
                    d_l = t_s - SlabSolidThickness + P_cl / (1.7 * f_cPrime * b_e); //(3-18b)
                    break;
            }


            return d_l;
        }


        protected override double Get_alphaTop()
        {
            //if (mu > nu)
            //{
            //    double alpha_v_t = mu / nu;
            //    overrideP_ch = true;
            //    return alpha_v_t;
            //}
            //else
            //{
            double mu = Get_mu_Top();
            double nu = Get_nu_Top();
            return Get_alpha(mu, nu);
            //}
        }

        #endregion
    }
}

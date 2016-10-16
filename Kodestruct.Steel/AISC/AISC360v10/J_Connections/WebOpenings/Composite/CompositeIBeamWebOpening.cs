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
    public partial class CompositeIBeamWebOpening : WebOpeningBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Section">I-section (as shape)</param>
        /// <param name="SlabEffectiveWidth"></param>
        /// <param name="SlabSolidThickness">Concrete fill</param>
        /// <param name="SlabDeckThickness">Metal deck thickness</param>
        /// <param name="F_y">Yield stress of shape</param>
        /// <param name="f_cPrime">Concrete strength</param>
        /// <param name="N_studs">Total number of studs (between point of zero and maximum moment)</param>
        /// <param name="Q_n">Stud strength (in shear)</param>
        /// <param name="N_o">Number of connectors over opening</param>
        /// <param name="a_o">Length of opening</param>
        /// <param name="h_o">Height of opening</param>
        /// <param name="e">Eccentriciy of opening with repect to neutral axis</param>
        /// <param name="t_r">Thickness of reinforcing plate</param>
        /// <param name="b_r">Width of reinforcing plate</param>
        /// <param name="DeckAtBeamCondition">Orientation of deck over beam </param>
        /// <param name="w_rMin">Minimum deck rib width (used in case deck is parallel to beam)</param>
        /// <param name="s_r">Deck rib spacing (used in case deck is parallel to beam)</param>
        /// <param name="IsSingleSideReinforcement"> Indicates if reinforcement is placed on one side of the web</param>
        /// <param name="PlateOffset">Reinforcement plate offset with respect to edge of web opening</param>
        

        public CompositeIBeamWebOpening(ISectionI Section, double SlabEffectiveWidth,
            double SlabSolidThickness, double SlabDeckThickness, double F_y, double f_cPrime, double N_studs, double Q_n, double N_o,
            double a_o, double h_o, double e, double t_r, double b_r, DeckAtBeamCondition DeckAtBeamCondition, double w_rMin, double s_r,
            bool IsSingleSideReinforcement = false, double PlateOffset=0)
            : base(Section, a_o, h_o, e, F_y, t_r, b_r,  IsSingleSideReinforcement, PlateOffset)
        {



                    if (IsSingleSideReinforcement==false)
                    {
                        A_r = t_r * b_r;
                    }
                    else
                    {
                        A_r = 2.0* t_r * b_r;
                    }
                
                this.N_o = N_o;
                this.N_studs = N_studs;
                this.Q_n = Q_n;
                SumQ_n = N_studs * Q_n;
                this.f_cPrime = f_cPrime;
                this.SlabEffectiveWidth=SlabEffectiveWidth;
                this.SlabSolidThickness=SlabSolidThickness;
                this.SlabDeckThickness = SlabDeckThickness;
                this.DeckAtBeamCondition = DeckAtBeamCondition;
                this.w_rMin = w_rMin;
                this.s_r = s_r;

                this.ValuesNeedRecalculation = true;
                this.ValuesCalculated = false;
        
        }

        //SectionI ISection { get; set; }
        public double  Q_n { get; set; }

        double SumQ_n;

        public double N_studs { get; set; }
        public double N_o { get; set; }
        public double A_r { get; set; }

        public double f_cPrime { get; set; }
        double SlabEffectiveWidth {get; set;}
        double SlabSolidThickness {get; set;}
        double SlabDeckThickness { get; set; }

        DeckAtBeamCondition DeckAtBeamCondition;
        double w_rMin; //Minimum deck rib width (used in case deck is parallel to beam)
        double s_r; //Deck rib spacing (used in case deck is parallel to beam)

        CompositeBeamSection compositeSection { get; set; }

        public override double Get_phi()
        {
            return 0.85;
        }

        public override double Get_nu_Top()
        {

             double nu = 0.0;
            if (A_r ==0) //unreinforced
            {
                nu = a_o / s_t;
            }
            else
            {
                double s_t_bar = Get_s_bar(s_t,Section.b_fTop);
                nu = a_o/s_t_bar;
            }
            return nu;

        }



        public override double Get_nu_Bottom()
        {
            double nu = 0.0;
            if (A_r ==0) //unreinforced
            {
                nu = a_o / s_b;
            }
            else
            {
                double s_b_bar = Get_s_bar(s_b,Section.b_fBot);
                nu = a_o/s_b_bar;
            }
            return nu;
        }



        public override double Get_mu_Bottom()
        {
 
            double P_r = GetP_r();
            double d_r = Get_d_rBottom(PlateOffset);
            double V_pb = GetV_pb();
            double mu = 2.0* P_r * d_r / (V_pb * s_b);

            return mu;
        }

        protected override double GetV_m(double V_mt, double V_mb, double mu, double nu)
        {
            double V_m;
            double V_m1 = V_mt + V_mb;
            double V_pt = GetV_pt();
            double V_mtSH = GetV_mtSH(V_pt);

            double V_cBar1=V_pt*(mu/nu-1.0);
            V_cBar1 = V_cBar1 <0 ? 0 :V_cBar1;
            double V_cBar2 = V_mtSH - V_pt;
            V_cBar2 = V_cBar2 < 0 ? 0 : V_cBar2;

            double V_cBar = Math.Min(V_cBar1, V_cBar2);

            ShearMemberFactory f = new ShearMemberFactory();

            //Refine this in future to reflect more possible cases
            double h = Section.d;
            double t_w = Section.t_w;
            double E = 29000;
            double a = 3*h; // no stiffeners are accounted here, arbitrarily set the stiffener spacing


            IShearMember UnperforatedShape = f.GetShearMemberNonCircular(ShearNonCircularCase.MemberWithoutStiffeners,
                 h, t_w, a, F_y, E);
            double V_pBar = UnperforatedShape.GetShearStrength();
            double V_m2 = 2.0 / 3.0 * V_pBar + V_cBar;
            V_m = Math.Min(V_m1, V_m2);

            //double V_cBar = V_pt
            return V_m;
        }
        protected override double GetV_mt(double V_pt,double alphaTop)
        {
            double V_mt1 = V_pt * alphaTop;
            double V_mt2 = GetV_mtSH(V_pt);
 

            double V_mt = Math.Min(V_mt1, V_mt2);
            return V_mt;

        }

        protected double GetV_mtSH(double V_pt)
        {
            double t_s = SlabSolidThickness + SlabDeckThickness;
            double t_e = Get_t_e();
            double A_wc = 3.0 * t_s * t_e;

            double V_mtSH = V_pt + 0.11 * Math.Sqrt(f_cPrime) * A_wc; //(3-21)
            return V_mtSH;
        }
        //public override double GetFlexuralStrength()
        //{
        //    SectionIWithReinfWebOpening SectionReinf = new SectionIWithReinfWebOpening(null, Section.d, Section.b_f, Section.tf, Section.t_w, h_o, e, b_r, t_r, IsSingleSideReinforcement);
        //    throw new NotImplementedException();
        //    //compositeSection = new CompositeBeamSection(Section, SlabEffectiveWidth, SlabSolidThickness, SlabDeckThickness,
        //    //    F_y, f_cPrime);
        //}

        private double Get_s_bar(double s, double b_f)
        {
            double s_bar = s-A_r/(2.0*b_f);
            return s_bar;
        }


        double GetP_r()
        {
            double P_r;
            double P_r1 = F_y * A_r;
            double t_w = Section.t_w;
            double P_r2 = F_y * t_w * a_o / (2.0 * Math.Sqrt(3.0));
            return Math.Min(P_r1, P_r2);

        }

        //Distance from outside edge of flange to centroid 
        //of opening reinforcement
        double Get_d_rTop(double PlateOffset)
        {
             double d = Section.d;
             double d_r = d/2.0 - (e + h_o / 2.0 + PlateOffset + t_r / 2.0);
             return d_r;
        }
        double Get_d_rBottom(double PlateOffset)
        {
            double d = Section.d;
            double d_r = d /2.0- (-e + h_o / 2.0 + PlateOffset + t_r / 2.0);
            return d_r;
        }

        double Get_t_e()
        {
            //Effective slab thickness
            //Concrete inside flutes is ignored here
            //DG-02 uses average if flutes are parallel to beam
            return SlabSolidThickness;
        }



    }
}

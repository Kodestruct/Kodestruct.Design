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
 
using Kodestruct.Concrete.ACI318_14.Anchorage.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI318_14.Anchorage.LimitStates
{
    public class ConcreteBreakoutShear : AnchorageConcreteLimitState
    {
        /// <summary>
        /// Calculation of concrete breakout strength of anchor in shear per 17.5.2 of ACI318-14
        /// </summary>
        /// <param name="n">Number of anchors</param>
        /// <param name="nFirstRow">Number of anchors in the direction in the first row parallel to edge</param>
        /// <param name="h_eff">Effective embedment depth of anchor</param>
        /// <param name="SteelFailureType">Ductile versus brittle failure type for steel anchor</param>
        /// <param name="NumberOfEdges">Number of edges locater less than 1.5*h_ef from the anchor</param>
        /// <param name="ca_1">Distance from the center of an anchor shaft to the edge of concrete in one direction, in. If shear is applied to anchor, ca1 is taken in the direction of the applied shear. If tension is applied to the anchor, ca1 is the minimum edge distance.</param>
        /// <param name="ca_2">Distance from center of an anchor shaft to the edge of concrete in the direction perpendicular to ca_1</param>
        /// <param name="s_Max">Maximum spacing between anchors within the group</param>
        /// <param name="e_prime_v">Eccentricity</param>
        /// <param name="ConcreteCondition">Cracked vs. uncracked concrete</param>
        /// <param name="SupplementalReinforcement">Presence of supplemental reinforcement</param>
        /// <param name="A_vc">Projected area of the failure surface on the side of the concrete member at its edge for a group  of anchors (or single anchor if there's no group effect)</param>
        /// <param name="h_a">Thickness of member in which an anchor is located, measured parallel to anchor axis.</param>
        /// <param name="IsCastContinuouslyWelded"></param>
        /// <param name="lambda"></param>
        /// <param name="TypeOfAnchorSleeve">Constant stiffness or separated sleeve (used in calculating l_e per 17.5.2.2) </param>
        /// <param name="d_a">Outside diameter of anchor</param>
        /// <param name="AnchorType">PostInstalled versus cast in place anchor</param>
        public ConcreteBreakoutShear
              ( IConcreteMaterial Material, int n, int nFirstRow, double h_eff, AnchorSteelElementFailureType SteelFailureType,
            int NumberOfEdges, double ca_1, double ca_2, double s_Max, double e_prime_v,
            ConcreteCrackingCondition ConcreteCondition, SupplementalReinforcementAtAnchor SupplementalReinforcement, double A_vc,
            double h_a, bool IsCastContinuouslyWelded, TypeOfAnchorSleeve TypeOfAnchorSleeve, double d_a,
            AnchorInstallationType AnchorType
            )
            : base(n, h_eff, AnchorType)
        {
            this.Material = Material;
            this.SteelFailureType = SteelFailureType;
            this.NumberOfEdges = NumberOfEdges;
            this.ca_1 = ca_1;
            this.ca_2 = ca_2;
            this.s_MAX = s_Max;
            this.ev_p = e_prime_v;
            this.ConcreteCondition=ConcreteCondition;
            this.SupplementalReinforcement = SupplementalReinforcement;
            this.A_vc = A_vc;
            this.h_a = h_a;
            this.nFirstRow = nFirstRow;
            this.IsCastContinuouslyWelded = IsCastContinuouslyWelded;
            this.TypeOfAnchorSleeve = TypeOfAnchorSleeve;
            this.d_a = d_a;
        }

        public IConcreteMaterial Material { get; set; }

        public int NumberOfEdges { get; set; }
        public AnchorSteelElementFailureType SteelFailureType { get; set; }
        public double ca_1 {get; set;}
        public double ca_2 { get; set; }
        public double s_MAX { get; set; }
        public double ev_p { get; set; }
        ConcreteCrackingCondition ConcreteCondition{get; set;}
        SupplementalReinforcementAtAnchor SupplementalReinforcement { get; set; }
        public double A_vc { get; set; }
        double h_a { get; set; }
        public int nFirstRow { get; set; }
        public bool IsCastContinuouslyWelded { get; set; }
        public TypeOfAnchorSleeve TypeOfAnchorSleeve { get; set; }
        double d_a { get; set; }

        public override double GetNominalStrength()
        {
            double ca1_used=Get_ca1_used  (h_eff,ca_1, ca_2,s_MAX);
            double A_vco=Get_Avco(ca1_used);
            double gamma_ec_V  =Get_gamma_ec_V(ev_p,ca1_used);
            double gamma_ed_V  =Get_gamma_ed_V(ca_2,ca1_used);
            double gamma_c_V   =Get_gamma_c_V (ConcreteCondition, SupplementalReinforcement);
            double gamma_h_V   =Get_gamma_h_V (h_a,ca1_used);
            double le = Get_le(TypeOfAnchorSleeve, h_eff, d_a);
            double V_b = Get_V_b(le,d_a,fc,ca1_used);
            
            
            double V_cb =Get_V_cb (A_vc,n,A_vco,gamma_ed_V,gamma_c_V,gamma_h_V,V_b);
            double V_cbg=Get_V_cbg(V_cb,gamma_ec_V);
            

            double V_n = Math.Min(V_cb * n, V_cbg * n / nFirstRow);
            return  V_n;
        }
        /// <summary>
        /// Load-bearing length of the anchor for shear per 17.5.2.2
        /// </summary>
        /// <param name="TypeOfAnchorSleeve"></param>
        /// <param name="h_eff"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        private double Get_le(TypeOfAnchorSleeve TypeOfAnchorSleeve, double h_eff, double da)
        {
            //TODO: add check for cas in place anchors
            if (TypeOfAnchorSleeve== TypeOfAnchorSleeve.ConstantStiffness)
            {
                return h_eff;
            }
            else
            {
                return 2 * d_a;
            }
        }

        /// <summary>
        /// Calculation of c_a1 per 17.5.2.4
        /// </summary>
        private double Get_ca1_used(double h_eff,double ca_1,  double ca_2, double s_MAX)
        {
            double ca_1_used=ca_1;
            if (NumberOfEdges == 3)
            {
                List<double> distances = new List<double>()
                {
                    h_eff/1.5,ca_2/1.5,s_MAX/3
                };
                ca_1_used = distances.Max();
            }
            else
            {
                ca_2 = ca_1;
            }
            return ca_1_used;
        }

        private double Get_Avco(double ca1_used)
        {
            double Avco = 4.5 * Math.Pow(ca1_used, 2);
            return Avco;
        }

        /// <summary>
        /// The modification factor for anchor groups loaded eccentrically in shear per 17.5.2.5
        /// </summary>
        /// <param name="e_prime_v">Eccentricity</param>
        /// <param name="ca1_used"></param>
        /// <returns></returns>
        private double Get_gamma_ec_V(double e_prime_v, double ca1_used)
        {
            //17.5.2.5
            double gamma_ec_V = Math.Min(1, 1 / (1 + 2.0 * e_prime_v / 3.0 / ca1_used));

            return gamma_ec_V;
        }

        /// <summary>
        /// The modification factor for edge effect for a single anchor or group of anchors per 17.5.2.6 
        /// </summary>
        /// <param name="ca_2"></param>
        /// <param name="ca1_used"></param>
        /// <returns></returns>
        private double Get_gamma_ed_V(double ca_2, double ca1_used)
        {
            double gamma_ed_V =0.0;
            if (ca_2>1.5*ca1_used)
            {
                gamma_ed_V = 1;
            }
            else
            {
                gamma_ed_V = Math.Max(1, 0.7 + 0.3 * ca_2 / (1.5 * ca1_used));
            }
            return gamma_ed_V;
        }

        /// <summary>
        /// Modification factor which allows for cracking
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Reinforcement"></param>
        /// <returns></returns>
        private double Get_gamma_c_V(ConcreteCrackingCondition Condition, SupplementalReinforcementAtAnchor Reinforcement)
        {
            double gamma_c_V = 0.0;
            switch (Condition)
            {
                case ConcreteCrackingCondition.Uncracked:
                    gamma_c_V = 1.4;
                    break;
                case ConcreteCrackingCondition.Cracked:
                    switch (Reinforcement)
	                {
                        case SupplementalReinforcementAtAnchor.NotPresent:
                            gamma_c_V = 1.0;
                            break;
                        case SupplementalReinforcementAtAnchor.No4OrLargerBar:
                            gamma_c_V = 1.2;
                            break;
                        case SupplementalReinforcementAtAnchor.No4OrLargerBarEnclosedWithTies:
                            gamma_c_V = 1.4;
                            break;
	                }
                    break;
            }
            return gamma_c_V;
        }

        /// <summary>
        /// Factor accounting foranchors located in a concrete member where ha is less than  1.5*c_a1
        /// </summary>
        /// <param name="ha"></param>
        /// <param name="ca1_used"></param>
        /// <returns></returns>
        private double Get_gamma_h_V(double ha,double  ca1_used)
        {
            double gamma_h_V = 0.0;
            gamma_h_V = ha >= 1.5 * ca1_used ? 1 : Math.Min(1, Math.Sqrt(1.5 * ca1_used / ha));
            return gamma_h_V;
        }

        /// <summary>
        /// Basic breakout strength in shear of a single anchor
        /// </summary>
        /// <param name="le"></param>
        /// <param name="da"></param>
        /// <param name="fc"></param>
        /// <param name="ca1_used"></param>
        /// <returns></returns>
        private double Get_V_b(double le, double da, double fc, double ca1_used)
        {
            //17.5.2.2a and 17.5.2.3
            double coeff =IsCastContinuouslyWelded == true? 8.0 :7.0;

            double V_b1 = coeff * (Math.Pow(le / da , 0.2) * Math.Sqrt(da) * Material.Sqrt_f_c_prime * lambda * Math.Pow(ca1_used, 1.5)) / 1000;
            
            //TODO: implement checks described in 17.5.2.3


            //17.5.2.2b
            double V_b2 = 9.0 * lambda * Material.Sqrt_f_c_prime * ca_1 * Math.Pow(ca1_used, 1.5);
            double V_b = Math.Min(V_b1, V_b2);
            return V_b;
        }

        /// <summary>
        /// Nominal concrete breakout strength in shear  of a GROUP OF ANCHORS
        /// </summary>
        /// <param name="Avc"></param>
        /// <param name="n"></param>
        /// <param name="Avco"></param>
        /// <param name="gamma_ed_V"></param>
        /// <param name="gamma_c_V"></param>
        /// <param name="gamma_h_V"></param>
        /// <param name="V_b"></param>
        /// <returns></returns>
        private double Get_V_cb(double Avc, int n, double Avco, double gamma_ed_V, double gamma_c_V, double gamma_h_V, double V_b)
        {
            //17.5.2.1a
            double V_cb = Math.Min(Avc, n * Avco) / Avco * gamma_ed_V * gamma_c_V * gamma_h_V * V_b;
            return V_cb;
        }

        /// <summary>
        /// Nominal concrete breakout strength in shear of a SINGLE anchor
        /// </summary>
        /// <param name="V_cb"></param>
        /// <param name="gamma_ec_V"></param>
        /// <returns></returns>
        private double Get_V_cbg(double V_cb, double gamma_ec_V)
        {
            //17.5.2.1b
            double V_cbg = V_cb * gamma_ec_V;
            return V_cbg;
        }

    }
}

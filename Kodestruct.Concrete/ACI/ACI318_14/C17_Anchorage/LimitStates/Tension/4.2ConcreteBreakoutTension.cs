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
    public class ConcreteBreakoutTension : AnchorageConcreteLimitState
    {
        public double e_p_Nx  {get; set;}
        public double e_p_Ny  {get; set;}
        public double ev_p    {get; set;}
        public double ca_MIN { get; set; }   //minimum distance from center of an anchor shaft to the edge of concrete
        public double ca_MAX { get; set; }  //maximum distance from center of an anchor shaft to the edge of concrete
        public double ca_c { get; set; }    //critical edge distance
        public double s_MAX { get; set; }
        AnchorInstallationType AnchorType               {get; set;}
        ConcreteCrackingCondition ConcreteCondition { get; set; }
        double kc_Override { get; set; }
        double gamma_c_NOverwrite { get; set; }
        AnchorQualification AnchorQualification { get; set; }
        CastInAnchorageType CastInAnchorageType { get; set; }
        List<double> EdgeDistances { get; set; }
        bool HasSupplementalReinforcement { get; set; }
        double A_nc { get; set; }
        public IConcreteMaterial Material { get; set; }
             
        public ConcreteBreakoutTension(
            IConcreteMaterial Material,
            int n, 
            double h_eff,
            double e_p_Nx,
            double e_p_Ny,
            double ev_p,  
            double ca_MIN,
            double ca_MAX,
            double s_MAX,
            double A_nc,
            AnchorInstallationType AnchorType, 
            ConcreteCrackingCondition ConcreteCondition,
            AnchorQualification AnchorQualification,
            double kc_Override,
            double gamma_c_NOverwrite,
            CastInAnchorageType CastInAnchorageType,
            bool HasSupplementalReinforcement
            )
            : base(n,
            h_eff, AnchorType)
        {
            this.Material = Material;
           this.A_nc = A_nc;
           this.e_p_Nx  =e_p_Nx ;
           this.e_p_Ny  =e_p_Ny ;
           this.ev_p    =ev_p   ;
           this.ca_MIN  =ca_MIN ;
           this.ca_MAX = ca_MAX;
           this.s_MAX = s_MAX;
           this.AnchorType=AnchorType       ;
           this.ConcreteCondition = ConcreteCondition;
           this.AnchorQualification = AnchorQualification;
           this.kc_Override=kc_Override;
           this.gamma_c_NOverwrite = gamma_c_NOverwrite;
           this.HasSupplementalReinforcement = HasSupplementalReinforcement;

        }
        public override double GetNominalStrength()
        {

            double hef_used = Get_hef_used(h_eff, ca_MAX,  s_MAX, EdgeDistances);
            double gamma_ec_Nx = GetGamma_ec_N_SingleAxis(e_p_Nx, hef_used);
            double gamma_ec_Ny = GetGamma_ec_N_SingleAxis(e_p_Ny, hef_used);
            double gamma_ec_N = GetGamma_ec_N_Biaxial(gamma_ec_Nx, gamma_ec_Ny);
            double gamma_ed_N = GetGamma_ed_N(ca_MIN, hef_used);
            double kc = Get_kc(AnchorType,kc_Override);
            double gamma_c_N = GetGamma_c_N(AnchorType, ConcreteCondition, AnchorQualification, kc, gamma_c_NOverwrite);
            double gamma_cp_N = GetGamma_cp_N(AnchorType, ConcreteCondition, HasSupplementalReinforcement, ca_MIN, ca_c, hef_used);
            double N_b = GetNb(CastInAnchorageType,hef_used,fc,lambda,kc);
            double A_Nco = GetA_Nco(hef_used);

            double N_cb;
            if (n <= 1)
            {
                //17.4.2.1a
                N_cb = A_nc / A_Nco * gamma_ed_N * gamma_c_N * gamma_cp_N * N_b;
            }
            else
            {
                //17.4.2.1b
                double N_cbg = A_nc / A_Nco * gamma_ec_N * gamma_ed_N * gamma_c_N * gamma_cp_N * N_b;
                N_cb = N_cbg;
            }
              return N_cb;
        }




        private double Get_hef_used(double h_eff, double ca_MAX, double s_MAX, List<double> EdgeDistances)
        {
            double h_eff_used = h_eff;
            var NumLessThanThreshhold = EdgeDistances.Where(d => d < 1.5 * h_eff).Count();
            if (NumLessThanThreshhold>=3)
            {
                h_eff_used = Math.Max(ca_MAX / 1.5, s_MAX / 3.0);
            }
            return h_eff_used;
        }

        private double GetA_Nco(double hef_used)
        {
            //17.4.2.1c
            double A_Nco = 9.0 * Math.Pow(hef_used, 2.0);
            return A_Nco;
        }

        //The basic concrete breakout strength of a single anchor in tension in cracked concrete  per 17.4.2.2 
        private double GetNb(CastInAnchorageType CastInAnchorageType, double hef_used, double fc,double lambda, double kc)
        {
            double Nb;
            if (CastInAnchorageType == CastInAnchorageType.HeadedBolt || CastInAnchorageType == CastInAnchorageType.HeadedStud)
            {
                if (hef_used>=11 || hef_used<=25)
                {
                    //17.4.2.2b
                   Nb = 16*Material.Sqrt_f_c_prime/1000*Math.Pow(hef_used,(5/3));
                }
                Nb = GetNbAnyAnchor(hef_used, fc, lambda, kc);
            }
            else
            {
                Nb = GetNbAnyAnchor(hef_used, fc, lambda, kc);
            }
            return Nb;
        }

        
        private double GetNbAnyAnchor(double hef_used, double fc, double lambda, double kc)
        {
            //17.4.2.2a
            double Nb = kc* Material.Sqrt_f_c_prime / 1000 * Math.Pow(hef_used, 1.5);
            return Nb;

        }

        private double Get_kc(AnchorInstallationType AnchorType,  double kcOverwrite)
        {
            double kc = 0.0;
            if (AnchorType == AnchorInstallationType.CastInPlace)
            {
                kc = 24;
            }
            else
            {
                if (kcOverwrite > 17)
                {
                    kc = kcOverwrite < 24 ? kcOverwrite : 24;
                }
            }
            return kc;
        }



        //The modification factor for post-installed anchors designed for uncracked concrete
        private double GetGamma_cp_N(AnchorInstallationType AnchorInstallationType,ConcreteCrackingCondition ConcreteCrackingCondition,
            bool HasSupplementalReinforcement, double c_a_Min, double c_a_c, double h_ef)
        {
            double Gamma_cp_N;

            if (AnchorInstallationType == AnchorInstallationType.PostInstalled && 
                ConcreteCrackingCondition == ConcreteCrackingCondition.Uncracked && 
                HasSupplementalReinforcement==false)
            {
                if (c_a_Min >= c_a_c)
                {
                    //17.4.2.7a
                    Gamma_cp_N = 1.0;
                }
                else
                {
                    //17.4.2.7b
                    Gamma_cp_N = c_a_Min / c_a_c;
                    Gamma_cp_N = Gamma_cp_N < 1.5 * h_ef * c_a_c ? 1.5 * h_ef * c_a_c : Gamma_cp_N;
                } 
            }
            else
            {
                Gamma_cp_N = 1.0;
            }
            return Gamma_cp_N;
        }

        //The modification factor for anchor groups loaded eccentrically
        private double GetGamma_ec_N_SingleAxis(double e_p_N, double hef_used)
        {
            //17.4.2.4 
            double Gamma_ec_N = 0.0;
            if (e_p_N == 0)
            {
                Gamma_ec_N = 1.0;
            }
            else
            {
                Gamma_ec_N = Math.Min(1.0, 1.0 / (1 + 2.0 * e_p_N / 3.0 / hef_used));
            }
            return Gamma_ec_N;
        }

        private double GetGamma_ec_N_Biaxial(double gamma_ec_Nx, double gamma_ec_Ny)
        {
            //Per 17.4.2.4
            //In the case where eccentric loading exists about two axes, the modification factor gamma_ec,N shall be calculated for each axis individually and the product of these factors used
            double Gamma_ec_N = gamma_ec_Nx * gamma_ec_Ny;
            return Gamma_ec_N;
        }

        private double GetGamma_ed_N(double ca_MIN, double hef_used)
        {
            double Gamma_ed_N = ca_MIN < 1.5 * hef_used ? 0.7 + 0.3 * ca_MIN / (1.5 * hef_used) : 1;
            return Gamma_ed_N;
        }

        private double GetGamma_c_N(AnchorInstallationType AnchorType, ConcreteCrackingCondition ConcreteCondition,
            AnchorQualification AnchorQualification, double kc, 
            double gamma_c_NOverwrite)
        {
            //17.4.2.6

            double Gamma_c_N = 0.0;
            if (ConcreteCondition == ConcreteCrackingCondition.Cracked)
            {
                Gamma_c_N = 1.0;
            }
            else
            {
                if (AnchorType == AnchorInstallationType.CastInPlace)
                {
                    Gamma_c_N = 1.25;
                }
                else
                {
                    //post-installed case

                    switch (AnchorQualification)
	                    {
                        case AnchorQualification.Conventional:  Gamma_c_N = 1.4;    break;
                        case AnchorQualification.SpecialForCrackedAndUncrackedConcrete: Gamma_c_N = gamma_c_NOverwrite;   break;
                        case AnchorQualification.SpecialForUncrackedConcrete:  Gamma_c_N = 1.0;    break;
                        default: Gamma_c_N = 1.0; break;
	                    }

                }
            }

            return Gamma_c_N;
        }


    }
}

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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;
using Kodestruct.Common.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers
{
    public partial class AffectedElementInFlexure: AffectedElement
    {


        bool IsCompactDoublySymmetricForFlexure;
        bool IsRolled;

        ISection GrossSectionShape   {get; set;}
        ISection NetSectionShape     { get; set; }

        public AffectedElementInFlexure(ISection GrossSection, ISection NetSection,
            double F_y, double F_u,  bool IsCompactDoublySymmetricForFlexure=true, bool IsRolled = false)
            : base(F_y, F_u)
        {
            SteelMaterial material = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
            this.Section = new SteelGeneralSection(GrossSection, material);
            this.SectionNet = new SteelGeneralSection(NetSection, material);
            this.GrossSectionShape =GrossSection ;
            this.NetSectionShape = NetSection;

            this.IsCompactDoublySymmetricForFlexure = IsCompactDoublySymmetricForFlexure;
            this.IsRolled = IsRolled;

            Log = new CalcLog();
        }

        public double GetFlexuralStrength(double L_b)
        {
            double phiM_n =0.0;
            double F_y = this.Section.Material.YieldStress;
            double F_u = this.Section.Material.UltimateStress;

            ISection section = Section.Shape;

            bool IsValidGrossSection = ValidateGrossSection();
            bool IsValidNetSection = true;
            if (this.NetSectionShape!=null)
            {
                IsValidNetSection = ValidateNetSection();
            }
    

            double phiM_nLTB, phiM_nNet, phiM_nGross;

            //Gross Section Yielding
            if (IsCompactDoublySymmetricForFlexure == true)
            {
                double Z = GrossSectionShape.Z_x;
                phiM_nGross = 0.9 * F_y * Z;
            }
            else
            {
                double S = Math.Min(GrossSectionShape.S_xBot, GrossSectionShape.S_xTop);
                phiM_nGross = 0.9*F_y*S;
            }

            //Net Section Fracture
            if ( NetSectionShape == null)
            {
                phiM_nNet = double.PositiveInfinity;
            }
            else
            {
                if (GrossSectionShape is ISectionI)
                {
                    if (NetSectionShape is ISectionI)
                    {
                        SectionIWithFlangeHoles netSec = NetSectionShape as SectionIWithFlangeHoles;
                        phiM_nNet = GetTensionFlangeRuptureStrength(GrossSectionShape as ISectionI, NetSectionShape as ISectionI);
                    }
                    else
                    {
                        throw new Exception("If I-Shape is used for Gross section,specify I-Shape with holes object type for net sections.");
                    }
                }
                else
                {
                    phiM_nNet = 0.75 * NetSectionShape.Z_x * F_u;
                }
            }

            //Lateral Stability
            if (L_b != 0)
            {
                double S = NetSectionShape == null? Math.Min(GrossSectionShape.S_xBot, GrossSectionShape.S_xTop) :   Math.Min(NetSectionShape.S_xBot, NetSectionShape.S_xTop); 
                double lambda = this.GetLambda(L_b);
                double Q = this.GetBucklingReductionCoefficientQ(lambda);
                double F_cr = F_y * Q;
                phiM_nLTB = 0.9 * S * F_cr;
            }
            else
            {
                phiM_nLTB = double.PositiveInfinity;
            }

            List<double> LimitStates = new List<double>()
            {
                phiM_nLTB, phiM_nNet, phiM_nGross
            };

            phiM_n = LimitStates.Min();

            return phiM_n;
        }

        private bool ValidateNetSection()
        {
            //if (this.NetSectionShape is SectionOfPlateWithHoles || this.NetSectionShape is SectionIWithFlangeHoles)
            if (this.NetSectionShape is SectionOfPlateWithHoles || this.NetSectionShape is SectionIWithFlangeHoles)
            {

                return true;

            }
            else
            {
                throw new Exception("Wrong section type. Use SectionOfPlateWithHoles or SectionIWithFlangeHoles.");
            }
        }

        private bool ValidateGrossSection()
        {

            if (this.GrossSectionShape is SectionRectangular || this.GrossSectionShape is SectionI)
            {

                return true;

            }
            else
            {
                throw new Exception("Wrong section type. Only SectionRectangular and SectionI are supported.");
            }
        }



        public double GetTensionFlangeRuptureStrength(ISectionI ShapeIGross, ISectionI ShapeINet)
        {
            double phiM_n = -1;
            double F_y = Section.Material.YieldStress;
            double F_u = Section.Material.UltimateStress;
            double S_g = Math.Min(ShapeIGross.S_xBot, ShapeIGross.S_xTop);

            SectionIWithFlangeHoles netSec = ShapeINet as SectionIWithFlangeHoles;
            if (netSec == null)
            {
                throw new Exception("Net section shape not recognized");
            }
            double A_fgB = ShapeIGross.b_fBot * ShapeIGross.t_fBot;
            double A_fgT = ShapeIGross.b_fTop * ShapeIGross.t_fTop;

            double A_fnB = netSec.b_fBot * netSec.t_fBot- netSec.b_hole * netSec.N_holes;
            double A_fnT = netSec.b_fTop * netSec.t_fTop- netSec.b_hole * netSec.N_holes;

            double Y_t = GetY_t();

            double BotFlangeRuptureMoment = GetNetSectionRuptureStrength(A_fnB, A_fgB, Y_t );
            double TopFlangeRuptureMoment = GetNetSectionRuptureStrength(A_fnT, A_fgT, Y_t);

            phiM_n = Math.Min(BotFlangeRuptureMoment, TopFlangeRuptureMoment);
            return phiM_n;
        }

        private double GetNetSectionRuptureStrength(double A_fn, double A_fg, double Y_t)
        {
  
            double phiM_n = 0.0;

            double F_y = Section.Material.YieldStress;
            double F_u = Section.Material.UltimateStress;

            if (F_u * A_fn >= Y_t * F_y * A_fg)
            {
                //LimitStateDoes not apply 
                return double.PositiveInfinity;
            }
            else
            {
                double S_g = Math.Min(GrossSectionShape.S_xBot, GrossSectionShape.S_xTop);
                double M_n = F_u * A_fn / A_fg * S_g; //F13-1
                phiM_n = 0.9 * M_n;
            }

            return phiM_n;
        }

        private double GetY_t()
        {
            double F_y = Section.Material.YieldStress;
            double F_u = Section.Material.UltimateStress;

            if (F_y/F_u<=0.8)
            {
                return 1.0;
            }
            else
            {
                return 1.1;
            }
        }
        

    }
}

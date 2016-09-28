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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers
{
    public partial class AffectedElementInFlexure: AffectedElement
    {
        double A_fg;
        double A_fn;
        bool HasHolesInTensionFlange;
        bool IsCompactDoublySymmetricForFlexure;
        bool IsRolled;

        public AffectedElementInFlexure(ISection section, double F_y, double F_u, bool HasHolesInTensionFlange, double A_fg, double A_fn, bool IsCompactDoublySymmetricForFlexure,  bool IsRolled=false)
            : base(F_y, F_u)
        {
            SteelMaterial material = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
            this.Section = new SteelGeneralSection(section, material);
            this.A_fg = A_fg;
            this.A_fn = A_fn;
            this.HasHolesInTensionFlange = HasHolesInTensionFlange;
            this.IsCompactDoublySymmetricForFlexure = IsCompactDoublySymmetricForFlexure;
            this.IsRolled=IsRolled;
        }
        public AffectedElementInFlexure(SectionOfPlateWithHoles Section, ISteelMaterial Material, ICalcLog CalcLog, bool IsRolled = false)
            : base(Section, Material, CalcLog)
        {
            this.HasHolesInTensionFlange = false;
            this.A_fg = 0;
            this.A_fn = 0;
            this.IsRolled = IsRolled;
        }

        public AffectedElementInFlexure(SectionRectangular Section, ISteelMaterial Material, ICalcLog CalcLog, bool IsRolled=false)
            : base(Section, Material, CalcLog)
        {
            this.HasHolesInTensionFlange = false;
            this.A_fg = 0;
            this.A_fn = 0;
            this.IsRolled = IsRolled;
        }


        public double GetFlexuralStrength()
        {
            double phiM_n =0.0;

            CalcLog log = new CalcLog();
            ISection section = Section.Shape;
            if (section is SectionRectangular || section is SectionOfPlateWithHoles || section is SectionI)
            {
                if (section is SectionOfPlateWithHoles)
                {
                    SectionOfPlateWithHoles plateWithHoles = section as SectionOfPlateWithHoles;
                    double S_g = plateWithHoles.B * Math.Pow(plateWithHoles.H, 2);
                    double Z_net = plateWithHoles.Z_x;
                    double Y = 0.9* this.Section.Material.YieldStress * S_g; //Flexural Yielding
                    double R = 0.75* this.Section.Material.UltimateStress * Z_net;
                    phiM_n = Math.Min(Y, R);
                }
                else if (section is ISectionI )
	            {
                        ISectionI IShape = section as ISectionI;
                        double R = GetTensionFlangeRuptureStrength(IShape);
                        if (IsCompactDoublySymmetricForFlexure == false)
                        {
                            throw new Exception("Noncompact and singly symmetric I-shapes are not supported for connection checks.");
                        }
                        else
                        {
                            BeamIDoublySymmetricCompact IBeam = new BeamIDoublySymmetricCompact(Section,this.IsRolled, Log);
                            double Y = 0.9 * IBeam.GetMajorNominalPlasticMoment();
                            phiM_n = Math.Min(Y, R);
                        }

	            }
                else //Rectangle
	            {
                    SectionRectangular plate = section as SectionRectangular;
                    if (plate !=null)
                    {
                        double Z = plate.Z_x;
                        double Y =0.9* this.Section.Material.YieldStress * Z;
                        phiM_n = Y; 
                    }
	            }
            }
            else
            {
                throw new Exception("Wrong section type. Only SectionRectangular, SectionOfPlateWithHoles and SectionI are supported.");
            }

            return phiM_n;
        }

        public double GetTensionFlangeRuptureStrength(ISectionI iShape)
        {
            double phiM_n = -1;
            double F_y = Section.Material.YieldStress;
            double F_u = Section.Material.UltimateStress;
            double S_g = Math.Min(iShape.S_xBot, iShape.S_xTop);

            double Y_t = GetY_t();
            if (F_u*A_fn>=Y_t*F_y*A_fg)
            {
                //LimitStateDoes not apply 
                return -1;
            }
            else
            {
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

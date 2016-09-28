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
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.Entities.Welds.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public class PJPGrooveWeld : GrooveWeld, IWeld
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="F_y">Base metal yield stress</param>
        /// <param name="F_u">Base metal tensile stress</param>
        /// <param name="F_EXX">Electrode strength</param>
        /// <param name="t_weld">Weld throat</param>
        /// <param name="A_nBase">Net area of base metal</param>
        /// <param name="l">Length of weld</param>
        /// <param name="Log">Calculation log</param>
        public PJPGrooveWeld(double F_y, double F_u, double F_EXX, double t_weld, double A_nBase,double l,  ICalcLog Log)
            : base(F_y, F_u, F_EXX, t_weld,A_nBase, l, Log)
        {
        }

        public PJPGrooveWeld(double F_y, double F_u, double F_EXX, double t_weld,  double A_nBase, double l)
            : base(F_y, F_u, F_EXX, t_weld, A_nBase, l)
        {

        }

        public double GetBaseMetalDesignStress(WeldLoadType loadType )
        {
            double phiR_n1 = 0.0;
            double phi_1 = 0.0;
            switch (loadType)
            {
                case WeldLoadType.WeldTensionNormal:
                        phi_1 = 0.75;
                        phiR_n1 = phi_1 * this.BaseMaterial.UltimateStress;
                    break;
                case WeldLoadType.WeldCompressionNormal:
                        //Compressive stress need not be considered in design of welds joining the parts.
                        phi_1 = 0.9;
                        phiR_n1 = phi_1 * this.BaseMaterial.YieldStress;
                    break;
                case WeldLoadType.WeldShear:
                        phi_1 = 0.75;
                        phiR_n1 = phi_1 *0.6* this.BaseMaterial.UltimateStress; //per J4
                    break;
                case WeldLoadType.WeldCompressionSpliceFinishedToBear:
                        phi_1 = 0.9;
                        phiR_n1 = phi_1 * this.BaseMaterial.YieldStress;
                    break;
                case WeldLoadType.WeldCompressionSpliceNotFinishedToBear:
                        phi_1 = 0.9;
                        phiR_n1 = phi_1 * this.BaseMaterial.YieldStress;
                    break;

            }
            return phiR_n1;
        }

        public double GetWeldMetalDesignStress(WeldLoadType loadType)
        {
            double phiR_n1 = 0.0;
            double phi_1 = 0.0;

            switch (loadType)
            {
                case WeldLoadType.WeldTensionNormal:
                    phi_1 =0.8;
                    phiR_n1 = phi_1* 0.6 * this.WeldMaterial.ElectrodeStrength;
                    break;
                case WeldLoadType.WeldCompressionNormal:
                    phiR_n1 = double.PositiveInfinity;
                    break;
                case WeldLoadType.WeldShear:
                    phi_1 = 0.75;
                    phiR_n1 = phi_1* 0.6 * this.WeldMaterial.ElectrodeStrength;
                    break;
                case WeldLoadType.WeldCompressionSpliceFinishedToBear:
                    phi_1 = 0.8;
                    phiR_n1 = phi_1* 0.6 * this.WeldMaterial.ElectrodeStrength;
                    break;
                case WeldLoadType.WeldCompressionSpliceNotFinishedToBear:
                    phi_1 = 0.8;
                     phiR_n1 = phi_1* 0.9 * this.WeldMaterial.ElectrodeStrength;
                    break;

            }
            return phiR_n1;
        }




        public double GetMinimumEffectiveThroat(double MaterialThicknessOfThinnerPartJoined)
        {
            double matThickness = MaterialThicknessOfThinnerPartJoined;
            double t_min = 1 / 8;

            if (matThickness <= 1 / 4) { t_min = 1 / 8; }
            else if (matThickness > 1 / 4 && matThickness <= 1 / 2) { t_min = 3 / 16; }
            else if (matThickness > 1 / 2 && matThickness <= 3 / 4) { t_min = 1 / 4; }
            else if (matThickness > 3 / 4 && matThickness <= 11 / 2) { t_min = 5 / 16; }
            else if (matThickness > 11 / 2 && matThickness <= 21 / 4) { t_min = 3 / 8; }
            else if (matThickness > 21 / 4 && matThickness <= 6) { t_min = 1 / 2; }
            else { t_min = 5 / 8; }

            return t_min;
        }

        public double GetStrength(WeldLoadType LoadType, double theta, bool IgnoreBaseMetal)
        {
            //ignore angle theta
            double baseMetalStrength;
            if (IgnoreBaseMetal==true)
            {
                baseMetalStrength = double.PositiveInfinity;
            }
            else
            {
                baseMetalStrength = GetBaseMetalDesignStress(LoadType) * A_nBase;
            }
            
            double weldMetalStrength = GetWeldMetalDesignStress(LoadType) * GetWeldArea();
            return Math.Min(baseMetalStrength, weldMetalStrength);
        }

        public double GetWeldArea()
        {
            return Size * Length;
        }
    }
}

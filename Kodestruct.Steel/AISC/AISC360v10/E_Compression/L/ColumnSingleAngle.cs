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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.Steel.Entities;


 
 

namespace  Kodestruct.Steel.AISC360v10
{
    public class ColumnSingleAngle : ColumnFlexuralBuckling
    {

        //        public ColumnSingleAngle(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog) 
        //    : base(Section, L_x,L_y,K_x,K_y, CalcLog) 
        //{

        public ColumnSingleAngle(ISteelSection Section, double L_x, double L_y, ICalcLog CalcLog) 
            : base(Section, L_x,L_y, CalcLog) 
        {

        }



        public override double CalculateCriticalStress(bool EccentricBrace)
        {
            throw new NotImplementedException();
        }

        public override SteelLimitStateValue GetFlexuralBucklingStrength()
        {

            double FeFlexuralBuckling = GetFlexuralElasticBucklingStressFe(); 
            double FcrFlexuralBuckling = GetCriticalStressFcr(FeFlexuralBuckling, 1.0);
            double Qflex = GetReductionFactorQ(FcrFlexuralBuckling);
            double FcrFlex = GetCriticalStressFcr(FeFlexuralBuckling, Qflex);

            double phiP_n = GetDesignAxialStrength(FcrFlex);

            SteelLimitStateValue ls = new SteelLimitStateValue(phiP_n, true);
            return ls;
        }

        public override SteelLimitStateValue GetTorsionalAndFlexuralTorsionalBucklingStrength(bool EccentricBrace)
        {

            SteelLimitStateValue ls;

            ISectionAngle a = this.Section.Shape as ISectionAngle;
            if (a == null)
            {
                throw new Exception("Incorrect section type. Section must be of type ISectionAngle.");
            }
            if (Math.Max(a.b,a.d)/a.t<=20.0)
            {
                ls = new SteelLimitStateValue(-1, false);
            }
            else
            {
                double FeTorsionalBuckling = GetTorsionalElasticBucklingStressFe();
                double FcrTorsionalBuckling = GetCriticalStressFcr(FeTorsionalBuckling, 1.0);
                double Qtors = GetReductionFactorQ(FcrTorsionalBuckling);
                double FcrTors = GetCriticalStressFcr(FeTorsionalBuckling, Qtors);

                double phiP_n = GetDesignAxialStrength(FcrTors);
                ls = new SteelLimitStateValue(phiP_n, true);
            }
            return ls;
        }

        private double GetTorsionalElasticBucklingStressFe()
        {
            throw new Exception("Torsional buckling check of slender unsymmetric shapes is not supported.");
            // For unsymmetric members, Fe is the lowest root of the cubic equation
            //(E4-6)
        }

        protected override double GetSlendernessKLr(bool IsMajorAxis)
        {
            double K;
            double L;
            double r;
            double KLrx;
            double KLry;
            double KLrz;

            ISectionAngle a = this.Section.Shape as ISectionAngle;
            if (a == null)
            {
                throw new Exception("Incorrect section type. Section must be of type ISectionAngle.");
            }
            KLrx = L_ex / a.r_x;
            KLry= L_ey / a.r_y;
            KLrz = L_ez / a.r_z;

            List<double> KLrs = new List<double>() 
            { KLrx,KLry,KLrz};



            //for major axis buckling "major" is considered only 
            //for geometric axis. The second case using r_z is always
            //minor. This is because in most practical applications
            //it is possible to reduce unbraced length in the angle
            //geometric axii, but to ensure that principal axis bracing is
            //provided at a skew angle is impractical.

            if (IsMajorAxis == true)
            {
                double KLr1;
                if (KLrx>=KLry)
                {
                    return Math.Min(KLrx, KLrz);
                }
                else
                {
                    return Math.Min(KLry, KLrz);
                }
            }
            else
            {
                if (KLrx <= KLry)
                {
                    return Math.Min(KLrx, KLrz);
                }
                else
                {
                    return Math.Min(KLry, KLrz);
                }

            }

        }

        public override double GetReductionFactorForStiffenedElementQa(double Fcr)
        {
            ISectionAngle ang = this.Section.Shape as ISectionAngle;
            double E= this.Section.Material.ModulusOfElasticity;
            double F_y = this.Section.Material.YieldStress;

            double b = Math.Min(ang.b, ang.d);
            double t = ang.t;

            double Q_s = 1.0;

            //AISC specification page 16.1–42 

            if (((b) / (t))<=0.45*Math.Sqrt(((E) / (F_y))))
            {
                Q_s = 1.0;
            }
            else
            {
                if (((b) / (t))>0.91*Math.Sqrt(((E) / (F_y))))
                {
                    Q_s = ((0.53 * E) / (F_y * (((b) / (t))) * (((b) / (t)))));
                }
                else
                {
                    Q_s = 1.34 - 0.76 * (((b) / (t))) * Math.Sqrt(((F_y) / (E)));
                }
            }
            return Q_s;
        }

    }
}

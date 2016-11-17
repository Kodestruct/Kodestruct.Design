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
using Kodestruct.Common.Entities;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI.Entities.ShearAndTorsion;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.ShearFriction
{


    public partial class ConcreteSectionShearFriction : AnalyticalElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Material"></param>
        /// <param name="A_c">area of concrete section resisting shear transfer</param>
        /// <param name="TransverseRebarMaterial"></param>
        /// <param name="A_v">area of reinforcement crossing the assumed  shear plane to resist shear</param>
        /// <param name="alpha">angle between shear-friction reinforcement and assumed shear plane</param>
        /// <param name="F_comp">Permanent net compression across the shearplane </param>
        public ConcreteSectionShearFriction(ShearFrictionSurfaceType ShearFrictionSurfaceType, IConcreteMaterial Material, double A_c, IRebarMaterial TransverseRebarMaterial, double A_v, 
            double alpha =90.0, double F_comp =0.0 )
        {
            this.Material = Material;
            this.ShearFrictionSurfaceType = ShearFrictionSurfaceType;
            this.A_c   =A_c;
            this.TransverseRebarMaterial =TransverseRebarMaterial ;
            this.A_v = A_v;
            this.alpha = alpha;
        }

            IConcreteMaterial Material { get; set; }
            double A_c                               {get; set;}
            IRebarMaterial TransverseRebarMaterial   {get; set;}
            double A_v { get; set; }
            ShearFrictionSurfaceType ShearFrictionSurfaceType { get; set; }
            double alpha { get; set; }

        /// <summary>
            /// Shear strength per 22.9.4.2 and 22.9.4.3
        /// </summary>
        /// <returns></returns>
        public double GetShearFrictionStrength( )
         {
             double mu = GetCoefficientOfFriction();
             double lambda = GetLambda();
             double f_y = TransverseRebarMaterial.YieldStress;
             double V_nSteel = A_v * f_y * (mu*lambda * Math.Sin(alpha.ToRadians()) + Math.Cos(alpha.ToRadians())); // Eq 22.9.4.3
             double f_cMax = GetMaximumStressAcrossShearPlane();
             double V_nMax = A_c * f_cMax;
             double V_n = Math.Min(V_nSteel, V_nMax);
             StrengthReductionFactorFactory f = new StrengthReductionFactorFactory();
             double phi = f.Get_phi_ShearReinforced();

             double phiV_n = phi * V_n;
             return phiV_n;
         }

        /// <summary>
        /// Section 22.9.4.4 
        /// </summary>
        /// <returns></returns>
        private double GetMaximumStressAcrossShearPlane()
        {
            //Maximum Vn across the assumed shear plane per Table 22.9.4.4
            if (Material.TypeByWeight == ConcreteTypeByWeight.Normalweight)
            {
                if ( ShearFrictionSurfaceType == ShearFrictionSurfaceType.HardenedRoughenedConcrete || ShearFrictionSurfaceType ==ShearFrictionSurfaceType.MonolithicConcrete  )
                {
                    return GetFullUpperShearStress();
                }
                else
                {
                    return GetReducedUpperShearStress();
                }
            }
            else
            {
                return GetReducedUpperShearStress();
            }
        }

        /// <summary>
        /// Table 22.9.4.4
        /// </summary>
        /// <returns></returns>
        private double GetReducedUpperShearStress()
        {
            double f_c = Material.SpecifiedCompressiveStrength;
            double fcMax1 = f_c * 0.2;
            double fcMax2 =800.0;
            return Math.Min(fcMax1, fcMax2);
        }

        /// <summary>
        /// Table 22.9.4.4
        /// </summary>
        /// <returns></returns>
        private double GetFullUpperShearStress()
        {

            double f_c = Material.SpecifiedCompressiveStrength;
            double fcMax1 = f_c * 0.2;
            double fcMax2 = 480.0 + 0.08 * f_c;
            double fcMax3 = 1600.0;

            List<double> fcs = new List<double>() { fcMax1, fcMax2, fcMax3 };
            return fcs.Min();
        }

        /// <summary>
        /// Footnotes of Table 22.9.4.2
        /// </summary>
        /// <returns></returns>
        private double GetLambda()
        {
            if (Material.TypeByWeight == ConcreteTypeByWeight.Normalweight)
            {
                return 1.0;
            }
            else
            {
                return 0.75;
            }
        }

        /// <summary>
        /// Table 22.9.4.2—Coefficients of friction
        /// </summary>
        /// <returns></returns>
        private double GetCoefficientOfFriction()
        {

            switch (ShearFrictionSurfaceType)
            {
                case ShearFrictionSurfaceType.MonolithicConcrete:

                    return 1.4;
                case ShearFrictionSurfaceType.HardenedRoughenedConcrete:
                    return 1.0;
                case ShearFrictionSurfaceType.HardenedNonRoughenedConcrete:
                    return 0.6;
                case ShearFrictionSurfaceType.ConcreteAgainstSteel:
                    return 0.7;
                default:
                    return 0.6;
            }
        }


    }
}

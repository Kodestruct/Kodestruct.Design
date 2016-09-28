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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;


namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{
    public abstract class CompactnessBase : ICompactnessElement
    {

        public CompactnessBase(ISteelMaterial Material)
        {
            this.material = Material;
        }

        private ISteelMaterial material;

        public ISteelMaterial Material
        {
            get { return material; }
            set { material = value; }
        }
        

        public CompactnessClassFlexure GetCompactnessFlexure()
        {
            double lambda = GetLambda();
            double lambda_r = GetLambda_r(StressType.Flexure);
            double lambda_p = GetLambda_p(StressType.Flexure);

            if (lambda > lambda_r)
            {
                return CompactnessClassFlexure.Slender;
            }
            else
            {
                if (lambda<= lambda_p )
                {
                    return CompactnessClassFlexure.Compact;
                }
                else
                {
                    return CompactnessClassFlexure.Noncompact;
                }
            }
        }

        public CompactnessClassAxialCompression GetCompactnessAxialCompression()
        {
            double lambda = GetLambda();
            double lambda_r = GetLambda_r(StressType.Axial);
            if (lambda>lambda_r)
            {
                return CompactnessClassAxialCompression.Slender;
            }
            else
            {
                return CompactnessClassAxialCompression.NonSlender;
            }
        }

        public abstract  double GetLambda();

        public abstract double GetLambda_p(StressType stress);

        public abstract double GetLambda_r(StressType stress);

        protected double SqrtE_Fy()
        {
            double Fy = material.YieldStress;
            double E = material.ModulusOfElasticity;

            if (Fy==0 || E == 0)
            {
                throw new Exception("Material Fy or E cannot be zero");
            }
            return Math.Sqrt(E / Fy);
        }


    }
}

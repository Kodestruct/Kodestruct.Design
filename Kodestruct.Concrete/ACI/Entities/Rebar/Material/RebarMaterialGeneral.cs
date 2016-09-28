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
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI
{
    //mild reinforcement
    public class RebarMaterialGeneral: IRebarMaterial
    {
        public RebarMaterialGeneral(double f_y)
        {

            yieldStress = f_y;

            
        }

        #region IRebarMaterial Members

        public string Name
        {
            get { return "General"; }
        }

        private double yieldStress;

        public double YieldStress
        {
            get { return yieldStress; }
            set { yieldStress = value; }
        }

        public double GetUltimateStrain(double BarDiameter)
        {
            double UltStrain=0.0;

            if (BarDiameter <= 6.0 / 8.0)
            {
                UltStrain = 0.14;
            }
            else
            {
                if (BarDiameter <= 11.0 / 8.0)
                {
                    UltStrain = 0.12;
                }
                else
                {
                    UltStrain = 0.1;
                }
            }

            return UltStrain;
        }
        

        public double GetStress(double Strain)
        {
            double e = Strain;
            double E = ConcreteConstants.SteelModulusOfElasticity; //psi
            double fy = this.yieldStress;
            double f;
            double theoreticalStress = Math.Abs(e * E);
            if (e>=0)
            {
               f =theoreticalStress > fy ? fy : theoreticalStress;
            }
            else
            {
                f = theoreticalStress > fy ? -fy : -theoreticalStress;
            }
           
            return f;
        }

        #endregion


        public double GetDesignStress()
        {
            return this.yieldStress;
        }




        double epsilon_ty;
        public double YieldStrain
        {
            get { 
                epsilon_ty = yieldStress/ ConcreteConstants.SteelModulusOfElasticity;
                return epsilon_ty;
            }
        }
    }
}

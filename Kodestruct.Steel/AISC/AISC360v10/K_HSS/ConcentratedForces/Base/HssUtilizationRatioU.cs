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
using Kodestruct.Common.Section.Interfaces;

using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
 
 

namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{

 



    public abstract partial class HssToPlateConnection : SteelDesignElement 
    {
        //public double GetUtilizationRatio(ISteelSection Section, double P_uHss, double M_uHss)
        //{
        //    double U = 0;
        //    double Fy = Section.Material.YieldStress;
        //    double Fc = 0.0;

        //    Fc = Fy;


        //    ISection sec = Section.Shape;
        //    double Ag = sec.A;
        //    double S = Math.Min(sec.S_xBot, sec.S_xTop);
        //    double Pro = RequiredAxialStrenghPro;
        //    double Mro = RequiredMomentStrengthMro;
        //    //(K1-6) from TABLE K1.2
        //    U=Math.Abs(Pro/(Fc*Ag)+Mro/(Fc*S));
        //    return U;
        //}


        private double _U;

        public double U
        {
            get
            {
                _U = GetU();
                return _U;
            }
            set { _U = value; }
        }

        private double GetU()
        {
            double A_g = GetArea();
            double S = GetSectionModulus();
            double Axial = Math.Abs((P_uHss) / (F_y * A_g));
            double Flexure = Math.Abs((M_uHss) / (F_y * S));
            double U = (Axial + Flexure);
            return U;
        }

        protected abstract double GetSectionModulus();
        protected abstract double GetArea();




        protected abstract double GetF_y();

       
        private double _F_y;

        protected double F_y
        {
            get
            {
                _F_y = GetF_y();
                return _F_y;
            }
            set { _F_y = value; }
        }

    }
}

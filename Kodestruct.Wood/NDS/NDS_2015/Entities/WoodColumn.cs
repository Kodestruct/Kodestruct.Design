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
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Wood.NDS.NDS2015
{
    public abstract partial class WoodColumn: WoodMember
    {
        double Emin;
        double CM; 
        double Ct; 
        double CT; 
        double Ci; 
        double Cr;

        public WoodColumn(double Emin, double CM, double Ct, double CT, double Ci, double Cr, ICalcLog CalcLog): base(CalcLog)
        {
            this.Emin=Emin;
            this.CM=  CM;
            this.Ct=  Ct;
            this.CT=  CT;
            this.Ci=  Ci;
            this.Cr=  Cr;
        }
        
        public virtual double GetAdjustedMinModulusOfElasticity()
        {
            double EminPrime = Emin * CM * Ct * Ci * CT;

            return EminPrime;
        }
        
    }
}

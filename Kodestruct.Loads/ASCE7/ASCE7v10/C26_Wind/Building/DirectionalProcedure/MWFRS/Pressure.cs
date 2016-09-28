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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.MWFRS
{
    public partial class Mwfrs : BuildingDirectionalProcedureElement
    {

        public double GetDesignPressureNet(double qz, double qh, double G, double Cpl, double Cpw)
        {
            double p1 = qz * G * Cpw;
            double p2 = qh * G * Cpl;
            double p = p1 - p2;


            return p;
        }

        public double GetDesignPressure(double q, double G, double Cp, double qi, double GCpi)
        {
           double p1 = q * G * Cp - qi * GCpi;
           double p2 = q * G * Cp + qi * GCpi;
           double p = Math.Max(Math.Abs(p1), Math.Abs(p2));

           return p;
        }
    }
}

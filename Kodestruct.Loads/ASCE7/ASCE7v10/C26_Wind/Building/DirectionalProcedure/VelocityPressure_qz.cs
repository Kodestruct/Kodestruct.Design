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
using Kodestruct.Common.CalculationLogger; 
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Loads.ASCE.ASCE7_10.WindLoads;
using Kodestruct.Loads.ASCE7.Entities;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure
{
    public partial class BuildingDirectionalProcedureElement: WindBuilding
    {

        public double GetVelocityPressure(double Kz, double Kzt, double Kd, double V, WindVelocityLocation Location)
        {
            double qz= 0.00256 * Kz * Kzt * Kd * Math.Pow(V, 2); //(28.3-1)

            return qz;
        }
    }
}

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
using Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.CC;
using Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.MWFRS;


namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure
{
    public partial class BuildingDirectionalProcedureElement : WindBuilding
    {
        public double GetWindPressureMWFRS(double q, double G, double Cp, double qi, double GCpi)
        {
            double p = 0.0;
            Mwfrs structure = new Mwfrs(this.Log);
            p = structure.GetDesignPressure(q, G, Cp, qi, GCpi);
            return p;

        }
        public double GetWindPressureMWFRSNet(double qz, double qh, double G, double Cpl, double Cpw)
        {
            double p = 0.0;
            Mwfrs structure = new Mwfrs(this.Log);
            p = structure.GetDesignPressureNet( qz,  qh,  G,  Cpl,  Cpw);
            return p;
        }

        public double GetWindPressureCC(double q, double GCpPos,double GCpNeg, double qi, double GCpi, double h)
        {
            double p = 0.0;
            ComponentOrCladding cc = new ComponentOrCladding(this.Log);
            p = cc.GetDesignPressure(q, GCpPos, GCpNeg, qi, GCpi, h);
            return p;
        }

    }
}

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

using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;
using d = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltDescriptions;
using f = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltFormulas;
using v = Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted.BoltValues;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class Bolt: BoltBase
    {
        public Bolt(double Diameter, BoltThreadCase ThreadType, 
            ICalcLog log): base(Diameter,
            ThreadType,log)
        {
            BoltHoleSizeCalculated = false;
        }

        public Bolt(double Diameter,ICalcLog log)
            : base(Diameter,
                BoltThreadCase.Included, log)
        {
            BoltHoleSizeCalculated = false;
        }

        public override abstract double NominalTensileStress { get; }
        public override abstract double NominalShearStress { get; }

        public override double GetAvailableTensileStrength()
        {
            double Ab = this.Area;//nominal unthreaded bolt area
            double Fnt = NominalTensileStress;
            double phiR_n;
            double Rn = Ab*Fnt; //Formula  J3-1

                phiR_n = 0.75 * Rn;


            return phiR_n;
        }

        public override double GetAvailableShearStrength(double N_ShearPlanes, bool IsEndLoadedConnectionWithLengthEfect)
        {
            double Ab = this.Area;//nominal unthreaded bolt area
            double Fnv = NominalShearStress;
            double R;
            double Rn = Ab * Fnv; //Formula  J3-1


            R = 0.75 * Rn * N_ShearPlanes;

            //For end loaded connections with a fastener pattern length greater than 38 in., Fnv shall be
            //reduced to 83.3% of the tabulated values. Fastener pattern length is the maximum distance parallel to the
            //line of force between the centerline of the bolts connecting two parts with one faying surface. 
            if (IsEndLoadedConnectionWithLengthEfect ==true)
            {
                R = R * 0.833;
            }

            return R;
        }


        
    }
}

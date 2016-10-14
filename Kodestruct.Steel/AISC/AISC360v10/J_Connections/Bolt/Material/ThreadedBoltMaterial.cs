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
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public class ThreadedBoltMaterial : IBoltMaterial
    {
        public double GetNominalShearStress(BoltThreadCase ThreadCase)
        {
            return 0.563 * F_u; //Table J3.2
        }

        public double GetNominalTensileStress(BoltThreadCase ThreadCase)
        {
            return 0.75 * F_u; //Table J3.2
        }

        double F_u;
        public ThreadedBoltMaterial (double F_u)
	    {
            this.F_u = F_u;
	    }



        public double GetNominalShearStress(string ThreadCase)
        {
            if (ThreadCase == "X")
            {
                return GetNominalShearStress(BoltThreadCase.Excluded);
            }
            else
            {
                return GetNominalShearStress(BoltThreadCase.Included);
            }
        }

        public double GetNominalTensileStress(string ThreadCase)
        {
            if (ThreadCase == "X")
            {
                return GetNominalTensileStress(BoltThreadCase.Excluded);
            }
            else
            {
                return GetNominalTensileStress(BoltThreadCase.Included);
            }
        }
    }
}

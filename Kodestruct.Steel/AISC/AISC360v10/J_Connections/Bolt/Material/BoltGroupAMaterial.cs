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
 

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public class BoltGroupAMaterial : IBoltMaterial
    {

        public double GetNominalShearStress(BoltThreadCase ThreadCase)
        {
            if (ThreadCase == BoltThreadCase.Included)
            {
                return 54.0;
            }
            else
            {
                return 68.0;
            }
        }

        public double GetNominalTensileStress(BoltThreadCase ThreadCase)
        {
            if (ThreadCase == BoltThreadCase.Included)
            {
                return 90.0;
            }
            else
            {
                return 90.0;
            }
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

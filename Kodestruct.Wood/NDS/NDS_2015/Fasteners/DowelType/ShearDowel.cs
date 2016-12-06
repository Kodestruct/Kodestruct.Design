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
 
using Kodestruct.Wood.NDS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners.DowelType
{
    public  class ShearDowel
    {
        public ShearDowel(double D)
        {

        }
    


        //Table 12.5.1B Spacing Requirements for 
        //Fasteners in a Row 
        public double GetMinimumSpacingOfFastenersWithinRow(LoadToGrainDirection LoadToGrainDirection)
        {
            throw new NotImplementedException();
        }

        //Table 12.5.1B Spacing Requirements for 
        //Fasteners in a Row 
        public double GetMinimumSpacingOfFastenersWithinRowForMaximumStrength(LoadToGrainDirection LoadToGrainDirection)
        {
            throw new NotImplementedException();
        }
        public double GetMinimumSpacingOfRows()
        {
            throw new NotImplementedException();
        }
        public double GetMinimumEdgeDistance()
        {
            throw new NotImplementedException();
        }

        //Table 12.5.1A End Distance Requirements
        public double GetMinimumEndDistance(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType)
        {
            throw new NotImplementedException();
        }

        //Table 12.5.1A End Distance Requirements
        public double GetMinimumEndDistanceForMaximumStrength(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType)
        {
            throw new NotImplementedException();
        }
        
    }

}

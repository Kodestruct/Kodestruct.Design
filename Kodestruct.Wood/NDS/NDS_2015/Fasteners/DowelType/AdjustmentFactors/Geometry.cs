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

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners

{
public partial class DowelFastenerBase : WoodFastener
{

    public double GetGeometryFactor(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType, double L_end)
    {
        double C_Delta = 0;
        throw new NotImplementedException();
        if (this.D<=0.25)
        {
            C_Delta = 1.0;
        }
        else
        {
            //End distance C_Delta
            double C_DeltaEnd = GetEndDistanceC_Delta(LoadToGrainDirection, FastenerEdgeBearingType, L_end);
        }

        return C_Delta;
    }

    private double GetEndDistanceC_Delta(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType,
        double L_end)
    {
        throw new NotImplementedException();
        double L_endC_delta05 = GetMinimumEndDistance(LoadToGrainDirection, FastenerEdgeBearingType);
        if (L_end < L_endC_delta05)
        {
            throw new Exception("End distance is smaller than minimum permitted. Revise design.");
        }
        double L_endC_delta1 = GetMinimumEndDistanceForMaximumStrength(LoadToGrainDirection, FastenerEdgeBearingType);
        double C_Delta = L_end / (L_endC_delta1);
        return C_Delta;

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

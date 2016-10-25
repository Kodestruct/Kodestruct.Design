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

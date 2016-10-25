using Kodestruct.Common.Entities;
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

        public double GetWetServiceFactor(MoistureCondition MoistureConditionAtFabrication,  MoistureCondition MoistureConditionAtService )
        {
            if (MoistureConditionAtService == MoistureCondition.Wet)
            {
                return 0.7;
            }
            else
            {
                if (MoistureConditionAtFabrication == MoistureCondition.Wet)
                {
                    return 0.8;
                }
                else
                {
                    return 1.0;
                }
            }
        }
    }
}

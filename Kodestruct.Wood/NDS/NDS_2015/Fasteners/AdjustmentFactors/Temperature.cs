using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners
{
    public partial class WoodFastener : AnalyticalElement
    {

        public double GetTemperatureFactor(double Temperature, MoistureCondition MoistureCondition)
        {
            if (MoistureCondition == Entities.MoistureCondition.Dry)
            {
                if (Temperature <= 100.0)
                {
                    return 1.0;
                }
                else
                {
                    if (Temperature <= 125.0)
                    {
                        return 0.8;
                    }
                    else
                    {
                        return 0.7;
                    }
                }
            }
            else
            {
                if (Temperature <= 100.0)
                {
                    return 1.0;
                }
                else
                {
                    if (Temperature <= 125.0)
                    {
                        return 0.7;
                    }
                    else
                    {
                        return 0.5;
                    }
                }
            }
        }

    }
}

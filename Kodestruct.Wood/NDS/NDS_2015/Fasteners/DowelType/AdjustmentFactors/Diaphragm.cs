using Kodestruct.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners
{
    public partial class DowelFastenerBase : WoodFastener
    {

        //12.5.3
        public double GetDiaphragmFactor(MechanicalDowelConnectionType DowelConnectionType,
            bool IsDiaphragmConnection)
        {
            string conString = DowelConnectionType.ToString();
            if (IsDiaphragmConnection == true )
            {
                if (conString.Contains("Nail") || conString.Contains("Spike"))
                {
                    return 1.15;
                }
                else
                {
                    return 1.0;
                }
            }
            else
            {
                return 1.0;
            }
        }
    }
}

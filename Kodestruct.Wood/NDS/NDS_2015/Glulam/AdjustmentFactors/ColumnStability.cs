using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GlulamMember : WoodMember
    {
        /// <summary>
        /// Factor per 3.7.1.3 
        /// </summary>
        /// <returns></returns>
        protected override double Get_c()
        {
            return 0.9;
        }
    }
}

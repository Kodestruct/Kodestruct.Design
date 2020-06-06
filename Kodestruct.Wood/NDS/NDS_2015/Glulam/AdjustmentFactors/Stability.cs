using System;
using System.Collections.Generic;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.GluLam
{
    public partial class GluelamMember : WoodMember
    {
        protected override bool DetermineIfMemberIsLaterallyBraced()
        {
            //determine lateral bracing requirements to skip stability checks
             return false;
        }
    }
}

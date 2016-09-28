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
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public  partial class RhsTrussTConnection : RhsTYXTrussBranchConnection
    {
        //no overrides compared to base RhsTYXTrussConnection class
        public RhsTrussTConnection(SteelRhsSection Chord, SteelRhsSection MainBranch, double thetaMain,
            SteelRhsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeMain, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord)
            : base(Chord, MainBranch, thetaMain, SecondBranch, thetaSecond, ForceTypeMain, ForceTypeSecond, IsTensionChord,
            P_uChord,  M_uChord)
        {
            if (thetaMain!=90)
            {
                throw new Exception("Please specify angles theta_Main and theta_Second as 90 degress for T connection");
            }
            if (thetaSecond!=thetaMain)
            {
                throw new Exception("Please specify same angles theta_Main and theta_Second as 90 degress for T connection");
            }
            if (MainBranch.Section.B != SecondBranch.Section.B || MainBranch.Section.H != SecondBranch.Section.H)
            {
                throw new Exception("Please specify same values for main and secondary branch for T connection.");
            }

        }
    }

}

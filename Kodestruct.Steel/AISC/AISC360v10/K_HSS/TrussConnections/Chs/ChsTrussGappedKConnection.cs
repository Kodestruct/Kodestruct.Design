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

namespace Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public partial class ChsTrussGappedKConnection : ChsTrussKConnection
    {
            public ChsTrussGappedKConnection(SteelChsSection Chord, SteelChsSection MainBranch, double thetaMain, AxialForceType ForceTypeMain, 
            SteelChsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord, double g): base( Chord,  MainBranch,  thetaMain,  ForceTypeMain, 
             SecondBranch,  thetaSecond,  ForceTypeSecond,  IsTensionChord,
             P_uChord,  M_uChord,g)
       {

       }

            //No override = > use base class

            protected override SteelChsSection getBranch()
            {
                return this.MainBranch;
            }
    }
}

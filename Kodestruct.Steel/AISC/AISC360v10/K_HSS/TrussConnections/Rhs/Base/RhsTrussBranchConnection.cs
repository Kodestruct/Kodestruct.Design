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
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.Steel.Entities;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class RhsTrussBranchConnection : HssTrussConnection,IHssTrussBranchConnection
    {

        public RhsTrussBranchConnection(SteelRhsSection Chord, SteelRhsSection MainBranch, double thetaMain, AxialForceType ForceTypeMain, 
            SteelRhsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord): base(IsTensionChord,P_uChord,M_uChord)
        {
            this.Chord = Chord;
            this.MainBranch       =MainBranch   ;
            this.thetaMain        =thetaMain    ;
            this.SecondBranch = SecondBranch;
            this.thetaSecond = thetaSecond;
            this.ForceTypeMain = ForceTypeMain;
            this.ForceTypeSecond = ForceTypeSecond;
        }


        public virtual SteelLimitStateValue GetChordWallPlastificationStrength(bool IsMainBranch)
        {
            return new SteelLimitStateValue(-1, false);
        }

        public virtual SteelLimitStateValue GetChordSidewallLocalYieldingStrength()
        {
            return new SteelLimitStateValue(-1, false);
        }
        public virtual SteelLimitStateValue GetChordSidewallShearStrength()
        {
            return new SteelLimitStateValue(-1, false);
        }
        public virtual SteelLimitStateValue GetChordSidewallLocalCripplingStrength()
        {
            return new SteelLimitStateValue(-1, false);
        }

        public virtual SteelLimitStateValue GetBranchPunchingStrength()
        {
            return new SteelLimitStateValue(-1, false);
        }

        public virtual SteelLimitStateValue GetBranchYieldingFromUnevenLoadDistributionStrength(bool IsMainBranch)
        {
            return new SteelLimitStateValue(-1, false);
        }

        protected SteelRhsSection Chord             { get; set; }
        protected  SteelRhsSection MainBranch     {get; set;}
        protected  double thetaMain               {get; set;}
        protected  SteelRhsSection SecondBranch   {get; set;}
        protected  double thetaSecond              { get; set; }
        protected AxialForceType ForceTypeMain   {get; set;}
        protected AxialForceType ForceTypeSecond { get; set; }

    }


}

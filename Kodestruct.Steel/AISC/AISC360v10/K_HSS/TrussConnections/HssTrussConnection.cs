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
using Kodestruct.Steel.AISC.SteelEntities;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public class HssTrussConnection : SteelDesignElement 
    {
        public HssTrussConnection(HssTrussConnectionChord chord,  List<HssTrussConnectionBranch> branches, 
          ICalcLog CalcLog): base ( CalcLog)
        {
            this.branches = branches;
            this.chord = chord;
        }

        private List<HssTrussConnectionBranch> branches;

        public List<HssTrussConnectionBranch> Branches
        {
            get { return branches; }
            set { branches = value; }
        }

        private HssTrussConnectionChord chord;

        public HssTrussConnectionChord Chord
        {
            get { return chord; }
            set { chord = value; }
        }
    }
}

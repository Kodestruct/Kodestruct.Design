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
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class RhsTrussConnection 
    {

        public RhsTrussConnection(SteelRhsSection Chord, SteelRhsSection Branch_i, double theta_i, SteelRhsSection Branch_j, double theta_j)
        {
            this.Chord = Chord;
        }

        public SteelRhsSection Chord { get; set; }
        SteelRhsSection Branch_i                 {get; set;}
        SteelRhsSection Branch_j                 {get; set;}
        double theta_i {get; set;} 
        double theta_j {get; set;} 

        public double GetBranch_iStrength()
        {
            throw new NotImplementedException();
        }
        public double GetBranch_jStrength()
        {
            throw new NotImplementedException();
        }

    }

    #region Obsolete
    //public abstract partial class RhsTrussConnection : HssTrussConnection
    //{
    //    public RhsTrussConnection(HssTrussConnectionChord chord, List<HssTrussConnectionBranch> branches, ICalcLog CalcLog)
    //        : base(chord, branches, CalcLog)
    //    {

    //    }

    //    internal ISectionTube GetChordSection()
    //    {
    //        ISectionTube chord = Chord.Section as ISectionTube;
    //        if (chord == null)
    //        {
    //            throw new SectionWrongTypeException(typeof(ISectionTube));
    //        }
    //        return chord;
    //    }


    //    internal double GetChordWallThickness()
    //    {
    //        ISectionTube chordSec = GetChordSection();
    //        return chordSec.t_des;
    //    }


    //    internal ISectionTube GetBranchSection(HssTrussConnectionBranch branch)
    //    {
    //        ISectionTube br = branch.Section as ISectionTube;
    //        if (br == null)
    //        {
    //            throw new SectionWrongTypeException(typeof(ISectionTube));
    //        }
    //        return br;
    //    }

    //    internal List<ISectionTube> GetBranchSections()
    //    {
    //        List<ISectionTube> pipeSections = new List<ISectionTube>();
    //        foreach (var branch in Branches)
    //        {
    //            pipeSections.Add(GetBranchSection(branch));
    //        }
    //        return pipeSections;
    //    }

    //    protected HssTrussConnectionBranch GetBranch(string Id)
    //    {
    //        foreach (var b in Branches)
    //        {
    //            if (b.ID == Id)
    //            {
    //                return b;
    //            }

    //        }
    //        return null;
    //    }

    //    internal bool DetermineIfChordIsInTension()
    //    {
    //        //TODO
    //        return true;
    //    }
    //} 
    #endregion
}

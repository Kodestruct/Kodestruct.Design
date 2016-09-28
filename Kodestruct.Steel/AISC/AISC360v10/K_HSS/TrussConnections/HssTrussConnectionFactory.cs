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
using System.Threading.Tasks;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

namespace Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public class HssTrussConnectionFactory
    {
        public IHssTrussBranchConnection GetConnection(HssTrussConnectionMemberType MemberType,
            HssTrussConnectionClassification Classification, ISectionHollow ChordSection, ISectionHollow MainBranchSection,
            ISectionHollow SecondaryBranchSection, double F_yChord, double F_yBranch,
            double thetaMainBranch, double thetaSecondaryBranch, AxialForceType ForceTypeMainBranch, 
            AxialForceType ForceTypeSecondaryBranch, bool IsTensionChord,
            double P_uChord, double M_uChord,
            double O_v=0)
        {
            if (MemberType == HssTrussConnectionMemberType.Rhs)
            {
                if (ChordSection is ISectionTube && MainBranchSection is ISectionTube && SecondaryBranchSection is ISectionTube)
                {

                    SteelMaterial matChord = new SteelMaterial(F_yChord);
                    SteelRhsSection Chord = new SteelRhsSection(ChordSection as ISectionTube, matChord);


                    SteelMaterial matBr = new SteelMaterial(F_yBranch);
                    SteelRhsSection MainBranch = new SteelRhsSection(MainBranchSection as ISectionTube, matBr);

                    SteelRhsSection SecondaryBranch = new SteelRhsSection(SecondaryBranchSection as ISectionTube, matBr);


                    switch (Classification)
                    {
                        case HssTrussConnectionClassification.T:
                            return new RhsTrussTConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            break;
                        case HssTrussConnectionClassification.Y:
                            return new RhsTrussYConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            
                            break;
                        case HssTrussConnectionClassification.X:
                            return new RhsTrussXConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            break;
                        case HssTrussConnectionClassification.GappedK:
                            return new RhsTrussGappedKConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch,
                                ForceTypeMainBranch, ForceTypeSecondaryBranch, IsTensionChord, P_uChord,M_uChord);
                            break;
                        case HssTrussConnectionClassification.OverlappedK:
                            return new RhsTrussOverlappedConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch,
                                ForceTypeMainBranch, ForceTypeSecondaryBranch, IsTensionChord, P_uChord, M_uChord,O_v);
                            break;
                        default:
                            throw new Exception("Connection classification not recognized.");
                            break;
                    }
                }
                else
                {
                    throw new Exception("One of the member section is not of type ISectionTube. Ensure that a rectangular hollow section object type is used for chord and branches.");
                }

            }
            else
            {
                throw new NotImplementedException("Circular HSS truss connections are not supported yet.");
            }
        }
    }




}

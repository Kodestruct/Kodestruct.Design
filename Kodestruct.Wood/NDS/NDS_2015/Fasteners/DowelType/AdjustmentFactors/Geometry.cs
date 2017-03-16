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
 
using Kodestruct.Wood.NDS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners

{
public partial class DowelFastenerBase : WoodFastener
{

    public double GetGeometryFactor(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType, double L_end,
        double L_spacing,
        double l_m, double l_s, bool IsLoadedEdge, bool IsSoftwood, bool IsLoadedAtAngle)
    {
        double C_Delta = 0;

        this.l_m          =l_m         ;
        this.l_s          =l_s         ;
        this.IsLoadedEdge =IsLoadedEdge;

        throw new NotImplementedException();
        if (this.D<=0.25)
        {
            C_Delta = 1.0;
        }
        else
        {
            //End distance C_Delta
            double C_DeltaEnd = GetEndDistanceC_Delta(LoadToGrainDirection, FastenerEdgeBearingType, L_end, IsSoftwood, IsLoadedAtAngle);
            double C_DeltaSpacing = GetSpacingC_Delta(LoadToGrainDirection, L_spacing);
            //Add edge disstance here
        }

        return C_Delta;
    }

        double l_m;
        double l_s;
        bool IsLoadedEdge;

    private double GetEndDistanceC_Delta(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType,
        double L_end, bool IsSoftwood, bool IsLoadedAtAngle)
    {
        
        if (IsLoadedAtAngle == true)
        {
            throw new Exception("Loading at angle to the fastener is not supported.");
        }
        double L_endC_delta05 = GetMinimumEndDistance(LoadToGrainDirection, FastenerEdgeBearingType);
        if (L_end < L_endC_delta05)
        {
            throw new Exception("End distance is smaller than minimum permitted. Revise design.");
        }
        double L_endC_delta1 = GetMinimumEndDistanceForMaximumStrength(LoadToGrainDirection, FastenerEdgeBearingType, IsSoftwood);
        double C_Delta = L_end / (L_endC_delta1);
        return C_Delta;

    }

    private double GetSpacingC_Delta(LoadToGrainDirection LoadToGrainDirection, double L_spacing)
    {

        double L_spaceC_delta05 = GetMinimumSpacingOfFastenersWithinRow(LoadToGrainDirection);
        if (L_spacing < L_spaceC_delta05)
        {
            throw new Exception("End distance is smaller than minimum permitted. Revise design.");
        }
        double L_spaceC_delta1 = GetMinimumSpacingOfFastenersWithinRowForMaximumStrength(LoadToGrainDirection);

        double C_Delta = L_spacing / (L_spaceC_delta1);
        return C_Delta;

    }

        //Table 12.5.1B Spacing Requirements for fasteners in a Row 
        public double GetMinimumSpacingOfFastenersWithinRow(LoadToGrainDirection LoadToGrainDirection)
        {
            return 3.0 * D;
        }

        //Table 12.5.1B Spacing Requirements for fasteners in a Row 
        public double GetMinimumSpacingOfFastenersWithinRowForMaximumStrength(LoadToGrainDirection LoadToGrainDirection)
        {
            return 4.0 * D;
            //For Perpendicular to Grain  load => Required spacing for attached members 
        }


        //Table 12.5.1A End Distance Requirements
        public double GetMinimumEndDistance(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType, bool IsSoftwood=true)
        {
            switch (LoadToGrainDirection)
            {
                case LoadToGrainDirection.ParallelToGrain:
                    if (FastenerEdgeBearingType == Entities.FastenerEdgeBearingType.CompressionBearingAwayFromEdge)
                    {
                        return 2.0 * D;
                    }
                    else
                    {
                        if (IsSoftwood)
                        {
                            return 3.5 * D;
                        }
                        else
                        {
                            return 2.5 * D;
                        }
                    }
                    break;
                case LoadToGrainDirection.PerpendicularToGrain:
                    return 2.0 * D;
                    break;
                default:
                    throw new Exception("LoadToGrainDirection not recognized");
                    break;
            }

        }

        //Table 12.5.1A End Distance Requirements
        public double GetMinimumEndDistanceForMaximumStrength(LoadToGrainDirection LoadToGrainDirection, FastenerEdgeBearingType FastenerEdgeBearingType, bool IsSoftwood)
        {

            switch (LoadToGrainDirection)
            {
                case LoadToGrainDirection.ParallelToGrain:
                    if (FastenerEdgeBearingType == Entities.FastenerEdgeBearingType.CompressionBearingAwayFromEdge)
                    {
                        return 4.0 * D;
                    }
                    else
                    {
                        if (IsSoftwood)
                        {
                            return 7.0 * D;
                        }
                        else
                        {
                            return 5.0 * D;
                        }
                    }
                    break;
                case LoadToGrainDirection.PerpendicularToGrain:
                    return 4.0 * D;
                    break;
                default:
                    throw new Exception("LoadToGrainDirection not recognized");
                    break;
            }
        }

        public double GetMinimumEdgeDistance(LoadToGrainDirection LoadToGrainDirection)
        {
            throw new NotImplementedException();
        }
        
    }

}

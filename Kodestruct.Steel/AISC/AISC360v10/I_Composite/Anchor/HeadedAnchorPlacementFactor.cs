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
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.Composite
{
    public partial class HeadedAnchor : AnalyticalElement
    {

      public double GetPlacementFactorR_p(DeckAtBeamCondition HeadedAnchorDeckCondition,HeadedAnchorWeldCase HeadedAnchorWeldCase,double e_mid_ht)
        {
            double R_p;

            if (HeadedAnchorWeldCase == AISC.HeadedAnchorWeldCase.WeldedDirectly)
            {
                //(1a) steel headed stud anchors welded directly to the steel shape;
                R_p = 0.75;
            }
            else
            {
                if (HeadedAnchorDeckCondition == AISC.DeckAtBeamCondition.Parallel)
                {
                    //(1c) steel headed stud anchors welded through steel deck, or steel sheet
                    //used as girder filler material, and embedded in a composite slab with
                    //the deck oriented parallel to the beam
                    R_p = 0.75;
                }
                else if (HeadedAnchorDeckCondition == AISC.DeckAtBeamCondition.Perpendicular)
                {
                    if (e_mid_ht>=2)
                    {
                            //(1b) steel headed stud anchors welded in a composite slab with the deck
                            //oriented perpendicular to the beam and emid-ht = 2 in.; 
                        R_p = 0.75;
                    }
                    else
                    {
                        //(3) for steel headed stud anchors welded in a composite slab with deck
                        //oriented perpendicular to the beam and emid-ht < 2 in
                        R_p = 0.6;
                    }
                }
                else //No decking
                {
                    R_p = 0.75;
                }
            }
            return R_p;
        }
    }
}

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
      public double GetGroupFactorR_g(DeckAtBeamCondition HeadedAnchorDeckCondition,HeadedAnchorWeldCase HeadedAnchorWeldCase, double N_saRib,double h_r,double w_r)
        {
            double R_g;

            if (HeadedAnchorWeldCase == AISC.HeadedAnchorWeldCase.WeldedDirectly)
            {
                //(1b) any number of steel headed stud anchors welded in a row directly to the steel shape
                R_g = 1.0;
            }
            else
            {
                double w_rTo_h_r = w_r / h_r;
                if (HeadedAnchorDeckCondition == AISC.DeckAtBeamCondition.Parallel)
                {
                    if (w_r / h_r>=1.5)
                    {
                        //(1c)
                        // any number of steel headed stud anchors welded in a row through
                        // steel deck with the deck oriented parallel to the steel shape and the
                        // ratio of the average rib width to rib depth = 1.5
                        R_g = 1.0;
                    }
                    else
                    {
                        if (N_saRib == 1)
                        {
                            //(2b) one steel headed stud anchor welded through steel deck with the deck
                            //oriented parallel to the steel shape and the ratio of the average rib
                            //width to rib depth < 1.5
                            R_g = 0.85;
                        }
                        else
                        {
                            R_g = 0.7; // this value is assumed as the spec does not explcitly cover the case
                        }
                    }
                }
                else if (HeadedAnchorDeckCondition == AISC.DeckAtBeamCondition.Perpendicular)
	            {
                    if (N_saRib ==1)
                    {
                        //(1a) one steel headed stud anchor welded in a steel deck rib with the deck
                        //oriented perpendicular to the steel shape;
                        R_g = 1.0;
                    }
                    else if (N_saRib ==2)
                    {
                        //(2a) two steel headed stud anchors welded in a steel deck rib with the deck
                        //oriented perpendicular to the steel shape;
                        R_g = 0.85;
                    }
                    else
                    {
                        //(3) for three or more steel headed stud anchors welded in a steel deck rib
                        //with the deck oriented perpendicular to the steel shape
                        R_g = 0.7;
                    }
	            }

                else //No deck
                {
                    R_g = 1.0;
                }
            }
            return R_g;
        }
    }
}

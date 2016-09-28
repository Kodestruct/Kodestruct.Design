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

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads.Components
{
    public class ComponentDeckWithNWFill1_5 : DeckWithConcreteNormalBase
    {
        public ComponentDeckWithNWFill1_5(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {
            if (Option2 == 0)
            {
                base.ProfileType = DeckProfileType.p1_5x6;
            }
            else
            {
                base.ProfileType = DeckProfileType.p1_5x6INV;
            }
        }



        protected override double GetTotalGepth()
        {
            switch (Option1)
            {
                case 0: return 3.5;
                case 1: return 4;
                case 2: return 4.5;
                case 3: return 5;
                case 4: return 5.5;
                case 5: return 6;
                default: return 4.5;
            }
        }
    }
}

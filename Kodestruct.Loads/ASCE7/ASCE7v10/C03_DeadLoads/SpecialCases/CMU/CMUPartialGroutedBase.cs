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
    public abstract class CMUPartialGroutedBase : CMUBase
    {
        public CMUPartialGroutedBase(int Option1, int Option2, double NumericValue)
            : base(Option1, Option2, NumericValue)
        {

        }

        protected abstract double GetDensity();

        protected override void SetValues()
        {
            Density = GetDensity();
            GroutingType = BlockGroutingType.PartialGrout;
            switch (Option1)
            {
                case 0:
                    UnitThickness = 6;
                    break;
                case 1:
                    UnitThickness = 8;
                    break;
                case 2:
                    UnitThickness = 10;
                    break;
                case 3:
                    UnitThickness = 12;
                    break;
            }

            switch (Option2)
            {
                case 0:
                    GroutSpacing = 48;
                    break;
                case 1:
                    GroutSpacing = 40;
                    break;
                case 2:
                    GroutSpacing = 32;
                    break;
                case 3:
                    GroutSpacing = 24;
                    break;
                case 4:
                    GroutSpacing = 16;
                    break;
            }
        }
    }
}

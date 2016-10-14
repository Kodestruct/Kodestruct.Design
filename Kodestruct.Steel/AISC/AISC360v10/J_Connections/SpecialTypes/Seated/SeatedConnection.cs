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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public partial class SeatedConnection : AnalyticalElement
    {
        double a_seat;
        double b_seat;
        double theta;
        double F_y;
        double E;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="a_seat">Vertical seat dimension</param>
        /// <param name="b_seat">Horizontal seat dimension</param>
        public SeatedConnection(double a_seat, double b_seat, double F_y, double E)
        {
            this.a_seat=a_seat;
            this.b_seat = b_seat;
            this.F_y = F_y;
            this.E = E;
            theta = Math.Atan(b_seat/a_seat );
        }
    }
}

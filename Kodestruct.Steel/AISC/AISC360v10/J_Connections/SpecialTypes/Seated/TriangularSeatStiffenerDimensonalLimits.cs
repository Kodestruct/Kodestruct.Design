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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public partial class SeatedConnection : AnalyticalElement
    {
        public double TriangularSeatStiffenerPlateThicknessToPrecludeBuckling(double a_seat, double b_seat, double F_y, double E)
        {
            //From Salmon Johnson Malhas. 2009 edition. page 706
            double t_p;
            if ((b_seat / a_seat)>=0.5 && (b_seat / a_seat)<= 1.0)
            {
                t_p= b_seat/(1.47*Math.Sqrt(((E) / (F_y)))); //Equation 13.5.4a

            }
            else if ((b_seat / a_seat)>=1.0 && (b_seat / a_seat)<= 2.0)
            {
                t_p = b_seat / (1.47 * (((b_seat) / (a_seat))) * Math.Sqrt(((E) / (F_y)))); //Equation 13.5.4b
            }
            else
	        {
                throw new Exception("The ratio of seat triangular stiffener dimensions a and b are outside of the range. Select 0.5 < a/b <2");
	        }

            return t_p;
        }

    }
}

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
using Kodestruct.Common.Interfaces;

namespace Kodestruct.Common.Entities
{
    public class Force: IForce
    {
        public string Location { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public double F3 { get; set; }
        public double M1 { get; set; }
        public double M2 { get; set; }
        public double M3 { get; set; }

        public Force()
        {

        }
        public Force(string Location, double F1, double F2, double F3, double M1, double M2, double M3)
        {
            this.Location = Location;
            this.F1 = F1;
            this.F2 = F2;
            this.F3 = F3;
            this.M1 = M1;
            this.M2 = M2;
            this.M3 = M3;

        }
        public Force(double F1, double F2, double F3, double M1, double M2, double M3)
            :this(null,F1,F2,F3,M1,M2,M3)
        {

        }
    }
}

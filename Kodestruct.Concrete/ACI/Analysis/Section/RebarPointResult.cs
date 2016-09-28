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


namespace Kodestruct.Concrete.ACI
{
    public class RebarPointResult 
    {
        public double Stress { get; set; }
        public double Strain { get; set; }
        public double Force { get; set; }
        public double DistanceToNeutralAxis { get; set; }
        public RebarPoint Point { get; set; }

        public RebarPointResult(double Stress, double Strain, double Force, double DistanceToNa, RebarPoint Point)
            
        {
            this.Stress = Stress;
            this.Strain = Strain;
            this.Force = Force;
            this.DistanceToNeutralAxis = DistanceToNa;
            this.Point = Point;
        }
    }
}

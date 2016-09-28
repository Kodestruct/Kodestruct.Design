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

namespace Kodestruct.Analysis
{
    public class BeamFactoryData
    {
        public BeamFactoryData (double L, double P,double M,double w,
           double a_load, double b_load, double c_load, double P1, double P2, 
           double M1, double M2, double E=0, double I=0)
       {
                this.L      =L;
                this.P      =P;
                this.M      =M;
                this.w      =w;
                this.a_load =a_load;
                this.b_load =b_load;
                this.c_load =c_load;
                this.P1     =P1;
                this.P2     =P2;
                this.M1     =M1;
                this.M2     =M2;
                this.E = E;
                this.I = I;
       }
        public double L { get; set; }
        public double P { get; set; }
        public double M { get; set; }
        public double w { get; set; }
        public double a_load { get; set; }
        public double b_load { get; set; }
        public double c_load { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
        public double M1 { get; set; }
        public double M2 { get; set; }

        public double E { get; set; }
        public double I { get; set; }
            
    }
}

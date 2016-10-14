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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.CalculationLogger;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{

    //Cb equation in section F1 (3) applies only to  singly symmetric members in single curvature 
    //and all doubly symmetric members:

    //this base class is used for Cb calculation

    public partial class GeneralFlexuralMember: AnalyticalElement
    {
        public GeneralFlexuralMember(ICalcLog Log)
            : base(Log)
        {

        }

        public double GetCb( double Mmax, double MA, double MB, double MC)

        {
            CbData data = new CbData() { M_A = MA, M_B = MB, M_C = MC, Mmax = Mmax };
            return GetCb(data);
        }

        internal double GetCb(CbData d)
        {
            //Cb, the lateral-torsional buckling modification factor for nonuniform moment
            //diagrams when both ends of the segment are braced.

            //  Mmax = absolute value of maximum moment in the unbraced segment, kip-in.(N-mm)
            //  MA = absolute value of moment at quarter point of the unbraced segment, kip-in. (N-mm)
            //  MB = absolute value of moment at centerline of the unbraced segment, kipin.(N-mm)
            //  MC = absolute value of moment at three-quarter point of the unbraced segment kip-in. (N-mm)
            //  For cantilevers or overhangs where the free end is unbraced, Cb = 1.0.


            double Mmax =d.Mmax; 
            double M_A=d.M_A;
            double M_B=d.M_B;
            double M_C = d.M_C;

            double Cb = 1.0;
            Cb = 12.5 * Mmax / (2.5 * Mmax + 3.0 * M_A + 4.0 * M_B + 3.0 * M_C); //(F1-1)


            return Cb;

        }


     public class CbData
    {
        public double Mmax { get; set; }
        public double M_A { get; set; }
        public double M_B { get; set; }
        public double M_C { get; set; }
    }
    }


}

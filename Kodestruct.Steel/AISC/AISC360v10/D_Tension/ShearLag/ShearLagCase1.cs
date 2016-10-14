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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10 
{
    // All tension members where the tension 
    // load is transmitted directly to each of the
    // cross-sectional elements by fasteners or
    // welds (except as in Cases 4, 5 and 6)

    public class ShearLagCase1 : ShearLagFactorBase
    {

        public ShearLagCase1()
            : this(new CalcLog())
        {

        }
        public ShearLagCase1(ICalcLog Log): base(Log)
        {

        }
        /// <summary>
        ///Calculates shear lag factor per AISC Table D3.1 "Shear Lag Factors for Connections  to Tension Members".
        /// </summary>
        public override double GetShearLagFactor()
        {
            return 1.0;
        }
    }
}

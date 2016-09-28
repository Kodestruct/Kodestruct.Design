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
using Kodestruct.Analysis.BeamForces.SimpleWithOverhang;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Analysis.BeamForces
{
    public class BeamSimpleWithOverhang: Beam
    {
        double Mx, Vx;
        ForceDataPoint Mmax, Mmin, Vmax;

        public double OverhangLength { get; set; }
        public double TotalLength { get; set; }

        public BeamSimpleWithOverhang(double Length, double OverhangLength, LoadBeam load, ICalcLog CalcLog)
            : base(Length, load, CalcLog, new BeamSimpleWithOverhangFactory())
        {

            this.OverhangLength = OverhangLength;
            TotalLength = Length + OverhangLength;
        }

        public override void EvaluateX(double X)
        {
            if (X < 0 || X > this.Length + OverhangLength)
            {
                throw new StationOutOfBoundsException(this.Length, X);
            }
        }
    }
}

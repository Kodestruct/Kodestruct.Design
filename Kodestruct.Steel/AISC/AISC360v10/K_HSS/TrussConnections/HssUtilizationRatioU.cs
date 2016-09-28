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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Section.Interfaces;

using Kodestruct.Steel.AISC.Interfaces;
 
 

namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public partial class ChsTrussConnection : HssTrussConnection
    {

        public double GetUtilizationRatio(ISteelSection Section, double RequiredAxialStrenghPro, double RequiredMomentStrengthMro)
        {
            double U = 0;
            double Fy = Section.Material.YieldStress;
            double Fc = 0.0;

            Fc = Fy;


            ISection sec = Section.Shape;
            double Ag = sec.A;
            double S = Math.Min(sec.S_xBot, sec.S_xTop);
            double Pro = RequiredAxialStrenghPro;
            double Mro = RequiredMomentStrengthMro;
            //(K1-6) from TABLE K1.2
            U = Math.Abs(Pro / (Fc * Ag) + Mro / (Fc * S));
            return U;
        }

        public double GetUtilizationRatio(HssKConnectionLoadCaseData LoadCase)
        {
            double P = 0.0;
            double M = 0.0;
            double U = GetUtilizationRatio(LoadCase,out P, out M);

            return U;
        }

        public double GetUtilizationRatio(HssKConnectionLoadCaseData LoadCase, out double RequiredAxialStrenghPro, 
            out double RequiredMomentStrengthMro)
        {
            ISteelSection chordSec = Chord as ISteelSection;
            double Umax = 0.0;
            double Fx = 0.0;
            double M = 0.0;

            if (chordSec!=null)
            {
                double D = GetChordDiameter();
                var forces = Chord.Forces.Where(f => f.LoadCaseName == LoadCase.LoadCaseName).ToList();
                foreach (var f in forces)
                {
                    Fx = Math.Abs(f.Fx);
                    M = Math.Sqrt(Math.Pow(f.My,2)+Math.Pow(f.Mz,2));
                    double U = GetUtilizationRatio(chordSec, Fx, M);
                } 
            }
            else
            {
                throw new Exception("Chord Member must implement ISteelSection interface");
            }

            RequiredAxialStrenghPro = Fx;
            RequiredMomentStrengthMro = M;
            return Umax;
        }
    }
}

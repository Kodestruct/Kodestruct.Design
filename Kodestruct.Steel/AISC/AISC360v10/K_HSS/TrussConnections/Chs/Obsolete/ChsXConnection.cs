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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public class ChsXConnection : ChsNonOverlapConnection
    {
        public ChsXConnection(HssTrussConnectionChord chord, List<HssTrussConnectionBranch> branches,  ICalcLog CalcLog)
            :base(chord, branches, CalcLog)
        {

        }

        public void CalculateAvailableStrengthForBranches()
        {
            foreach (var force in Chord.Forces)
            {
                double Mro = Math.Sqrt(Math.Pow(force.Mx, 2) + Math.Pow(force.Mx, 2));
                double Pro = force.Fx;
                double D = GetChordDiameter();
                bool ChordIsInTension = DetermineIfChordIsInTension();
                double Qf = GetChordStressInteractionFactorQf(ChordIsInTension,Pro,Mro);
                foreach (var b in Branches)
                {
                    double BranchYieldingLs =  GetBranchShearYielding(b);
                    double ChordPlastificationLs = CheckChordPlastification(b,D,Qf);
                    double capacity = Math.Min(Math.Abs(BranchYieldingLs), Math.Abs(ChordPlastificationLs));
                    b.AddStrengthValue(capacity, force.LoadCaseName);
                }
            }
        }

        internal  double CheckChordPlastification(HssTrussConnectionBranch branch,double D, double Qf)
        {
            double P = 0;
            double Pn;
            double theta = branch.Angle;
            double sinTheta = Math.Sin(theta.ToRadians());
            ISectionPipe section = GetBranchSection(branch);
            double Fy = branch.Section.Material.YieldStress;
            double t = section.t_des;
            double Db = section.D;
            double beta = Db / D;
               //(K2-3)
           Pn = Fy*Math.Pow(t,2)*(5.7/(1.0-0.81*beta)*Qf)/sinTheta;

                P = 0.9 * Pn;

            return P;
        }
    }
}

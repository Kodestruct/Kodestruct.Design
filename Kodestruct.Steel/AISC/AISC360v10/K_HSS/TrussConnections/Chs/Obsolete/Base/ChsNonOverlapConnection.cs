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
using Kodestruct.Common.Section.Interfaces;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public class ChsNonOverlapConnection: ChsTrussConnection
    {
        public ChsNonOverlapConnection(HssTrussConnectionChord chord, List<HssTrussConnectionBranch> branches, ICalcLog CalcLog)
            : base(chord, branches, CalcLog)
 
        {

        }



        internal virtual double GetBranchShearYielding(HssTrussConnectionBranch branch)
        {
            double P = 0;
            double Pn;
            double theta = branch.Angle;
            double sinTheta = Math.Sin(theta.ToRadians());
            double pi = Math.PI;
            ISectionPipe section = GetBranchSection(branch);
            double Fy = branch.Section.Material.YieldStress;
            double t = section.t_des;
            double Db = section.D;

            if (Db<(Db-2.0*t))
            {
                //(K2-1)
                Pn = 0.6 * Fy * t * pi * Db * (1.0 + sinTheta / (2.0 * Math.Pow(sinTheta, 2)));

                P = Pn * 0.95;

            }
            else
            {
                P = double.PositiveInfinity;
            }

            return P;

        }
    }
}

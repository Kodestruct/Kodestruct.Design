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
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public  partial class RhsTrussXConnection : RhsTYXTrussBranchConnection
    {


        public RhsTrussXConnection(SteelRhsSection Chord, SteelRhsSection MainBranch, double thetaMain,
            SteelRhsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeMain, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord)
            : base(Chord, MainBranch, thetaMain, SecondBranch, thetaSecond, ForceTypeMain, ForceTypeSecond, IsTensionChord,
            P_uChord, M_uChord)
        {
            if (ForceTypeMain == AxialForceType.Tension && ForceTypeSecond != AxialForceType.Tension )
            {
                throw new Exception("Specify the same type of force for both branches or switch to K connection");
            }
            if (ForceTypeSecond == AxialForceType.Tension && ForceTypeMain != AxialForceType.Tension)
            {
                throw new Exception("Specify the same type of force for both branches or switch to K connection");
            }

        }

        /// <summary>
        /// K2-11
        /// </summary>
        /// <returns></returns>
        public override SteelLimitStateValue GetChordSidewallLocalCripplingStrength()
        {

                double P_n = 0.0;
                double phi = 0.9;

                if (beta == 1.0)
                {
                    if (ForceTypeMain == AxialForceType.Compression || ForceTypeMain == AxialForceType.Reversible)
                    {
                        //Note: per AISC DG24 example 8.3 Page 109
                        //if both chords are in compression for CROSS connection
                        //P_n isdividedby 2 (for 2 branches)

                        double NPlanes = 1;
                        if (ForceTypeMain == AxialForceType.Compression || ForceTypeMain == AxialForceType.Reversible)
                        {
                            if (ForceTypeSecond == AxialForceType.Compression || ForceTypeSecond == AxialForceType.Reversible)
                            {
                                NPlanes = 2;
                            }
                        }

                        P_n = (((((48.0 * Math.Pow(t, 3)) / (H - 3.0 * t))) * Math.Sqrt(E * F_y) * Q_f) / (sin_theta));
                        P_n = P_n / NPlanes; //to get "per branch" force
                        double phiP_n = phi * P_n;
                        return new SteelLimitStateValue(phiP_n, true);
                    }
                    else
                    {
                        return new SteelLimitStateValue(-1, false);
                    }
                }
                else
                {
                    return new SteelLimitStateValue(-1, false);
                }
        }
    }

}

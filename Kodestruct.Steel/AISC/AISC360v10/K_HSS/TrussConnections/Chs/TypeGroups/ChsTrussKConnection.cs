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
using Kodestruct.Common.Mathematics;
using Kodestruct.Steel.AISC.Steel.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public abstract partial class ChsTrussKConnection : ChsTrussBranchConnection
    {
        /// <summary>
        /// K-connection base class
        /// NOTE THAT IT IS CRITICAL TO SPECIFY MAIN BRANCH AS COMPRESSION BRANCH
        /// </summary>
        /// <param name="Chord">Chord object instance</param>
        /// <param name="MainBranch">Branch object instance</param>
        /// <param name="thetaMain">Main branch angle</param>
        /// <param name="ForceTypeMain">Main branch force type</param>
        /// <param name="SecondBranch">Branch object instance</param>
        /// <param name="thetaSecond">Secondary branch angle</param>
        /// <param name="ForceTypeSecond">Secondary branch force type</param>
        /// <param name="IsTensionChord">Identifies if branch is in tension</param>
        /// <param name="P_uChord">Design axial force in chord</param>
        /// <param name="M_uChord">Design moment in chord</param>
        /// <param name="g">Gap</param>
        public ChsTrussKConnection(SteelChsSection Chord, SteelChsSection MainBranch, double thetaMain, AxialForceType ForceTypeMain, 
            SteelChsSection SecondBranch, double thetaSecond, AxialForceType ForceTypeSecond, bool IsTensionChord,
            double P_uChord, double M_uChord, double g): base( Chord,  IsTensionChord,
             P_uChord,  M_uChord)
       {
           this.g = g;
           
            #region Assign chord sections based on forces
           if (ForceTypeMain == AxialForceType.Compression && ForceTypeSecond == AxialForceType.Compression)
           {
               throw new Exception("Branch force types need to be specified to have opposite signs for K conncection");
           }
           if (ForceTypeMain == AxialForceType.Tension && ForceTypeSecond == AxialForceType.Tension)
           {
               throw new Exception("Branch force types need to be specified to have opposite signs for K conncection");
           }
           //if (ForceTypeMain == AxialForceType.Reversible && ForceTypeSecond == AxialForceType.Reversible)
           //{
           //    this.MainBranch = MainBranch;
           //    this.thetaMain = thetaMain;
           //    this.ForceTypeMain = ForceTypeMain;
           //    this.SecondBranch = SecondBranch;
           //    this.thetaSecond = thetaSecond;
           //    this.ForceTypeSecond = ForceTypeSecond;
           //}
           //if (ForceTypeMain == AxialForceType.Compression && ForceTypeSecond == AxialForceType.Reversible)
           //{
               this.MainBranch = MainBranch;
               this.thetaMain = thetaMain;
               this.ForceTypeMain = ForceTypeMain;
               this.SecondBranch = SecondBranch;
               this.thetaSecond = thetaSecond;
               this.ForceTypeSecond = ForceTypeSecond;
           //}
           //if (ForceTypeMain == AxialForceType.Tension && ForceTypeSecond == AxialForceType.Reversible)
           //{

           //    this.MainBranch = SecondBranch;
           //    this.thetaMain = thetaSecond;
           //    this.ForceTypeMain = ForceTypeSecond;
           //    this.SecondBranch = MainBranch;
           //    this.thetaSecond = thetaMain;
           //    this.ForceTypeSecond = ForceTypeMain;
           //} 
           #endregion

       }

       /// <summary>
       /// Gap
       /// </summary>
            public double g { get; set; }

            private double _Q_g;


            public double Q_g
            {
                get
                {
                    _Q_g = GetQ_g();
                    return _Q_g;
                }
                set { _Q_g = value; }
            }


            protected double GetQ_g()
            {
                double Q_g = Math.Pow(gamma, 0.2) * (1 + ((0.024 * Math.Pow(gamma, 1.2)) / (Math.Exp(((0.5 * g) / (t)) - 1.33) + 1.0)));
                return Q_g;
            }



        /// <summary>
        /// K2-4 and K2-5
        /// </summary>
        /// <returns></returns>
            public override SteelLimitStateValue GetChordWallPlastificationStrength(bool IsMainBranch)
        {

            double P_n = 0.0;
            double phi = 0.90;
            this.IsMainBranch = IsMainBranch;

            double P_nComp = GetP_nComp();
            double P_nTens = GetP_nTens(P_nComp);

            if (IsMainBranch == true)
            {
                P_n = P_nComp;
            }
            else
            {
                P_n = P_nTens;
            }

            double phiP_n = phi * P_n;
            return new SteelLimitStateValue(phiP_n, true);
        }

        /// <summary>
        /// K2-4
        /// </summary>
        /// <returns></returns>
        double GetP_nComp()
            {
                double P_n = 0.0;
                P_n=((F_y*Math.Pow(t, 2)*(2.0+11.33*((D_bComp) / (D)))*Q_g*Q_f) / (sin_theta));

                return P_n;
            }

        /// <summary>
        ///  K2-5
        /// </summary>
        /// <param name="P_nComp"></param>
        /// <returns></returns>
        double GetP_nTens(double P_nComp)
        {
            double P_n = 0.0;
            P_n = P_nComp * Math.Sin(thetaMain.ToRadians()) / Math.Sin(thetaSecond.ToRadians());
            return P_n;
        }



    }
}

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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Data;
using Kodestruct.Steel.AISC.Entities.Enums.FloorVibrations;
using Kodestruct.Steel.Properties;

namespace Kodestruct.Steel.AISC.Entities.FloorVibrations
{
    public partial class FloorVibrationBeamGirderPanel
    {

        public double GetEffectiveJoistWidth(double fc_prime, double w_c, double h_solid, double h_rib,double w_r,double s_r,
            DeckAtBeamCondition DeckAtBeamCondition,double L_j, double I_j,double S_j, double L_floor, BeamFloorLocationType  BeamLocation)
        {
            double D_e = this.Get_d_e(h_solid, h_rib, w_r, s_r);
            double n = Get_n(fc_prime, w_c);
            double D_j = GetDistributedJoistMomentOfInertia(I_j, S_j);
            double D_s = GetDistributedSlabMomentOfInertia(n, D_e);
            double B_j = GetEffectiveJoistWidth(D_s, D_j, L_j, BeamLocation, L_floor);
            return B_j;
        }

        private double Get_n(double fc_prime, double w_c)
        {

            double E_c = Math.Pow(w_c, 1.5) * Math.Sqrt(fc_prime);
            return 29000 / E_c;
        }

        /// <summary>
        /// Returns B_j: effecive panel width for joist mode
        /// </summary>
        /// <param name="D_s">Distributed slab moment of inertia</param>
        /// <param name="D_j">Distributed joist moment of inertia</param>
        /// <param name="beamType"> Beam type (inner or edge)</param>
        /// <param name="L_floor"> Length of floor for panel (use pebble rule to determine L_floor)</param>
        /// <returns></returns>
        public double GetEffectiveJoistWidth(double D_s, double D_j,double L_j,
            BeamFloorLocationType beamType, double L_floor)
        {

            double C_j = 2.0;

            if (beamType == BeamFloorLocationType.AtFreeEdge)
            {
                C_j = 1.0;
            }

            double B_j = C_j * Math.Pow((((D_s) / (D_j))), 1 / 4.0)*L_j;

            if (B_j > 2.0 / 3.0 * L_floor)
            {
                return 2.0 / 3.0 * L_floor;
            }
            else
            {
                return B_j;
            }

        }

        public double GetEffectiveGirderWidth(double L_g,double L_j,double I_g,double I_j,
            double S_j,double  L_floor,
            BeamFloorLocationType beamType,
            JoistToGirderConnectionType ConnectionType)
        {
            double D_g = GetDistributedGirderMomentOfInertia(I_g, L_j);
            double D_j = GetDistributedJoistMomentOfInertia(I_j, S_j);
            return GetEffectiveGirderWidth(L_floor, L_floor, L_j, D_j, D_g, beamType, ConnectionType);
        }
        public double GetEffectiveGirderWidth(double L_g, double L_floor, double L_j,
    double D_j, double D_g, BeamFloorLocationType beamType,
    JoistToGirderConnectionType ConnectionType)
        {
            double C_g = 1.8;
            if (ConnectionType == JoistToGirderConnectionType.PlacementAtTopFlange)
            {
                C_g = 1.8;
            }

            double B_g = C_g * Math.Pow((((D_j) / (D_g))), 1 / 4.0) * L_g;
            if (beamType == BeamFloorLocationType.Inner)
            {
                return Math.Min(B_g, 2.0 / 3.0 * L_floor);
            }
            else
            {
                return Math.Min(B_g, 2.0 / 3.0 * L_j);
            }
        }
    }
}

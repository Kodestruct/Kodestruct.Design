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

        public double GetJoistModeEffectiveWeight(double w_j, double S_j, double B_j, double L_j,
            AdjacentSpanWeightIncreaseType AdjacentSpanWeightIncreaseType = AdjacentSpanWeightIncreaseType.None)
        {

            //Joist mode weight
            double W_j = ((w_j) / (S_j)) * B_j * L_j;

            double JoistWeightIncrease = 1.0;
            switch (AdjacentSpanWeightIncreaseType)
            {
                case AdjacentSpanWeightIncreaseType.HotRolledBeamOverTheColumn:
                    JoistWeightIncrease = 1.5;
                    break;
                case AdjacentSpanWeightIncreaseType.JoistWithExtendedBottomChord:
                    JoistWeightIncrease = 1.3;
                    break;
                case AdjacentSpanWeightIncreaseType.None:
                    JoistWeightIncrease = 1.0;
                    break;
            }

            W_j = W_j * JoistWeightIncrease;
            return W_j;
        }

        public double GetGirderModeEffectiveWeight(double w_g, double L_g, double B_g, double L_jAverage)
        {

            double W_g = (((w_g) / (L_jAverage))) * B_g * L_g;
            return W_g;
        }

        public double GetCombinedModeEffectiveWeight(double Delta_j, double Delta_g, double W_j, double W_g)
        {
            //double B_j = GetEffectiveJoistWidth_B_j(D_s, D_j, JoistLocationType, L_floor);
            //double B_g = GetEffectiveGirderWidth_B_g(L_g, B_floor, L_j, D_j, D_g, GirderLocationType, ConnectionType);


            double W = ((Delta_j) / (Delta_j + Delta_g)) * W_j + ((Delta_g) / (Delta_j + Delta_g)) * W_g;
            return W;
        }
    }

}

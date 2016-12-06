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
 
using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Wood.NDS.NDS_2015.Fasteners
{
    public partial class WoodFastener : AnalyticalElement
    {

/// <summary>
/// 
/// </summary>
/// <param name="ConnectorType"></param>
/// <param name="E_m"></param>
/// <param name="A_m"></param>
/// <param name="E_s"></param>
/// <param name="A_s"></param>
/// <param name="D"></param>
/// <returns></returns>
       public double GetGroupActionFactor(MechanicalConnectorTypeForGroupEffects ConnectorType, 
           double E_m, double A_m, double E_s, double A_s, double n, double s, double D=0.25)
       {
           bool IsDowelTypeFastener = false;

           if (ConnectorType == MechanicalConnectorTypeForGroupEffects.DowelTypeFastenerInWoodToMetalConnection ||
               ConnectorType == MechanicalConnectorTypeForGroupEffects.DowelTypeFastenerInWoodToWoodConnection)
           {
               IsDowelTypeFastener = true;
           }
           double C_g;
           if (IsDowelTypeFastener == true && D< 0.25)
           {
               C_g = 1.0;
           }
           else
           {
               double gamma = GetGamma(ConnectorType, D);
               double R_EA=Math.Min(((E_s*A_s) / (E_m*A_m)),((E_m*A_m) / (E_s*A_s)));

               double u = 1 + gamma * ((s) / (2.0)) * ((1.0)/ (E_m * A_m)) + (1.0 / (E_s * A_s));
               double m = u - Math.Sqrt(Math.Pow(u, 2) - 1.0);

               C_g = (((m * (1 - Math.Pow(m, 2))) / (n * ((1 + R_EA * Math.Pow(m, n)) * (1 + m) - 1 + Math.Pow(m, 2.0*n))))) * (1);
           }

           return C_g;
       }

       private double GetGamma(MechanicalConnectorTypeForGroupEffects ConnectorType, double D)
       {


           switch (ConnectorType)
           {
               case MechanicalConnectorTypeForGroupEffects.FourInchSplitRing:
                   return 500000.0;
                   break;
               case MechanicalConnectorTypeForGroupEffects.ShearPlateConnector:
                   return 500000.0;
                   break;
               case MechanicalConnectorTypeForGroupEffects.TwoAndHalfInchSplitRing:
                   return 400000.0;
                   break;
               case MechanicalConnectorTypeForGroupEffects.TwoAndFiveEighthShearPlateConnectors:
                   return 400000.0;
                   break;
               case MechanicalConnectorTypeForGroupEffects.DowelTypeFastenerInWoodToWoodConnection:
                   return 180000.0 * Math.Pow(D, 1.5);
                   break;
               case MechanicalConnectorTypeForGroupEffects.DowelTypeFastenerInWoodToMetalConnection:
                   return 180000.0 * Math.Pow(D, 1.5);
                   break;
               default:
                   throw new Exception("Unrecognized connector type.");
                   break;
           }
       }
    }
}

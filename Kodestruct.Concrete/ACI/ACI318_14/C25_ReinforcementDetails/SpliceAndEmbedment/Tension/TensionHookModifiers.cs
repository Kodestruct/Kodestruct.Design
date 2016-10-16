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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Reports; 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class StandardHookInTension : Development
    {
        
        private double GetSideCoverModifier(HookType hookType, double sideCover, double barExtensionCover)
        {
            //(a) For No. 11 bar and smaller hooks with side cover
            //(normal to plane of hook) not less than 2-1/2 in., and
            //for 90-degree hook with cover on bar extension
            //beyond hook not less than 2 in.
            
            double SideCoverModifier=1.0;

            if (hookType== HookType.Degree90)
            {
                if (sideCover!=0.0 && barExtensionCover!=0.0)
                {
                    if (db<=11/8)
                    {
                        if (sideCover>=2.5 && barExtensionCover>=2.0)
                        {
                            SideCoverModifier = SetSideCoverModifier();
                        }
                    }
                }
                
            }
            else
            {
                if (sideCover!=0.0)
                {
                    if (db <= 11 / 8)
                    {
                        if (sideCover>=2.5)
                        {
                            SideCoverModifier = SetSideCoverModifier();
                        }
                    }
                }
            }
            return SideCoverModifier;
        }


        private double GetConfinementModifier(HookType hookType, bool enclosingRebarIsPerpendicular, double enclosingRebarSpacing)
        {
            double confinementModifier=1.0;
            if (hookType== HookType.Degree90 && db<=11/8)
            {
                //      (b) For 90-degree hooks of No. 11 and smaller bars
                //      that are either enclosed within ties or stirrups
                //      perpendicular to the bar being developed, spaced
                //      not greater than 3db along ldh; or enclosed within
                //      ties or stirrups parallel to the bar being developed,
                //      spaced not greater than 3db along the length of the
                //      tail extension of the hook plus bend.................... 0.8
                if (enclosingRebarIsPerpendicular == true && enclosingRebarSpacing <= 3.0 * db)
                {
                    confinementModifier = SetConfinementModifier();
                } 
                if (enclosingRebarIsPerpendicular ==true && enclosingRebarSpacing<3.0*db)
                {
                    confinementModifier = SetConfinementModifier();
                }
            }
            if (hookType== HookType.Degree180 && db<=11/8)
            {
                if (enclosingRebarIsPerpendicular==true && enclosingRebarSpacing<=3*db)
                {
                    confinementModifier = SetConfinementModifier();
                }
            }
            return confinementModifier;
        }

                   
        private double SetSideCoverModifier()
        {
            double SideCoverModifier;
                SideCoverModifier = 0.7;

            return SideCoverModifier;
        }

                   
        private double SetConfinementModifier()
        {
            double ConfinementModifier;

                ConfinementModifier = 0.8;

            return ConfinementModifier;
        }
    }
}

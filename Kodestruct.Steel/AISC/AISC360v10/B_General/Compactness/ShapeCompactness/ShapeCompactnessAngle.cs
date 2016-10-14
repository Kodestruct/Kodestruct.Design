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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{

    public partial class ShapeCompactness
    {

        public class AngleMember : ShapeCompactnessBase
        {

            public AngleMember(ISectionAngle Section, ISteelMaterial Material, AngleOrientation AngleOrientation)
            {
                SetCompactness( Section, Material, AngleOrientation);

            }

            protected virtual void SetCompactness(ISectionAngle ang, ISteelMaterial Material, AngleOrientation AngleOrientation)
           {

  
                   double shortLeg;
                   double longLeg;

                   if (ang.d >= ang.b)
                   {
                       longLeg = ang.d;
                       shortLeg = ang.b;
                   }
                   else
                   {
                       longLeg = ang.b;
                       shortLeg = ang.d;
                   }

                   //make differentiation based on angle orientation

                   if (AngleOrientation == AngleOrientation.LongLegVertical)
                   {
                       FlangeCompactness = new LegOfSingleAngle(Material, shortLeg, ang.t);
                       WebCompactness = new LegOfSingleAngle(Material, longLeg, ang.t);
                   }
                   else
                   {
                       FlangeCompactness = new LegOfSingleAngle(Material, longLeg, ang.t);
                       WebCompactness = new LegOfSingleAngle(Material, shortLeg, ang.t);
                   }

               }


          }
        }

    
}

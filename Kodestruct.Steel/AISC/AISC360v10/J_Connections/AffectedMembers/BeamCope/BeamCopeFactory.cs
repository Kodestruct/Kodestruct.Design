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
using System.Threading.Tasks;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public class BeamCopeFactory
    {

        public IBeamCope GetCope(BeamCopeCase BeamCopeCase, double d, double b_f, double t_f, double t_w, double d_c, double c, double F_y, double F_u)
       {
           ISteelMaterial material = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
           ISectionI section = new SectionI(null, d, b_f, t_f, t_w);
           IBeamCope cope=null;
           switch (BeamCopeCase)
           {
               case BeamCopeCase.Uncoped:
                   cope = new BeamUncoped(section, material);
                   break;
               case BeamCopeCase.CopedTopFlange:
                   cope = new BeamCopeSingle(c, d_c, section, material);
                   break;
               case BeamCopeCase.CopedBothFlanges:
                   cope = new BeamCopeDouble(c, d_c, section, material);
                   break;
           }
           return cope;

       }

    }
}

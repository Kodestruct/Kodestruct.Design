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
using Kodestruct.Steel.AISC.Interfaces;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{

    public class WebOfChannel : WebOfDoublySymI
    {
        ISectionChannel SectionChannel;

        public WebOfChannel(ISteelMaterial Material, double Width, double Thickness)
            :base(Material,Width,Thickness)
        {
            
        }

        public WebOfChannel(ISteelMaterial Material, ISectionChannel SectionChannel)
            :base(Material)
        {
            this.SectionChannel = SectionChannel;
            ISectionChannel s = SectionChannel;
            this.Width = s.d - (2*s.k);
            if (this.Width<=0)
            {
                throw new Exception("Clear web distance cannot be less than or equal to 0");
            }
            this.Thickness = s.t_w;
        }
    }
}

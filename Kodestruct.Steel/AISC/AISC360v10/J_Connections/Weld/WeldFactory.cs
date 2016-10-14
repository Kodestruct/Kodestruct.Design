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
using System.Threading.Tasks;
using Kodestruct.Steel.AISC.Entities.Welds.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public class WeldFactory
    {

        public IWeld GetWeld(WeldType weldType,double F_y, double F_u, double F_EXX, double Size, double A_nBase, double Length)
        {
            IWeld weld =null;
            switch (weldType)
            {
                case WeldType.CJP:
                    weld = new CJPGrooveWeld(F_y, F_u, F_EXX, Size, A_nBase, Length);
                    break;
                case WeldType.PJP:
                    weld = new PJPGrooveWeld(F_y, F_u, F_EXX, Size, A_nBase, Length);
                    break;
                case WeldType.Fillet:
                    weld = new FilletWeld(F_y, F_u, F_EXX, Size, A_nBase, Length);
                    break;
            }
            return weld;
        }
    }
}

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

namespace Kodestruct.Steel.AISC.AISC360v10
{
    public enum ShearLagCase
    {
        Case1,  //tension load is transmitted directly 
        Case2,  //general case excluding HSS
        Case3,  //tension load is transmitted by transverse welds
        Case4,  //plates with longitudinal welds only
        Case5,  //round HSS with concentric plate
        Case6a,  //rectangular HSS with concentric plate
        Case6b,  //rectangular HSS with side plates
        Case7a, //W, M, S, HP or Tees flange connected with 2 or more lines of bolts
        Case7b,  //W, M, S, HP or Tees flange connected with 3 or more lines of bolts
        Case7c,  //W, M, S, HP or Tees web connected with 4 or more lines of bolts
        Case8a,  //Single or double angle with 2 lines of bolts
        Case8b,  //Single or double angle with 3 lines of bolts
        Case8c,  //Single or double angle with 4 lines of bolts
    }
}

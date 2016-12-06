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

namespace Kodestruct.Concrete.ACI
{
    public interface IConcreteTorsionalShape
    {
        //Area within centerline of closed stirrups
        double GetA_oh();
        //area enclosed by outside perimeter of concrete
        double GetA_cp();

        //perimeter of centerline of outermost closed transverse torsional reinforcement
        double Get_p_h();

        //outside perimeter of concrete cross section
        double Get_p_cp();

        //Threshold torsion
        double Get_T_th(double N_u);

        IConcreteMaterial Material { get; set; }
    }
}

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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.Mathematics;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    //Inherits from base class. Added here for convenience
    public class BoltGroup : BoltGroupGeneral
    {
        public BoltGroup(List<ILocationArrayElement> Bolts):base (Bolts)
        {

        }
        /// <summary>
        /// Bolt group constructor
        /// </summary>
        /// <param name="NRows">Number of rows</param>
        /// <param name="NColumns">Number of columns</param>
        /// <param name="p_h">Horizontal pitch (spacing)</param>
        /// <param name="p_v">Vertical pitch (spacing)</param>
        public BoltGroup(int NRows, int NColumns, double p_h, double p_v)
        {
            List<ILocationArrayElement> bolts = new List<ILocationArrayElement>();
            double L_h = (NColumns-1) * p_h;
            double L_v = (NRows-1) * p_v;
            double x_init = -L_h / 2.0;
            double y_init = -L_v / 2.0;
            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NColumns; j++)
                {
                    bolts.Add(new BoltPoint(x_init + j * p_h, y_init + i * p_v));
                }
            }
            base.Bolts = bolts;
        }



        public double GetInstantaneousCenterCoefficient(double e_x , double AngleOfLoad)
        {
             return FindUltimateEccentricForce(e_x , AngleOfLoad);
        }
    }
}

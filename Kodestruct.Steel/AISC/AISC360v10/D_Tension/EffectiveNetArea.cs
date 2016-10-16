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
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.SteelEntities;

namespace Kodestruct.Steel.AISC.AISC360v10.Tension
{
    public partial class TensionMember : SteelDesignElement
    {
        public TensionMember(ICalcLog Log): base(Log)
        {

        }

        public TensionMember()
            : base()
        {

        }

        /// <summary>
        /// Calculates effective net area per AISC section D3
        /// </summary>
        /// <param name="A_n">Net area</param>
        /// <param name="U">Shear Lag factor</param>
        /// <param name="A_g">Gross area of section</param>
        /// <param name="A_connected">Area of elements, connected with transverse welds</param>
        /// <param name="PartiallyWeldedWithTransverseWelds">Identifies whether this is a tension members where the tension load is transmitted only by transverse welds to some but not all of the cross-sectional elements</param>
        /// <param name="IsBoltedSplice">Identifies whether member is spliced using bolted plates</param>
        /// <returns>Effective net area A_e</returns>
        public double GetEffectiveNetArea(double A_n, double U, double A_g, double A_connected, bool PartiallyWeldedWithTransverseWelds,
            bool IsBoltedSplice )
        {
            double Ae = A_n * U; //D3-1
            if (IsBoltedSplice!=true)
            {


                    if (PartiallyWeldedWithTransverseWelds == false)
                    {
                        return Ae;
                    }
                    else
                    {
                        return A_connected;
                    } 
                }

            
            else
            {
                //For bolted splice plates Ae=An =0.85Ag, according to Section J4.1
                double AeMax = 0.85 * A_g;
                Ae = Ae < AeMax ? AeMax : Ae;
                return Ae;
            }

        }
    }
}

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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
 

namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class ChsToPlateConnection: HssToPlateConnection
    {
        public ISteelSection GetHssSteelSection()
        {
            ISteelSection s = Hss.Section as ISteelSection;
            if (s==null)
            {
                throw new Exception("Hss must implement ISteelSection interface");   
            }
            return s;
        }


        public double Q_f
        {
            get {
                double _Q_f = GetStressInteractionQf();
                return _Q_f; }

        }
        


        //K1-5
        internal double GetStressInteractionQf()
        {
            double Qf = 0.0;

            if (IsTensionHss == false)
            {
                Qf = 1.0 - 0.3 * U * (1 + U);
            }
            else
            {
                Qf = 1.0;
            }
            return Qf;
        }
    }
}

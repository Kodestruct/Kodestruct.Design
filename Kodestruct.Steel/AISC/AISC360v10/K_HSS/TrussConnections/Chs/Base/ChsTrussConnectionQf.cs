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
using Kodestruct.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Interfaces;

namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public abstract partial class ChsTrussBranchConnection: HssTrussConnection, IHssTrussBranchConnection
    {
        private double _Q_f;

        public double Q_f
        {
            get {
                _Q_f = GetQ_f();
                return _Q_f; }
            set { _Q_f = value; }
        }

        private double GetQ_f()
        {
            if (IsTensionChord == true)
            {
                return 1.0;
            }
            else
            {
                return GetQ_fInCompression();
            }
        }

        protected double GetQ_fInCompression()
        {
            double Q_f = 1 - 0.3 * U * (1 + U);
            return Q_f;
        }


        
        
    }
}

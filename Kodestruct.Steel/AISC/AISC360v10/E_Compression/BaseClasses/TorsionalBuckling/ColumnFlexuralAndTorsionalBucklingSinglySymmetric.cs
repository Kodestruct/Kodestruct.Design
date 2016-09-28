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
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public abstract partial class ColumnFlexuralAndTorsionalBucklingSinglySymmetric : ColumnFlexuralAndTorsionalBuckling
    {

        public ColumnFlexuralAndTorsionalBucklingSinglySymmetric(ISteelSection Section, double L_x, double L_y,  double L_z, ICalcLog CalcLog)
            : base(Section,L_x,L_y, L_z, CalcLog)
        {

        }

        protected abstract double GetCriticalStressFey();//note: for channels Fey is diiferent
        //from the normal orinetation of section Y-axis.

        public  double GetTorsionalElasticBucklingStressFe()
        {
            double H = GetH();
            double Fey = GetCriticalStressFey();
            double Fez = GetFez();

            double Fe;
            //(E4-5)
            Fe = ((Fey + Fez) / (2.0 * H)) * (1.0 - Math.Sqrt(1.0 - (4.0 * Fey * Fez * H) / Math.Pow(Fey * Fez, 2)));
            return Fe;
        }



    }
}

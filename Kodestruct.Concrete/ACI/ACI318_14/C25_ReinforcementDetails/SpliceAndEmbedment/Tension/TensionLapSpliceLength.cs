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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Reports; 
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Concrete.ACI318_14
{
    public partial class TensionLapSplice : Splice, ISplice
    {

           
        private double GetLs()
        {
            double ls;

            if (Rebar1Diameter>11.0/8.0 ||Rebar2Diameter>11.0/8.0)
            {
                throw new Exception("Lap splices not permitted  for sizes over #11");
            }

            double ld1 = Rebar1DevelopmentLength;
            double ld2 = Rebar2DevelopmentLength;

            double ls1;
            double ls2;

            if (Rebar1Diameter != Rebar2Diameter)
            {

               

                if (spliceClass == TensionLapSpliceClass.A)
                {
                    ls1 = ld1;
                    ls2 = ld2;
                            ls = Math.Max(ls1, ls2);

                    
                }
                else //class B
                {
                    ls1 = 1.3 * ld1;
                    ls2 = 1.3 * ld2;

                    ls =
                        Math.Min(ls1, ls2) > Math.Max(ld1, ld2) ?
                        Math.Min(ls1, ls2) :
                        Math.Max(ld1, ld2);
                        
                }

            }
            else //if both diameters are same
            {
                
                if (spliceClass == TensionLapSpliceClass.A)
                {
                        ls = 1.0 * ld1;
                }
                else
                {
                        ls = 1.3 * ld1;
                }

            }


            return ls;
        }
    }
}

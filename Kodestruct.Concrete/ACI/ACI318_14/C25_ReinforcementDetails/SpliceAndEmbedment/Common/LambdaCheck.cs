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
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class Development : AnalyticalElement, IDevelopment
    {
        
        public double CheckLambda(double lambda)
        {


                if (conc.TypeByWeight == ConcreteTypeByWeight.Lightweight &&
                    lambda > 0.75)
                {
                    //(d) Where lightweight concrete is used, ? shall not
                    //exceed 0.75 unless fct is specified (see 8.6.1).
                    //Where normalweight concrete is used, ? = 1.0.
                    if (conc.AverageSplittingTensileStrength == 0.0)
                    {


                        lambda = 0.75;

                    }
                }
            

            return lambda;
        }
    }
}


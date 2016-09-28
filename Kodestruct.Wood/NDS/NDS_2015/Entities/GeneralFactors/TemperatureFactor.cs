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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public abstract partial class WoodMember : AnalyticalElement
    {

        public virtual double GetTemperatureFactorCt(ReferenceDesignValueType ValueType, double Temperature,
            ServiceMoistureConditions Conditions)
        {
            double T =Temperature;
            double Ct=1.0;

		 if (ValueType == ReferenceDesignValueType.TensionParallelToGrain || 
             ValueType == ReferenceDesignValueType.ModulusOfElasticity || 
             ValueType == ReferenceDesignValueType.ModulusOfElasticityMin)
	    {
           if (T<=100)
	            {
                    Ct= 1.0;
	            }
            else if (T>100 && T<=125)
	            {
                    Ct = 0.9;
	            }
            else
	            {
                    Ct = 0.9;
	            }
	    }
                    else
	        {
                     if (Conditions == ServiceMoistureConditions.Dry)
	                    {
		                        if (T<=100)
	                                {
                                        Ct = 1.0;
	                                }
                                else if (T>100 && T<=125)
	                                {
                                        Ct = 0.8;
	                                }
                                else
	                                {
                                        Ct = 0.7;
	                                }
	                    }
                     else
	                    {
                                    if (T<=100)
	                                    {
                                            Ct = 1.0;
	                                    }
                                    else if (T>100 && T<=125)
	                                    {
                                            Ct = 0.7;
	                                    }
                                    else
	                                    {
                                            Ct = 0.5;
	                                    }

	                    }
	        }

                return Ct;
        }
    }
}

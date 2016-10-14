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


namespace Kodestruct.Concrete.ACI318_14
{
    public partial class DevelopmentTension:Development
    {

    
         
        public double GetKsi_t()
        {
            //12.2.4
            //(a) Where horizontal reinforcement is placed such
            //that more than 12 in. of fresh concrete is cast below
            //the development length or splice, ?t = 1.3. For other
            //situations, ?t = 1.0.
             
    double ksi_t;
    
    if (isTopRebar==true)
	{
		 ksi_t=1.3;
	}
    else
	{
        ksi_t=1.0;
	}


            return ksi_t;
        }
           
        
        public double GetKsi_e()
        {
            //(b) For epoxy-coated bars or wires with cover less
            //than 3db, or clear spacing less than 6db, ?e = 1.5.
            //For all other epoxy-coated bars or wires, ?e = 1.2.
            //For uncoated and zinc-coated (galvanized) reinforcement,
            //?e = 1.0.
            
            double sc = clearCover;

            double ksi_e;



            if (Rebar.IsEpoxyCoated == false)
            {

                ksi_e = 1.0;


            }
            else
            {
                if (db<=0 || clearCover<=0)
                {
                    throw new Exception("bar diameter or clear cover cannot be <= 0. Please check input");
                }
                if (clearCover < 3.0 * db || clearSpacing < 6.0 * db)
                {
                    ksi_e = 1.5;
                }
                else
                {
                    ksi_e = 1.2;
                }
            }

            return ksi_e;
        }


           
        public double GetKsi_s()
        {
            //(c) For No. 6 and smaller bars and deformed wires,
            //ksi_s = 0.8. For No. 7 and larger bars, ?s = 1.0.

            

            if (db<=0.0)
            {
                throw new Exception("Bar diamater cannot be <=0. Please check input");
            }

           

            double ksi_s;

            if (Rebar.Diameter<7/8)
            {
                ksi_s = 0.8;
            }
            else
            {
                ksi_s = 1.0;
            }

            return ksi_s;
        }


public double Getksi_tAndKsi_eProduct(double ksi_t, double ksi_e)
{
    double ksi_tAndKsi_eProduct = ksi_t * ksi_e;
    //However, the product ksi_t  ksi_e need not be greater than 1.7.

    if (ksi_tAndKsi_eProduct > 1.7)
    {
        ksi_tAndKsi_eProduct = 1.7;

    }
    return ksi_tAndKsi_eProduct;
}


    }
}

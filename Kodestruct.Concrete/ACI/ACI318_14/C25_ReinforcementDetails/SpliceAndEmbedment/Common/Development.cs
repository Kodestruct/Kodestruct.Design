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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public abstract partial  class Development : AnalyticalElement, IDevelopment

    {

        private Development(IConcreteMaterial Concrete, Rebar Rebar, ICalcLog log)
            : base(log)
        {
            this.conc = Concrete;
            this.rebar = Rebar;
        }
        public Development(IConcreteMaterial Concrete, Rebar Rebar, double ExcessReinforcementRatio, ICalcLog log)
            : this(Concrete, Rebar, log)
        {
            this.excessFlexureReinforcementRatio = ExcessReinforcementRatio;
        }


        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        private IConcreteMaterial conc;

        public IConcreteMaterial Concrete
        {
            get { return conc; }
            set { conc = value; }
        }


        private Rebar rebar;

        public Rebar Rebar
        {
            get { return rebar; }
            set { rebar = value; }
        }

       

        private double excessFlexureReinforcementRatio;

        public double ExcessFlexureReinforcementRatio
        {
            get { return excessFlexureReinforcementRatio; }
            set { excessFlexureReinforcementRatio = value; }
        }

       
        
        public double GetSqrt_fc()
        {
            return conc.Sqrt_f_c_prime;
        }
    }
}

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

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads
{
    public abstract class BuildingComponentBase: IBuildingComponent
    {
        public BuildingComponentBase(int Option1, int Option2, double NumericValue)
        {
            this.Option1 = Option1;
            this.Option2 = Option2;
            this.NumericValue = NumericValue;
        }
        protected abstract void Calculate();

        double weight;
        public virtual double Weight
        {
            get
            {
                this.Calculate();
                return weight;
            }
            set { weight = value; }
        }
        public string Notes { get; set; }


        //int Option1, int Option2, double NumericValue
        protected int Option1 { get; set; }
        protected int Option2 { get; set; }
        protected double NumericValue { get; set; }
    }
}

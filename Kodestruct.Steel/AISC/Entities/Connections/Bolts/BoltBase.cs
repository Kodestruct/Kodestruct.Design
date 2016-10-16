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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.Entities;

using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.SteelEntities.Bolts
{
    public abstract class BoltBase : AnalyticalElementForceBased, IDesignElement, IBolt
    {
        public BoltBase(double Diameter, BoltThreadCase ThreadType, 
            ICalcLog log)
             : base(log)
        {
            this.diameter = Diameter;
            this.threadType = ThreadType;
        }

        public abstract double NominalShearStress { get; }

        public abstract double NominalTensileStress { get; }

        private double diameter;

        public double Diameter
        {
            get { return diameter; }
            set { diameter = value; }
        }
        
        double area;
        public double Area
        {
            get 
            {
                area = Math.PI * Math.Pow(diameter, 2) / 4.0; 
                return area;
            }
        }

        private BoltThreadCase threadType;

        public BoltThreadCase ThreadType
        {
            get { return threadType; }
            set { threadType = value; }
        }



        public abstract double GetAvailableShearStrength(double N_ShearPlanes, bool IsEndLoadedConnectionWithLengthEfect);


        public abstract double GetAvailableTensileStrength();


       
    }
}

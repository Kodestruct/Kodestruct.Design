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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Concrete.ACI.Entities;

 

namespace Kodestruct.Concrete.ACI
{
    //base class for all ACI materials

    public abstract class ConcreteMaterialBase : AnalyticalElement, IConcreteMaterial
    {
        public ConcreteMaterialBase(double SpecifiedConcreteStrength,
        ConcreteTypeByWeight ConcreteType, double Density, double AverageSplittingTensileStrength, 
            ICalcLog log)
            : base(log)
        {
            this.fc = SpecifiedCompressiveStrength;
            this.typeByWeight = ConcreteType;
            this.wc = Density;
            this.averageSplittingTensileStrength = AverageSplittingTensileStrength;
        }

        public ConcreteMaterialBase(double SpecifiedConcreteStrength,
            ConcreteTypeByWeight ConcreteType, double Density, ICalcLog log): base(log)
        {
            this.fc = SpecifiedConcreteStrength;
            this.typeByWeight = ConcreteType;
            this.wc = Density;
        }

        public ConcreteMaterialBase(double SpecifiedConcreteStrength,
            ConcreteTypeByWeight ConcreteType, ICalcLog log)
            : this(SpecifiedConcreteStrength, ConcreteType, 0.0, log)
        {

        }

        private double fc;

        public double SpecifiedCompressiveStrength
        {
            get { return fc; }
            set { fc = value; }
        }

      

        private double averageSplittingTensileStrength;

        public double AverageSplittingTensileStrength
        {
            get { return averageSplittingTensileStrength; }
            set { averageSplittingTensileStrength = value; }
        }

        public abstract double ModulusOfElasticity { get;  }


        public abstract double ModulusOfRupture { get;  }

        public abstract double lambda { get; }

        public abstract double beta1 { get; }

        private double wc;

        public double Density
        {
            get { return wc; }
            set { wc = value; }
        }

        private ConcreteTypeByWeight typeByWeight;

        public ConcreteTypeByWeight TypeByWeight
        {
            get { return typeByWeight; }
            set { typeByWeight = value; }
        }

        double ultimateCompressiveStrain;

        public virtual double UltimateCompressiveStrain
        {
            get
            {
                return ultimateCompressiveStrain;
            }
            set
            {
                ultimateCompressiveStrain = value;
            }
        }


        public double Sqrt_f_c_prime
        {
            get
            {
                return GetSqrt_f_c_prime();
            }

        }

        private double GetSqrt_f_c_prime()
        {
            double Sqrt_f_c_prime = Math.Sqrt(SpecifiedCompressiveStrength);
            if (Sqrt_f_c_prime>100)
            {
                return 100;
            }
            return Sqrt_f_c_prime;
        }
    }
}

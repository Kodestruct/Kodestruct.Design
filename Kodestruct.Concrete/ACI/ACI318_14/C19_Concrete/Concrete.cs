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
using Kodestruct.Concrete.ACI.Entities;



namespace Kodestruct.Concrete.ACI318_14.Materials
{
	public class ConcreteMaterial: ConcreteMaterialBase
	{

		public ConcreteMaterial(double SpecifiedConcreteStrength,
			ConcreteTypeByWeight ConcreteType, double Density, ICalcLog log)
			: base(SpecifiedConcreteStrength, ConcreteType, Density, log)
		{
		}

		public ConcreteMaterial(double SpecifiedConcreteStrength,
		ConcreteTypeByWeight ConcreteType,ICalcLog log)
			: base(SpecifiedConcreteStrength, ConcreteType, 150.0, log)
		{
		}

		public ConcreteMaterial(double SpecifiedConcreteStrength,
			ConcreteTypeByWeight ConcreteType, double Density, double AverageSplittingTensileStrength, ICalcLog log)
			: base(SpecifiedConcreteStrength, ConcreteType,AverageSplittingTensileStrength, Density, log)
		{
			
		}

		public ConcreteMaterial(double SpecifiedConcreteStrength,
		ConcreteTypeByWeight ConcreteType, double Density, TypeOfLightweightConcrete LightWeightConcreteType, ICalcLog log)
			: base(SpecifiedConcreteStrength, ConcreteType, Density, log)
		{
			this.LightWeightConcreteType = LightWeightConcreteType;

		}

		public ConcreteMaterial(double SpecifiedConcreteStrength,
		ConcreteTypeByWeight ConcreteType, TypeOfLightweightConcrete LightWeightConcreteType, ICalcLog log)
			: base(SpecifiedConcreteStrength, ConcreteType, log)
		{
			this.LightWeightConcreteType = LightWeightConcreteType;

		}


		public override double ModulusOfElasticity
		{
			get
			{
				return GetEc();
			}
		}

		public override double ModulusOfRupture
		{
			get
			{
                double f_r =GetModulusOfRupture();
                return f_r;
			}
		}
	

		private double GetEc()
		{

			double fc = this.SpecifiedCompressiveStrength;
			double sqrt_fc = GetSqrtFc();

			double E;

			if (_lambda == 0.0)
			{
				_lambda = GetLambda();
			}

			if (this.Density==0.0)
			{
				E = 57000 * lambda* sqrt_fc;


			}
			else
			{
				double wc = this.Density;
				if (wc>=90 && wc<=160)
				{
						E= Math.Pow(wc, 1.5) * 33.0 * lambda * sqrt_fc;

				}
				else
				{
					throw new Exception("Concrete density is outside of the normal range. Density between 90 and 160 is expected");
					  
				}
			}

			return E;
		}

		private double GetSqrtFc()
		{
			double fc = this.SpecifiedCompressiveStrength;
			double sqrt_fc = this.Sqrt_f_c_prime;
			return sqrt_fc;
		}

		private double _lambda;

		public override double lambda
		{
			get 
			{
				if (_lambda ==0.0)
				{
					_lambda = GetLambda(); 
				}
				return _lambda; 
			}
		}


		private TypeOfLightweightConcrete lightWeightType;

		public TypeOfLightweightConcrete LightWeightConcreteType
		{
			get { return lightWeightType; }
			set { lightWeightType = value; }
		}

		   
		private double GetLambda()
		{
			double lambda;
			ICalcLogEntry ent = Log.CreateNewEntry();
			
			if (this.TypeByWeight == ConcreteTypeByWeight.Normalweight)
			{
				lambda = 1.0;
			}
			else
			{
				double fct = AverageSplittingTensileStrength;
				if (fct>0.0)
				{
                    double sqrt_fc = this.Sqrt_f_c_prime;
					lambda=fct/(6.7*sqrt_fc);
				}
				else
				{
					lambda = lightWeightType == TypeOfLightweightConcrete.SandLightweightConcrete ? 0.85 : 0.75;

				}
			}
			if (LogModeActive==true)
			{
				AddToLog(ent); 
			}
			ent.VariableValue = lambda.ToString();
			return lambda;
		}

		   
		private double GetModulusOfRupture()
		{


            double sqrt_fc = this.Sqrt_f_c_prime;
			double fr = 7.5 * lambda * sqrt_fc;

			return fr;
		}


        public override double beta1
        {
            get

            {
                double fc = base.SpecifiedCompressiveStrength;
                double _beta1 = 0.85 - 0.05 * ((fc - 4000.0) / 1000.0);
                double beta1_corrected;
                if (_beta1 <= 0.65)
                {
                    beta1_corrected = 0.65;
                }
                else if (_beta1 >= 0.85)
                {
                    beta1_corrected = 0.85;
                }
                else
                {
                    beta1_corrected = _beta1;
                }

                return beta1_corrected;

            }
        }
        private double epsilon_u;

	public double  ConcreteUltimateStrain
	{
		get { return 0.003; }
	}
		

		}


	
}

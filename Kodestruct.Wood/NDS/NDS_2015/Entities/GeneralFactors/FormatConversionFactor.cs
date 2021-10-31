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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public abstract partial class WoodMember : AnalyticalElement
    {

        public double GetFormatConversionFactor_K_F(ReferenceDesignValueType ValueType)
        {
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    return 2.54;
                case ReferenceDesignValueType.TensionParallelToGrain:
                    return 2.7;
                case ReferenceDesignValueType.ShearParallelToGrain:
                    return 2.88;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:
                    return 1.67;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    return 2.4;
                case ReferenceDesignValueType.ModulusOfElasticity:
                    return 1.0;
                case ReferenceDesignValueType.ModulusOfElasticityMin:
                    return 1.76;
                default:
                    return 2.54;
            }

        }
    }
}

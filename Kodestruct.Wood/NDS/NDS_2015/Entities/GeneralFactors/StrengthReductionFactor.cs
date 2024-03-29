﻿#region Copyright
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

        public double GetStrengthReductionFactor_phi(ReferenceDesignValueType ValueType)
        {
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    return 0.85;
                case ReferenceDesignValueType.TensionParallelToGrain:
                    return 0.8;
                case ReferenceDesignValueType.ShearParallelToGrain:
                    return 0.75;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:
                    return 0.9;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    return 0.9;
                case ReferenceDesignValueType.ModulusOfElasticity:
                    return 0.85;
                case ReferenceDesignValueType.ModulusOfElasticityMin:
                    return 0.85;
                default:
                    return 0.85;
            }

        }
    }
}

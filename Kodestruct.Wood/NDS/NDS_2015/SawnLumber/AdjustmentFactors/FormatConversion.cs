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
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    { 
        public double GetFormatConversionFactor(ReferenceDesignValueType ValueType )
        {
            double K_F;
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    K_F = 2.54;
                    break;
                case ReferenceDesignValueType.TensionParallelToGrain:
                    K_F = 2.70;
                    break;
                case ReferenceDesignValueType.ShearParallelToGrain:
                    K_F = 2.88;
                    break;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:
                    K_F = 1.67;
                    break;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    K_F = 2.4;
                    break;
                case ReferenceDesignValueType.ModulusOfElasticity:
                    K_F = 1.0;
                    break;
                case ReferenceDesignValueType.ModulusOfElasticityMin:
                    K_F = 1.76;
                    break;
                default:
                    K_F = 1.0;
                    break;
            }

            return K_F;
        }
    }
}

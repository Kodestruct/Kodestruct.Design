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
using Kodestruct.Wood.NDS.Entities;


namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {

         public double GetResistanceFactor(ReferenceDesignValueType ValueType )
        {
            double phi=1.0;
            switch (ValueType)
            {
                case ReferenceDesignValueType.Bending:
                    phi = 0.85;
                    break;
                case ReferenceDesignValueType.TensionParallelToGrain:
                    phi = 0.80;
                    break;
                case ReferenceDesignValueType.ShearParallelToGrain:
                    phi = 0.75;
                    break;
                case ReferenceDesignValueType.CompresionPerpendicularToGrain:
                    phi = 0.9;
                    break;
                case ReferenceDesignValueType.CompresionParallelToGrain:
                    phi = 0.9;
                    break;
                case ReferenceDesignValueType.ModulusOfElasticity:
                    phi = 1.0;
                    break;
                case ReferenceDesignValueType.ModulusOfElasticityMin:
                    phi = 0.85;
                    break;
                default:
                    break;
            }

            return phi;
        }
    
    }
}

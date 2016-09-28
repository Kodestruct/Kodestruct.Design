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
using Kodestruct.Common.Entities;
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.NDS2015
{
    public partial class SawnLumberMember : WoodMember
    {

        public double GetInsizingFactor(
        ReferenceDesignValueType ReferenceDesignValueType
        )
        {
            //NDS 2015 Table 4.3.8
            if (ReferenceDesignValueType == ReferenceDesignValueType.ModulusOfElasticity
                || ReferenceDesignValueType == ReferenceDesignValueType.ModulusOfElasticityMin)
            {
                return 0.95;
            }
            else if (ReferenceDesignValueType == ReferenceDesignValueType.CompresionPerpendicularToGrain)
            {
                return 1.0;
            }
            else
            {
                return 0.8;
            }
        }
    }
}

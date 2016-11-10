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
using System.Threading.Tasks;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI
{
    public class RebarMaterialFactory
    {
        public IRebarMaterial GetMaterial(string SpecificationId="A615Grade60")
        {
            switch (SpecificationId)
            {
                case "A615Grade40":
                    return new MaterialAstmA615(A615Grade.Grade40);
                    break;
                case "A615Grade60":
                    return new MaterialAstmA615(A615Grade.Grade60);
                    break;
                case "A615Grade75":
                    return new MaterialAstmA615(A615Grade.Grade75);
                    break;
                case "A706":
                    return new MaterialAstmA706();
                    break;
                case "A416Grade250LRS":
                    return new MaterialAstmA416(A416Grade.Grade250, StrandType.LowRelaxation);
                case "A416Grade250SR":
                    return new MaterialAstmA416(A416Grade.Grade250, StrandType.StressRelieved);
                    break;
                case "A416Grade270LRS":
                    return new MaterialAstmA416(A416Grade.Grade270, StrandType.LowRelaxation);
                case "A416Grade270SR":
                    return new MaterialAstmA416(A416Grade.Grade270, StrandType.StressRelieved);
                    break;
                default:
                    return new MaterialAstmA615(A615Grade.Grade60);
                    break;
            }
        }
    }
}

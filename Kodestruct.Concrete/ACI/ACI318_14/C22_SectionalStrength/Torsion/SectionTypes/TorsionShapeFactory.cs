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
 
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI
{
    public class TorsionShapeFactory
    {
        public IConcreteTorsionalShape GetShape(ISection Shape, IConcreteMaterial Material, double c_transv_ctr)
        {
            if (Shape is ISectionRectangular)
            {
                SectionRectangular RectangularShape = Shape as SectionRectangular;
                return new TorsionSectionRectangularNonPrestressed(RectangularShape,Material, c_transv_ctr);
            }
            else if (Shape is TorsionSectionGeneric)
	        {
                return Shape as TorsionSectionGeneric;
	        }
            else
            {
                throw new Exception("Shape type not supported for torsional analysis.");
            }
        }
    }
}

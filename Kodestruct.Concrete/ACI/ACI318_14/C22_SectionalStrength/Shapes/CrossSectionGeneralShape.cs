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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.General;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Concrete.ACI;

namespace Kodestruct.Concrete.ACI318_14
{
    public class CrossSectionGeneralShape : IConcreteSection
    {

        public CrossSectionGeneralShape(IConcreteMaterial Material, string Name, ISliceableSection SliceableShape, double b_w, double d)
        {
            this.material = Material;
            this._d =d;
            this._b_w = b_w;
            this.sliceableShape = SliceableShape;
        }

        ISliceableSection sliceableShape;
        public ISliceableSection SliceableShape
        {
            get 
            {
                return sliceableShape; 
            }
        }

        IConcreteMaterial material;
        public IConcreteMaterial Material
        {
            get { return material; }
        }


        double _b_w;
        public double b_w
        {
            get { return _b_w; }
        }

        double _d;
        public double d
        {
            get { return _d; }
        }
    }
}

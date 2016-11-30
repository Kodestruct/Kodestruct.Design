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
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI318_14
{
    public class CrossSectionIShape : IConcreteSection
    {

        public CrossSectionIShape(IConcreteMaterial Material, string Name, double d, double b_fTop, double b_fBot, double t_fTop, double t_fBot, double t_w)
        {
            this.material = Material;
            shape = new SectionI(Name, d, b_fTop, b_fBot, t_fTop, t_fBot, t_w);
        }

        private SectionI shape;

        public SectionI Shape
        {
            get { return shape; }
            set { shape = value; }
        }
        

        public ISliceableSection SliceableShape
        {
            get { return shape; }
        }

        IConcreteMaterial material;
        public IConcreteMaterial Material
        {
            get { return material; }
        }


        public double b_w
        {
            get { return this.shape.t_w; }
        }


    }
}

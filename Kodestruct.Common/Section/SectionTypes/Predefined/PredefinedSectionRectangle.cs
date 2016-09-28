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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;


namespace Kodestruct.Common.Section.Predefined
{
    /// <summary>
    /// Predefined rectangular section is used for rectangular shapes having known properties 
    /// from catalog such as precast concrete shapes.
    /// </summary>
    public class PredefinedSectionRectangle : SectionPredefinedBase, ISectionRectangular, ISliceableShapeProvider
    {
        public PredefinedSectionRectangle(ISection section)
            : base(section)
        {
            
        }

        public PredefinedSectionRectangle(double Width, double Height, ISection section)
            : base(section)
        {
            this.B = Width;
            this.H = Height;
        }

        public double H { get; set; }
        public double B { get; set; }

        public ISection GetWeakAxisClone()
        {
            throw new NotImplementedException();
        }

        //public override ISection Clone()
        //{
        //    throw new NotImplementedException();
        //}

        public ISliceableSection GetSliceableShape()
        {
            SectionRectangular r = new SectionRectangular(this.B, this.H);
            return r;
        }
    }
}

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
 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//   http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//   */
//#endregion
 
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Kodestruct.Concrete.ACI;
// using Kodestruct.Common.Entities;
//using Kodestruct.Common.Mathematics;
//using Kodestruct.Concrete.ACI318_14;
//using Kodestruct.Common.Section.Interfaces;
//using Kodestruct.Concrete.ACI.Infrastructure.Entities.Rebar;

//namespace Kodestruct.Concrete.ACI318_14
//{
//    public class PrestressedConcreteSectionGeneral : CrossSectionGeneralShape, IPrestressedConcreteSection
//    {
//        public PrestressedConcreteSectionGeneral(IPrestressedConcreteMaterial Material, ISliceableSection shape, 
//            List<RebarPoint> LongitudinalBars,
//            string Name, double b_w, double d)
//            : base(Material, Name,shape, b_w, d)
//        {
//            this.Material = Material;
//            this.longitudinalBars = LongitudinalBars;
//        }

//        public new IPrestressedConcreteMaterial Material { get; set; }

//        List<RebarPoint> longitudinalBars;
//        public List<RebarPoint> LongitudinalBars
//        {
//            get
//            {
//                return longitudinalBars;
//            }
//            set
//            {
//                longitudinalBars = value;
//            }
//        }
//    }
//}

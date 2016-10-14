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
//using Kodestruct.Concrete.ACI.Infrastructure.Entities.Rebar;

//namespace Kodestruct.Concrete.ACI318_14
//{
//    public class PrestressedConcreteSectionRectangular: CrossSectionRectangularShape, IPrestressedConcreteSection
//    {
//        public PrestressedConcreteSectionRectangular(IPrestressedConcreteMaterial Material,
//            List<RebarPoint> LongitudinalBars,
//            string Name, double Width, double Height)
//            :base(Material,Name,Width,Height)
//        {
//            this.Material = Material;
//            this.longitudinalBars = LongitudinalBars;
//        }

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
//        public new IPrestressedConcreteMaterial Material { get; set; }
//    }
//}

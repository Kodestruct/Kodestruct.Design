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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.B_General;
using Kodestruct.Steel.AISC.Interfaces;
 
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.General.Compactness
{

    public partial class ShapeCompactness
    {
        public abstract class ShapeCompactnessBase : IShapeCompactness
        {
            protected ICompactnessElement FlangeCompactness { get; set; }
            protected ICompactnessElement WebCompactness { get; set; }


            public CompactnessClassFlexure GetFlangeCompactnessFlexure()
            {
                return FlangeCompactness.GetCompactnessFlexure();
            }

            public CompactnessClassAxialCompression GetFlangeCompactnessCompression()
            {
                return FlangeCompactness.GetCompactnessAxialCompression();
            }

            public CompactnessClassFlexure GetWebCompactnessFlexure()
            {
                return WebCompactness.GetCompactnessFlexure();
            }

            public CompactnessClassAxialCompression GetWebCompactnessCompression()
            {
                return WebCompactness.GetCompactnessAxialCompression();
            }

            public double GetCompressionFlangeLambda()
            {
                return FlangeCompactness.GetLambda();
            }
            public double GetFlangeLambda_p(StressType stress)
            {
                return FlangeCompactness.GetLambda_p(stress);
            }

            public double GetFlangeLambda_r(StressType stress)
            {
                return FlangeCompactness.GetLambda_r(stress);
            }

            public double GetWebLambda()
            {
                return WebCompactness.GetLambda();
            }

            public double GetWebLambda_r(StressType stress)
            {
                return WebCompactness.GetLambda_r(stress);
            }

            public double GetWebLambda_p(StressType stress)
            {
                return WebCompactness.GetLambda_p(stress);
            }

        }


    }
}

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
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
namespace Kodestruct.Steel.AISC.AISC360v10.B_General
{
    public interface IShapeCompactness
    {
        double GetCompressionFlangeLambda();
        CompactnessClassAxialCompression GetFlangeCompactnessCompression();
        CompactnessClassFlexure GetFlangeCompactnessFlexure();
        double GetFlangeLambda_p(StressType stress);
        double GetFlangeLambda_r(StressType stress);
        CompactnessClassAxialCompression GetWebCompactnessCompression();
        CompactnessClassFlexure GetWebCompactnessFlexure();
        double GetWebLambda();
        double GetWebLambda_p(StressType stress);
        double GetWebLambda_r(StressType stress);
    }
}

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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;




namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberRhsBase : FlexuralMember
    {
        private ShapeCompactness.HollowMember compactness;

        public ShapeCompactness.HollowMember Compactness
        {
            get { return compactness; }
            set { compactness = value; }
        }

        protected virtual double GetLambdapf(FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
        {
            compactness = new ShapeCompactness.HollowMember(this.Section, compressionFiberPosition, MomentAxis);
            return compactness.GetFlangeLambda_p(StressType.Flexure);
        }

        protected virtual double GetLambdarf(FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
        {
            compactness = new ShapeCompactness.HollowMember(this.Section, compressionFiberPosition, MomentAxis);
            return compactness.GetFlangeLambda_r(StressType.Flexure);
        }

        public double GetLambdaCompressionFlange(FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
        {
            ShapeCompactness.HollowMember compactness = new ShapeCompactness.HollowMember(this.Section, compressionFiberPosition, MomentAxis);
            return compactness.GetCompressionFlangeLambda();
        }

        public double GetLambdaWeb(FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
        {
            ShapeCompactness.HollowMember compactness = new ShapeCompactness.HollowMember(this.Section, compressionFiberPosition, MomentAxis);
            return compactness.GetWebLambda();
        }

        public CompactnessClassFlexure GetFlangeCompactness(FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis)
        {
            ShapeCompactness.HollowMember compactness = new ShapeCompactness.HollowMember(this.Section, compressionFiberPosition, MomentAxis);
           return compactness.GetFlangeCompactnessFlexure();
        }
    }
}

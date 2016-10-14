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
    public abstract partial class FlexuralMemberIBase : FlexuralMember
    {
        public  double GetEffectiveRadiusOfGyration_r_t( FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            //todo: add alternative calc
            // For I-shapes with a channel cap or a cover plate attached to the compression flange are not covered
            double rt = 0.0;
            double ho = 0.0;
            double bfc = 0.0;
            double tfc = 0.0;
            double h = 0.0;
            double hc = 0.0;
            double aw = 0.0;
            double tw = 0.0;
            double d = 0;

                ho = this.GetFlangeCentroidDistanceho();
                bfc = this.GetCompressionFlangeWidthbfc(compressionFiberPosition);
                h = SectionI.h_web;
                hc = Gethc(compressionFiberPosition);
                tfc =  GetCompressionFlangeThicknesstfc(compressionFiberPosition);
                tw = this.Gettw();
                aw = Getaw(hc, tw, bfc, tfc);
                d = this.GetHeight();//confirm ....

                rt = bfc / (Math.Sqrt(12.0 * (ho / d + 1 / 6.0 * aw * Math.Pow(h, 2) / (ho * d)))); //(F4-11)

            return rt;
        }
    }
}

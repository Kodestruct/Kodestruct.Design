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
using Kodestruct.Steel.AISC.Interfaces;
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;



namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMemberIBase : FlexuralMember
    {

        public double GetWebPlastificationFactorForTensionRpt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Rpt = 0.0;

            double Mp = GetMajorNominalPlasticMoment();
            double Myt = GetTensionFiberYieldMomentMyt(compressionFiberPosition);

            double hc = Gethc(compressionFiberPosition);
            double tw = this.Gettw();

            ShapeCompactness.IShapeMember compactness = new ShapeCompactness.IShapeMember(Section, IsRolledMember, compressionFiberPosition);
            double lambdaWeb = compactness.GetWebLambda();
            double lambdapw = compactness.GetWebLambda_p(StressType.Flexure);
            double lambdarw = compactness.GetWebLambda_r(StressType.Flexure);

            //double lambda = hc / tw;
            double lambda = compactness.GetWebLambda();

            if (lambda <= lambdapw)
            {
                Rpt = Mp / Myt; //(F4-16a)
            }

            else
            {
                Rpt = Mp / Myt - (Mp / Myt - 1) * ((lambda - lambdapw) / (lambdarw - lambdapw)); //(F4-16b)
            }


            return Rpt;
        } 
    }
}

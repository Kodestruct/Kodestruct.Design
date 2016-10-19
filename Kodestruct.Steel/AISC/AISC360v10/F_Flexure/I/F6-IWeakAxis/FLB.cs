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
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.AISC360v10.B_General;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamIWeakAxis : FlexuralMemberIBase
    {
        public double GetCompressionFlangeLocalBucklingCapacity()
        {
            // compactness criteria is selected by the most slender flange

            bool LimitStateApplicable = DetermineIfFLBLimitStateIsApplicable();

            double Mn = 0;

            CompactnessResult compactnessResult = GetShapeCompactness();
            IShapeCompactness thisShapeCompactness = compactnessResult.ShapeCompactness;
            CompactnessClassFlexure flangeCompactness = compactnessResult.FlangeCompactness;

            double b = compactnessResult.b;
            double tf = compactnessResult.tf;
            double lambda = compactnessResult.lambda;



            double Mp = Z_y * F_y;
            double lambdapf = thisShapeCompactness.GetFlangeLambda_p( StressType.Flexure);
            double lambdarf = thisShapeCompactness.GetFlangeLambda_r(StressType.Flexure);

            switch (flangeCompactness)
            {
                case CompactnessClassFlexure.Compact:
                    Mn = double.PositiveInfinity;
                    break;
                case CompactnessClassFlexure.Noncompact:
                    Mn = Mp - (Mp - 0.7 * F_y * S_y) * ((lambda - lambdapf) / (lambdarf - lambdapf)); //(F6-2)
                    break;
                case CompactnessClassFlexure.Slender:

                    double Fcr = 0.69 * E / Math.Pow(b / tf, 2.0); //(F6-4)
                    Mn = Fcr * S_y; //(F6-3)
                    break;
            }
            double phiM_n = 0.9 * Mn;
            return phiM_n;
        }

        protected virtual CompactnessResult GetShapeCompactness()
        {
            ShapeCompactness.IShapeMember compactnessTop = new ShapeCompactness.IShapeMember(Section, IsRolledMember, FlexuralCompressionFiberPosition.Top);
            CompactnessClassFlexure flangeCompactnessTop = compactnessTop.GetFlangeCompactnessFlexure();

            ShapeCompactness.IShapeMember compactnessBot = new ShapeCompactness.IShapeMember(Section, IsRolledMember, FlexuralCompressionFiberPosition.Top);
            CompactnessClassFlexure flangeCompactnessBot = compactnessTop.GetFlangeCompactnessFlexure();

            double lambda = 0.0;
            double lambdaTop = compactnessTop.GetCompressionFlangeLambda();
            double lambdaBot = compactnessBot.GetCompressionFlangeLambda();

            CompactnessClassFlexure flangeCompactness;
            ShapeCompactness.IShapeMember thisShapeCompactness;


            double b = 0;
            double tf = 0.0;

            if (lambdaTop > lambdaBot)
            {
                thisShapeCompactness = compactnessTop;
                flangeCompactness = flangeCompactnessTop;
                lambda = lambdaTop;
                b = GetBfTop();
                tf = Get_tfTop();
            }
            else
            {
                thisShapeCompactness = compactnessBot;
                flangeCompactness = flangeCompactnessBot;
                lambda = lambdaBot;
                b = GetBfBottom();
                tf = Get_tfBottom();
            }

            return new CompactnessResult()
                { ShapeCompactness=thisShapeCompactness,
                    FlangeCompactness = flangeCompactness,
                    b = b,
                    tf = tf,
                    lambda = lambda
                };
        }
        protected virtual bool DetermineIfFLBLimitStateIsApplicable()

        {
            return true;
        }

        protected class CompactnessResult
        {
            public IShapeCompactness ShapeCompactness { get; set; }
            public CompactnessClassFlexure FlangeCompactness { get; set; }

           public double b { get; set; }
           public double tf { get; set; }
           public double lambda { get; set; }
        }
    }
}

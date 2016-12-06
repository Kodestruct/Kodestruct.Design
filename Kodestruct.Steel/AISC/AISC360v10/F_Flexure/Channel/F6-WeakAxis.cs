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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamChannelWeakAxis : BeamIWeakAxis
    {
        public BeamChannelWeakAxis(ISteelSection section, bool IsRolledMember, ICalcLog CalcLog)
            : base(section, IsRolledMember, CalcLog)
        {

            SectionChannel = Section.Shape as ISectionChannel;
        }

        ISectionChannel SectionChannel;
            
            
        protected override CompactnessResult GetShapeCompactness()
        {

            //use only top fiber position properties
            // it is assumed that Channel shape is symmetrical
            ShapeCompactness.ChannelMember compactnessTop = new ShapeCompactness.ChannelMember(Section, IsRolledMember, FlexuralCompressionFiberPosition.Top);
            CompactnessClassFlexure flangeCompactnessTop = compactnessTop.GetFlangeCompactnessFlexure();


            double lambda = 0.0;
            double lambdaTop = compactnessTop.GetCompressionFlangeLambda();


            CompactnessClassFlexure flangeCompactness;
            ShapeCompactness.ChannelMember thisShapeCompactness;


            double b = 0;
            double tf = 0.0;


                thisShapeCompactness = compactnessTop;
                flangeCompactness = flangeCompactnessTop;
                lambda = lambdaTop;
                b = SectionChannel.b_f;
                tf = SectionChannel.t_f;


            return new CompactnessResult()
            {
                ShapeCompactness = thisShapeCompactness,
                FlangeCompactness = flangeCompactness,
                b = b,
                tf = tf,
                lambda = lambda
            };
        }
    }
}

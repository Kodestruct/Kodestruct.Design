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

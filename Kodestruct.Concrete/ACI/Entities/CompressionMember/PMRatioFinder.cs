using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.Entities
{
    public class PMRatioFinder
    {
        public double FindPMRatio(List<PMPair> RightHand, List<PMPair> LeftHand, PMPair Point)
        {
            throw new NotImplementedException();
            var LeftPolyOrdered = LeftHand.OrderBy(p => p.P).ToList();
            var CombinedPolygon = RightHand.OrderByDescending(p => p.P).Concat(LeftPolyOrdered).ToList();

            //double MMaxAbs = CombinedPolygon.Select(p => p.M)

            Complex PointPM = new Complex(Point.M, Point.P);

            double LengthOfFuthestPoint = CombinedPolygon.Max(pm =>
            {
                Complex interactionPoint = new Complex(pm.M, pm.P);
                return interactionPoint.Magnitude;
            });


            double scale = LengthOfFuthestPoint / PointPM.Magnitude;
            double AdjustmentScaleOfRay = scale >= 1 ? scale * 1.15 : 1.15;

            PMPair pairScaled = new PMPair(Point.M * AdjustmentScaleOfRay, Point.P * AdjustmentScaleOfRay);
        }

    }
}

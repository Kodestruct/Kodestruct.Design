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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.SteelEntities
{
    public abstract partial class ConnectionGroup
    {


        bool ElasticPropertiesWereCalculated;
        /// <summary>
        /// Calculates elastic coefficient C. This coefficient, multiplied by
        /// maximum element force will produce the moment that will cause the
        /// maximum element force to be developed in at least one element of the group.
        /// </summary>
        /// <returns></returns>
        public double CalculateElasticGroupMomentCoefficientC()
        {

            if (ElasticPropertiesWereCalculated == false)
            {
                CalculateElastcCentroid();
                CalculatePolarMomentOfInertia();
            }

            double distanceMax = double.NegativeInfinity;
            foreach (var e in Elements)
            {
                double dx = ElasticCentroid.X - e.Location.X;
                double dy = ElasticCentroid.Y - e.Location.Y;
                double thisDistance = Math.Sqrt(dx * dx + dy * dy);
                distanceMax = thisDistance >= distanceMax ? thisDistance : distanceMax;
            }
            double C = J / distanceMax;
            return C;
        }

        private Point2D _c;

        protected Point2D ElasticCentroid
        {
            get
            {
                if (_c == null)
                {
                    CalculateElastcCentroid();
                }
                return _c;
            }

        }


        protected double _J;

        public double J
        {
            get
            {
                if (_J == null)
                {
                    CalculatePolarMomentOfInertia();
                }
                return _J;
            }

        }

        private void CalculatePolarMomentOfInertia()
        {
            //Calculate J for the entire group
            foreach (var e in Elements)
            {
                double dx = ElasticCentroid.X - e.Location.X;
                double dy = ElasticCentroid.Y - e.Location.Y;
                _J = _J + (Math.Pow(dx, 2) + Math.Pow(dy, 2));
            }
        }

        private void CalculateElastcCentroid()
        {
            double cx=0.0;
            double cy = 0.0;
            //Find the elastic centroid of the group
            foreach (var e in Elements)
            {
                cx = cx + e.Location.X;
                cy = cy + e.Location.Y;
            }
            _c = new Point2D(cx, cy);
        }

        public double GetForceResultant(double V_u, double H_u)
        {
            double R = Math.Sqrt(Math.Pow(V_u, 2) + Math.Pow(H_u, 2));
            return R;
        }

        public double GetAngle(double V_u, double H_u)
        {
            double AngleDegrees = Math.Atan(H_u/V_u);  //Angle measured from vertical
            return AngleDegrees;
        }
    }
}

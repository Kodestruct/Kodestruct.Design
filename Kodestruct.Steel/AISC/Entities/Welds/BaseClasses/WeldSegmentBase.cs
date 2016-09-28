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
using MoreLinq;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.SteelEntities.Welds
{
    /// <summary>
    /// Weld segment is subdivided into multiple weld elements. 
    /// A weld line or a weld circle are examples of weld segment.
    /// </summary>
    public abstract class WeldSegmentBase
    {

        public abstract double GetInertiaYAroundPoint(Point2D center);
        public abstract double GetInertiaXAroundPoint(Point2D center);
        public abstract double GetPolarMomentOfInetriaAroundPoint(Point2D center);
        public abstract double GetArea();
        public abstract double GetSumAreaDistanceX(Point2D refPoint);
        public abstract double GetSumAreaDistanceY(Point2D refPoint);

        private double electrodeStrength;

        public double ElectrodeStrength
        {
            get { return electrodeStrength; }
            set { electrodeStrength = value; }
        }

        private int numberOfSubdivisions;

        public int NumberOfSubdivisions
        {
            get { return numberOfSubdivisions; }
            set { numberOfSubdivisions = value; }
        }

        private double leg;

        public double Leg
        {
            get { return leg; }
            set { leg = value; }
        }

        protected abstract void CalculateElements();

        private List<IWeldElement> weldElements;

        public List<IWeldElement> WeldElements
        {
            get 
            {
                if (weldElements==null)
                {
                    this.CalculateElements();
                }
                return weldElements; 
            }
            set { weldElements = value; }
        }

        public IWeldElement GetFurthestElement(Point2D point)
        {
            var result = WeldElements.MaxBy(x => x.GetCentroidDistanceToNode(point));
            return result;
        }
    }
}

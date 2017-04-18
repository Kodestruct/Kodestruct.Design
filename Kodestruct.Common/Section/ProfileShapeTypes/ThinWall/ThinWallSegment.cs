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
 
using Kodestruct.Common.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Section
{
    public class ThinWallSegment
    {
        public double WallThickness { get; set; }
        public Line2D Line { get; set; }

        /// <summary>
        /// Vertical thin wall segment
        /// </summary>
        /// <param name="YMin">Y coordinate for lowest point</param>
        /// <param name="YMax">Y coordinate for highest point</param>
        /// <param name="WallThickness">Wall thickness</param>
        public ThinWallSegment(double YMin, double YMax, double WallThickness)
        {
            Line = new Line2D(new Point2D(0, YMin), new Point2D(0, YMax));
            this.WallThickness = WallThickness;
        }

        public ThinWallSegment(Line2D Line, double WallThickness)
        {
            this.Line = Line;
            this.WallThickness = WallThickness;
        }
        
    }
}

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

namespace Kodestruct.Steel.AISC.SteelEntities.Bolts
{
    /// <summary>
    /// An element used in instantaneous center of rotation
    /// calculations. It can be a bolt or a weld element
    /// </summary>
    public class BoltPoint: ILocationArrayElement
    {
        /// <summary>
        /// Constructor with Location as Point2D type.
        /// </summary>
        /// <param name="Location">Coordinates of center of element</param>
        public BoltPoint(Point2D Location)
        {
            this.Location = Location;
        }
        public BoltPoint(double X, double Y)
        {
            this.Location = new Point2D(X, Y);
        }
        /// <summary>
        /// Point locating the element center
        /// </summary>
        public Point2D Location { get; set; }
        /// <summary>
        /// X-component of element force
        /// </summary>
        public double R_x { get; set; }

        /// <summary>
        /// Y-component of element force
        /// </summary>
        public double R_y { get; set; }

        /// <summary>
        /// Distance to Instantaneous Center (IC)
        /// </summary>
        public double d_i { get; set; }

        /// <summary>
        /// Element deformation
        /// </summary>
        public double Delta { get; set; }



        public double LimitDeformation { get; set; }



        public double GetDistanceToPoint(Point2D point)
        {
            return Math.Sqrt(Math.Pow(point.X-this.Location.X,2)+Math.Pow(point.Y - this.Location.Y,2));
        }
    }
}

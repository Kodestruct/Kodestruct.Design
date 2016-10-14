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
using System.Threading.Tasks;

namespace Kodestruct.Common.Mathematics
{
    public class DataPoint2D 
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Value { get; set; }

        public DataPoint2D(decimal X, decimal Y, decimal Value)
        {
            this.X = X;
            this.Y = Y;
            this.Value = Value;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            DataPoint2D p = obj as DataPoint2D;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public bool Equals(DataPoint2D p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode()
        {
            double x = (double)X;
            double y = (double)Y;
            return (int)Math.Pow(x,y);
        }

        public bool IsNear(DataPoint2D p, decimal Tolerance)
        {
            double X1 = (double)p.X;
            double X2 = (double)X;
            double Y1 = (double)p.Y;
            double Y2 = (double)Y;
            double distance = Math.Sqrt(Math.Pow(X1-X2,2)+Math.Pow(Y1-Y2,2));
            decimal dist = (decimal)distance;
            if (dist<Tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

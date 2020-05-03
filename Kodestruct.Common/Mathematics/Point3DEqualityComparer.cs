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

    //https://stackoverflow.com/questions/1606679/remove-duplicates-in-the-list-using-linq
    public class Point3DEqualityComparer : IEqualityComparer<Point3D>
    {
        double Precision;
        public Point3DEqualityComparer(double Precision)
        {
            this.Precision = Precision;
        }
        public bool Equals(Point3D p1, Point3D p2)
        {
 
         bool IsEqual = 
          Math.Abs(p1.X - p2.X)<= Precision &&
          Math.Abs(p1.Y - p2.Y) <= Precision &&
          Math.Abs(p1.Z - p2.Z) <= Precision;

            return IsEqual;
        }



        //https://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
        public  int GetHashCode(Point3D obj)
        {
            if (Math.Round(obj.X, 2) == 1286.47 && Math.Round(obj.Y, 2) == 1059.25)
            {
                var v1 = Math.Round(obj.Y, 6 + 1).GetHashCode();
                var v2 = Math.Round(obj.Y, 6 + 1);
            }
                //determine the precision of rounding 
                var precDec = Convert.ToDecimal(Precision);
            //https://stackoverflow.com/questions/13477689/find-number-of-decimal-places-in-decimal-value-regardless-of-culture
            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(precDec)[3])[2];

            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                hash = hash * 23 + Math.Round(obj.X, decimalPlaces+1).GetHashCode();
                hash = hash * 23 + Math.Round(obj.Y, decimalPlaces+1).GetHashCode();
                hash = hash * 23 + Math.Round(obj.Z, decimalPlaces+1).GetHashCode();
                return hash;
            }
        }
    }
}

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

namespace Kodestruct.Common.Mathematics
{
    public static class DoubleExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }


        public static double ToDegrees(this double val)
        {
            return (180.0/Math.PI ) * val;
        }


        //public static double[] Minimum(this double[] values)
        //{
        //    double Minimum = double.PositiveInfinity;
        //    for (int i = 0; i < values.Length; i++)
        //        if (values[i] < Minimum)
        //        {
        //            Minimum = values[i];
        //        }
        //    return Minimum;
        //}
    }
}

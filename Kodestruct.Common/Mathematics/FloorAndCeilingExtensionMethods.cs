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
    public static class FloorAndCeilingExtensionMethods
    {

        public static decimal CeilingWithSignificance(this decimal value, decimal significance)
        {
            decimal retval = value;
            if ((value % significance) != 0)
            {
                decimal wholePart = (int)(value / significance) * significance;
                if (value > 0)
                {
                    retval = wholePart + significance;
                }
                else
                {
                    retval = wholePart;
                }

                //return ((int)(value / significance) * significance) + significance;
            }
            return retval;
            //return Convert.Todecimal(value);
        }



        public static decimal FloorWithSignificance(this decimal value, decimal significance)
        {
            decimal retval = value;
            if ((value % significance) != 0)
            {
                decimal wholePart = (int)(value / significance) * significance;
                

                if (value<0)
                {
                    retval = wholePart  - significance;
                }
                else
                {
                    retval = wholePart;
                }
            }

            return retval;
        }
    }
}

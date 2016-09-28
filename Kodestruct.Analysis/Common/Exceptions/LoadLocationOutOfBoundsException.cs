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

namespace Kodestruct.Analysis
{
    public class LoadLocationOutOfBoundsException : ApplicationException
    {
        double Length, ParameterValue;
        string ParameterName;

        public LoadLocationOutOfBoundsException(double Length, double ParameterValue, string ParameterName)
        {
            this.Length = Math.Round(Length, 3);
            this.ParameterValue = Math.Round(ParameterValue, 3);
            this.ParameterName = ParameterName;
        }

        string typeString;

        // Override the Exception.Message property.
        public override string Message
        {
            get
            {
                return string.Format("Load location parameter ({0}={1}) is invalid for member length (L={2})", ParameterName,ParameterValue, Length);
            }
        }
    }

}

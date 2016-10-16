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

namespace Kodestruct.Analysis
{
    public class LoadConcentratedDoubleSymmetrical : LoadConcentrated
    {
        public double Dimension_a { get; set; }
        public LoadConcentratedDoubleSymmetrical(double P, double Dimension_a)
            : base(new List<double>() { P })
        {
            this.Dimension_a = Dimension_a;
            this._P = P;
        }

        private double _P;

        public double P
        {
            get { return _P; }
            set
            {
                _P = value;
                if (Values.Count > 1)
                {
                    Values[0] = value;
                }
                else
                {
                    Values.Add(value);
                }
            }
        }
    }
}

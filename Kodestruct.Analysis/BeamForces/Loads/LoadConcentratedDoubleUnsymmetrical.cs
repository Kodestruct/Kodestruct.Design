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
    public class LoadConcentratedDoubleUnsymmetrical : LoadConcentrated
    {
        public double Dimension_a { get; set; }
        public double Dimension_b { get; set; }


        private double _P1;

        public double P1
        {
            get { return _P1; }
            set {
                if (Values.Count>=2)
                {
                    Values[0] = value;
                }
                else
                {
                    Values.Add(value);
                }
                _P1 = value; 
            }
        }

        private double _P2;
        public double P2
        {
            get { return _P2; }
            set
            {
                if (Values.Count >= 2)
                {
                    Values[0] = value;
                }
                else
                {
                    Values.Add(value);
                }
                _P2 = value;
            }
        }

        public LoadConcentratedDoubleUnsymmetrical(double P1, double P2, double Dimension_a, double Dimension_b)
            : base(new List<double>() { P1,P2 })
        {
            this.P1 = P1;
            this.P2 = P2;
            this.Dimension_a = Dimension_a;
            this.Dimension_b = Dimension_b;
        }
    }
}

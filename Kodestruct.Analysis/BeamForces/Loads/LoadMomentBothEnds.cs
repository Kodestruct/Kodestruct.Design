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
    public class LoadMomentBothEnds : LoadMoment
    {
        public LoadMomentBothEnds(double M1, double M2)
            : base(new List<double>() { M1,M2 })
        {
            this.M1 = M1;
            this.M2 = M2;
        }


        private double _M1;

        public double M1
        {
            get { return _M1; }
            set
            {
                _M1 = value;
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

        private double _M2;

        public double M2
        {
            get { return _M2; }
            set 
            {
                _M2 = value;
                if (Values.Count > 2)
                {
                    Values[1] = value;
                }
                else
                {
                    Values.Add(value);
                }
            }
        }
        
    }
}

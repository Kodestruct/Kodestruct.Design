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


namespace Kodestruct.Concrete.ACI
{
    public class SectionFlexuralAnalysisResult 
    {
        public SectionFlexuralAnalysisResult()
        {

        }
        public SectionFlexuralAnalysisResult( double Moment, LinearStrainDistribution StrainDistribution)
        {
            this.moment = Moment;
            this.strainDistribution = StrainDistribution;
        }

        private double moment;

        public double Moment
        {
            get { return moment; }
            set { moment = value; }
        }

        private LinearStrainDistribution strainDistribution;

        public LinearStrainDistribution StrainDistribution
        {
            get { return strainDistribution; }
            set { strainDistribution = value; }
        }
        
    }
}

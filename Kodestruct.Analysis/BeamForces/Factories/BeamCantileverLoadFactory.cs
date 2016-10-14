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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Interfaces;

namespace Kodestruct.Analysis
{
    /// <summary>
    /// Overrides beam load factory  for the cantilever beam case
    /// </summary>
    public class BeamCantileverLoadFactory : BeamLoadFactory
    {

        //public BeamCantileverLoadFactory(IParameterExtractor Extractor)
        //    : base(Extractor)
        //{

        //}

        public BeamCantileverLoadFactory(BeamFactoryData data)
            : base(data)
        {

        }
        public override LoadBeam GetConcentratedCase(string subCase)
        {
            LoadBeam Load = null;
            switch (subCase)
            {
                case "1":
                    Load = new LoadConcentratedSpecial(d.P, LoadConcentratedSpecialCase.CantileverTip);
                    break;
                default:
                    Load = new LoadConcentratedGeneral(d.P, d.a_load);
                    break;
            }
            return Load;
        }

        public override LoadBeam GetPartialDistributedCase(string subCase)
        {
            return new  LoadDistributedGeneral(d.w,0.0,d.b_load);
        }

        public override LoadBeam GetVaryingCase(string subCase)
        {
            LoadBeam Load = null;
            switch (subCase)
            {
                case "2":
                    Load = new LoadDistributedUniform(d.w, LoadDistributedSpecialCase.InvertedTriangle);
                    break;
                default:
                    Load = new LoadDistributedUniform(d.w, LoadDistributedSpecialCase.Triangle);
                    break;
            }
            return Load;
        }
    

    }
}

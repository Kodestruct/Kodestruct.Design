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
using Kodestruct.Analysis.BeamForces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Interfaces;

namespace Kodestruct.Analysis
{
    public class BeamInstanceFactory
    {
        //IParameterExtractor e;

        //public BeamInstanceFactory(IParameterExtractor Extractor)
        //{
        //    this.e = Extractor;
        //}

        BeamFactoryData d;

        public BeamInstanceFactory(BeamFactoryData data)
        {
            this.d = data;
        }


        public IAnalysisBeam CreateBeamInstance(string BeamCaseId, LoadBeam load, ICalcLog Log)
        {
            double L = d.L;
            double LoadDimension_a = d.a_load;

            Beam bm = null;
            if (BeamCaseId.StartsWith("C1") == true)
            {
                bm = new BeamSimple(d.L, load, Log);
            }
            else if (BeamCaseId.StartsWith("C2") == true)
            {
                bm = new BeamSimpleWithOverhang(L, LoadDimension_a, load, Log);
            }
            else if (BeamCaseId.StartsWith("C3") == true)
            {
                bm = new BeamPinnedFixed(L,load, Log);
            }
            else if (BeamCaseId.StartsWith("C4") == true)
            {
                bm = new BeamFixedFixed(L, load, Log);
            }
            else //else if (BeamCaseId.StartsWith("C5") == true)
            {
                bm = new BeamCantilever(L, load, Log);
            }

            bm.ModulusOfElasticity = d.E;
            bm.MomentOfInertia = d.I;

            return bm;
        }
    }
}

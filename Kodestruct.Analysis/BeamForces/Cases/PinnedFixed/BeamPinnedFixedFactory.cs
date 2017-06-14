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


namespace Kodestruct.Analysis.BeamForces.PinnedFixed
{
    public class BeamPinnedFixedFactory : IBeamCaseFactory
    {
        BeamPinnedFixed beam;
        const string CaseNotSupportedExceptionText = "This loading case is not supported for deflection calculations.";

        public ISingleLoadCaseBeam GetForceCase(LoadBeam load, IAnalysisBeam beam)
        {
            BeamCase bc = GetCase(load, beam);
            return bc.ForceCase;
        }


        public ISingleLoadCaseDeflectionBeam GetDeflectionCase(LoadBeam load, IAnalysisBeam beam)
        {
            BeamCase bc = GetCase(load, beam);
            if (bc.DeflectionCase == null)
            {
                throw new Exception(CaseNotSupportedExceptionText);
            }
            return bc.DeflectionCase;
        }
        public BeamCase GetCase(LoadBeam load, IAnalysisBeam beam)
        {
            this.beam = beam as BeamPinnedFixed;
            BeamCase BeamLoadCase = null;

            if (load is LoadConcentrated)
            {
                BeamLoadCase = GetConcentratedLoadCase(load);
            }
            else if (load is LoadDistributed)
            {
                BeamLoadCase = GetDistributedLoadCase(load);
            }
            else if (load is LoadMoment)
            {
                BeamLoadCase = GetMomentLoadCase(load);
            }

            return BeamLoadCase;
        }

        private BeamCase GetDistributedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadDistributedUniform)
            {
                LoadDistributedUniform cl = load as LoadDistributedUniform;
                UniformlyDistributedLoad b =new UniformlyDistributedLoad(beam, cl.Value); //3B.1
                beamForceCase = b;
                beamDeflectionCase = b;
            }

            if (load is LoadDistributedGeneral) //3C.1
            {
                LoadDistributedGeneral cl = load as LoadDistributedGeneral;
                beamForceCase = new UniformPartialLoad(beam, cl.Value, cl.XLocationStart, 
                    cl.XLocationEnd - cl.XLocationStart);
            }
            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

        private BeamCase GetConcentratedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadConcentratedSpecial) //3A.1
            {
                LoadConcentratedSpecial cl = load as LoadConcentratedSpecial;
                ConcentratedLoadAtCenter b = new ConcentratedLoadAtCenter(beam, cl.P);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            if (load is LoadConcentratedGeneral) //3A.2
            {
                LoadConcentratedGeneral cl = load as LoadConcentratedGeneral;
                ConcentratedLoadAtAnyPoint b  = new ConcentratedLoadAtAnyPoint(beam, cl.P, cl.XLocation);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

        private BeamCase GetMomentLoadCase(LoadBeam load)
        {

            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            LoadMomentLeftEnd cl = load as LoadMomentLeftEnd; //3E.1
            MomentAtFreeEnd b = new MomentAtFreeEnd(beam, cl.Mo);
            beamForceCase = b;
            beamDeflectionCase = b;

            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

    }
}

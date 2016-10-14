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


namespace Kodestruct.Analysis.BeamForces.Cantilever
{
    public class BeamCantileverFactory : IBeamCaseFactory
    {
        BeamCantilever beam;
        const string CaseNotSupportedExceptionText = "This loading case is not supported for deflection calculations.";
        public ISingleLoadCaseBeam GetForceCase(LoadBeam load, IAnalysisBeam beam)
        {
            BeamCase bc = GetCase(load, beam);
            return bc.ForceCase;
        }
        public ISingleLoadCaseDeflectionBeam GetDeflectionCase(LoadBeam load, IAnalysisBeam beam)
        {
            BeamCase bc = GetCase(load, beam);
            if (bc.DeflectionCase == null || bc == null)
            {
                throw new Exception(CaseNotSupportedExceptionText);
            }
            return bc.DeflectionCase;
        }


        public BeamCase GetCase(LoadBeam load, IAnalysisBeam beam)
        {
            this.beam = beam as BeamCantilever;
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

        private BeamCase GetMomentLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadMomentLeftEnd)//5E.1
            {
                LoadMomentLeftEnd cl = load as LoadMomentLeftEnd;
                MomentAtTip b = new MomentAtTip(beam, cl.Mo);
                beamForceCase = b;
                beamDeflectionCase = b;
            }

            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

        private BeamCase GetDistributedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadDistributedUniform)
            {
                LoadDistributedUniform cl = load as LoadDistributedUniform;
                switch (cl.Case)
                {
                    case LoadDistributedSpecialCase.Triangle:
                        DistributedUniformlyIncreasingToBase b1 = new DistributedUniformlyIncreasingToBase(beam, cl.Value);//5D.1
                        beamForceCase = b1;
                        beamDeflectionCase = b1;
                        break;
                    case LoadDistributedSpecialCase.InvertedTriangle:
                        DistributedUniformlyDecreasingToBase b2 = new DistributedUniformlyDecreasingToBase(beam, cl.Value); //5D.2
                        beamForceCase = b2;
                        beamDeflectionCase = b2;
                        break;
                    default:
                        UniformLoad b3 = new UniformLoad(beam, cl.Value); //5B.1
                        beamForceCase = b3;
                        beamDeflectionCase = b3;
                        break;
                }

            }
            if (load is LoadDistributedGeneral) //5C.1
            {
                LoadDistributedGeneral cl = load as LoadDistributedGeneral;
                UniformPartialLoad b = new UniformPartialLoad(beam, cl.Value, cl.XLocationEnd);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

        private BeamCase GetConcentratedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;
            ISingleLoadCaseBeam beamForceCase = null;

            if (load is LoadConcentratedSpecial) //5A.1
            {
                LoadConcentratedSpecial cl = load as LoadConcentratedSpecial;
                ConcentratedLoadAtTip b1 = new ConcentratedLoadAtTip(beam, cl.P);
                beamForceCase = b1;
                beamDeflectionCase = b1;
            }
            if (load is LoadConcentratedGeneral) //5A.2
            {
                LoadConcentratedGeneral cl = load as LoadConcentratedGeneral;
                ConcentratedLoadAtAnyPoint b2 = new ConcentratedLoadAtAnyPoint(beam, cl.P, cl.XLocation);
                beamForceCase = b2;
                beamDeflectionCase = b2;
            }
            return new BeamCase(beamForceCase, beamDeflectionCase);
        }



    }
}

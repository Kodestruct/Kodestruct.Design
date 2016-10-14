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


namespace Kodestruct.Analysis.BeamForces.SimpleWithOverhang
{
    public class BeamSimpleWithOverhangFactory : IBeamCaseFactory
    {

        BeamSimpleWithOverhang beam;
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
            this.beam = beam as BeamSimpleWithOverhang;
            BeamCase BeamLoadCase = null;

            if (load is LoadConcentrated)
            {
                BeamLoadCase = GetConcentratedLoadCase(load);
            }
            else if (load is LoadDistributed)
            {
                BeamLoadCase = GetDistributedLoadCase(load);
            }

            return BeamLoadCase;
        }

        private BeamCase GetConcentratedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
 
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadConcentratedGeneral) //1B.1
            {
                LoadConcentratedGeneral cl = load as LoadConcentratedGeneral;
                ConcentratedLoadBetweenSupports b = new ConcentratedLoadBetweenSupports(beam, cl.P, cl.XLocation);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            else if (load is LoadConcentratedSpecial)
            {
                LoadConcentratedSpecial cl = load as LoadConcentratedSpecial;
                if (cl.Case == LoadConcentratedSpecialCase.CantileverTip)
                {
                    ConcentratedLoadOverhang b = new ConcentratedLoadOverhang(beam, cl.P, beam.OverhangLength);
                    beamForceCase = b;
                    beamDeflectionCase = b;
                }
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
                    case LoadDistributedSpecialCase.CantileverOverhang:
                        DistributedLoadOverhang b1 = new  DistributedLoadOverhang(beam,cl.Value); //2C.2
                        beamForceCase = b1;
                        beamDeflectionCase = b1;
                        break;
                    case LoadDistributedSpecialCase.CantileverMainSpan:
                        DistributedLoadBetweenSupports b2 = new DistributedLoadBetweenSupports(beam, cl.Value); //2C.1
                        beamForceCase = b2;
                        beamDeflectionCase = b2;
                        break;
                    default:
                        UniformLoadFull b3 = new UniformLoadFull(beam, cl.Value); //2B.1
                        beamForceCase = b3;
                        beamDeflectionCase = b3;
                        break;
                }
                
            }

            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

    }
}

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


namespace Kodestruct.Analysis.BeamForces.Simple
{
    
    public class BeamSimpleFactory : IBeamCaseFactory
    {
        const string CaseNotSupportedExceptionText = "This loading case is not supported for deflection calculations.";
        BeamSimple beam;



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


        private BeamCase GetCase(LoadBeam load, IAnalysisBeam beam)
        {
            this.beam = beam as BeamSimple;
            //ISingleLoadCaseBeam BeamLoadCase = null;
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

        
        #region Forces
        private BeamCase GetConcentratedLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;
            if (load is LoadConcentratedSpecial) //1A.1
            {
                LoadConcentratedSpecial cl = load as LoadConcentratedSpecial;
                ConcentratedLoadAtCenter b = new ConcentratedLoadAtCenter(beam, cl.P);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            if (load is LoadConcentratedGeneral) //1A.2
            {
                LoadConcentratedGeneral cl = load as LoadConcentratedGeneral;
                ConcentratedLoadAtAnyPoint b=new ConcentratedLoadAtAnyPoint(beam, cl.P, cl.XLocation);
                beamForceCase = b;
                beamDeflectionCase = b;
            }
            if (load is LoadConcentratedDoubleSymmetrical) //1A.3
            {
                LoadConcentratedDoubleSymmetrical cl = load as LoadConcentratedDoubleSymmetrical;
                TwoConcentratedLoadsSymmetrical b =
                   new TwoConcentratedLoadsSymmetrical(beam, cl.P, cl.Dimension_a);
                beamForceCase = b;
                beamDeflectionCase = b;
            }

            if (load is LoadConcentratedDoubleUnsymmetrical) //1A.4
            {
                LoadConcentratedDoubleUnsymmetrical cl = load as LoadConcentratedDoubleUnsymmetrical;
                TwoConcentratedLoadsUnsymmetrical b =
                   new TwoConcentratedLoadsUnsymmetrical(beam, cl.P1, cl.P2, cl.Dimension_a, cl.Dimension_b);
                beamForceCase = b;
                beamDeflectionCase = null;
            }
            if (load is LoadConcentratedCenterWithEndMoments) //1A.5
            {
                LoadConcentratedCenterWithEndMoments cl = load as LoadConcentratedCenterWithEndMoments;
                ConcentratedLoadAtCenterAndVarEndMoments b =
                   new ConcentratedLoadAtCenterAndVarEndMoments(beam, cl.P, cl.M1, cl.M2);
                beamForceCase = b;
                beamDeflectionCase = null;
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
                        DistributedUniformlyIncreasingToEnd b1 = new DistributedUniformlyIncreasingToEnd(beam, cl.Value);//1D.1
                        beamForceCase = b1;
                        beamDeflectionCase = b1;
                        break;
                    case LoadDistributedSpecialCase.DoubleTriangle:
                        DistributedDoubleTriangle b2 = new DistributedDoubleTriangle(beam, cl.Value); //1D.2
                        beamForceCase = b2;
                        beamDeflectionCase = b2;
                        break;
                    default:
                        UniformLoad b3 = new UniformLoad(beam, cl.Value); //1B.1
                        beamForceCase = b3;
                        beamDeflectionCase = b3;
                        break;
                }

            }

            if (load is LoadDistributedUniformWithEndMoments) //1B.2
            {
                LoadDistributedUniformWithEndMoments cl = load as LoadDistributedUniformWithEndMoments;
                beamForceCase = new UniformLoadAndEndMoments(beam, cl.Value, cl.M1, cl.M2);
                beamDeflectionCase = null;
            }

            if (load is LoadDistributedGeneral) //1C.1
            {
                LoadDistributedGeneral cl = load as LoadDistributedGeneral;
                beamForceCase = new UniformPartialLoad(beam, cl.Value, cl.XLocationStart, cl.XLocationEnd - cl.XLocationStart);
                beamDeflectionCase = null;
            }

            return new BeamCase(beamForceCase, beamDeflectionCase);
        }
        private BeamCase GetMomentLoadCase(LoadBeam load)
        {
            ISingleLoadCaseBeam beamForceCase = null;
            ISingleLoadCaseDeflectionBeam beamDeflectionCase = null;

            if (load is LoadMomentLeftEnd)//1E.1
            {
                LoadMomentLeftEnd cl = load as LoadMomentLeftEnd;
                MomentAtOneEnd b1 = new MomentAtOneEnd(beam, cl.Mo);
                beamForceCase = b1;
                beamDeflectionCase = b1;
            }
            if (load is LoadMomentGeneral)//1E.2
            {
                LoadMomentGeneral cl = load as LoadMomentGeneral;
                MomentAtAnyPoint b2 = new MomentAtAnyPoint(beam, cl.Mo, cl.Location);
                beamForceCase = b2;
                beamDeflectionCase = null;
            }
            if (load is LoadMomentBothEnds)//1E.3
            {
                LoadMomentBothEnds cl = load as LoadMomentBothEnds;
                beamForceCase = new MomentAtBothEnds(beam, cl.M1, cl.M2);
                beamDeflectionCase = null;
            }
            if (load is LoadMomentRightEnd)//1E.4
            {
                LoadMomentRightEnd cl = load as LoadMomentRightEnd;
                beamForceCase = new MomentAtFarEnd(beam, cl.Mo);
                beamDeflectionCase = null;
            }
            return new BeamCase(beamForceCase, beamDeflectionCase);
        }

        #endregion



    }
}

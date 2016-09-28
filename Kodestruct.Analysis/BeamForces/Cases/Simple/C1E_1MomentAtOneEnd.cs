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

namespace Kodestruct.Analysis.BeamForces.Simple
{


    public class MomentAtOneEnd : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
        {

            const string CASE = "C1E_1";

            BeamSimple beam;
            double Mo, V;
            double L;
            bool ShearHasBeenCalculated;

            public MomentAtOneEnd(BeamSimple beam, double Mo)
            {
                this.beam = beam;
                L = beam.Length;
                this.Mo = Mo;
            }

            public double Moment(double X)
            {
                beam.EvaluateX(X);
                double Mx = Mo * (1.0 - X / L);
                    BeamEntryFactory.CreateEntry("Mx", Mx, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"Mo",Mo},
                         }, CASE, beam);
                return Mx;
            }

            public ForceDataPoint MomentMax()
            {
                if (Mo>=0)
                {
                    BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"Mo",Mo },
                         }, CASE, beam, true);
                     return new ForceDataPoint(0.0, Mo);
                }
                else
                {
                        BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                        null, CASE, beam, true);
                    return new ForceDataPoint(L, 0.0);
                }
               
            }

            public ForceDataPoint MomentMin()
            {
                if (Mo >= 0)
                {
                        BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                        null, CASE, beam, false, true);
                    return new ForceDataPoint(L, 0.0);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"Mo",Mo },
                         }, CASE, beam,false, true);
                    return new ForceDataPoint(0.0, Mo);
                }
            }

            public double Shear(double X)
            {
                if (ShearHasBeenCalculated==false)
                {
                    CalculateShear();
                }
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"Mo",Mo }
                         }, CASE, beam);
                return Math.Abs(V);
            }

            public ForceDataPoint ShearMax()
            {
                if (ShearHasBeenCalculated==false)
                {
                    CalculateShear();
                }
                BeamEntryFactory.CreateEntry("Vx", Math.Abs(V), BeamTemplateType.Vmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",0.0 },
                           {"Mo",Mo }
                         }, CASE, beam, true);
                return new ForceDataPoint(0.0, Math.Abs(V));
            }

            private void CalculateShear()
            {
                V = Mo / L;
                ShearHasBeenCalculated = true;
            }

            public double MaximumDeflection()
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;
                double delta_Maximum = ((0.0642 * Mo * L * L) / (E * I));
                return delta_Maximum;
            }
        }

}

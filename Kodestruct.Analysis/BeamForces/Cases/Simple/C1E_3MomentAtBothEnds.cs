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

    public class MomentAtBothEnds : ISingleLoadCaseBeam
        {
            const string CASE = "C1E_3";
            BeamSimple beam;
            double M1, M2;
            double L;
            bool ShearWasCalculated;

            public MomentAtBothEnds(BeamSimple beam, double M1, double M2)
            {
                this.beam = beam;
                L = beam.Length;
                this.M1 = M1;
                this.M2 = M2;
            }

            public double Moment(double X)
            {
                beam.EvaluateX(X);
                double M = (M2 - M1) * X / L + M1;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                return M;
            }

            public ForceDataPoint MomentMax()
            {
                if (M1>=M2)
                {
                       BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                        null, CASE, beam, true);
                       return new ForceDataPoint(0.0, M1);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                    null, CASE, beam, true);
                    return new ForceDataPoint(L, M2);
                }
            }

            public ForceDataPoint MomentMin()
            {
                  if (M1 <= M2)
                {
                    BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                     null, CASE, beam, false, true);
                    return new ForceDataPoint(0.0, M1);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                    null, CASE, beam, false, true);
                    return new ForceDataPoint(L, M2);
                }
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                //double V = (M2 - M1) / L;
                double V = CalculateShear();
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                return V;
            }

            public ForceDataPoint ShearMax()
            {
                double V = CalculateShear();
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                        null, CASE, beam, true);
                        return new ForceDataPoint(0.0, Math.Abs(V));
            }

            private double CalculateShear()
            {
                double V = (M2 - M1) / L;
                return V;
            }
        }

}

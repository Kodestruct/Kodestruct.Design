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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Analysis.BeamForces.Cantilever
{
    public class ConcentratedLoadAtTip : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C5A_1";

        BeamCantilever beam;
        double L;

        double P;

        public ConcentratedLoadAtTip(BeamCantilever beam, double P)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
        }

        public ForceDataPoint MomentMax()
        {

            double M;

            if (P < 0.0)
            {
                M = -P * L;


                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"P",P},
                           {"L",L},
                           {"X",L}
                        }, CASE, beam, true);

            }
            else
            {
                M = 0.0;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
            }
            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint MomentMin()
        {

            double M;

            if (P < 0.0)
            {
                M = 0.0;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);

            }
            else
            {
                M = -P * L;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L},
                           {"P",P},
                        }, CASE, beam, false, true);


            }
            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint ShearMax()
        {

            double V;
            V = Math.Abs(P);

            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"P",P},
                         }, CASE, beam, true);


            return new ForceDataPoint(L , V);
        }

        public double Moment(double X)
        {

            beam.EvaluateX(X);

            double M;

            M = P * X;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                        }, CASE, beam);



            return M;

        }


        public double Shear(double X)
        {

            beam.EvaluateX(X);
            double V = P;

            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);


            return V;
        }



        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;
            
            double delta = ((P * Math.Pow(L, 3)) / (3.0 * E * I));

            return delta;
        }
    }
}

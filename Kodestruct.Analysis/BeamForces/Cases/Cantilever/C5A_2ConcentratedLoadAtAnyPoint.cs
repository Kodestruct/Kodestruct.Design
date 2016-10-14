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

    public class ConcentratedLoadAtAnyPoint : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamCantilever beam;
        double P, a, b, L;
        const string CASE = "C5A_2";
        bool ShearForcesCalculated;

        public ConcentratedLoadAtAnyPoint(BeamCantilever beam, double P, double a)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
            if (a > L)
            {
                throw new LoadLocationParametersException(L, "a");
            }
            this.a = a;
            this.b = L - a;
            ShearForcesCalculated = false;
        }


        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            if (X <= a)
            {
                M = 0;
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.Mx, 1,
                new Dictionary<string, double>() { { "X", X } }, CASE, beam, true);
                
            }
            else
            {
                M = P * (X - a);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                 new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam);
            }
            return M;
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V;

            if (X<=a)
            {
                V = 0;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {{"X",X }}, CASE, beam);
            }
            else
            {
                V = P;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                    new Dictionary<string, double>()
                        { {"P",P },
                           {"X",X }
                         }, CASE, beam);
            }
            
            
            return V;
        }


        public ForceDataPoint MomentMax()
        {
            double M;

            if (P < 0.0)
            {
                M = -P * b;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                 new Dictionary<string, double>()
                        {  {"X",a },
                           {"P",P},
                           {"b",b }
                         }, CASE, beam, true);
            }
            else
            {
                M = 0.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);

            }
            return new ForceDataPoint(a, M);
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

                M = -P * b;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                 new Dictionary<string, double>()
                        {  {"X",a },
                           {"P",P},
                           {"b",b }
                         }, CASE, beam, false, true);
            }
            return new ForceDataPoint(a, M);
        }

        public ForceDataPoint ShearMax()
        {
            double V = Math.Abs(P);
                   BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {  {"X",L },
                           {"P",P}
                         }, CASE, beam);

            return new ForceDataPoint(a, V);
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta = ((P * Math.Pow(b, 2)) / (6.0 * E * I)) * (3.0 * L - b);


            return delta;
        }
    }

}

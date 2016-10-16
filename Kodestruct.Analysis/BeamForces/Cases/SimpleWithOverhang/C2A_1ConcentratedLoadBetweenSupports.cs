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
    public class ConcentratedLoadBetweenSupports : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamSimpleWithOverhang beam;
            const string CASE = "C2A_1";
            double P, a, b, L;

            //note for overhang beam a is the overhang with 
            //1 exception of concentrated load between supports
            public ConcentratedLoadBetweenSupports(BeamSimpleWithOverhang beam, double P, double a)
	        {
                this.beam = beam;
                L = beam.Length;
                this.P = P;
                this.a = a;
                this.b = L - a;
            }

            public double Moment(double X)
            {
                beam.EvaluateX(X);
                double M;
                if (X <= a)
                {
                    M = P * b * X / L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                     new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"b",b }
                         }, CASE, beam);
                }
                else
                {
                    if (X>L)
                    {
                        M = 0.0;
                    }
                    else
                    {
                        M = P * a * (L - X) / L;
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                         new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam); 
                    }
                }
                return M;
            }

            public double Shear(double X)
            {

                beam.EvaluateX(X);

                double V;

                if (X < a)
                {
                    V = GetShearLeft();
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"b",b }
                         }, CASE, beam);
                }
                else if (X > a)
                {
                    if (X > L)
                    {
                        V = 0.0;
                    }
                    else
                    {
                        V = GetShearRight();
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam);
                    }
                }
                else //right at the point of load
                {
                    if (a > b)
                    {
                        V = GetShearRight();
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam);
                    }
                    else 
                    {
                        V = GetShearLeft();
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"b",b }
                         }, CASE, beam);
                    }
                }

                return V;
            }

            private double GetShearRight()
            {
                double V = P * a / L;

                return V;
            }

            private double GetShearLeft()
            {
                double V = P * b / L;

                return V;
            }

            public ForceDataPoint MomentMax()
            {
                double M;

                if (P > 0.0)
                {
                    M = P * a * b / L;

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                     new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"a",a },
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

                if (P > 0.0)
                {

                    M = 0.0;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                    null, CASE, beam, false, true);
                }
                else
                {

                    M = P * a * b / L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                         }, CASE, beam, false, true);
                }
                return new ForceDataPoint(a, M);
            }

            public ForceDataPoint ShearMax()
            {
                double V;
                if (a > b)
                {
                    V = GetShearRight();
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"a",a },
                         }, CASE, beam, true);
                }
                else
                {
                    V = GetShearLeft();
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"b",b },
                         }, CASE, beam, true);
                }

                return new ForceDataPoint(a, V);
            }

            public double MaximumDeflection()
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double delta_Maximum = ((P * a * b * (a + 2 * b) * Math.Sqrt(3 * a * (a + 2 * b))) / (27 * E * I));

                return delta_Maximum;

            }
    }
}

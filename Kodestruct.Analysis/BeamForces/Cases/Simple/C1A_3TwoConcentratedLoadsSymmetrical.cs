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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Analysis.BeamForces.Simple
{



    public class TwoConcentratedLoadsSymmetrical : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
        {

            BeamSimple beam;
            double P;
            double L;
            double a;
            const string CASE = "C1A_3";

            public TwoConcentratedLoadsSymmetrical(BeamSimple beam, double P, double a)
            {
                this.beam = beam;
                this.a = a;
                this. P = P;
                L = beam.Length;
                if (a > L / 2.0)
                {
                    throw new LoadLocationOutOfBoundsException(L, a, "a");
                }
            }

            public double Moment(double X)
            {

                beam.EvaluateX(X);


                 double M;
                if (X <=a)
                {
                    M = P * X;

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);
                }
                else
                {
                    if (X>a && X<L-a) //between
                    {
                        M=P*a;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"P",P},
                           {"X",X },
                           {"a",a }
                         }, CASE, beam);

                    }
                    else
                    {
                        M = P * (L - X);
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P}
                         }, CASE, beam, true);
                    }
                    
                }

                return M;
            }

            public ForceDataPoint MomentMax()
            {

                double M;

                if (P>0.0)
                {
                    M = P * a;

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"a",a }
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
                    null, CASE, beam, false,true);                   
                }
                else
                {
                    M = P * a;

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",a },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam, false, true);

                }

                return new ForceDataPoint(a, M);
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                double V;
               
                if (X<=a || X>=(L-a))
                {
                   V = Math.Abs(P);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);
               
                }
                else
                {
                    V = 0;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                     new Dictionary<string, double>()
                        {
                           {"X",X },
                         }, CASE, beam);
                }
                return V;
            }

            public ForceDataPoint ShearMax()
            {

                double V = P;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                    null, CASE, beam, true);
                    return new ForceDataPoint(a,V);
            }


            public double MaximumDeflection()
            {
                double delta = 0.0;
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;
                delta = ((P * a) / (24 * E * I)) * (3 * L * L - 4 * a * a);

            BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.deltaMax, 0,
            new Dictionary<string, double>()
                                    {
                                       {"P",P},
                                       {"a",a },
                                       {"E",E},
                                       {"I",I },
                                       {"L",L}
                                     }, CASE, beam);

            return delta;
            }

            public double Deflection(double X)
            {
                double delta;
                int CaseId;

                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                if (X<a)
                {
                    delta = ((P * X) / (6.0 * E * I)) * (3.0 * L * a - 3.0 * a * a - X * X);
                    CaseId = 1;
                }
                else
                {
                    if (X<L-a)
                    {
                        delta = ((P * a) / (6 * E * I)) * (3 * L * X - 3 * X * X - a * a);
                        CaseId = 2;
                    }
                    else
                    {
                        delta = ((P * (L - X)) / (6 * E * I)) * (3 * L * a - 3 * a * a - (L - X) * (L - X));
                        CaseId = 3;
                    }
                }

                BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.delta, CaseId,
                new Dictionary<string, double>()
                                    {
                                       {"X",X },
                                       {"P",P},
                                       {"a",a },
                                       {"E",E},
                                       {"I",I },
                                       {"L",L}
                                     }, CASE, beam);

            return delta;

            }
        }

}

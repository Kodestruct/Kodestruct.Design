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
    public class ConcentratedLoadAtCenter : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C1A_1";

        BeamSimple beam;
        double L;
        double P;

        public ConcentratedLoadAtCenter(BeamSimple beam, double P)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
        }

        public ForceDataPoint MomentMax()
        {

            double M;

            if (P > 0.0)
            {
                M = P * L / 4.0;


                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"P",P},
                           {"L",L},
                           {"X",L / 2.0}
                        }, CASE, beam, true);

            }
            else
            {
                M = 0.0;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
            }
            return new ForceDataPoint(L / 2.0, M);
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
                M = P * L / 4.0;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L/2.0 },
                           {"P",P},
                        }, CASE, beam, false, true);


            }
            return new ForceDataPoint(L / 2.0, M);
        }

        public ForceDataPoint ShearMax()
        {

            double V;
            V = Math.Abs(P / 2.0);

            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                        {
                           {"X",0.0 },
                           {"P",P},
                         }, CASE, beam, true);


            return new ForceDataPoint(L / 2.0, V);
        }

        public double Moment(double X)
        {

            beam.EvaluateX(X);

            double M;

            if (X <= L / 2.0)
            {
                M = P * X / 2.0;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                        }, CASE, beam);

                return M;
            }
            else
            {
                M = P * (L - X) / 2.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                  new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);
            }

            return M;

        }


        public double Shear(double X)
        {

            beam.EvaluateX(X);
            double V = P / 2.0;

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

            double delta = (P * Math.Pow(L, 3)) / (48 * E * I);
            BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.deltaMax, 0,
            new Dictionary<string, double>()
            {
            {"P",P},
            {"E",E},
            {"I",I },
            {"L",L}
            }, CASE, beam);

            return delta;
        }

        public double Deflection(double X)
        {
            double delta;
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            int CaseId = 1;
            if (X<=L/2.0)
            {
                delta =(P*X)/(48*E*I)*(3*Math.Pow(L,2.0)-4*Math.Pow(X,2.0));
                CaseId = 1;
            }
            else
            {
                delta =(P*(L-X))/(48*E*I)*(3*Math.Pow(L,2.0)-4*Math.Pow(L-X,2.0));
                CaseId = 2;
            }
            BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.deltaMax, CaseId,
            new Dictionary<string, double>()
                                    {
                                       {"X",X },
                                       {"P",P},
                                       {"E",E},
                                       {"I",I },
                                       {"L",L}
                                     }, CASE, beam);

            return delta;
        }
    }
}

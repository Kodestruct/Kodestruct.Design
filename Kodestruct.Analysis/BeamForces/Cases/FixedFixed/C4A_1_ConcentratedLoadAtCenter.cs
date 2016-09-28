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

namespace Kodestruct.Analysis.BeamForces.FixedFixed
{
    public class ConcentratedLoadAtCenter : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C4A_1";

        BeamFixedFixed beam;
        double L;

        double P;

        public ConcentratedLoadAtCenter(BeamFixedFixed beam, double P)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"P",P}
                     };

            if (X<=L/2.0)
            {
                M = P / 8.0 * (4 * X - L);

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    valDic, CASE, beam);
            }
            else
            {
                M = P / 8.0 * (3 * L - 4*X);

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                valDic, CASE, beam);
            }
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            double Pabs = Math.Abs(P);
            double M = Pabs * L / 8.0;
            double X;

            if (P>=0.0)
            {
                X = L / 2.0;
            }
            else
            {
                X = 0.0;
            }
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"P",P},
                      };

            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
            valDic, CASE, beam, true);

            return new ForceDataPoint(X, M);
        }

        public ForceDataPoint MomentMin()
        {
            double Pabs = Math.Abs(P);
            double M = -Pabs * L / 8.0;
            double X;

            if (P <= 0.0)
            {
                X = L / 2.0;
            }
            else
            {
                X = 0.0;
            }
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"P",P},
                      };

            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
            valDic, CASE, beam, false, true);

            return new ForceDataPoint(X, M);
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

        public ForceDataPoint ShearMax()
        {
            double V = Math.Abs(P / 2.0);
            double X = L / 2.0;
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                {
                    {"X",X },
                    {"P",P},
                    }, CASE, beam, true);
            return new ForceDataPoint(X, V);
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta_Maximum = ((P * Math.Pow(L, 3)) / (192.0 * E * I));
            return delta_Maximum;
        }
    }
}

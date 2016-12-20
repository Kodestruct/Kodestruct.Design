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

namespace Kodestruct.Analysis.BeamForces.Cantilever
{

    public class UniformLoad : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamCantilever beam;
        double w;
        double L;
        const string CASE = "C5B_1";

        public UniformLoad(BeamCantilever beam, double w)
        {
            this.beam = beam;
            L = beam.Length;
            this.w = w;
        }

        public ForceDataPoint MomentMax()
        {

            double M;

            if (w < 0.0)
            {
                M = -w * Math.Pow(L, 2) / 2.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam, true);
            }
            else
            {
                M = 0.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
            }
            return new ForceDataPoint(L , M);
        }

        public ForceDataPoint MomentMin()
        {

            double M;

            if (w < 0.0)
            {
                M = 0.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);
            }
            else
            {
                M =- w * Math.Pow(L, 2) / 2.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                    {
                        {"L",L },
                        {"X",L },
                        {"w",w }
                        }, CASE, beam, false, true);

            }
            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint ShearMax()
        {
            double V;

            V = w * L ;
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam, true);
            return new ForceDataPoint(0.0, Math.Abs(V));
        }

        public double Moment(double X)
        {

            beam.EvaluateX(X);
            double M = w * X*X / 2.0 ;
            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam);

            return M;
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V = w * X;
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam);
            return V;
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta = ((w * Math.Pow(L, 4)) / (8.0 * E * I));
            return delta;

        }
    }

}

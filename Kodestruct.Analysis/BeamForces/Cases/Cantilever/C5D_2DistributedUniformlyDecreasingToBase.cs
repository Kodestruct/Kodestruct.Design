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

    public class DistributedUniformlyDecreasingToBase : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C5D_2";

        BeamCantilever beam;
        double w;
        double L;
        double W;
        bool ResultantCalculated;


        public DistributedUniformlyDecreasingToBase(BeamCantilever beam, double w)
        {
            this.beam = beam;
            L = beam.Length;
            this.w = w;
            ResultantCalculated = false;

        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M = w * Math.Pow(X, 2) / (6.0 * L)*(X-3*L);
            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                         }, CASE, beam);
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            double M, X;
            if (w < 0)
            {
                M = -w * L * L / 3.0;
                X = L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {  {"X",X },
                           {"L",L },
                           {"w",w }
                         }, CASE, beam, true);
                return new ForceDataPoint(L, M);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
                return new ForceDataPoint(0.0, 0.0);
            }
        }

        public ForceDataPoint MomentMin()
        {
            double M, X;
            if (w < 0)
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
                return new ForceDataPoint(0.0, 0.0);
            }
            else
            {
                M = -w * L * L / 3.0;
                X = L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {  {"X",X },
                           {"L",L },
                           {"w",w }
                         }, CASE, beam, false, true);
                return new ForceDataPoint(L, M);
            }
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V = w * X / (L)*(L-X/2.0);
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam);
            return V;
        }

        public ForceDataPoint ShearMax()
        {
            double Vmax;
            Vmax = w * L / 2.0;
            BeamEntryFactory.CreateEntry("Vx", Vmax, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam,true);
            return new ForceDataPoint(L, Vmax);
        }




        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta = ((11.0 * w * Math.Pow(L, 4)) / (120.0 * E * I));

            return delta;
        }
    }

}

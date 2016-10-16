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

namespace Kodestruct.Analysis.BeamForces.PinnedFixed
{
    public class MomentAtFreeEnd : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C3E_1";

        BeamPinnedFixed beam;
        double L, Mo;

        public MomentAtFreeEnd(BeamPinnedFixed beam, double Mo)
	    {
                this.beam = beam;
                this.L = beam.Length;
                this.Mo=Mo;
	    }


        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            M = Mo/(2.0*L)*(2.0*L-3.0*X);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"Mo",Mo }
                         }, CASE, beam);
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            if (Mo>=0.0)
            {
                BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                    {
                        {"X",0.0 }
                        }, CASE, beam, true);
                return new ForceDataPoint(0.0, Mo);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", Mo/2.0, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                    {
                        {"X",L }
                        }, CASE, beam, true);
                return new ForceDataPoint(L, Mo/2.0);
            }

        }

        public ForceDataPoint MomentMin()
        {
            if (Mo <= 0.0)
            {
                BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                    {
                        {"X",0.0 }
                        }, CASE, beam, false, true);
                return new ForceDataPoint(0.0, Mo);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", Mo / 2.0, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                    {
                        {"X",L}
                        }, CASE, beam, false, true);
                return new ForceDataPoint(L, Mo / 2.0);
            }
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                {
                    {"L",L },
                    {"X",X },
                    {"Mo",Mo }
                    }, CASE, beam);
            return V;
        }


        public double V
        {
            get {
                double V = 3.0 * Mo / (2.0 * L);
                return V; }
        }
        

        public ForceDataPoint ShearMax()
        {
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                {
                    {"L",L },
                    {"X",0.0 },
                    {"Mo",Mo }
                    }, CASE, beam, true);
            return new ForceDataPoint(0.0,V);
        }

        public double MaximumDeflection()
        {

            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;
            double delta_Maximum = ((Mo * Math.Pow(L, 2)) / (27.0 * E * I));
            return delta_Maximum;
        }
    }
}

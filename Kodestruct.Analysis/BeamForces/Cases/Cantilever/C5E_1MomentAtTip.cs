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


    public class MomentAtTip : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {

        const string CASE = "C5E_1";

        BeamCantilever beam;
        double Mo, V;
        double L;

        public MomentAtTip(BeamCantilever beam, double Mo)
        {
            this.beam = beam;
            L = beam.Length;
            this.Mo = Mo;
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double Mx = Mo;
            BeamEntryFactory.CreateEntry("Mx", Mx, BeamTemplateType.Mx, 1,
            new Dictionary<string, double>()
                        {  {"X",X },
                           {"Mo",Mo},
                         }, CASE, beam);
            return Mx;
        }

        public ForceDataPoint MomentMax()
        {
            if (Mo >= 0)
            {
                BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"Mo",Mo },
                         }, CASE, beam, true);
                return new ForceDataPoint(0.0, Mo);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
                return new ForceDataPoint(L, 0.0);
            }

        }

        public ForceDataPoint MomentMin()
        {
            if (Mo >= 0)
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);
                return new ForceDataPoint(L, 0.0);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", Mo, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"Mo",Mo },
                         }, CASE, beam, false, true);
                return new ForceDataPoint(0.0, Mo);
            }
        }

        public double Shear(double X)
        {
            double V = 0.0;
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                        { {"X",X }
                         }, CASE, beam);
            return V;
        }

        public ForceDataPoint ShearMax()
        {
            double V = 0.0;
            BeamEntryFactory.CreateEntry("Vx", Math.Abs(V), BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                        {  {"X",0.0 }
                         }, CASE, beam, true);
            return new ForceDataPoint(0.0, Math.Abs(V));
        }



        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta = ((Mo * Math.Pow(L, 2)) / (2 * I));

            return delta;

        }
    }

}

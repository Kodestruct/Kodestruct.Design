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
    public class ConcentratedLoadAtCenter : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C3A_1";

        BeamPinnedFixed beam;
        double L;
        double P;

        public ConcentratedLoadAtCenter(BeamPinnedFixed beam, double P)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            if (X<=L/2.0)
            {
                M = 5.0 / 16.0 * P * X;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);
            }
            else
            {
                M=P*(L/2.0-11.0/16.0*X);
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

        public ForceDataPoint MomentMax()
        {
            
            double M;
            if (P<0.0)
            {
                M = 3.0 / 16.0 * P * L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"P",P}
                         }, CASE, beam, true);
            }
            else
            {
                M = 5.0 / 32.0 * P * L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"P",P}
                         }, CASE, beam, true);
            }


            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint MomentMin()
        {
            double M;
            if (P > 0.0)
            {
                M = -3.0 / 16.0 * P * L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"P",P}
                         }, CASE, beam, false, true);
            }
            else
            {
                M = -5.0 / 32.0 * P * L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"P",P}
                         }, CASE, beam, true);
            }


            return new ForceDataPoint(L, M);
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V;
            int VCase =1;

            if (X<L/2.0)
	        {
                V = Math.Abs(V1);
                VCase = 1;
	        }
            else if (X>L/2.0)
	        {
                V = Math.Abs(V2);
                VCase = 2;
	        }
             else
	        {
                if (Math.Abs(V1)>Math.Abs(V2))
                {
                    V = Math.Abs(V1);
                    VCase = 1;
                }
                else
                {
                    V = Math.Abs(V2);
                    VCase = 2;
                }
	        }
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, VCase,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P}
                         }, CASE, beam);
            return V;
        }

        public ForceDataPoint ShearMax()
        {
            double V;
            int VCase = 1;
            //if (Math.Abs(V1) > Math.Abs(V2))
            //{
            //    V = V1;
            //    VCase = 1;
            //}
            //else
            //{
                V = Math.Abs(V2);
                VCase = 1;
            //}
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, VCase,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L/2.0 },
                           {"P",P},
                         }, CASE, beam,true);
            return new ForceDataPoint(L / 2.0, V);
        }


        public double V1
        {
            get { 
                
                return 5.0/16.0*P; }
        }

        public double V2
        {
            get
            {

                return 11.0 / 16.0 * P;
            }
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;
            double delta_Maximum = ((P * Math.Pow(L, 3)) / (48.0 * E * I * Math.Sqrt(5.0)));
            return delta_Maximum;
        }
    }
}

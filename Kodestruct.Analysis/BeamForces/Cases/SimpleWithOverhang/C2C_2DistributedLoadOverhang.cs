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
    public class DistributedLoadOverhang : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamSimpleWithOverhang beam;
        const string CASE = "C2C_2";
        double w, a, L, X1;
        bool X1Calculated;

        //note for overhang beam a is the overhang with 
        //1 exception of concentrated load between supports
        public DistributedLoadOverhang(BeamSimpleWithOverhang beam, double w)
        {
            this.beam = beam;
            L = beam.Length;
            this.w = w;
            this.a = beam.OverhangLength;
            X1Calculated = false;
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;

            if (X <= L)
            {

                M = (w * a * a * X) / (2 * L);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"a",a },
                           {"w",w }
                         }, CASE, beam);

                return M;
            }
            else
            {
                if (X1Calculated == false)
                {
                    CalculateX1(X);
                }
                M = w / 2 * Math.Pow(a - X1, 2);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                new Dictionary<string, double>()
                        {
                           {"X1",X1 },
                           {"a",a },
                           {"w",w }
                         }, CASE, beam);

                return M;
            }

            
        }

        public ForceDataPoint MomentMax()
        {
            double M;
            if (w>0.0)
            {
                M = w * a * a / 2.0;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, true);
            }
            else
            {
                        BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                        null, CASE, beam, true);
                        M = 0.0;
            }
            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint MomentMin()
        {
            double M;
            if (w < 0.0)
            {
                M = w * a * a / 2.0;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, false, true);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);
                M = 0.0;
            }
            return new ForceDataPoint(L, M);
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V1 = CalculateV1();
            double V2 = CalculateV2();
            if (X<L)
            {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam);
                return V1;
            }
            else if (X>L)
            {
                if (X1Calculated==false)
	            {
		             CalculateX1(X);
	            }
                double V = w * (a - X1);
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                new Dictionary<string, double>()
                        {
                           {"X1",X1 },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam);
                return V;
            }
            else //right at the support
            {
                if (V1 > V2)
                {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam);
                    return V1;
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vx, 2,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam);
                    return V2;
                }
            }
        }

        public ForceDataPoint ShearMax()
        {
            double V1 = Math.Abs(CalculateV1());
            double V2 = Math.Abs(CalculateV2());

                if (V1 > V2)
                {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam, true);
                    return  new ForceDataPoint(L, V1);
                }
                else 
                {
                    BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam, true);
                    return new ForceDataPoint(L,V2);
                }
            
        }

        private void CalculateX1(double X)
        {
            X1 = X - L;
            BeamEntryFactory.CreateEntry("X1", X1, BeamTemplateType.Mmax, 1,
            new Dictionary<string, double>()
                {
                    {"L",L },
                    {"X1",X1 },
                    }, CASE, beam, true);
            X1Calculated = true;
        }

        private double CalculateV1()
        {
            double V = w * a * a / (2.0 * L);
            return V;
        }
        private double CalculateV2()
        {
            double V = w * a;
            return V;
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;
            double delta_Maximum = ((w * Math.Pow(a, 3)) / (24.0 * E * I)) * (4.0 * L + 3.0 * a);
            return delta_Maximum;
        }
    }
}

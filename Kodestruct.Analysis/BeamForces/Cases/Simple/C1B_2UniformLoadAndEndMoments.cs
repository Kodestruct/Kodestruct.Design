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
using MoreLinq;

namespace Kodestruct.Analysis.BeamForces.Simple
{

    public class UniformLoadAndEndMoments : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
        {
            BeamSimple beam;
            double L, w, M1, M2;
            private ForceDataPoint _Mx;
            double E, I;
            const string CASE = "C1B_2";

            public ForceDataPoint Mx
            {
                get 
                {
                    if (_Mx==null)
                    {
                        _Mx = CalculateMidspanMoment();
                    }
                    return _Mx; 
                }

            }

            public UniformLoadAndEndMoments(BeamSimple beam, double w, double M1, double M2)
	        {
                    this.beam = beam;
                    this.L = beam.Length;
                    this.w = w;
                    this.M1 = M1;
                    this.M2 = M2;
	        }

            public double Moment(double X)
            {
                beam.EvaluateX(X);
                double M;
                M=w*X/2.0*(L-X)+((M1-M2)/L)*X-M1;

                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                return M;
            }

            public ForceDataPoint MomentMax()
            {
                List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                {
                    new ForceDataPoint(0.0,M1),
                    Mx,
                    new ForceDataPoint(L,M2)
                };
                var MaxMoment = Moments.MaxBy(m => m.Value);
                AddGoverningMomentEntry(MaxMoment.Value, true, false);
                return MaxMoment;
            }


            public ForceDataPoint MomentMin()
            {
                List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                {
                    new ForceDataPoint(0.0,M1),
                    Mx,
                    new ForceDataPoint(L,M2)
                };
                var MinMoment = Moments.MinBy(m => m.Value);
                AddGoverningMomentEntry(MinMoment.Value, false, true);
                return MinMoment;
            }

            private double GetMidPointPeakLocation()
            {
                double XMaxMid= L/2.0+(M1+M2)/(w*L);
                return XMaxMid;
            }

            private void AddGoverningMomentEntry(double GoverningMoment, bool IsMax, bool IsMin)
            {
                double X;
                if (GoverningMoment ==M1)
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.Mmax, 1,
                    null, CASE, beam, IsMax, IsMin);
                    X = 0;
                }
                else if (GoverningMoment == M2)
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.Mmax, 3,
                    null, CASE, beam, IsMax, IsMin);
                    X = L;
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",GetMidPointPeakLocation() },
                           {"w",w },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam, IsMax, IsMin);
                }
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                double V;
                V = w * (L / 2.0 - X) + (M1 - M2) / L;

                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                return V;
            }

            public ForceDataPoint ShearMax()
            {
               //End Shears
                double V1 = w*L/2.0+(M1-M2)/L;
                double V2 = w * L / 2.0 - (M1 - M2) / L;

                if (Math.Abs(V1)> Math.Abs(V2))
                {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",0.0},
                           {"w",w },
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam, true, false);
                    return new ForceDataPoint(0.0, Math.Abs(V1));
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L},
                           {"X",L},
                           {"w",w},
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam, true, false);
                    return new ForceDataPoint(L, Math.Abs(V2));
                }

            }

            public ForceDataPoint CalculateMidspanMoment()
            {
                double M3, XM3;
                M3 = w*Math.Pow(L,2.0)/8.0-((M1+M2)/2.0)+(Math.Pow(M1-M2,2.0)/(2.0*w*Math.Pow(L,2.0)));
                XM3=L/2.0+(M1-M2)/(w*L);
                return new ForceDataPoint(XM3, M3);
            }

            public double MaximumDeflection()
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double NSteps = 30;
                double Step = beam.Length / NSteps;
                double delta = 0.0;
                for (int i = 0; i < NSteps; i++)
                {
                    double x = Step*i;
                    double thisDelta = Deflection(x);
                    if (Math.Abs(thisDelta)>delta)
                    {
                        delta = Math.Abs(thisDelta);
                    }
                }
                BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.deltaMax, 0,
                new Dictionary<string, double>()
                                        {
                                           {"M1",M1 },
                                           {"M2",M2},
                                           {"w",w },
                                           {"E",E},
                                           {"I",I },
                                           {"L",L}
                                         }, CASE, beam);

                return delta;
            }

            public double Deflection(double X)
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double delta = ((w * X) / (24.0 * E * I)) * (Math.Pow(X, 3) - (2.0 * L + ((4.0 * M1) / (w * L)) - ((4.0 * M2) / (w * L))) * X * X + ((12.0 * M1) / (w)) * X + Math.Pow(L, 3) - ((8.0 * M1 * L) / (w)) - ((4.0 * M2 * L) / (w)));
                        BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.delta, 0,
                        new Dictionary<string, double>()
                                                {
                                                   {"X",X },
                                                   {"M1",M1},
                                                   {"M2",M2},
                                                   {"w",w },
                                                   {"E",E},
                                                   {"I",I },
                                                   {"L",L}
                                                 }, CASE, beam);

                        return delta;
            
            }
        }

}

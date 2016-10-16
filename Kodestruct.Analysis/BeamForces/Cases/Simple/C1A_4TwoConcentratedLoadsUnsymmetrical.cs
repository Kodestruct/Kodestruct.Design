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

namespace Kodestruct.Analysis.BeamForces.Simple
{

        public class TwoConcentratedLoadsUnsymmetrical :ISingleLoadCaseBeam
        {

            BeamSimple beam;
            double L, a, b, P1, P2;

            //common values
            double R1, R2, M1, M2;
            bool MomentsM1M2Calculated;
            const string CASE = "C1A_4";

            public TwoConcentratedLoadsUnsymmetrical(BeamSimple beam, double P1, double P2, double a, double b)
            {
                    this.beam = beam;
                    L = beam.Length;
                    this.a = a;
                    this.b = b;
                    this.P1 = P1;
                    this.P2 = P2;
                    if (a + b > L)
                    {
                        throw new LoadLocationParametersException(L, "a", "b");
                    }
                    CalculateReactions();
                    MomentsM1M2Calculated = false;
            }
            
            public double Moment( double X)
            {
                beam.EvaluateX(X);
                double M;

                if (X<=a)
                {
                    M = R1 * X; //Case1
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"R1",R1}
                         }, CASE, beam);
                }
                else
                {
                        if (X>=L-b)
                        {
                            M = R2 * (L-X); //Case 3
                            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                            new Dictionary<string, double>()
                                {
                                   {"L",L },
                                   {"X",X },
                                   {"R2",R2},
                                 }, CASE, beam);
                        }
                        else // (X>a&& X<(L-b))
                        {
                            M = R1 * X - P1 * (X - a); //Case 2
                            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                            new Dictionary<string, double>()
                                {
                                   {"L",L },
                                   {"X",X },
                                   {"a",a },
                                   {"R1",R1},
                                   {"P1",P1 },
                                 }, CASE, beam);
                        }
                }
                return M;
            }

            private double GetM1()
            {
                M1 = R1 * a;
                return M1;
            }

            private double GetM2()
            {
                M2 = R2 * b;
                return M2;
            }

            public ForceDataPoint MomentMax()
            {
                if (MomentsM1M2Calculated==false)
                {
                    M1 = GetM1();
                    M2 = GetM2();
                }
                if (M1>0 || M2>0)
                {
                    if (M1 >= M2)
                    {
                        BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                        new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",a },
                           {"a",a }
                         }, CASE, beam, true);
                        return new ForceDataPoint(a, M1);
                    }
                    else
                    {
                        BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",b },
                           {"b",b },
                           {"R2",R2}
                         }, CASE, beam, true);
                        return new ForceDataPoint(L - b, M2);
                    } 
                }
                else
                {
                    double M = 0.0;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                    null, CASE, beam, true);
                    return new ForceDataPoint(0, M);
                }

            }

            public ForceDataPoint MomentMin()
            {
                if (M1<0 || M2<0)
                {
                    if (M1 < M2)
                    {
                        BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                        new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",a },
                           {"a",a }
                         }, CASE, beam, false, true);
                        return new ForceDataPoint(a, M1);
                    }
                    else
                    {
                        BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"b",b },
                           {"X",b },
                           {"R2",R2}
                         }, CASE, beam, false, true);
                        return new ForceDataPoint(L - b, M2);
                    } 
                }
                else
                {
                    double M = 0.0;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.M_zero, 0,
                    null, CASE, beam, false, true); //added min switch
                    return new ForceDataPoint(0, M);
                }
            }

            public double Shear( double X)
            {
                beam.EvaluateX(X);
                double V;
                if (X<=a) //left hand portion
                {
                    if (X==a)
                    {
                        if (Math.Abs(R1)>Math.Abs(R1 - P1))
                        {
                            V = Math.Abs(R1);
                            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                            new Dictionary<string, double>()
                                {
                                   {"R1",R1 },
                                   {"X",X }
                                 }, CASE, beam);
                        }
                        else
                        {
                            V = Math.Abs(R1 - P1);
                            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                            new Dictionary<string, double>()
                                {
                                   {"R1",R1 },
                                   {"P1",P1 },
                                   {"X",X }
                                 }, CASE, beam);
                        }
                    }
                    else
                    {
                        V = Math.Abs(R1);
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                        new Dictionary<string, double>()
                                {
                                   {"R1",R1 },
                                   {"X",X }
                                 }, CASE, beam);
                    }
                    
                }
                else
                {
                    if (X>=L-b) //right-hand portion
                    {
                        if (X ==L-b)
                        {
                            if (Math.Abs(R2) > Math.Abs(R1 - P1))
                            {
                                V = Math.Abs(R2);
                                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 3,
                                new Dictionary<string, double>()
                                {
                                   {"R2",R2 },
                                   {"X",X }
                                 }, CASE, beam);
                            }
                            else
                            {
                                V = Math.Abs(R1 - P1);
                                BeamEntryFactory.CreateEntry("V", V, BeamTemplateType.Vx, 2,
                                new Dictionary<string, double>()
                                {
                                   {"R1",R1 },
                                   {"P1",P1 },
                                   {"X",X }
                                 }, CASE, beam);
                            }
                        }
                        else
                        {
                            V = Math.Abs(R2);
                            BeamEntryFactory.CreateEntry("V", V, BeamTemplateType.Vx, 3,
                            new Dictionary<string, double>()
                                {
                                   {"R2",R2 },
                                   {"X",X }
                                 }, CASE, beam);
                        }
                    }
                    else //middle portion
                    {
                        V = Math.Abs(R1 - P1);
                        BeamEntryFactory.CreateEntry("V", V, BeamTemplateType.Vx, 2,
                        new Dictionary<string, double>()
                                {
                                   {"R1",R1 },
                                   {"P1",P1 },
                                   {"X",X }
                                 }, CASE, beam);
                    }
                }
                return V;
            }

            public ForceDataPoint ShearMax()
            {
                double R1a = Math.Abs(R1);
                double R2a = Math.Abs(R2);
                if (R1a > R2a)
                {
                    BeamEntryFactory.CreateEntry("Vx", R1a, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1a },
                           {"X",0 },
                         }, CASE, beam, true);
                    return new ForceDataPoint(a, R1a);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Vx", R1a, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"R2",R2a },
                           {"X",L },
                         }, CASE, beam, true);
                    return new ForceDataPoint(L - b, R2a);
                }

            }

            private void CalculateReactions()
            {
                 R1 = (P1 * (L - a) + P2 * b) / L;
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"P1",P1},
                           {"P2",P2},
                           {"a",a },
                           {"b",b }
                         }, CASE, beam);
                 R2 = (P1 * a + P2 * (L - b)) / L;
                 BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"P1",P1},
                           {"P2",P2},
                           {"a",a },
                           {"b",b }
                         }, CASE, beam);

            }

        }

}

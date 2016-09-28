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

    public class UniformPartialLoad : ISingleLoadCaseBeam
            {
                const string CASE = "C1C_1";

                BeamSimple beam;
                double w, L, a, b, c;
                double R1, R2;
                bool ReactionStateIsUpdated;
                ForceDataPoint Mmax;

                public UniformPartialLoad(BeamSimple beam, double w, double a, double b)
                {
                    this.beam = beam;
                    L = beam.Length;
                    this.w = w;
                    this.a = a;
                    this.b = b;
                    this.c =L-a-b;
                    if (a + b + c > L)
                    {
                        throw new LoadLocationParametersException(L, "a", "b");
                    }
                    this.ReactionStateIsUpdated = false;
                }

                public double Moment(double X)
                {
                    beam.EvaluateX(X);
                    double M;
                    if (ReactionStateIsUpdated ==false)
                    {
                        CalculateReactions();
                    }
                    
                    if (X<=a)
                    {
                        //left-hand segment
                        M = R1 * X;
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                        new Dictionary<string, double>()
                            {
                               {"L",L },
                               {"X",X },
                               {"R1",R1}
                             }, CASE, beam);
                    }
                    else
                    {
                        if (X>=L-b)
                        {
                            //right hand side segment
                            M = R2 * (L - X);
                            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                            new Dictionary<string, double>()
                            {
                               {"L",L },
                               {"X",X },
                               {"R2",R2}
                             }, CASE, beam);
                        }
                        else
                        {
                            //middle segment
                            M = R1 * X - w / 2.0 * Math.Pow(X - a, 2);
                            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                            new Dictionary<string, double>()
                            {
                               {"L",L },
                               {"X",X },
                               {"w",w },
                               {"a",a },
                               {"R1",R1}
                             }, CASE, beam);
                        }
                    }
                    return M;
                }

                public ForceDataPoint MomentMax()
                {
                    if (ReactionStateIsUpdated == false)
                    {
                        CalculateReactions();
                    }
                    if (Mmax==null)
                    {
                        Mmax=CalculateMmax();
                    }
                    if (w>=0)
                    {
                        BeamEntryFactory.CreateEntry("Mx", Mmax.Value, BeamTemplateType.Mmax, 1,
                        new Dictionary<string, double>()
                            {
                               {"L",L },
                               {"X",Mmax.X },
                               {"w",w },
                               {"a",a },
                               {"R1",R1 },
                               {"c",c },
                             }, CASE, beam, true);
                        return Mmax;
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
                    if (ReactionStateIsUpdated == false)
                    {
                        CalculateReactions();
                    }
                    if (Mmax == null)
                    {
                        Mmax=CalculateMmax();
                    }

                    if (w >= 0)
                    {
                        BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                        null, CASE, beam, false, true);
                        return new ForceDataPoint(0.0, 0.0);
                    }
                    else
                    {
                        BeamEntryFactory.CreateEntry("Mx", Mmax.Value, BeamTemplateType.Mmax, 1,
                        new Dictionary<string, double>()
                            {
                               {"L",L },
                               {"X",Mmax.X },
                               {"w",w },
                               {"a",a },
                               {"b",b },
                               {"c",c },
                             }, CASE, beam, false, true);
                        return Mmax;
                    }
                }

                public double Shear(double X)
                {
                    beam.EvaluateX(X);
                    if (ReactionStateIsUpdated == false)
                    {
                        CalculateReactions();
                    }
                    double V;
                    
                    if (X <= a)
                    {
                        //left-hand segment
                        V = R1;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"R1",R1 },
                         }, CASE, beam);
                    }
                    else
                    {
                        if (X >= L - b)
                        {
                            //right hand side segment
                            V = R2;
                            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 3,
                            new Dictionary<string, double>()
                            {
                                {"L",L },
                                {"X",X },
                                {"R2",R2 },
                                }, CASE, beam);
                        }
                        else
                        {
                            //middle segment
                            V = R1 - w * (X - a);
                            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                            new Dictionary<string, double>()
                                {
                                   {"L",L },
                                   {"X",X },
                                   {"R1",R1 },
                                   {"w",w},
                                   {"a",a }
                                 }, CASE, beam);
                        }
                    }
                    return V;
                }

                public ForceDataPoint ShearMax()
                {
                    ForceDataPoint V;
                    double V1 = Math.Abs(R1);
                    double V2 = Math.Abs(R2);
                    if (V1>V2)
                    {
                        V = new ForceDataPoint(0.0,V1);
                            BeamEntryFactory.CreateEntry("Vx", V.Value, BeamTemplateType.Vmax, 1,
                            new Dictionary<string, double>()
                                {
                                   {"R1",V1 },
                                   {"X",0 },
                                 }, CASE, beam, true);
                    }
                    else
                    {
                        V = new ForceDataPoint(L, V2);
                            BeamEntryFactory.CreateEntry("Vx", V.Value, BeamTemplateType.Vmax, 2,
                            new Dictionary<string, double>()
                                {
                                   {"R2",V2 },
                                   {"X",0 },
                                 }, CASE, beam, true);

                    }
                    return V;
                }

                private void CalculateReactions()
                {
                    Dictionary<string,double>ValDic = new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                         };
                    R1  = w*b/(2.0*L)*(2.0*c+b);
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    ValDic, CASE, beam);

                    R2 = w*b/(2.0*L)*(2.0*a+b);
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    ValDic, CASE, beam);

                    ReactionStateIsUpdated = true;
                }

                private ForceDataPoint CalculateMmax()
                {
                    double Mmax = R1 * (a + R1 / (2.0 * w));
                    double XMax = a + R1 / w;

                    ForceDataPoint fdp = new ForceDataPoint(XMax, Mmax);
                    return fdp;
                }


              //private double GetTermXAn(double X, double a,double n)
              //  {
              //      if (X>a)
              //      {
              //          return Math.Pow(X - a, n);
              //      }
              //      else
              //      {
              //          return 0.0;
              //      }
              //  }
            }

}

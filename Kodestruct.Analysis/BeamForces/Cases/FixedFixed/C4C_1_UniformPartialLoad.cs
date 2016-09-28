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

namespace Kodestruct.Analysis.BeamForces.FixedFixed
{
    public class UniformPartialLoad: ISingleLoadCaseBeam
    {
            BeamFixedFixed beam;
            double w, L, a, b, c, d,e;
            double R1, R2;
            ForceDataPoint M1, M2, Mmid;
            bool ReactionsWereCalculated;
            const string CASE = "C4C_1";
            bool MomentsWereCalculated;

            public UniformPartialLoad(BeamFixedFixed beam, double w, double a, double b)
	    {
                this.beam = beam;
                L = beam.Length;
                this.w = w;
                this.a = a;
                this.b = b;
                this.c = L - a - b;
                this.d = a + b / 2;
                this.e = L - d;
                if (a + b + c > L)
                {
                    throw new LoadLocationParametersException(L, "a", "b");
                }
                this.ReactionsWereCalculated = false;
                MomentsWereCalculated = false;
	    }

            public double Moment(double X)
            {
                beam.EvaluateX(X);

                if (ReactionsWereCalculated == false) CalulateReactions();
                if (MomentsWereCalculated == false) CalculateMoments();
                double M;

                if (X<a)
                {
                    M = M1.Value + R1 * X;
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                        new Dictionary<string, double>()
                            {
                               {"M1",M1.Value },
                               {"X",X },
                               {"R1",R1 }
                             }, CASE, beam);
                }
                else if (X>a+b)
                {
                    M = M2.Value + R2 * (L - X);
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                        new Dictionary<string, double>()
                            {
                               {"M2",M2.Value },
                               {"X",X },
                               {"R2",R2 }
                             }, CASE, beam);
                }
                else
                {
                    M=M1.Value+R1*X-w/2*Math.Pow(X-a,2);
                        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                        new Dictionary<string, double>()
                            {
                               {"M1",M1.Value },
                               {"X",X },
                               {"a",a },
                               {"w",w },
                               {"R1",R1 }
                             }, CASE, beam);
                }
                return M;
            }


            public ForceDataPoint MomentMax()
            {
                if (ReactionsWereCalculated == false) CalulateReactions();
                if (MomentsWereCalculated == false) CalculateMoments();

                List<ForceDataPoint> Moments = new List<ForceDataPoint>
                {
                    M1,M2,Mmid
                };
                ForceDataPoint Mmaximum = Moments.MaxBy(m => m.Value);
                this.AddMomentEntry(Mmaximum, true, false);
                return Mmaximum;
            }

            public ForceDataPoint MomentMin()
            {
                if (ReactionsWereCalculated == false) CalulateReactions();
                if (MomentsWereCalculated == false) CalculateMoments();

                List<ForceDataPoint> Moments = new List<ForceDataPoint>
                {
                    M1,M2,Mmid
                };
                ForceDataPoint Mminimum = Moments.MinBy(m => m.Value);
                this.AddMomentEntry(Mminimum, false, true);
                return Mminimum;
            }

            private void AddMomentEntry(ForceDataPoint M, bool IsMax, bool IsMin)
            {

                if (M==M1)
                {
                    BeamEntryFactory.CreateEntry("Mx", M.Value, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",M.X },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                           {"d",d},
                           {"e",e}
                         }, CASE, beam, IsMax,IsMin);
                }
                else if (M==M2)
                {
                    BeamEntryFactory.CreateEntry("Mx", M.Value, BeamTemplateType.Mmax, 3,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",M.X },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                           {"d",d},
                           {"e",e}
                         }, CASE, beam, IsMax, IsMin);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", M.Value, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"M1",L },
                           {"X",M.X },
                           {"w",w },
                           {"a",a },
                           {"R1",R1 }
                         }, CASE, beam, IsMax, IsMin);
                }

            }

            public double Shear(double X)
            {
                if (ReactionsWereCalculated == false) CalulateReactions();
                if (MomentsWereCalculated == false) CalculateMoments();
                double V;
                if (X<=a)
                {
                    V = Math.Abs(R1);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X }
                          }, CASE, beam);
                }
                else if (X>=b)
                {
                    double Vmid = R1 - w * (X - a);
                    V = Math.Abs(Vmid);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"w",w },
                           {"a",a },
                           {"X",X },
                          }, CASE, beam);
                }
                else
                {
                    V = Math.Abs(R2);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 3,
                    new Dictionary<string, double>()
                        {
                           {"R2",R2 },
                           {"X",X }
                          }, CASE, beam);
                }
                return V;
            }

            public ForceDataPoint ShearMax()
            {
                if (ReactionsWereCalculated == false) CalulateReactions();
                if (MomentsWereCalculated == false) CalculateMoments();
                double V1 = Math.Abs(R1);
                double V2 = Math.Abs(R2);
                if (V1>=V2)
                {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",0.0}
                         }, CASE, beam,true);
                    return new ForceDataPoint(0, V1);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",0.0}
                         }, CASE, beam, true);
                    return new ForceDataPoint(0, V2);
                }
            }


            private void CalculateMoments()
            {

                double M1v =(w*b)/(24.0*L*L )*(b*b*(L+3.0*(c-a))-24.0*e*e*d);
                M1 = new ForceDataPoint(0.0, M1v);
                    BeamEntryFactory.CreateEntry("M1", M1v, BeamTemplateType.M, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                           {"d",d},
                           {"e",e}
                         }, CASE, beam);

                double M2v = R1 * L - w * b * e + M1v;
                M2 = new ForceDataPoint(L, M2v);

                BeamEntryFactory.CreateEntry("M2", M2v, BeamTemplateType.M, 2,
                new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"M1",M1v },
                           {"L",L },
                           {"w",w },
                           {"b",b },
                           {"c",c },
                           {"e",e}
                         }, CASE, beam);
                double MmidV = M1v+R1*(a+R1/(2.0*w));
                double XmidV = a+R1/w;
                
                Mmid = new ForceDataPoint(XmidV, MmidV);
                
                MomentsWereCalculated = true;
            }

            private void CalulateReactions()
            {

                    R1 = w*b/(4.0*Math.Pow(L,3))*(4.0*e*e*(L+2.0*d)-b*b*(c-a));   

                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    new Dictionary<string, double>()
                    {   {"L",L },
                        {"w",w },
                        {"a",a },
                        {"b",b },
                        {"c",c },
                        {"d",d },
                        {"e",e }
                        }, CASE, beam);

                    R2 = w * b - R1;
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    new Dictionary<string, double>()
                    {   {"R1",R1 },
                        {"w",w },
                        {"b",b }
                        }, CASE, beam);

                    ReactionsWereCalculated =true;

            }
    }
}

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

namespace Kodestruct.Analysis.BeamForces.SimpleWithOverhang
{
    public class UniformLoadFull : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamSimpleWithOverhang beam;
        const string CASE = "C2B_1";
        double w, a, L, X1;
        bool X1Calculated;
        
        //note for overhang beam a is the overhang with 
        //1 exception of concentrated load between supports
        public UniformLoadFull(BeamSimpleWithOverhang beam, double w)
        {
            this.beam = beam;
            L = beam.Length;
            this.w = w;
            this.a = beam.OverhangLength;
            X1Calculated = false;
        }

        private void CalculateX1(double X)
        {
            X1 = X1 - L;
            BeamEntryFactory.CreateEntry("X1", X1, BeamTemplateType.Mmax, 1,
            new Dictionary<string, double>()
                {
                    {"L",L },
                    {"X",X },
                    }, CASE, beam, true);
            X1Calculated = true;
        }


        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            

            if (X<=L) //Case between supports
            {
                M = w * X / (2.0 * L) * (L * L - a * a - X * L);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam);
            }
            else
            {
                
                if (X1Calculated == false)
                {
                    CalculateX1(X);
                }
                M = w / 2 * Math.Pow(a - X1, 2); //overhang case
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X1",X1 },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam);
            }

            return M;
        }

        public ForceDataPoint MomentMax()
        {
            double M1 = CalculateM1();
            double M2 = CalculateM2();

            if (M1 < 0.0 && M2 < 0.0)
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
                return new ForceDataPoint(L, 0.0);
            }
            else
            {
                if (M1 > M2)
                {
                    double Xmax = CalculateM1_X();

                    BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",Xmax },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam, true);

                    return new ForceDataPoint(Xmax, M1);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, true);
                    return new ForceDataPoint(L, M2);
                }
            }

        }

        public ForceDataPoint MomentMin()
        {
            double M1 = CalculateM1();
            double M2 = CalculateM2();

            if (M1 > 0.0 && M2 > 0.0)
            {
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);
                return new ForceDataPoint(L, 0.0);
            }
            else
            {
                if (M1 < M2)
                {
                    double Xmax = CalculateM1_X();

                    BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",Xmax },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam, false, true);
                    return new ForceDataPoint(Xmax, M1);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"X",L },
                           {"L",L },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, false, true);
                    return new ForceDataPoint(L, M2);
                }
            }
        }

        public double Shear(double X)
        {
            double V;
            beam.EvaluateX(X);

            if (X<L)
            {
                V = w / (2.0 * L) * (Math.Pow(L,2.0) - Math.Pow(a,2.0)) - w * X;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                           }, CASE, beam);
            }
            else
            {
                if (X1Calculated == false)
                {
                    CalculateX1(X);
                }
                V = w * (a - X1);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                    new Dictionary<string, double>()
                        {
                           {"X1",X1 },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam);
            }
            return V;
        }

        public ForceDataPoint ShearMax()
        {
              double V1 = Math.Abs(CalculateV1());
              double V2 = Math.Abs(CalculateV1());
              double V3 = Math.Abs(CalculateV1());

            List<ForceDataPoint> ShearList = new List<ForceDataPoint>()
            {
                new ForceDataPoint(0.0, V1),
                new ForceDataPoint(L, V2),
                new ForceDataPoint(L, V3)
            };

            var MaxShearPoint = ShearList.MaxBy(v => v.Value);

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"w",w},
                           {"a",a },
                     };

            if (MaxShearPoint.Value==V1)
            {
                BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 1, valDic, CASE, beam);
            }
            else if (MaxShearPoint.Value==V2)
            {
                BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 2, valDic, CASE, beam);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 3, valDic, CASE, beam);
            }

            return MaxShearPoint;
        }

        private void AddShearEntry(ForceDataPoint shearCase)
        {
            double V = shearCase.Value;
            double X = shearCase.X;
            if (V==Math.Abs(CalculateV1()))
	        {
		            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, true);
	        }
            else if (V ==Math.Abs( CalculateV2()))
            {
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 2,
                new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, true);
            }
            else // (V == CalculateV3())
            {
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 3,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"a",a },
                         }, CASE, beam, true);
            }
        }

        private double CalculateM1()
        {
            double M=w/(8.0*L*L)*Math.Pow(L+a,2)*Math.Pow(L-a,2);
            return M;
        }
        private double CalculateM2()
        {
            double M = -w * a * a / 2.0;
            return M;
        }

        private double CalculateM1_X()
        {
            double X = L/2*(1.0-Math.Pow(a,2)/Math.Pow(L,2.0));
            return X;
        }

        private double CalculateV1()
        {
           double V1 = w/(2.0*L)*(Math.Pow(L,2.0)-Math.Pow(a,2.0));
           return V1;
        }
        private double CalculateV2()
        {
            double V2 = w * a;
            return V2;
        }
        private double CalculateV3()
        {
            double V3 = w / (2.0 * L) * (Math.Pow(L, 2.0) + Math.Pow(a, 2.0));
            return V3;
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;
            double delta_Maximum = ((5.0 * w * Math.Pow(L, 4)) / (384.0 * E * I));
            return delta_Maximum;
        }
    }
}

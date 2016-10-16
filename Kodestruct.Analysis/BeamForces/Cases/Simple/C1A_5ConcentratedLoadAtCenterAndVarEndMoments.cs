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
using MoreLinq;

namespace Kodestruct.Analysis.BeamForces.Simple
{

    public class ConcentratedLoadAtCenterAndVarEndMoments : ISingleLoadCaseBeam
        {
            BeamSimple beam;
            double L, P, M1, M2;
            private ForceDataPoint _Mmid;
            const string CASE = "C1A_5";

            public ForceDataPoint MMid
            {
                get
                {
                    if (_Mmid == null)
                    {
                        _Mmid = CalculateMidspanMoment();
                    }
                    return _Mmid;
                }

            }

            private ForceDataPoint CalculateMidspanMoment()
            {
                double Mmax = P*L/4.0 - (M1+M2)/2.0;
                return new ForceDataPoint(L / 2.0, Mmax);
            }

            public ConcentratedLoadAtCenterAndVarEndMoments(BeamSimple beam, double P, double M1, double M2)
            {
                this.beam = beam;
                this.L = beam.Length;
                this.P = P;
                this.M1 = M1;
                this.M2 = M2;
            }

            public double Moment(double X)
            {
                double M;
                beam.EvaluateX(X);
                if (X<=L/2.0)
                {
                    M = (P/2.0+(M1-M2)/L)*X-M1;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                }
                else
                {
                    M=P/2.0*(L-X)+((M1-M2)*X)/L-M1;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam);
                }
                return M;
            }

            public ForceDataPoint MomentMax()
            {
                List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                {
                    new ForceDataPoint(0.0,M1),
                    MMid,
                    new ForceDataPoint(FindZeroPointLocation(),0.0),
                    new ForceDataPoint(L,M2)
                };
                var MaxMoment = Moments.MaxBy(m => m.Value);
                AddMaxMomentEntry(MaxMoment.Value, true, false);
                return MaxMoment;
            } 

            private double FindZeroPointLocation()
            {
                double x=0.0;
                if (P/2+(M1-M2)/L!=0)
                {
                    x = M1/(P/2+(M1-M2)/L);
                }
                return x;
            }

            public ForceDataPoint MomentMin()
            {
                List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                {
                    new ForceDataPoint(0.0,M1),
                    MMid,
                    new ForceDataPoint(FindZeroPointLocation(),0.0),
                    new ForceDataPoint(L,M2)
                };
                var MinMoment = Moments.MinBy(m => m.Value);
                AddMaxMomentEntry(MinMoment.Value, false, true);

                return MinMoment;
            }

            private void AddMaxMomentEntry(double GoverningMoment, bool IsMax, bool IsMin)
            {
                if (GoverningMoment ==M1)
                {
                    BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {{"M1",M1},
                         }, CASE, beam, IsMax, IsMin);
                }
                else if (GoverningMoment == M2)
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.Mmax, 3,
                    new Dictionary<string, double>()
                        {{"M2",M2},
                         }, CASE, beam, IsMax, IsMin);
                }
                else if (GoverningMoment == 0.0)
                {
                    BeamEntryFactory.CreateEntry("Mx", M2, BeamTemplateType.M_zero, 0,
                    null, CASE, beam, IsMax, IsMin);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", MMid.Value, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L/2.0 },
                           {"P",P},
                           {"M1",M1},
                           {"M2",M2},
                         }, CASE, beam, IsMax, IsMin);
                }

            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                Dictionary<string, double> ValDic=
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"M1",M1},
                           {"M2",M2},
                         };
                double V;
                if (X<L/2.0)
                {
                    V = GetShearLeft();
                    //V = P/2.0 +(M1-M2)/L;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                        ValDic, CASE, beam);
                }
                else if (X>L/2.0)
                {
                    //V = P / 2.0 - (M1 - M2) / L;
                    V = GetShearRight();
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                        ValDic, CASE, beam);
                }
                else
                {
                    if (Math.Abs(GetShearLeft())>Math.Abs(GetShearRight()))
                    {
                        V = GetShearLeft();
                        //V = P/2.0 +(M1-M2)/L;
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                            ValDic, CASE, beam);
                    }
                    else
                    {
                        V = GetShearRight();
                        BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                            ValDic, CASE, beam);
                    }
                    
                    //if X is exactly at the center
                }
                return V;
            }

            private double GetShearLeft()
            {
                double V = P/2.0 +(M1-M2)/L;
                    return V;
            }
            private double GetShearRight()
            {
                double V = P / 2.0 - (M1 - M2) / L;
                    return V;
            }

            public ForceDataPoint ShearMax()
            {
                Dictionary<string, double> ValDic =
                        new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"P",P},
                           {"M1",M1},
                           {"M2",M2},
                         };
                double V, X;
                if (Math.Abs(GetShearLeft()) > Math.Abs(GetShearRight()))
                {
                    V = GetShearLeft();
                    //V = P/2.0 +(M1-M2)/L;
                    ValDic.Add("X", 0.0);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                        ValDic, CASE, beam, true);
                            if (Math.Abs(MMid.Value)>Math.Abs(M1))
	                        {
		                        X=L/2.0;
	                        }
                            else { X=0.0; }
                }
                else
                {
                    V = GetShearRight();
                    ValDic.Add("X", L);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 2,
                        ValDic, CASE, beam, true);
                            if (Math.Abs(MMid.Value)>Math.Abs(M2))
	                        {
		                        X=L/2.0;
	                        }
                            else { X=L; }
                }
                return new ForceDataPoint(X, V);
            }

            private void CalculateReactions()
            {
                //obsolete
                //R1 = P/2.0+(M1-M2)/L;
                //ReactionsCalculated = true;
            }
        }

}

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


    public class MomentAtAnyPoint : ISingleLoadCaseBeam
        {
            const string CASE = "C1E_2";
            BeamSimple beam;
            double Mo, V, MmaxRight, MmaxLeft;
            double L, a, b;
            bool ShearHasBeenCalculated, MomentsWereCalculated;

            public MomentAtAnyPoint(BeamSimple beam, double Mo, double a)
            {
                this.beam = beam;
                L = beam.Length;
                this.a = a;
                this.b = L - a;
                this.Mo = Mo;
                ShearHasBeenCalculated = false;
                MomentsWereCalculated = false;
                if (a>L)
                {
                    throw new LoadLocationParametersException(L, "a");   
                }
            }

            public double Moment(double X)
            {
                

                beam.EvaluateX(X);
                double Mx;

                   Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"Mo",Mo },
                     };


                if (X<=0.0)
                {
                    Mx = Mo * X / L;
                    BeamEntryFactory.CreateEntry("Mx", Mx, BeamTemplateType.Mx, 1,
                    valDic, CASE, beam);
                }
                else
                {
                    Mx = Mo * (1-X / L);
                    BeamEntryFactory.CreateEntry("Mx", Mx, BeamTemplateType.Mx, 2,
                    valDic, CASE, beam);
                }
                return Mx;
            }

            public ForceDataPoint MomentMax()
            {
                if (MomentsWereCalculated==false)
                {
                    CalculateMomentsAtPointOfApplication();
                }


                   List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                    {
                        new ForceDataPoint(0.0,0.0),
                        new ForceDataPoint(a, MmaxRight),
                        new ForceDataPoint(a,MmaxLeft),
                    };

                   var MaxMoment = Moments.MaxBy(m => m.Value);
                   AddGoverningMomentEntry(MaxMoment.Value, true, false);
                   return MaxMoment;
            }

            public ForceDataPoint MomentMin()
            {
                if (MomentsWereCalculated == false)
                {
                    CalculateMomentsAtPointOfApplication();
                }


                List<ForceDataPoint> Moments = new List<ForceDataPoint>()
                    {
                        new ForceDataPoint(0.0,0.0),
                        new ForceDataPoint(a, MmaxRight),
                        new ForceDataPoint(a,MmaxLeft),
                    };

                var MinMoment = Moments.MinBy(m => m.Value);
                AddGoverningMomentEntry(MinMoment.Value, false, true);
                return MinMoment;
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                if (ShearHasBeenCalculated==false)
                {
                    CalculateShear();
                }
                //V = Mo / L;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"Mo",Mo }
                         }, CASE, beam);

                return Math.Abs(V);
            }

            public ForceDataPoint ShearMax()
            {
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"Mo",Mo }
                         }, CASE, beam, true);

                return new ForceDataPoint(0.0, Math.Abs(V));
            }

            private void CalculateShear()
            {
                V = Mo / L;
                ShearHasBeenCalculated = true;
            }
             
            private void CalculateMomentsAtPointOfApplication()
            {
                MmaxLeft = -Mo * a / L;
                MmaxRight = Mo*(1.0 - a / L);
                MomentsWereCalculated = true;
            }


            private void AddGoverningMomentEntry(double GoverningMoment, bool IsMax, bool IsMin)
            {
                Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",a },
                           {"Mo",Mo },
                           {"a",a }
                     };

                if (GoverningMoment == MmaxLeft)
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.Mmax, 1,
                    valDic, CASE, beam, IsMax, IsMin);

                }
                else if (GoverningMoment == MmaxRight)
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.Mmax, 2,
                    valDic, CASE, beam, IsMax, IsMin);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", GoverningMoment, BeamTemplateType.M_zero, 0,
                        null, CASE, beam, true);
                }
            }



        }

}

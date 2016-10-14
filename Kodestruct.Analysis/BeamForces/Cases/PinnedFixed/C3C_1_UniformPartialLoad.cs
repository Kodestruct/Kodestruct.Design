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
    public class UniformPartialLoad : ISingleLoadCaseBeam
    {
        const string CASE = "C3C_1";

        BeamPinnedFixed beam;
        double L, a,b,c,d,e, w, R1, R2;
        bool ReactionsWereCalculated , DimensionsWereCalculated;
        public int NumberOfStations { get; set; }

        public UniformPartialLoad(BeamPinnedFixed beam, double w,
            double a, double b)
	    {

                this.beam = beam;
                L = beam.Length;
                this.w = w;
                this.a = a;
                this.b = b;
                if (a+b>L)
                {
                    throw new LoadLocationParametersException(L, "a", "b");
                }
                this.c = L - (a + b);
                        DimensionsWereCalculated = false;
                        ReactionsWereCalculated = false;
               NumberOfStations = 40;
	    }



        public double Moment(double X)
        {
            beam.EvaluateX(X);
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }

            double M;
            if (X<=a) //left-hand segment
            {
                M = R1 * X;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X }
                         }, CASE, beam);
            }
            else if (X >= a + b) //right-hand segment
            {
                M = R1 * X - w * b * (X - d);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X },
                           {"w",w },
                           {"d",d},
                           {"b",b }
                         }, CASE, beam);
            }
            else
            {
                M = R1 * X - w / 2.0 * Math.Pow(X-a,2);
                                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"R1",R1},
                           {"a",a },
                         }, CASE, beam);
            }
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }
            double M, X;
            if (w>0)
            {
                M = M1;
                X = M1Location;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"w",w },
                           {"R1",R1},
                           {"a",a }
                         }, CASE, beam, true);
            }
            else
            {
                M = MSupport;
                X = L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                           {"d",d},
                           {"e",e}
                         }, CASE, beam, true);
            }
            return new ForceDataPoint(X, M);
            //double M = double.NegativeInfinity;
            //double Xgovern=0;
            //beam.LogModeActive = false;
            //List<double> Stations = CreateStationList();
            //foreach (var sta in Stations)
            //{
            //    double Mcurrent = Moment(sta);
            //    M = Mcurrent > M ? Mcurrent : M;
            //    Xgovern = Mcurrent > M ? sta : Xgovern;
            //}
            //beam.LogModeActive = true;
            //return CreateEntry(Xgovern, M, true, false);
        }

        //private ForceDataPoint CreateEntry(double X, double M, bool IsMax, bool IsMin)
        //{

        //    Dictionary<string, double> valDic = new Dictionary<string, double>()
        //            {      {"L",L },
        //                   {"X",X },
        //                   {"w",w },
        //                   {"a",a },
        //                   {"b",b },
        //                   {"c",c },
        //                   {"d",d},
        //                   {"e",e}
        //             };

        //    if (X <= a) //left-hand segment
        //    {
        //        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
        //         valDic, CASE, beam, IsMax, IsMin);
        //    }
        //    else if (X >= a + b) //right-hand segment
        //    {
        //        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 3,
        //         valDic, CASE, beam, IsMax, IsMin);
        //    }
        //    else
        //    {
        //        BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
        //         valDic, CASE, beam, IsMax, IsMin);
        //    }

        //    return new ForceDataPoint(X, M);
        
        //}

        private List<double> CreateStationList()
        {
            List<double> Stations = new List<double>();
            int N = NumberOfStations;
            int Nsegments = N - 1;
            double step = L / Nsegments;
            for (int i = 0; i < NumberOfStations; i++)
            {
                double X = i * step;
                Stations.Add(X);
            }
            return Stations;
        }

        public ForceDataPoint MomentMin()
        {
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }

            double M, X;
            if (w < 0)
            {
                M = M1;
                X = M1Location;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"w",w },
                           {"R1",R1},
                           {"a",a }
                         }, CASE, beam, false,true);
            }
            else
            {
                M = MSupport;
                X = L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w },
                           {"a",a },
                           {"b",b },
                           {"c",c },
                           {"d",d},
                           {"e",e}
                         }, CASE, beam, false, true);
            }
            return new ForceDataPoint(X, M);


            //double M = double.PositiveInfinity;
            //double Xgovern = 0;
            //beam.LogModeActive = false;
            //List<double> Stations = CreateStationList();
            //foreach (var sta in Stations)
            //{
            //    double Mcurrent = Moment(sta);
            //    M = Mcurrent < M ? Mcurrent : M;
            //    Xgovern = Mcurrent < M ? sta : Xgovern;
            //}
            //beam.LogModeActive = true;
            //return CreateEntry(Xgovern, M, false, true);

        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }
            double V;
            if (X<=a)
            {
                V = R1;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X }
                         }, CASE, beam);
            }
            else if (X>=b)
            {
                V = R2;
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 3,
                new Dictionary<string, double>()
                        {
                           {"X",X }
                         }, CASE, beam);

            }
            else
            {
                V = R1 - w * (X - a);
                BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"R1",R1 },
                           {"a",a },
                           {"w",w }
                         }, CASE, beam);
            }

            return V;
        }

        public ForceDataPoint ShearMax()
        {
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }

            double R1a = Math.Abs(R1);
            double R2a = Math.Abs(R2);
            double X, V;

            if (R1a>R2a)
            {
                    X = 0; V = R1;
                    BeamEntryFactory.CreateEntry("Vx", R1, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",X }
                         }, CASE, beam, true);
            }
            else
            {
                    X=L; V=R1;
                    BeamEntryFactory.CreateEntry("Vx", R2, BeamTemplateType.Vmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"X",X }
                         }, CASE, beam, true);
            }
            return new ForceDataPoint(X, V);
        }


        void CalculateReactions()
        {
           if (DimensionsWereCalculated==false)
	        {
		         CalculateDimensions();
	        }

            ReactionsWereCalculated = true;
        
            R1=w*b/(8.0*Math.Pow(L,3))*(12.0*e*e*L-4.0*Math.Pow(e,3)+b*b*d);
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"e",e },
                           {"d",d },
                           {"w",w},
                           {"a",a },
                           {"b",b },
                           {"c",c }
                        }, CASE, beam);
            R2 = w * b - R1;
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"w",w },
                           {"b",b }
                        }, CASE, beam);
        }



        public double M1Location
        {
            get
            {
                if (ReactionsWereCalculated==false)
                {
                    CalculateReactions();
                }
                double X = a + R1 / w;
                return X;
            }

        }

        public double M1
        {
            get
            {
            if (ReactionsWereCalculated==false)
                {
                    CalculateReactions();
                }
                double M = R1*(a+R1/(2.0*w));

                return M;
            }

        }

        public double MSupport
        {
            get
            {
               double M = (w*b)/(8.0*L*L)*(12.0*e*e*L-4.0*Math.Pow(e,3)+b*b*d-8.0*e*L*L);
                return M;
            }

        }

        private void CalculateDimensions()
        {
            this.d = a+b/2;
            this.e = c+b/2;
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {
                           {"a",a },
                           {"b",b },
                           {"c",c }, 
                           {"d",d },
                           {"e",e },
                           {"L",L }
                     };
                    BeamEntryFactory.CreateEntry("c", c, BeamTemplateType.c, 0,
                    valDic, CASE, beam);
                    BeamEntryFactory.CreateEntry("c", c, BeamTemplateType.d, 0,
                    valDic, CASE, beam);
                    BeamEntryFactory.CreateEntry("c", c, BeamTemplateType.e, 0,
                    valDic, CASE, beam);
               DimensionsWereCalculated = true;
        }


    }
}

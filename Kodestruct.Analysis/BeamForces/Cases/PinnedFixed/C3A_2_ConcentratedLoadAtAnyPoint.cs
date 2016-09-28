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

namespace Kodestruct.Analysis.BeamForces.PinnedFixed
{
    public class ConcentratedLoadAtAnyPoint : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    
    {

        const string CASE = "C3A_2";

        BeamPinnedFixed beam;
        double L, P, a, b, R1,R2;
        bool ReactionsWereCalculated;

        public ConcentratedLoadAtAnyPoint(BeamPinnedFixed beam, double P, double a)
        {
            this.beam = beam;
            L = beam.Length;
            this.P = P;
            this.a = a;
            this.b = L - a;
            ReactionsWereCalculated = false;
        }


        public double Moment(double X)
        {
            beam.EvaluateX(X);
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            double M;
            double R1 = V1;

            if (X<=a)
            {
                M = R1 * X;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1},
                           {"X",X }
                         }, CASE, beam);
            }
            else
            {
                M = R1 * X - P * (X - a);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X },
                           {"P",P },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam);
            }
            return M;
        }



        public ForceDataPoint MomentMax()
        {
            if (ReactionsWereCalculated == false)
            {
                CalculateReactions();
            }
            double X, M;

            if (M1>=M2)
            {
                M = M1;
                X = a;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam, true);
            }
            else
            {
                M = M2;
                X = L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                         }, CASE, beam, true);
            }
            return new ForceDataPoint(X, M);
        }

        public ForceDataPoint MomentMin()
        {
            if (ReactionsWereCalculated == false)
            {
                CalculateReactions();
            }

            if (ReactionsWereCalculated == false)
            {
                CalculateReactions();
            }
            double X, M;

            if (M1 <= M2)
            {
                M = M1;
                X = a;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"X",X },
                           {"P",P},
                           {"a",a }
                         }, CASE, beam, false, true);
            }
            else
            {
                M = M2;
                X = L;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 2,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                         }, CASE, beam, false, true);
            }
            return new ForceDataPoint(X, M);
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);

            double V1A = Math.Abs(V1);
            double V2A = Math.Abs(V2);

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                        {      {"L",L },
                               {"X",X},
                               {"P",P},
                               {"a",a },
                               {"b",b }
                     };

            if (X<a)
            {
                BeamEntryFactory.CreateEntry("Vx", V1A, BeamTemplateType.Vx, 1,
                    valDic, CASE, beam);
                return V1A;
            }
            else if (X>a)
            {
                BeamEntryFactory.CreateEntry("Vx", V1A, BeamTemplateType.Vx, 2,
                    valDic, CASE, beam);
                return V2A;
            }
            else //right at the point of load
            {
                if (V1A>V2A)
                {
                BeamEntryFactory.CreateEntry("Vx", V1A, BeamTemplateType.Vx, 1,
                    valDic, CASE, beam);
                    return V1A;
                }
                else
                {
                BeamEntryFactory.CreateEntry("Vx", V2A, BeamTemplateType.Vx, 2,
                    valDic, CASE, beam);
                    return V2A;
                }
            }

        }

        public ForceDataPoint ShearMax()
        {
            double V1A = Math.Abs(V1);
            double V2A = Math.Abs(V2);
            double X;

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                     };

            if (V1A > V2A)
            {
                valDic.Add("X", a);
                BeamEntryFactory.CreateEntry("Vx", V1A, BeamTemplateType.Vmax, 1,
                  valDic, CASE, beam, true);
                X = a;
                return new ForceDataPoint(X, V1A);
            }
            else
            {
                X = L;
                valDic.Add("X", L);
                BeamEntryFactory.CreateEntry("Vx", V1A, BeamTemplateType.Vmax, 2,
                  valDic, CASE, beam, true);
                return new ForceDataPoint(X,V2A);
            }
        }


        double V1
        {
            get
            {
                double V = P * b * b / (2.0 * Math.Pow(L, 3)) * (a + 2.0 * L);
                return V;
            }
        }

         double V2
        {
            get
            {
                double V = P*a/(2.0*Math.Pow(L,3))*(3.0*L*L-a*a);
                return V;
            }
        }

         double M1
         {
             get
             {
                 double M = R1 * a;
                 return M;
             }
         }

         double M2
         {
             get
             {
                 double M = -(P*a*b)/(2.0*L*L)*(a+L);
                 return M;
             }
         }

        void CalculateReactions()
         {
             // double V = P * b * b / (2.0 * Math.Pow(L, 3)) * (a + 2.0 * L);
             // double V = P*a/(2.0*Math.Pow(L,3))*(3.0*L*L-a*a);
             
                   Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                     };

               
            
            double R1 = V1;
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    valDic, CASE, beam);
            double R2 = V2;
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    valDic, CASE, beam);

             ReactionsWereCalculated = true;
         }



        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta_Maximum = ((P * a * Math.Pow((Math.Pow(L, 2) - Math.Pow(a, 2)), 3.0)) / (3.0 * E *I * Math.Pow((3 * Math.Pow(L, 2) - Math.Pow(a, 2)), 2)));

            return delta_Maximum;
        }
    }
}

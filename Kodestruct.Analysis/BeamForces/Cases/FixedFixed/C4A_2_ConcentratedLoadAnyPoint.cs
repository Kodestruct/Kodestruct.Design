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

namespace Kodestruct.Analysis.BeamForces.FixedFixed
{
    public class ConcentratedLoadAnyPoint : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {

        const string CASE = "C4A_2";

        BeamFixedFixed beam;
        double L, a,b, P, R1, R2;
        bool ReactionsWereCalculated, MomentsWereCalculated;
        ForceDataPoint M1, M2, Ma;

        public ConcentratedLoadAnyPoint(BeamFixedFixed beam, double P, double a)
        {
            this.beam = beam;
            this.L = beam.Length;
            this.a = a;
            this.b = L - a;
            this.P = P;
            if (a>L)
            {
                throw new LoadLocationParametersException(L, "a");
            }
            ReactionsWereCalculated = false;
            MomentsWereCalculated = false;
        }


        public double Moment(double X)
        {
            double M;
            beam.EvaluateX(X);
            Dictionary<string, double> valDic = new Dictionary<string, double>()
            {      {"L",L },
                    {"X",X },
                    {"P",P},
                    {"a",a },
                    {"b",b }
                };
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            if (X<=a)
            {
                M= R1*X-(P*a*b*b)/Math.Pow(L,2);
                valDic.Add("R1", R1);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                valDic, CASE, beam);
            }
            else if (X>a)
            {
                M = R2 * (L - X) - (P * a *a * b) / Math.Pow(L, 2);
                valDic.Add("R2", R2);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                valDic, CASE, beam);
            }
            else //at the point of load
            {
                M = (2*P*a*a*b*b) / Math.Pow(L, 3);
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 3,
                valDic, CASE, beam);
            }

            return M;
        }

        private void CalculateMoments()
        {
            M1 = GetM1();
            M2 = GetM2();
            Ma = GetMa();
            MomentsWereCalculated = true;
        }

        private ForceDataPoint GetM1()
        {
            double M, X;

                M = -P * a * b * b / Math.Pow(L, 2.0);
                X = 0.0;

            return new ForceDataPoint(X,M);
        }

        private ForceDataPoint GetM2()
        {
            double M, X;

                M = -P * a * a * b / Math.Pow(L, 2.0);
                X = L;

             return new ForceDataPoint(X, M);
        }

        private ForceDataPoint GetMa()
        {
            double M, X;


            M = 2.0* P * a * a * b*b / Math.Pow(L, 3.0);
            X = L;

            return new ForceDataPoint(X, M);
        }

        public ForceDataPoint MomentMax()
        {
            if (MomentsWereCalculated==false) {CalculateMoments();}


            if (P>=0.0)
            {
                GenerateMomentEntry(Ma, 3, true, false);
                return Ma;
            }
            else
            {
                if (a <= b)
                {
                    GenerateMomentEntry(M1, 1, true, false);
                    return M1;
                }
                else
                {
                    GenerateMomentEntry(M2, 2, true, false);
                    return M2;
                }
            }
            
        }

        public ForceDataPoint MomentMin()
        {

            if (MomentsWereCalculated == false) { CalculateMoments(); }


            if (P < 0.0)
            {
                GenerateMomentEntry(Ma, 3, false, true);
                return Ma;
            }
            else
            {
                if (a <= b)
                {
                    GenerateMomentEntry(M1, 1, false, true);
                    return M1;
                }
                else
                {
                    GenerateMomentEntry(M2, 2, false, true);
                    return M2;
                }
            }
            
        }

        void GenerateMomentEntry(ForceDataPoint M, int TemplateId, bool IsMax, bool IsMin)
        {

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",M.X },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                     };

            BeamEntryFactory.CreateEntry("Mx", M.Value, BeamTemplateType.Mmax, TemplateId,
            valDic, CASE, beam, IsMax, IsMin);
        }

        public double Shear(double X)
        {
            if (ReactionsWereCalculated ==false)
            {
                CalculateReactions();
            }
            double V1a = Math.Abs(R1);
            double V2a = Math.Abs(R2);
            double V;
            int CaseId;

            beam.EvaluateX(X);
            if (X<a)
            {
                V = V1a; CaseId = 1;
            }
            else if (X>a)
            {
                V = V2a; CaseId = 2;
            }
            else
            {
                if (V1a>V2a)
                {
                    V = V1a; CaseId = 1;
                }
                else
                {
                    V = V2a; CaseId = 2;
                }
            }
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, CaseId,
                    new Dictionary<string, double>()
                        {
                           {"X",X },
                           {"R1",R1 },
                           {"R2",R2 }
                         }, CASE, beam);
            return V;

        }

        public ForceDataPoint ShearMax()
        {
            if (ReactionsWereCalculated ==false)
            {
                CalculateReactions();
            }
            double R1a = Math.Abs(R1);
            double R2a = Math.Abs(R2);
            double X;

            if (R1a>R2a)
            {
                X = 0.0;
                    BeamEntryFactory.CreateEntry("Vx", R1a, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        { {"X",X }
                         }, CASE, beam, true);
                return new ForceDataPoint(X, R1);
            }
            else
            {
                X = L;
                BeamEntryFactory.CreateEntry("Vx", R2a, BeamTemplateType.Vmax, 2,
                new Dictionary<string, double>()
                        { {"X",X }
                         }, CASE, beam, true);
                return new ForceDataPoint(X, R1);
            }
        }

        private void CalculateReactions()
        {                   
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"P",P},
                           {"a",a },
                           {"b",b }
                     };

            R1 =P*b*b/Math.Pow(L,3)*(3.0*a+b);
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    valDic, CASE, beam);
            R2 =P*a*a/Math.Pow(L,3)*(a+3.0*b);
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    valDic, CASE, beam);

           ReactionsWereCalculated = true;
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta_Maximum = ((2 * P * Math.Pow(a, 3) * Math.Pow(b, 2)) / (3.0 * E * I * Math.Pow((3.0 * a + b), 2)));
            return delta_Maximum;

        }
    }
}

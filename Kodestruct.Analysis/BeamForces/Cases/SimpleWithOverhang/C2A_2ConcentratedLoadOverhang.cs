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

namespace Kodestruct.Analysis.BeamForces.SimpleWithOverhang
{
    public class ConcentratedLoadOverhang : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        BeamSimpleWithOverhang beam;
        const string CASE = "C2A_2";
        double P, a, L, X1;
        bool X1Calculated;

        //note for overhang beam a is the overhang with 
        //1 exception of concentrated load between supports
        public ConcentratedLoadOverhang(BeamSimpleWithOverhang beam, double P, double a)
	    {
                this.beam = beam;
                L = beam.Length;
                this.P = P;
                this.a = a;
                X1Calculated = false;
        }

        private void CalculateX1(double X)
        {
            X1 = X - L;
            X1Calculated = true;
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            if (X<=L)
            {
                M = P * a * X / L;                    
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a },
                         }, CASE, beam);
            }
            else
            {
                if (X1Calculated == false)
                {
                    CalculateX1(X);
                }
                M = P * (a - X1);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"X1",X1 },
                           {"P",P},
                           {"a",a },
                         }, CASE, beam);
            }
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            double M;
            if (P>=0.0)
            {
                M = P * a;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"X",X1 },
                           {"P",P},
                           {"a",a },
                         }, CASE, beam, true); 
            }
            else
            {
                M = 0;
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, true);
            }
            return new ForceDataPoint(L, M);
        }

        public ForceDataPoint MomentMin()
        {
            double M;
            if (P < 0.0)
            {
                M = P * a;
                BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"X",X1 },
                           {"P",P},
                           {"a",a },
                         }, CASE, beam, false, true);
            }
            else
            {
                M = 0;
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                null, CASE, beam, false, true);
            }
            return new ForceDataPoint(L, M);
        }

        public double Shear(double X)
        {
            double V1 = GetShearBetweenSupports();
            double V2 = GetShearAtOverhang();

                   Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"P",P},
                           {"a",a },
                     };


            if (X<L)
            {
                BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vx, 1,
                valDic, CASE, beam);
                return V1;
            }
            else if (X>L)
            {
                BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vx, 2,
                valDic, CASE, beam);
                return V2;
            }
            else //right at support point
            {
                if (V1>V2)
                {
                    BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vx, 1,
                    valDic, CASE, beam);
                    return V1;
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vx, 2,
                    valDic, CASE, beam);
                    return V2;
                }
            }
        }

        public ForceDataPoint ShearMax()
        {
            double V1 = GetShearBetweenSupports();
            double V2 = GetShearAtOverhang();

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"P",P},
                           {"a",a },
                     };

            if (V1 > V2)
            {
                BeamEntryFactory.CreateEntry("Vx", V1, BeamTemplateType.Vmax, 1,
                valDic, CASE, beam);
                return new  ForceDataPoint(L,V1);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Vx", V2, BeamTemplateType.Vmax, 2,
                valDic, CASE, beam);
                return new ForceDataPoint(L, V2);
            }
        }

        private double GetShearBetweenSupports()
        {

            double V = P*a/L;
            return V;
        }
        private double GetShearAtOverhang()
        {

            double V = P;
            return V;
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta_Maximum = ((P * Math.Pow(a, 2)) / (3.0 * E * I)) * (L + a);
            return delta_Maximum;
        }
    }
}

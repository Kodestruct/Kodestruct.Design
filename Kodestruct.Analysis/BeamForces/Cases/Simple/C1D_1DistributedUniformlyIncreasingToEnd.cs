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

    public class DistributedUniformlyIncreasingToEnd : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
        {
            const string CASE = "C1D_1";

            BeamSimple beam;
            double w;
            double L;
            double W;
            double E, I;
            bool ResultantCalculated;

            ForceDataPoint _Mx;
            public ForceDataPoint Mmax
            {
                get
                {
                    if (_Mx == null)
                    {
                        _Mx = CalculateMmax();
                    }
                    return _Mx;
                }

            }


            public DistributedUniformlyIncreasingToEnd(BeamSimple beam, double w)
            {
                this.beam = beam;
                L = beam.Length;
                this.w = w;
                E = beam.ModulusOfElasticity; 
                I = beam.MomentOfInertia;
                ResultantCalculated =false;

            }

            public double Moment(double X)
            {
                beam.EvaluateX(X);
                double M;
               if (ResultantCalculated==false)
	            {
                    CalculateResultantLoad();
	            }
               double Mtest = W * X / (3.0 * Math.Pow(L, 2)) * (Math.Pow(L, 2.0) - Math.Pow(X, 2.0));
               M = w * X / (6.0 * L) * (Math.Pow(L, 2.0) - Math.Pow(X, 2.0));

                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                         }, CASE, beam);
                return M;
            }

            public ForceDataPoint MomentMax()
            {
                if (w>=0)
                {
                    //Mmax = 2 * w * Math.Pow(L,2) / (9.0 * Math.Sqrt(3.0));
                    BeamEntryFactory.CreateEntry("Mx", Mmax.Value, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",Mmax.X },
                           {"w",w },
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
                if (w >= 0)
                {
                    BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                    null, CASE, beam, true);
                    return new ForceDataPoint(0.0, 0.0);
                }
                else
                {
                    BeamEntryFactory.CreateEntry("Mx", Mmax.Value, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",Mmax.X },
                           {"w",w }
                         }, CASE, beam, false, true);
                    return Mmax;
                }
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                if (ResultantCalculated == false)
                {
                    CalculateResultantLoad();
                }

                double W = L * w / 2.0;
                double V;
                double Vtest = W/3.0-(W*Math.Pow(X,2.0))/Math.Pow(L,2.0);
                V = w*L / 6.0 - (w *L* Math.Pow(X, 2.0)) /(2* Math.Pow(L, 2.0));
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                         }, CASE, beam);
                return V;
            }

            public ForceDataPoint ShearMax()
            {
                double Vmax;
                double VmaxTest= 2.0 / 3.0 * W;
                Vmax= w*L/ 3.0;
                    BeamEntryFactory.CreateEntry("Vx", Vmax, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"w",w }
                         }, CASE, beam, true);
                return new ForceDataPoint(L, Vmax);
            }

            private void CalculateResultantLoad()
            {
                 W = L * w / 2.0;
                ResultantCalculated =true;
            }
            private ForceDataPoint CalculateMmax()
            {
                double MmaxTest = 2*W*L/(9.0*Math.Sqrt(3.0));
                double Mmax = w * Math.Pow(L,2) / (9.0 * Math.Sqrt(3.0));
                double Xmax = L / Math.Sqrt(3.0);
                return new ForceDataPoint(Xmax, Mmax);
            }

            public double MaximumDeflection()
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double delta = 0.00652 * ((w * Math.Pow(L, 4)) / (E * I));
                BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.deltaMax, 0,
                new Dictionary<string, double>()
                                        {
                                           {"w",w },
                                           {"E",E},
                                           {"I",I },
                                           {"L",L}
                                         }, CASE, beam);

                return delta;

            }

            public double Deflection(double X)
            {
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double delta = ((w * X) / (360 * E * I * L)) * (3 * Math.Pow(X, 4) - 10 * L * L * X * X + 7 * Math.Pow(L, 4));
                        BeamEntryFactory.CreateEntry("delta", delta, BeamTemplateType.delta, 0,
                        new Dictionary<string, double>()
                                                {
                                                   {"X",X },
                                                   {"w",w },
                                                   {"E",E},
                                                   {"I",I },
                                                   {"L",L}
                                                 }, CASE, beam);

                        return delta;
            }
        }

}

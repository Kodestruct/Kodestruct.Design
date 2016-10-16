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

    public class DistributedDoubleTriangle : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
        {
            const string CASE = "C1D_2";
            BeamSimple beam;
            double w;
            double L;
            double W;
            bool ResultantCalculated;

            ForceDataPoint _Mmax;
            public ForceDataPoint Mmax
            {
                get
                {
                    if (_Mmax == null)
                    {
                        _Mmax = CalculateMmax();
                    }
                    return _Mmax;
                }

            }

            private ForceDataPoint CalculateMmax()
            {
                if (ResultantCalculated==false)
                {
                    this.CalculateResultantLoad();
                }
                double MmidTest = W * L / 6.0;
                double Mmid = w * L *L/ 12.0;
                return new ForceDataPoint(L / 2.0, Mmid);
            }

            public DistributedDoubleTriangle(BeamSimple beam, double w)
            {
                this.beam = beam;
                L = beam.Length;
                this.w = w;
                ResultantCalculated = false;
            }

            private void CalculateResultantLoad()
            {
                W = ( L * w / 2.0);
                ResultantCalculated = true;
            }

            public double Moment(double X)
            {
                double M = 0;

                beam.EvaluateX(X);
                if (ResultantCalculated == false)
                {
                    this.CalculateResultantLoad();
                }
                double xeff;
                if (X<=L/2.0)
                {
                    xeff = X;
                    M = (w*X*(3.0*L*L-4.0*X*X))/(12.0*L);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           }, CASE, beam);
                }
                else
                {
                    xeff = L - X;
                    M=-(w*(L-X)*(L*L-8.0*L*X+4.0*X*X))/(12.0*L);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           }, CASE, beam);
                }

                double MTest = W * xeff * (1 / 2.0 - (2.0 * Math.Pow(xeff, 2)) / 3.0 / Math.Pow(L, 2));
                return M;
            }

            public ForceDataPoint MomentMax()
            {
                if (ResultantCalculated == false)
                {
                    this.CalculateResultantLoad();
                }
                if (w>=0)
                {
                    
                    //Mmid = w * L / 12.0;
                    BeamEntryFactory.CreateEntry("Mx", Mmax.Value, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"X",Mmax.X },
                           {"L",L },
                           {"w",w }
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
                if (ResultantCalculated == false)
                {
                    this.CalculateResultantLoad();
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
                           {"X",Mmax.X },
                           {"L",L },
                           {"w",w }
                          }, CASE, beam,false, true);
                    return Mmax;
                }
            }

            public double Shear(double X)
            {
                beam.EvaluateX(X);
                if (ResultantCalculated == false)
                {
                    this.CalculateResultantLoad(); //this is obsolete, and used only for checking
                }
                beam.EvaluateX(X);
                double V;

                   Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"w",w },
                      };
               if (X<=L/2.0)
                {
                    double Vtest = W / (2.0 * Math.Pow(L, 2.0)) * (Math.Pow(L, 2.0) - 4.0 * Math.Pow(X, 2.0));
                    V = w / (4.0 * L) * (Math.Pow(L, 2.0) - 4.0 * Math.Pow(X, 2.0));

                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    valDic, CASE, beam);
                }
               else
               {
                   V = w / (4.0 * L) * (Math.Pow(L, 2.0) - 4.0 * Math.Pow(L-X, 2.0));
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                    valDic, CASE, beam);
               }
               
                return V;
            }

            public ForceDataPoint ShearMax()
            {
                if (ResultantCalculated == false)
                {
                    this.CalculateResultantLoad();
                }
                double VTest = W / 2.0;
                double V = w*L / 2.0;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"w",w },
                         }, CASE, beam, true);

                return new ForceDataPoint(0.0, V);
            }

            public double MaximumDeflection()
            {
                throw new NotImplementedException();
                double E = beam.ModulusOfElasticity;
                double I = beam.MomentOfInertia;

                double delta_Maximum = ((w * Math.Pow(L, 4)) / (30.0 * E * I));

                return delta_Maximum;
                
            }
        }

}

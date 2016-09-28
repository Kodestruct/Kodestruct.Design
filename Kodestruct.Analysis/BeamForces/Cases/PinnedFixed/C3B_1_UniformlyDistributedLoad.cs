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
    public class UniformlyDistributedLoad : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {

        const string CASE = "C3B_1";

        BeamPinnedFixed beam;
        double L, w, R1, R2;
        bool ReactionsWereCalculated;

        public UniformlyDistributedLoad (BeamPinnedFixed beam, double w)
	    {
                this.beam = beam;
                L = beam.Length;
                this.w = w;
	    }



        public double Moment(double X)
        {
            beam.EvaluateX(X);
            if (ReactionsWereCalculated==false)
            {
                CalculateReactions();
            }
            double M = R1 * X - w * X * X / 2.0;
            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
            new Dictionary<string, double>()
                        {
                           {"R1",R1},
                           {"w",w},
                           {"L",L},
                           {"X",X }
                         }, CASE, beam);
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            if (w>=0.0)
            {
                    BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",M1Location },
                           {"w",w }
                         }, CASE, beam, true);
                return new ForceDataPoint(M1Location,M1);
            }
            else
            {
                    BeamEntryFactory.CreateEntry("Mx", MSupport, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam, true);
                return new ForceDataPoint(L, MSupport);
            }
        }

        public ForceDataPoint MomentMin()
        {
            if (w <= 0.0)
            {
                BeamEntryFactory.CreateEntry("Mx", M1, BeamTemplateType.Mmax, 2,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",M1Location },
                           {"w",w }
                         }, CASE, beam, false, true);
                return new ForceDataPoint(M1Location, M1);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", MSupport, BeamTemplateType.Mmax, 1,
                new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam, false, true);
                return new ForceDataPoint(L, MSupport);
            } throw new NotImplementedException();
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            if (ReactionsWereCalculated == false)
            {
                CalculateReactions();
            }
            double V = R1 - w * X;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"R1",R1 },
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam);
             return V;
        }

        public ForceDataPoint ShearMax()
        {
            double Vmax = R2;
                    BeamEntryFactory.CreateEntry("Vx", Vmax, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",L },
                           {"w",w }
                         }, CASE, beam, true);
            return new ForceDataPoint(L, Vmax);
        }

        void CalculateReactions()
        {
                   Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"w",w }
                     };

            R1 = 3.0/8.0*w*L;
                    BeamEntryFactory.CreateEntry("R1", R1, BeamTemplateType.R, 1,
                    valDic, CASE, beam);
            R2 = 5.0 / 8.0 * w * L;
                    BeamEntryFactory.CreateEntry("R2", R2, BeamTemplateType.R, 2,
                    valDic, CASE, beam);

            ReactionsWereCalculated = true;
        }



        public double M1Location
        {
            get
            {
                double X = 3.0/8.0* L;
                return X;
            }

        }

        public double M1
        {
            get 
            {
                double M = 9.0 / 128.0 * w * L * L;
                return M; 
            }

        }

        public double MSupport
        {
            get
            {
                double M =- w * L * L / 8.0;
                return M;
            }

        }


        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta_Maximum = ((w * Math.Pow(L, 4)) / (185.0 * E * I));
            return delta_Maximum;
        }
    }
}

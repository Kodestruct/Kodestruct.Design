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

namespace Kodestruct.Analysis.BeamForces.FixedFixed
{
    public class UniformlyDistributedLoad : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
            BeamFixedFixed beam;
            double w;
            double L;
            const string CASE = "C4B_1";
            bool MomentsWereCalculated;

        public UniformlyDistributedLoad(BeamFixedFixed beam, double w)
	    {
                this.beam = beam;
                L = beam.Length;
                this.w = w;
                MomentsWereCalculated = false;
	    }


        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            M = w / 12.0 * (6.0 * L * X - L * L - 6.0 * X * X);
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
            double M, X;
            int CaseId = 1;
            if (MomentsWereCalculated == false) CalculateMoments();
            if (w>=0)
            {
                M = M1;
                X = L / 2.0;
                CaseId=2;
            }
            else
            {
                M = MEnd;
                X = 0.0;
                CaseId = 1;
            }
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, CaseId,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam, true);
            return new ForceDataPoint(X, M);
        }

        private void CalculateMoments()
        {
            _M1 = w * L * L / 24.0;
            _MEnd = -w * L * L / 12.0;
            MomentsWereCalculated = true;
        }

        private double _M1;

        public double M1
        {
            get
            { if (MomentsWereCalculated == false) CalculateMoments(); return _M1; }
        }

        private double _MEnd;

        public double MEnd
        {
            get { if (MomentsWereCalculated == false) CalculateMoments();  return _MEnd; }
        }
        
        public ForceDataPoint MomentMin()
        {
            double M, X;
            int CaseId = 1;
            if (MomentsWereCalculated == false) CalculateMoments();
            if (w < 0)
            {
                M = M1;
                X = L / 2.0;
                CaseId = 2;
            }
            else
            {
                M = MEnd;
                X = 0.0;
                CaseId = 1;
            }
            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, CaseId,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam, false, true);
            return new ForceDataPoint(X, M);
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);
            double V;
            V = w*(L/2.0-X);
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam);
                    return V;
        }

        public ForceDataPoint ShearMax()
        {
            
            double V;
            V = w * L / 2.0 ;
            double X = 0.0;
            BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vmax, 1,
            new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w }
                         }, CASE, beam, true);
            return new ForceDataPoint(X, V);
        }

        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta = ((w * Math.Pow(L, 4)) / (384.0 * E * I));

            return delta;

        }
    }
}

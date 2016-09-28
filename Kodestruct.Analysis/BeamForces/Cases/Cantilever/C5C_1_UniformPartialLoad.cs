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

namespace Kodestruct.Analysis.BeamForces.Cantilever
{

    public class UniformPartialLoad : ISingleLoadCaseBeam, ISingleLoadCaseDeflectionBeam
    {
        const string CASE = "C5C_1";

        BeamCantilever beam;
        double w, L, b, c, e;
        bool eWasCalculated;

        public UniformPartialLoad(BeamCantilever beam, double w, double b)
        {
            this.beam = beam;
            L = beam.Length;
            this.w = w;
            this.b = b;
            this.c = L -b;
            eWasCalculated = false;
            if (b + c > L)
            {
                throw new LoadLocationParametersException(L, "b");
            }
        }

        private void  Calculate_e()
        {
            
          this.e = c + b / 2.0;
                    BeamEntryFactory.CreateEntry("e", e, BeamTemplateType.e, 1,
                    new Dictionary<string, double>()
                        { {"b",b },
                           {"c",c }
                         }, CASE, beam);
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            if(eWasCalculated == false) Calculate_e();

            if (X <= b)
            {
                M =- w * X * X / 2.0;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 1,
                    new Dictionary<string, double>()
                        {  {"X",X },
                           {"w",w }
                         }, CASE, beam);
            }
            else
            {
                M = w * b / 2.0 * (b - 2.0 * X);
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, 2,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"b",b }
                         }, CASE, beam);
            }
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            double M, X;
            if (w < 0)
            {
                M = -w*b*e;
                X = L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {  {"X",X },
                           {"w",w },
                           {"e",e},
                           {"b",b }
                         }, CASE, beam, true);
            }
            else
            {
                M = 0.0;
                X = 0;
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                new Dictionary<string, double>() { { "X", X } }, CASE, beam, true);
            }

            return new ForceDataPoint(X, M);
        }

        public ForceDataPoint MomentMin()
        {
            double M, X;
            if (w < 0)
            {
                M = 0; X = 0;
                BeamEntryFactory.CreateEntry("Mx", 0.0, BeamTemplateType.M_zero, 0,
                new Dictionary<string, double>() { { "X", X } }, CASE, beam, false, true);
                
            }
            else
            {
                M = -w * b * e;
                X = L;
                    BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mmax, 1,
                    new Dictionary<string, double>()
                        {
                           {"L",L },
                           {"X",X },
                           {"w",w },
                           {"b",b },
                           {"e",e }
                         }, CASE, beam, false, true);
            }
            return new ForceDataPoint(X, M);
        }

        public double Shear(double X)
        {
            beam.EvaluateX(X);

            double V;

            if (X <= b)
            {
                V = w * X;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 1,
                    new Dictionary<string, double>()
                        {  {"X",X },
                           {"w",w }
                         }, CASE, beam);
            }
            else
            {
                V = w * b;
                    BeamEntryFactory.CreateEntry("Vx", V, BeamTemplateType.Vx, 2,
                    new Dictionary<string, double>()
                        {  {"X",X },
                           {"b",b },
                           {"w",w }
                         }, CASE, beam);
            }
    
            return V;
        }

        public ForceDataPoint ShearMax()
        {

            double Vval = w * b;
            BeamEntryFactory.CreateEntry("Vx", Vval, BeamTemplateType.Vmax, 2,
            new Dictionary<string, double>()
                        {  {"X",L },
                           {"b",b },
                           {"w",w }
                         }, CASE, beam, true);
            ForceDataPoint V = new ForceDataPoint(L, Vval);
            return V;
        }



        public double MaximumDeflection()
        {
            double E = beam.ModulusOfElasticity;
            double I = beam.MomentOfInertia;

            double delta=((w*b) / (48.0*E*I))*(8.0*Math.Pow(e, 3)-24.0*Math.Pow(e, 2)*L-Math.Pow(b, 3));

            return delta;

        }
    }

}

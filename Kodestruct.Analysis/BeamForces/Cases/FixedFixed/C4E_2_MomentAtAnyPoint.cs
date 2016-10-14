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
using MoreLinq;

namespace Kodestruct.Analysis.BeamForces.FixedFixed
{
    public class MomentAtAnyPoint : ISingleLoadCaseBeam
    {
        BeamFixedFixed beam;
        double Mo, L, a, b;
        ForceDataPoint M1, M2, MPointLeft, MPointRight;
        const string CASE = "C4E_2";
        bool MomentsWereCalculated;
        
        
        public MomentAtAnyPoint(BeamFixedFixed beam, double Mo, double a)
        {
            this.beam = beam;
            L = beam.Length;
            this.a = a;
            this.b = L - a;
            this.Mo = Mo;
            MomentsWereCalculated = false;
            if (a > L)
            {
                throw new LoadLocationParametersException(L, "a");
            }
        }

        public double Moment(double X)
        {
            beam.EvaluateX(X);
            double M;
            int CaseId;

            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      {"L",L },
                           {"X",X },
                           {"Mo",Mo },
                           {"a",a },
                           {"b",b }
                     };

            if (X<=a)
            {
                M = -Mo / (L *L)*((6.0 * a * b * X) / L + b*(L - 3.0 * a));
                CaseId = 1;
            }
            else
            {
                M = (Mo * a) / (L *L)*(6.0 * b - (6.0 * b * X) / L - 2.0 * L + 3.0 * a);
                CaseId = 2;
            }

            BeamEntryFactory.CreateEntry("Mx", M, BeamTemplateType.Mx, CaseId,
                    valDic, CASE, beam);
            return M;
        }

        public ForceDataPoint MomentMax()
        {
            if (MomentsWereCalculated == false) CalculateMoments();
            List<ForceDataPoint> Moments = new List<ForceDataPoint>() { M1, MPointLeft, MPointRight, M2 };
            ForceDataPoint MaxMoment = Moments.MaxBy(m => m.Value);
            AddMomentEntry(MaxMoment, true, false);
            return MaxMoment;
        }

        private void CalculateMoments()
        {
            double M1v = -(Mo * b) / (L * L) * (L - 3.0 * a);
            M1 = new ForceDataPoint(0.0, M1v);

            double MPointLeftv = Mo * (-(6.0 * a * a * b) / Math.Pow(L, 3) - b / (L * L) * (L - 3.0 * a));
            MPointLeft = new ForceDataPoint(a, MPointLeftv);

            double MPointRightv = Mo * (-(6.0 * a * a * b) / Math.Pow(L, 3) - b / (L * L) * (L - 3.0 * a)+1.0);
            MPointRight= new ForceDataPoint(a, MPointRightv);

            double M2v = -(Mo * a) / (L * L) * (2.0*L - 3.0 * a);
            M2 = new ForceDataPoint(L, M2v);

        }

        public ForceDataPoint MomentMin()
        {
            if (MomentsWereCalculated == false) CalculateMoments();
            List<ForceDataPoint> Moments = new List<ForceDataPoint>() { M1, MPointLeft, MPointRight, M2 };
            ForceDataPoint MinMoment = Moments.MinBy(m => m.Value);
            AddMomentEntry(MinMoment, false, true);
            return MinMoment;
        }

        public double Shear(double X)
        {
            double Shear = Math.Abs(V);
            BeamEntryFactory.CreateEntry("Vx", Shear, BeamTemplateType.Vx, 1,
            new Dictionary<string, double>()
                {
                    {"L",L },
                    {"X",X },
                    {"Mo",Mo },
                    {"a",a },
                    {"b",b }
                    }, CASE, beam);
            return Shear;
        }

        public ForceDataPoint ShearMax()
        {
            double Shear = Math.Abs(V);
                    BeamEntryFactory.CreateEntry("Vx", Shear, BeamTemplateType.Vmax, 1,
                    new Dictionary<string, double>()
                        {
                            {"L",L },
                            {"X",0.0 },
                            {"Mo",Mo },
                            {"a",a },
                            {"b",b }
                         }, CASE, beam,true);
               return new ForceDataPoint(0.0, Shear);
        }


        public double V
        {
            get 
            {
                double shear = -6.0 * Mo * a * b / Math.Pow(L, 3);
                return shear; 
            }

        }
        

        private void AddMomentEntry(ForceDataPoint M, bool IsMax, bool IsMin)
        {
            BeamTemplateType BTemplate;
            BTemplate = (IsMax == true) ? BeamTemplateType.Mmax : BeamTemplateType.Mmin;
                   
            
            Dictionary<string, double> valDic = new Dictionary<string, double>()
                    {      
                         {"L",L },
                           {"X",M.X },
                           {"a",a },
                           {"b",b },
                           {"Mo",Mo }
                     };


            if (M == M1)
            {
                BeamEntryFactory.CreateEntry("Mx", M.Value, BTemplate, 1,
                valDic, CASE, beam, IsMax, IsMin);
            }
            else if (M == MPointLeft)
            {
                BeamEntryFactory.CreateEntry("Mx", M.Value, BTemplate, 2,
                valDic, CASE, beam, IsMax, IsMin);
            }
            else if (M == MPointRight)
            {
                BeamEntryFactory.CreateEntry("Mx", M.Value, BTemplate, 3,
                valDic, CASE, beam, IsMax, IsMin);
            }
            else
            {
                BeamEntryFactory.CreateEntry("Mx", M.Value, BTemplate, 4,
                valDic, CASE, beam, IsMax, IsMin);
            }

        }
    }
}

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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;

namespace Kodestruct.Analysis
{
    public abstract class Beam : AnalyticalElement, IAnalysisBeam
    {

        double Mx, Vx;
        double delta_Max;
        ForceDataPoint Mmax, Mmin, Vmax;
        bool hasCalculatedForces;
        bool hasCalculatedDeflections;

        public Beam(double Length, LoadBeam Load, ICalcLog CalcLog, IBeamCaseFactory BeamCaseFactory)
            : base(CalcLog)
        {
            this.length = Length;
            hasCalculatedForces = false;
            hasCalculatedDeflections = false;
            this.BeamCaseFactory = BeamCaseFactory;
            this.Load = Load;
        }

        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }


        
        

        public IBeamCaseFactory BeamCaseFactory { get; set; }


        private LoadBeam load;

        public LoadBeam Load
        {
            get {

                return load; }
            set { load = value; }
        }
        
 

        public virtual void EvaluateX(double X)
        {
            if (X < 0 || X > this.Length)
            {
                throw new StationOutOfBoundsException(this.Length, X);
            }
        }

        public void CalculateForces(double X)
        {


                    ISingleLoadCaseBeam bm = BeamCaseFactory.GetForceCase(load, this);
                    if (ReportX ==false)
                    {
                        LogModeActive = false;
                    }
                    else
                    {
                        LogModeActive = true;
                    }
                    Mx = bm.Moment(X);
                    Vx = bm.Shear(X);
                    //Switch on or off reporting mode to avoid unnecessary data
                    if (ReportMax == false)
                    {
                        LogModeActive = false;
                    }
                    else
                    {
                        LogModeActive = true;
                    }
                    Mmax = bm.MomentMax();
                    Mmin = bm.MomentMin();
                    Vmax = bm.ShearMax();

                    LogModeActive = true;

            hasCalculatedForces = true;
        }

        private void CalculateDeflections()
        {
            ISingleLoadCaseDeflectionBeam bm = BeamCaseFactory.GetDeflectionCase(load, this);
            delta_Max = bm.MaximumDeflection();
            hasCalculatedDeflections = true;
        }

        protected ForceDataPoint FindForceValueMax(List<double> SpecialPoints,FindValueAtXDelegate evaluateForce)
        {
            //divide beam into equal segments
            //add special points 
            //create sorted list
            //step through points and evaluate maximum
            throw new NotImplementedException();
        }

        public double GetMaxMomentBetweenPoints(double X_min, double X_max, int Steps=20)
        {
            if (X_min<0 || X_max > Length)
            {
                throw new Exception("Invalid minimum or maximum X values for sub-segments.");
            }
            double segLen = X_max - X_min;
            double segStep = segLen / Steps;
            List<double> Ms = new List<double>();

            for (int i = 0; i <= Steps; i++)
            {
                double X_pt = segStep * i + X_min;
                double M_x = GetMoment(X_pt);
                Ms.Add(M_x);
            }
            var M_max = Ms.Max();
            return  M_max;
        }
        public double GetMinMomentBetweenPoints(double X_min, double X_max, int Steps=20)
        {
            if (X_min<0 || X_max > Length)
            {
                throw new Exception("Invalid minimum or maximum X values for sub-segments.");
            }
            double segLen = X_max - X_min;
            double segStep = segLen / Steps;
            List<double> Ms = new List<double>();

            for (int i = 0; i <= Steps; i++)
            {
                double X_pt = segStep * i + X_min;
                double M_x = GetMoment(X_pt);
                Ms.Add(M_x);
            }
            var M_min = Ms.Min();
            return  M_min;
        }

        public virtual double GetMoment(double X)
        {
            if (hasCalculatedForces == false)
            {
                CalculateForces(X);
            }
            return Mx;
        }

        public virtual double GetShear(double X)
        {
            if (hasCalculatedForces == false)
            {
                CalculateForces(X);
            }
            return Vx;
        }

        public virtual ForceDataPoint GetMomentMaximum()
        {
            if (hasCalculatedForces == false)
            {
                CalculateForces(Length / 2.0); // if no other X is provided
            }
            return Mmax;
        }

        public virtual ForceDataPoint GetMomentMinimum()
        {
            if (hasCalculatedForces == false)
            {
                CalculateForces(0.0); // if no other X is provided
            }
            return Mmin;
        }

        public virtual ForceDataPoint GetShearMaximumValue()
        {
            if (hasCalculatedForces == false)
            {
                CalculateForces(0.0); // if no other X is provided
            }
            return Vmax;
        }

        public bool ReportX { get; set; }
        public bool ReportMax { get; set; }


        protected double E;

        public double ModulusOfElasticity
        {
            get { return E; }
            set { E = value; }
        }


        protected double I;

        public double MomentOfInertia
        {
            get { return I; }
            set { I = value; }
        }



        public double GetMaximumDeflection()
        {
            if (hasCalculatedDeflections == false)
            {
                CalculateDeflections();
            }
            return delta_Max;
        }


    }
}

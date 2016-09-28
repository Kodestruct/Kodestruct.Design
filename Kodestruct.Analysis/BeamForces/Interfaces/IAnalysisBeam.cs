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
using Kodestruct.Common.CalculationLogger.Interfaces;
namespace Kodestruct.Analysis
{
    public interface IAnalysisBeam : ILoggable
    {
        double GetMoment(double X);
        ForceDataPoint GetMomentMaximum();
        ForceDataPoint GetMomentMinimum();

        double GetMaxMomentBetweenPoints(double X_min, double X_max, int Steps);
        double GetMinMomentBetweenPoints(double X_min, double X_max, int Steps);
        double GetShear(double X);
        ForceDataPoint GetShearMaximumValue();
        double Length { get; set; }

        double ModulusOfElasticity { get; set; }
        double MomentOfInertia { get; set; }
        void EvaluateX(double X);
        double GetMaximumDeflection();
       // BeamTemplatePathLocator ResourceLocator { get; } //for templates
    }
}

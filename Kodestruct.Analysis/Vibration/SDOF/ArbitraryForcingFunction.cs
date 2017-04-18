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
using System.Threading.Tasks;

namespace Kodestruct.Analysis.Vibration
{
    public partial class SDOFSystem
    {

        /// <summary>
        /// Response of a single D.O.F system under arbitrary forcing function using the method in section 4.8. S. Rao Mechanical Vibrations (4th Ed)
        /// </summary>
        /// <param name="ff">Array containing the value of the forcing function at various time stations</param>
        /// <param name="xai">Damping factor</param>
        /// <param name="omn">Indamped natural frequency of the system</param>
        /// <param name="delt">Incremental time between consecutive time stations</param>
        /// <param name="xk">Spring stiffness</param>
        /// <param name="t_start">Start time for output</param>
        /// <param name="t_end">End time for output</param>
        public List<double> GetArbitraryForcingFunctionX(List<double> f, double xai, double omn, double delt, double xk,
            double t_start, double t_end)
        {
            	double xn, pd, del;

                List<double>    delf   = new List<double>();
                List<double>     t     = new List<double>();
                List<double>     x     = new List<double>();
                List<double>     xd    = new List<double>();
                List<double>     x1    = new List<double>();
                List<double>     xd1   = new List<double>();
                List<double>     x2    = new List<double>();
                List<double>     xd2   = new List<double>();
                List<double>     x3    = new List<double>();
                List<double>     xd3   = new List<double>();
                List<double>     x4    = new List<double>();
                List<double>     xd4   = new List<double>();



	
	// np = number of points at which value of f is known
            int np = f.Count();
            int np1 = np-1;
            int np2 = np-2;
	
	xn = xai * omn;
	pd = omn * Math.Sqrt(1.0 - xai * xai);

            	//Solution according to method 3 using the idealization of Fig. 4.29  per S. Rao Mechanical Vibrations (4th Ed)
	x[0] = 0.0;
	xd[0] = 0.0;

            

	for (int j = 0; j < np1; j ++)
		f[j] = f[j+1];

	                f[np-1] = 0.0;
	                for (int j = 1; j < np; j ++)
	                {
		                delf[j] = f[j] - f[j-1];
		                x[j] = (delf[j] / (xk * delt)) * (delt - (2.0 * xai / omn) + Math.Exp(-xn * delt) *
			                   ((2.0 * xai / omn) * Math.Cos(pd * delt) - (( pd * pd - xn * xn) / (omn * omn * pd)) 
			                   * Math.Sin(pd * delt))) + (f[j-1] / xk) * (1.0 - Math.Exp(-xn * delt) * (Math.Cos(pd * delt)
			                   + (xn / pd) * Math.Sin(pd * delt))) + Math.Exp(-xn * delt) * (x[j-1] * Math.Cos(pd * delt)
			                   + ((xd[j-1] + xn * x[j-1]) / pd) * Math.Sin(pd * delt));
		                xd[j] = (delf[j] / (xk * delt)) * (1.0 - Math.Exp(-xn * delt ) * (((xn * xn + pd * pd) 
			                    / (omn * omn)) * Math.Cos(pd * delt) + (( Math.Pow(xn, 3) + xn * pd * pd) 
				                / (pd * (omn * omn))) * Math.Sin(pd * delt ))) + (f[j-1] / xk) * Math.Exp(-xn * delt) 
				                * ((xn * xn / pd) + pd) * Math.Sin(pd * delt) + Math.Exp( -xn * delt) * (xd[j-1] 
				                * Math.Cos(pd * delt) - ((xn * xd[j-1] + xn * xn * x[j-1] + pd * pd * x[j-1]) / pd) 
				                * Math.Sin(pd * delt));
	                }
           throw new NotImplementedException();
        }
    }
}

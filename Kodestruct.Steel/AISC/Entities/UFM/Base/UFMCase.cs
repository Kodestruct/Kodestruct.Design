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
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Steel.AISC.UFM
{
    public class UFMCase
    {
        public UFMCase(double d_b, double d_c, double theta)
        {
           this.d_b    =d_b  ;
           this.d_c    =d_c  ;
           this.theta = theta;
        }

       public  double d_b    {get; set;}
       public  double d_c    {get; set;}
       public  double theta { get; set; }


        public double tan_theta
        {
            get
            {
                double thetaRad = theta.ToRadians();
                return Math.Tan(thetaRad);
            }
        }

        public double e_b
        {
            get { 
                
                return d_b/2.0; 
            }
        }


        public double e_c
        {
            get
            {

                return d_c / 2.0;
            }
        }
        
        public double Get_r(double alpha, double beta)
        {
            double r=Math.Sqrt(Math.Pow((alpha+e_c), 2)+Math.Pow((beta+e_b), 2));
            return r;
        }
        public double Get_alpha(double beta)
        {
            double alpha = e_b * tan_theta - e_c + beta * tan_theta;
            return alpha;
        }

        public double Get_beta(double alpha)
        {
            double beta = ((alpha + e_c) / (tan_theta)) - e_b;
            return beta;
        }
    }
}

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
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;

namespace Kodestruct.Steel.AISC.AISC360v10.J_Connections.AffectedMembers
{
    public class ShearAreaCalculator : AffectedElement
    {
        ShearAreaCase Case;
        double N_BoltParallel    {get; set;}
        double N_BoltPerpendicular {get; set;}
        double d_Hole    {get; set;}
        double t        { get; set; }
        double p_parallel       {get; set;}
        double p_perpendicular  {get; set;}

        double l_edgeParallel       {get;set;}
        double l_edgePerpendicular { get; set; }


        public ShearAreaCalculator(ShearAreaCase Case, double N_BoltParallel, double N_BoltPerpendicular, 
            double p_parallel, double p_perpendicular, double d_Hole, double t,
            double l_edgeParallel, double l_edgePerpendicular)
        {
            this.Case = Case;
            this.N_BoltParallel   =N_BoltParallel;   
            this.N_BoltPerpendicular=N_BoltPerpendicular;
            this.d_Hole      =d_Hole;
            this.t = t;     
            this.p_parallel = p_parallel;
            this.p_perpendicular = p_perpendicular;
            this.l_edgeParallel = l_edgeParallel;
            this.l_edgePerpendicular = l_edgePerpendicular;
        }

        public double GetNetAreaShear()
        {
            double A_nv = 0.0;
            switch (Case)
            {
                case ShearAreaCase.StraightLine:
                    A_nv = ((2 * l_edgeParallel + (N_BoltParallel-1) * p_parallel) - (N_BoltParallel * d_Hole)) * t;
                    break;
                case ShearAreaCase.TBlock:
                    A_nv = (((l_edgeParallel + (N_BoltParallel - 1) * p_parallel) - ((N_BoltParallel - 0.5) * d_Hole)) * t) * 2.0;
                    break;
                case ShearAreaCase.UBlock:
                    A_nv = (((l_edgeParallel + (N_BoltParallel - 1) * p_parallel) - ((N_BoltParallel - 0.5) * d_Hole)) * t) * 2.0;
                    break;
                case ShearAreaCase.LBlock:
                    A_nv = ((l_edgeParallel + (N_BoltParallel - 1) * p_parallel) - ((N_BoltParallel - 0.5) * d_Hole)) * t;
                    break;

            }
            return A_nv;
        }

        public double GetGrossAreaShear()
        {
            double A_gv = 0.0;
            switch (Case)
            {
                case ShearAreaCase.StraightLine:
                    A_gv = (2 * l_edgeParallel + (N_BoltParallel - 1) * p_parallel) * t;
                    break;
                case ShearAreaCase.TBlock:
                    A_gv = ((l_edgeParallel + (N_BoltParallel - 1) * p_parallel) * t) * 2.0;
                    break;
                case ShearAreaCase.UBlock:
                    A_gv = ((l_edgeParallel + (N_BoltParallel - 1) * p_parallel) * t) * 2.0;
                    break;
                case ShearAreaCase.LBlock:
                    A_gv = (l_edgeParallel + (N_BoltParallel - 1) * p_parallel) * t;
                    break;

            }
            return A_gv;
        }
        public double GetNetAreaTension()
        {
            double A_nt = 0.0;
            switch (Case)
            {
                case ShearAreaCase.StraightLine:
                    A_nt = 0;
                    break;
                case ShearAreaCase.TBlock:
                    A_nt = (((N_BoltPerpendicular-1) * p_perpendicular) - ((N_BoltPerpendicular - 1) * d_Hole)) * t;
                    break;
                case ShearAreaCase.UBlock:
                    A_nt = ((l_edgePerpendicular)*2 -  d_Hole) * t;
                    break;
                case ShearAreaCase.LBlock:
                    A_nt = (l_edgePerpendicular + ((N_BoltPerpendicular - 1) * p_perpendicular) - ((N_BoltPerpendicular - 0.5) * d_Hole)) * t;
                    break;

            }
            return A_nt;
        }

    }
}

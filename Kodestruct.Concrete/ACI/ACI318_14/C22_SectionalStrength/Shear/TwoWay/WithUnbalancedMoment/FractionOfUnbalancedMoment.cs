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
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public partial class ConcreteSectionTwoWayShear
    {



        #region Factor used to determine unbalanced moment
        public double Get_gamma_vy()
        {
            return this.Get_gamma_vy(l_x, l_y);
        }

        public double Get_gamma_vx()
        {
            return this.Get_gamma_vx(l_x, l_y);
        }

        public double Get_gamma_vy(double l_x, double l_y)
        {
            double gamma_vy = 1.0;
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)))));
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }

                    break;
                case PunchingPerimeterConfiguration.EdgeRight:

                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }

                    break;
                case PunchingPerimeterConfiguration.EdgeTop:

                    gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)))));

                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)))));

                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    if ((l_x) / (l_y) > 0.2)
                    {
                        gamma_vy = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_x) / (l_y)) - 0.2)));
                    }
                    else
                    {
                        gamma_vy = 0.0;
                    }
                    break;
            }
            return gamma_vy;
        }

        public double Get_gamma_vx(double l_x, double l_y)
        {
            double gamma_vx = 1.0;
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    gamma_vx = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_y) / (l_x)))));
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    gamma_vx = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_y) / (l_x)))));
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    gamma_vx = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_y) / (l_x)))));
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    if ((l_y) / (l_x) > 0.2)
                    {
                        gamma_vx = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_y) / (l_x)) - 0.2)));
                    }
                    else
                    {
                        gamma_vx = 0.0;
                    }

                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    if ((l_y) / (l_x) > 0.2)
                    {
                        gamma_vx = 1.0 - ((1.0) / (1 + ((2.0) / (3.0)) * Math.Sqrt(((l_y) / (l_x)) - 0.2)));
                    }
                    else
                    {
                        gamma_vx = 0.0;
                    }

                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    gamma_vx = 0.4;
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    gamma_vx = 0.4;
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    gamma_vx = 0.4;
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    gamma_vx = 0.4;
                    break;
            }
            return gamma_vx;
        }

        #endregion




    }
}

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


                /// <summary>
        /// Two-way shear section (constructor used for stress calculation)
        /// </summary>
        /// <param name="Perimeter">Shear perimeter</param>
        /// <param name="d">Effective slab section</param>
        /// <param name="c_x">Column dimension (along X axis)</param>
        /// <param name="c_y">Column dimension (along Y axis)</param>
        /// <param name="ColumnType">Identifies if the column is located at the interior, slab edge, or slab corner</param>
        ///  <param name="AtColumnFace">Identifies if the section is adjacent to column face (typical) or away from column face (as is the case with shear studs away from the face)</param>

        public ConcreteSectionTwoWayShear( PunchingPerimeterData Perimeter, double d,
            double c_x, double c_y, PunchingPerimeterConfiguration ColumnType, bool AtColumnFace = true): this(null,
            Perimeter,d,c_x,c_y,ColumnType,AtColumnFace)
        {
        }
        /// <summary>
        /// Two-way shear section (constructor used for stress calculation)
        /// </summary>
        /// <param name="Material">Concrete material</param>
        /// <param name="Perimeter">Shear perimeter</param>
        /// <param name="d">Effective slab section</param>
        /// <param name="c_x">Column dimension (along X axis)</param>
        /// <param name="c_y">Column dimension (along Y axis)</param>
        /// <param name="ColumnType">Identifies if the column is located at the interior, slab edge, or slab corner</param>
        ///  <param name="AtColumnFace">Identifies if the section is adjacent to column face (typical) or away from column face (as is the case with shear studs away from the face)</param>

        public ConcreteSectionTwoWayShear(IConcreteMaterial Material, PunchingPerimeterData Perimeter, double d,
            double c_x, double c_y, PunchingPerimeterConfiguration ColumnType, bool AtColumnFace = true)
        {
            this.Material    =Material      ;
            this.Segments    =Perimeter.Segments      ;
            this.d           =d             ;
            this.c_x         =c_x           ;
            this.c_y         =c_y           ;
            this.ColumnType = ColumnType    ;
            this.ColumnCenter = Perimeter.ColumnCentroid;
            this.AtColumnFace = AtColumnFace;
            cen = PunchingPerimeterCentroid;
            CalculatePerimeterProperties();
            
        }

        Point2D cen;
        double l_x;
        double l_y;
        List<PerimeterLineSegment> RotatedSegments;
        double thetaRad;
        double A_c;


        private void CalculatePerimeterProperties()
        {
            adjustedSegments = AdjustSegments(cen);
            A_c = AdjustedSegments.Sum(s => s.Length) * d;

            double J_x_bar = GetJx(AdjustedSegments);   // d times product of inertia of assumed shear critical section about nonprincipal axes x
            double J_y_bar = GetJy(AdjustedSegments);   // d times product of inertia of assumed shear critical section about nonprincipal axes y
            double J_xy_bar = GetJxy(AdjustedSegments);

            thetaRad = Get_thetaRad(J_xy_bar, J_x_bar, J_y_bar);
            //The absolute value of ? is less than ?/2; when the value is
            //positive, ? is measured in the clockwise direction

           RotatedSegments = GetRotatedSegments(AdjustedSegments, thetaRad);
           l_x = Get_l_x(RotatedSegments);
           l_y = Get_l_y(RotatedSegments);
        }


        /// <summary>
        /// Indicates if this a section at column interface. This parameter is set to false for critical section of shear reinforcement outside of column perimeter
        /// </summary>
        public bool AtColumnFace { get; set; }

        Point2D ColumnCenter { get; set; }
        IConcreteMaterial Material { get; set; }
        public double d { get; set; }

        public List<PerimeterLineSegment> Segments { get; set; }

        public PunchingPerimeterConfiguration ColumnType { get; set; }


        /// <summary>
        /// Column dimension X
        /// </summary>
        public double c_x { get; set; }

        /// <summary>
        /// Column dimension Y
        /// </summary>
        public double c_y { get; set; }




    }
}

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
        /// Two-way shear section
        /// </summary>
        /// <param name="Material">Concrete material</param>
        /// <param name="Segments">Shear perimeter segments</param>
        /// <param name="d">Effective slab section</param>
        /// <param name="c_x">Column dimension (along X axis)</param>
        /// <param name="c_y">Column dimension (along Y axis)</param>
        /// <param name="AtColumnFace">Identifies if the section is adjacent to column face (typical) or away from column face (as is the case with shear studs away from the face)</param>
        /// <param name="ColumnType">Identifies if the column is located at the interior, slab edge, or slab corner</param>
        /// <param name="ColumnCenter">Controid pointof column</param>
        public ConcreteSectionTwoWayShear(IConcreteMaterial Material, PunchingPerimeterData Perimeter, double d,
            double c_x, double c_y, bool AtColumnFace, PunchingPerimeterConfiguration ColumnType, Point2D ColumnCenter)
        {
            this.Material    =Material      ;
            this.Segments    =Segments      ;
            this.d           =d             ;
            this.c_x         =c_x           ;
            this.c_y         =c_y           ;
            this.AtColumnFace=AtColumnFace  ;
            this.ColumnType = ColumnType    ;
            this.ColumnCenter = ColumnCenter;
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

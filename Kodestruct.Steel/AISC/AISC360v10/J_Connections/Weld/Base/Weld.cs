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
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.Entities.Materials;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public abstract class Weld : SteelDesignElement
    {
        public Weld(double F_y, double F_u, double F_EXX, double Size, double A_nBase, double l, ICalcLog Log)
            : base( Log)
        {
            this.WeldMaterial = new WeldMaterial(F_EXX);
            this.BaseMaterial = new SteelMaterial(F_y, F_u, 0.0, 0.0);
            this.A_nBase = A_nBase;
            this.Size = Size;
            this.Length = l;
        }

        public Weld(double Fy, double Fu, double Fexx, double Size,  double A_nBase, double l)
            : base()
        {
            this.WeldMaterial = new WeldMaterial(Fexx);
            this.BaseMaterial = new SteelMaterial(Fy, Fu, 0.0, 0.0);
            this.A_nBase = A_nBase;
            this.Size = Size;
            this.Length = l;
        }

        ////ctor for welds where base material is checked independently
        //public Weld(double Fexx, double Size)
        //    : base()
        //{
        //    this.WeldMaterial = new WeldMaterial(Fexx);
        //    this.Size = Size;
        //}

        public WeldMaterial WeldMaterial { get; set; }
        public SteelMaterial BaseMaterial { get; set; }
        public double A_nBase { get; set; }
        public double Length { get; set; }
        public double Size { get; set; }

    }
}

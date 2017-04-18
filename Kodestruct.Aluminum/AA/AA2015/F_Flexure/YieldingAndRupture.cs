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
using Kodestruct.Aluminum.AA.Entities;
using Kodestruct.Aluminum.AA.Entities.Exceptions;
using Kodestruct.Aluminum.AA.Entities.Interfaces;
using Kodestruct.Aluminum.AA.Entities.Section;
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.Flexure
{
    public partial class AluminumFlexuralMember : IAluminumBeamFlexure
    {

        #region Yield moment
        internal double GetCompressionFiberYieldMomentMyc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Myc = 0.0;
            double Sxc = 0.0;

            double F_cy = Section.Material.F_cy;


            Sxc = GetSectionModulusCompressionSxc(compressionFiberPosition);

            Myc = F_cy * Sxc;
            return Myc;
        }

        internal double GetTensionFiberYieldMomentMyt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Myt = 0.0;
            double Sxt = 0.0;


            double F_ty = Section.Material.F_ty;

            Sxt = GetSectionModulusTensionSxt(compressionFiberPosition);

            Myt = F_ty * Sxt;
            return Myt;
        }


        public double GetSectionModulusCompressionSxc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Sxc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Sxc = Section.Shape.S_xTop;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Sxc = Section.Shape.S_xBot;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Sxc;
        }

        public double GetSectionModulusTensionSxt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Sxt = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Sxt = Section.Shape.S_xBot;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Sxt = Section.Shape.S_xTop;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Sxt;
        }

        public double GetSectionModulusCompressionSyc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Syc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Left:
                    Syc = Section.Shape.S_yLeft;
                    break;
                case FlexuralCompressionFiberPosition.Right:
                    Syc = Section.Shape.S_yRight;
                    break;
                default:
                    throw new CompressionFiberPositionException();

            }

            return Syc;
        }

        public double GetSectionModulusTensionSyt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Syc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Left:
                    Syc = Section.Shape.S_yRight;
                    break;
                case FlexuralCompressionFiberPosition.Right:
                    Syc = Section.Shape.S_yLeft;
                    break;
                default:
                    throw new CompressionFiberPositionException();

            }

            return Syc;
        }


        private double GetPlasticMoment()
        {
            double Z_x = Section.Shape.Z_x;
            double F_cy = Section.Material.F_cy;

            return Z_x * F_cy;
        }

        #endregion



        public AluminumLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double M_nc = 1.5 * GetCompressionFiberYieldMomentMyc(CompressionLocation);
            double M_nt = 1.5 * GetTensionFiberYieldMomentMyt(CompressionLocation);
            double M_np = GetPlasticMoment();

            List<double> LimitStates = new List<double>()
            {
                M_nc,
                M_nt,
                M_np
            };

            double M = 0.9* LimitStates.Min();
            return new AluminumLimitStateValue(M, true);
        }




        public AluminumLimitStateValue GetFlexuralRuptureStrength()
        {

            //F.2-1
            double Z = Section.Shape.Z_x;
            double F_tu = Section.Material.F_tu;
            double k_t = Section.Material.k_t;

            double M_nu = 0.75 * Z * F_tu / k_t;

            return new AluminumLimitStateValue(M_nu, true);
        }
    }
}

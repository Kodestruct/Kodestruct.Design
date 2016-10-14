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
 using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
 

namespace Kodestruct.Concrete.ACI
{
    public abstract partial class PrestressedBeamSectionBase : ConcreteFlexuralSectionBase, IConcreteFlexuralMember
    {
        public bool CheckAllowableMoment(FlexuralCompressionFiberPosition CompressionFiberPosition,
            double ExternalMoment,StageType Stage, LoadType LoadType)
        {
            double fct = GetAllowableTension(Stage);
            double fcc = GetAllowableCompression(Stage, LoadType);

            double M = Math.Abs(ExternalMoment);
            double e = GetPrestressingForceEccentricityAtTransfer();
            double P = Math.Abs(GetPrestressForceAtTransfer());

            double ft = getTopStress(P, e, ExternalMoment, CompressionFiberPosition);
            double fb = getBottomStress(P, e, ExternalMoment, CompressionFiberPosition);

            double MaxPosForce = Math.Max(ft, fb); //positive is for compression
            double MaxNegForce = Math.Min(ft, fb); //positive is for compression

            if (MaxPosForce<=fcc && MaxNegForce >= -fct)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public double getMaximumAllowableMoment(FlexuralCompressionFiberPosition CompressionFiberPosition, StageType Stage, LoadType LoadType)
        {
            double e = GetPrestressingForceEccentricityAtTransfer();
            double P = Math.Abs(GetPrestressForceAtTransfer());
            double A = this.Section.SliceableShape.A;
            double Sb = this.Section.SliceableShape.S_xBot;
            double St = this.Section.SliceableShape.S_xTop;

            double Mcomp=0;
            double Mtens=0;

            double fct = GetAllowableTension(Stage);
            double fcc = GetAllowableCompression(Stage, LoadType);

            switch (CompressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Mcomp=(fcc-P/A-(P*e)/St)*St;
                    Mtens = (fct + P / A - (P * e ) / Sb) * Sb;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Mcomp=(fcc-P/A+(P*e)/Sb)*Sb;
                    Mtens = (fct + P / A + (P * e) / St) * St;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Math.Min(Mcomp, Mtens);
        }


        private double getTopStress(double P, double e, double M, FlexuralCompressionFiberPosition fiberPosition)
        {
            double A = this.Section.SliceableShape.A;
            double Sb = this.Section.SliceableShape.S_xBot;
            double St = this.Section.SliceableShape.S_xTop;

            if (fiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                return P / A + P * e  / St + M /St; 
                // note e is expected to be negative if below centroid line
            }
            if (fiberPosition == FlexuralCompressionFiberPosition.Bottom)
            {
                return P / A + P * e /St - M /St;
            }
            else
            {
                throw new CompressionFiberPositionException();
            }

        }
        private double getBottomStress(double P, double e, double M, FlexuralCompressionFiberPosition fiberPosition)
        {
            double A = this.Section.SliceableShape.A;
            double Sb = this.Section.SliceableShape.S_xBot;
            double St = this.Section.SliceableShape.S_xTop;

            if (fiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                return P / A - P * e /Sb - M /Sb;
                // note e is expected to be negative if below centroid line
            }
            if (fiberPosition == FlexuralCompressionFiberPosition.Bottom)
            {
                return P / A - P * e /Sb + M /Sb;
            }
            else
            {
                throw new CompressionFiberPositionException();
            }
        }

        protected abstract double GetAllowableTension(StageType Stage);
        protected abstract double GetAllowableCompression(StageType Stage, LoadType LoadType);


    }
}

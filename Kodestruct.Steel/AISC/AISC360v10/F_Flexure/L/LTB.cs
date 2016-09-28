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
using Kodestruct.Common;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Interfaces;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamAngle : FlexuralMemberAngleBase
    {


        private double GetFlexuralTorsionalBucklingMomentCapacity(double L_b, double C_b,
            FlexuralCompressionFiberPosition CompressionLocation, FlexuralAndTorsionalBracingType BracingType, MomentAxis MomentAxis)
        {

            double M_n = 0.0;
            double M_n1 = 0.0;
            double M_n2 = 0.0;
            double M_e = GetM_e(L_b, C_b, CompressionLocation, BracingType, MomentAxis);
            double M_y = GetYieldingMomentGeometricXCapacity(CompressionLocation, BracingType);

            if (M_e<=M_y)
            {
                //F10-2
                M_n1=(0.92-((0.17*M_e) / (M_y)))*M_e;
            }
            else
            {
                //F10-3
                M_n1=(1.92-1.17*Math.Sqrt(((M_y) / (M_e))))*M_y; 
            }

            M_n2 = 1.5*M_y;
            M_n= Math.Min(M_n1,M_n2);
            double phiM_n = 0.9 * M_n;
            return phiM_n;
        }

        private double GetM_e(double L_b, double C_b, FlexuralCompressionFiberPosition CompressionLocation, 
            FlexuralAndTorsionalBracingType BracingType, MomentAxis MomentAxis)
        {
            double M_e;
            if (MomentAxis == MomentAxis.MajorPrincipalAxis)
            {
                if (IsEqualLeg == true)
                {
                    M_e = ((0.46 * E * Math.Pow(b, 2) * Math.Pow(t, 2) * C_b) / (L_b)); //F10-4
                }
                else
                {
                    double beta_wCorrected;
                    //F10-5
                    if (CompressionLocation == FlexuralCompressionFiberPosition.Top)
                    {
                        if (AngleRotation == AngleRotation.FlatLegBottom)
                        {
                            if (d < b) //short leg in compression
                            {
                                beta_wCorrected = beta_w;
                            }
                            else
                            {
                                beta_wCorrected = -beta_w;
                            }
                        }
                        else
                        {
                            if (d > b) //short leg in compression
                            {
                                beta_wCorrected = beta_w;
                            }
                            else
                            {
                                beta_wCorrected = -beta_w;
                            }
                        }

                    }
                    else if (CompressionLocation == FlexuralCompressionFiberPosition.Bottom)
                    {

                        if (AngleRotation == AngleRotation.FlatLegBottom)
                        {
                            if (d > b) 
                            {
                                beta_wCorrected = beta_w;
                            }
                            else
                            {
                                beta_wCorrected = -beta_w;
                            }
                        }
                        else
                        {
                            if (d < b)
                            {
                                beta_wCorrected = beta_w;
                            }
                            else
                            {
                                beta_wCorrected = -beta_w;
                            }
                        }
                    }
                    else
                    {
                        throw new CompressionFiberPositionException();
                    }
                    M_e = ((4.9 * E * I_z * C_b) / (Math.Pow(L_b, 2))) * (Math.Sqrt(Math.Pow(beta_wCorrected, 2) + 0.052 * Math.Pow((((L_b * t) / (r_z))), 2)) + beta_wCorrected);
                }
            }
            else
            {
                if (CompressionLocation == FlexuralCompressionFiberPosition.Top)
                {
                    //F10-6a
                    M_e = ((0.66 * E * Math.Pow(b, 4) * t * C_b) / (Math.Pow(L_b, 2))) * (Math.Sqrt(1 + 0.78 * Math.Pow((((L_b * t) / (Math.Pow(b, 2)))), 2)) - 1);
                }
                else
                {
                    //F10-6b
                    M_e = ((0.66 * E * Math.Pow(b, 4) * t * C_b) / (Math.Pow(L_b, 2))) * (Math.Sqrt(1 + 0.78 * Math.Pow((((L_b * t) / (Math.Pow(b, 2)))), 2)) + 1);
                }
                if (BracingType == FlexuralAndTorsionalBracingType.AtPointOfMaximumMoment)
                {
                    M_e = 1.25 * M_e;
                }
            }
            return M_e;
        }

       

    }
}

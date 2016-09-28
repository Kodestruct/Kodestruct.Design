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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;
using Kodestruct.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure;
using Kodestruct.Loads.ASCE7.Entities;
using Kodestruct.Loads.Properties;
using MoreLinq;

namespace Kodestruct.Loads.ASCE7.ASCE7_10.Wind
{


    public partial class WindStructureGeneral : BuildingDirectionalProcedureElement
    {
        /// <summary>
        /// Calculates coefficient C_f for freestanding walls and signs
        /// </summary>
        /// <param name="h">Sign height</param>
        /// <param name="B">Horizontal dimension of sign, in feet</param>
        /// <param name="s">vertical dimension of the sign, in feet </param>
        /// <param name="epsion_s">ratio of solid area to gross area</param>
        /// <param name="x">horizontal distance from  windward edge</param>
        /// <param name="L">Length of return wall</param>
        /// <param name="LoadCase">Case A, B or C</param>
        /// <returns></returns>
        public double GetWallOrSignPressureCoefficient(double h, double B, double s, double epsion_s, double x = 0, double L = 0, FreestandingWallLoadCase LoadCase = FreestandingWallLoadCase.C)
        {
            throw new NotImplementedException();
            double C_f = 0.0;
            if (LoadCase == FreestandingWallLoadCase.A || LoadCase == FreestandingWallLoadCase.B)
            {
                C_f = GetCoefficientCaseAB(h, B, s, epsion_s);
            }
            else if (LoadCase == FreestandingWallLoadCase.C)
            {
                C_f = GetCoefficientCaseC(h, B, s, epsion_s, x, L);
            }
            else
            {
                throw new NotImplementedException();
            }
            Quad q = new Quad();


        }

        private double GetCoefficientCaseC(double h, double B, double s, double epsion_s, double x, double L)
        {
            throw new NotImplementedException();
        }

        private double GetCoefficientCaseAB(double h, double B, double s, double epsion_s)
        {
            throw new NotImplementedException();
            using (StringReader reader = new StringReader(Resources.ASCE7_10Figure29_4_1SignsCaseAB))
            {
                List<double> B_sRatios = GetColumnHeaders(reader);
                List<double> s_hRatios = GetRowHeaders(reader);
                List<List<double>> Data = GetData(reader);
                double ColumnValue = B / s;
                double RowValue = s / h;

                Quad ValueQuad = GetQuad(ColumnValue, RowValue,B_sRatios,s_hRatios,  Data);

                return (double)ValueQuad.GetInterpolatedValue((decimal)ColumnValue, (decimal)RowValue);
            }

        }

        private Quad GetQuad(double ColumnValue, double RowValue, List<double> ColumnHeaders, List<double> RowHeaders, List<List<double>> Data)
        {
            //check for boundaries

            double ColumnLeft, ColumnRight, RowAbove, RowBelow;
            int columnLeftIndex, columnRightIndex, rowAboveIndex, rowBelowIndex;


            if (RowHeaders.Count != Data.Count || ColumnHeaders.Count!=Data[0].Count)
            {
                throw new Exception("Inconsistent data. Row or column headers do not match data.");
            }

            //SPECIAL CASES: coumn or row value out of range
            //----------------------------------------------------

            if (ColumnValue < ColumnHeaders[0] || ColumnValue > ColumnHeaders[ColumnHeaders.Count - 1] || RowValue < RowHeaders[0] || RowValue > RowHeaders[RowHeaders.Count - 1])
            {
                if (ColumnValue < ColumnHeaders[0] || ColumnValue > ColumnHeaders[ColumnHeaders.Count - 1])
                {
                    if (ColumnValue < ColumnHeaders[0])
                    {
                        columnLeftIndex = 0;
                        columnRightIndex = 0;
                    }
                    else // if ( ColumnValue > ColumnHeaders[ColumnHeaders.Count - 1])
                    {
                        columnLeftIndex = ColumnHeaders.Count - 1;
                        columnRightIndex = ColumnHeaders.Count - 1;
                    }

                    //Find low and high ROW values
                    RowAbove = RowHeaders.Where(rv => rv <= RowValue).Max();
                    rowAboveIndex = RowHeaders.LastIndexOf(RowAbove);
                    RowBelow = RowHeaders.Where(rv => rv >= RowValue).Min();
                    rowBelowIndex = RowHeaders.LastIndexOf(RowBelow);
                }


                else //if (RowValue < RowHeaders[0] || RowValue > RowHeaders[RowHeaders.Count - 1])
                {
                    if (RowValue < RowHeaders[0])
                    {
                        rowAboveIndex = 0;
                        rowBelowIndex = 0;
                    }
                    else  //if (RowValue > RowHeaders[RowHeaders.Count-1])
                    {
                        rowAboveIndex = RowHeaders.Count - 1;
                        rowBelowIndex = RowHeaders.Count - 1;
                    }
                    //Find low and high COLUMN values
                    ColumnLeft = ColumnHeaders.Where(cv => cv <= ColumnValue).Max();
                    columnLeftIndex = RowHeaders.LastIndexOf(ColumnLeft);
                    ColumnRight = ColumnHeaders.Where(cv => cv >= ColumnValue).Min();
                    columnRightIndex = RowHeaders.LastIndexOf(ColumnRight);
                }


            }
            else
            {
                //Interpolated value within data table 
                // REGULAR CASE
                //----------------------------------------------------

                List<List<DataPoint2D>> DataPoints = new List<List<DataPoint2D>>();

                //Create array of datapoints
                for (int i = 0; i < RowHeaders.Count; i++)
                {
                    int j = 0;
                    List<DataPoint2D> thisRowOfPoints = new List<DataPoint2D>();

                    foreach (var item in Data[i])
                    {
                        thisRowOfPoints.Add(new DataPoint2D((decimal)ColumnHeaders[j], (decimal)RowHeaders[i], (decimal)Data[i][j]));
                    }
                    DataPoints.Add(thisRowOfPoints);
                }

                ColumnLeft = ColumnHeaders.Where(cv => cv <= ColumnValue).Max();
                columnLeftIndex = ColumnHeaders.FindIndex( x => x==ColumnLeft);
                ColumnRight= ColumnHeaders.Where(cv => cv >= ColumnValue).Min();
                columnRightIndex = ColumnHeaders.FindIndex(x => x ==ColumnRight);


                RowAbove = RowHeaders.Where(cv => cv <= RowValue).Max();
                rowAboveIndex = RowHeaders.FindIndex(x => x==RowAbove);
                RowBelow = RowHeaders.Where(cv => cv >= RowValue).Min();
                rowBelowIndex = RowHeaders.FindIndex(x => x ==RowBelow);
            }


            DataPoint2D lowerLeftPoint  = new DataPoint2D((decimal)ColumnHeaders[columnLeftIndex], (decimal)RowHeaders[rowBelowIndex], (decimal)Data[rowBelowIndex][columnLeftIndex]);
            DataPoint2D lowerRightPoint = new DataPoint2D((decimal)ColumnHeaders[columnRightIndex],(decimal)RowHeaders[rowBelowIndex], (decimal)Data[rowBelowIndex][columnRightIndex]);
            DataPoint2D upperLeftPoint  = new DataPoint2D((decimal)ColumnHeaders[columnLeftIndex], (decimal)RowHeaders[rowAboveIndex], (decimal)Data[rowAboveIndex][columnLeftIndex]);
            DataPoint2D upperRightPoint = new DataPoint2D((decimal)ColumnHeaders[columnRightIndex],(decimal)RowHeaders[rowAboveIndex], (decimal)Data[rowAboveIndex][columnRightIndex]);

            Quad q = new Quad()
            {
                LowerLeftPoint = lowerLeftPoint,
                LowerRightPoint = lowerRightPoint,
                UpperLeftPoint = upperLeftPoint,
                UpperRightPoint = upperRightPoint
            };

            return q;
        }

        private List<List<double>> GetData(StringReader reader)
        {
            throw new NotImplementedException();
        }

        private List<double> GetRowHeaders(StringReader reader)
        {
            throw new NotImplementedException();
        }

        private List<double> GetColumnHeaders(StringReader reader)
        {
            throw new NotImplementedException();
        }

    }
}

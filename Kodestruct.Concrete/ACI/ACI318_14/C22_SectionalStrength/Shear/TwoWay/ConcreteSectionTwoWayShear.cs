using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI.Entities;

namespace Kodestruct.Concrete.ACI.ACI318_14.C22_SectionalStrength.Shear.TwoWay
{
    public class ConcreteSectionTwoWayShear
    {
        /// <summary>
        /// Two-way shear section
        /// </summary>
        /// <param name="Material">Concrete material</param>
        /// <param name="Segments">Shear perimeter segments</param>
        /// <param name="d">Effective slab section</param>
        /// <param name="b_x">Column dimension (normal to slab edge for edge column)</param>
        /// <param name="b_y">Column dimension (parallel to slab edge for edge column)</param>
        /// <param name="AtColumnFace">Identifies if the section is adjacent to column face (typical) or away from column face (as is the case with shear studs away from the face)</param>
        /// <param name="ColumnType">Identifies if the column is located at the interior, slab edge, or slab corner</param>
        public ConcreteSectionTwoWayShear(IConcreteMaterial Material, List<PerimeterLineSegment> Segments, double d,
            double b_x, double b_y, bool AtColumnFace, PunchingPerimeterConfiguration ColumnType, Point2D ColumnCenter)
        {
            this.Material    =Material      ;
            this.Segments    =Segments      ;
            this.d           =d             ;
            this.b_x         =b_x           ;
            this.b_y         =b_y           ;
            this.AtColumnFace=AtColumnFace  ;
            this.ColumnType = ColumnType    ;
            this.ColumnCenter = ColumnCenter;
        }

        /// <summary>
        /// Two-way shear section
        /// </summary>
        /// <param name="Material">Concrete material</param>
        /// <param name="Segments">Shear perimeter segments</param>
        /// <param name="d">Effective slab section</param>
        /// <param name="b_x">Column dimension (normal to slab edge for edge column)</param>
        /// <param name="b_y">Column dimension (parallel to slab edge for edge column)</param>
        /// <param name="AtColumnFace">Identifies if the section is adjacent to column face (typical) or away from column face (as is the case with shear studs away from the face)</param>
        /// <param name="ColumnType">Identifies if the column is located at the interior, slab edge, or slab corner</param>
        public ConcreteSectionTwoWayShear(IConcreteMaterial Material, List<PerimeterLineSegment> Segments, double d,
            double b_x, double b_y, bool AtColumnFace, PunchingPerimeterConfiguration ColumnType)
            :this(Material,Segments,d,b_x,b_y,AtColumnFace,ColumnType, new Point2D(0.0,0.0))
        {

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
        /// Column dimension (normal to slab edge for edge column)
        /// </summary>
        public double b_x { get; set; }

        /// <summary>
        /// Column dimension (parallel to slab edge for edge column)
        /// </summary>
        public double b_y { get; set; }


        /// <summary>
        /// Shear strength (as stress) per table 22.6.5.2
        /// </summary>
        /// <returns></returns>
        public double GetTwoWayStrengthForUnreinforcedConcrete()
        {
            double f_c = Material.SpecifiedCompressiveStrength;
            double v_cControlling = 0.0;

            double lambda = Material.lambda;

            if (AtColumnFace == true)
            {


                double beta = GetBeta();
                double alpha_s = Get_alpha_s();
                double b_o = Get_b_o();
                

                double v_c1 = lambda * 4.0 * Math.Sqrt(f_c);
                double v_c2 = lambda * (2.0 + ((4.0) / (beta))) * Math.Sqrt(f_c);
                double v_c3 = lambda * (((alpha_s * d) / (b_o)) + 2.0) * Math.Sqrt(f_c);

                List<double> vc_s = new List<double>()
            {
                v_c1,
                v_c2,
                v_c3
            };
                v_cControlling =vc_s.Min();
            }
            else
            {
                v_cControlling = lambda * 2.0 * Math.Sqrt(f_c);
            }

            double phi = 0.75;
            return phi * v_cControlling;
        }

        /// <summary>
        /// Returns shear stress
        /// </summary>
        /// <param name="V_u"> Punching shear force</param>
        /// <returns></returns>
        public double GetConcentricShearStress(double V_u)
        {
            double b_o = Get_b_o();
            double v_u = V_u / (b_o * d);
            return v_u;
        }

        /// <summary>
        /// Shear stress due to unbalanced moment going into the column-slab intersection through varying shear distribution per 421.1R-13 section 8.4.4.2.3 
        /// </summary>
        /// <param name="gamma_vM_ux">Adjusted moment around axis normal to slab edge or in the direction of corner bisecting ray. This moment must be reported at column centoid.</param>
        /// <param name="gamma_vM_uy">Adjusted moment around axis parallel to slab edge or in the direction normal to corner bisecting ray. This moment must be reported at column centoid.</param>
        /// <param name="V_u">Punching shear force</param>
        /// <param name="AllowSinglePointStressRedistribution">Determines if the reduction of peak stress if the maximum stress occurs at a single point (per ACI 421.1R-13)</param>
        /// <returns>Maximum and minimum stress (maximum is additive to the concentic shear) </returns>
        public ResultOfShearStressDueToMoment GetShearStressDueToMomement(double gamma_vM_ux, double gamma_vM_uy, 
            double V_u, bool AllowSinglePointStressRedistribution)
        {
            Point2D cen = FindPunchingPerimeterCentroid();
            adjustedSegments = AdjustSegments(cen);

            double J_x = GetJx(adjustedSegments);
            double J_y = GetJy(adjustedSegments);

            double cxMax = Get_c_xMax(adjustedSegments,AllowSinglePointStressRedistribution);
            double cxMin = Get_c_xMin(adjustedSegments,AllowSinglePointStressRedistribution);
            double cyMax = Get_c_yMax(adjustedSegments,AllowSinglePointStressRedistribution);
            double cyMin = Get_c_yMin(adjustedSegments,AllowSinglePointStressRedistribution);

            //Adjust Design moment as a function of the eccentricity of the section centroid to column centroid
            throw new NotImplementedException();
        }

        private double Get_c_yMin(List<PerimeterLineSegment> adjustedSegments, bool AllowSinglePointStressRedistribution)
        {
            throw new NotImplementedException();
        }

        private double Get_c_yMax(List<PerimeterLineSegment> adjustedSegments, bool AllowSinglePointStressRedistribution)
        {
            throw new NotImplementedException();
        }

        private double Get_c_xMin(List<PerimeterLineSegment> adjustedSegments, bool AllowSinglePointStressRedistribution)
        {
            throw new NotImplementedException();
        }

        private double Get_c_xMax(List<PerimeterLineSegment> adjustedSegments, bool AllowSinglePointStressRedistribution)
        {
            throw new NotImplementedException();
        }

        private List<PerimeterLineSegment> AdjustSegments(Point2D cen)
        {
            List<PerimeterLineSegment> movedSegments = new List<PerimeterLineSegment>();
            foreach (var s in Segments)
            {

                PerimeterLineSegment adjustedSeg = new PerimeterLineSegment(
                    new Point2D(s.PointI.X+cen.X, s.PointI.Y+cen.Y),
                    new Point2D(s.PointJ.X + cen.X, s.PointJ.Y + cen.Y)
                    );
                movedSegments.Add(adjustedSeg);
            }


            if (this.ColumnType== PunchingPerimeterConfiguration.CornerLeftBottom ||
                this.ColumnType== PunchingPerimeterConfiguration.CornerLeftTop ||
                this.ColumnType== PunchingPerimeterConfiguration.CornerRightBottom ||
                this.ColumnType== PunchingPerimeterConfiguration.CornerRightTop
                )
            {
                double J_x_bar = GetJx  (movedSegments);
                double J_y_bar = GetJy  (movedSegments);
                double J_xy_bar = GetJxy(movedSegments);

                //421.1R-13 Equation (B-10)
                double theta = Math.Atan(-2.0 * J_xy_bar / (J_x_bar - J_y_bar));

                //Calculate rotated coordinates per B-12 and B-13
                List<PerimeterLineSegment> rotatedSegments = new List<PerimeterLineSegment>();
                foreach (var s in movedSegments)
                {
                    PerimeterLineSegment rotatedSeg = new PerimeterLineSegment(
                    new Point2D(s.PointI.X * Math.Cos(theta) + s.PointI.Y * Math.Sin(theta), -s.PointI.X * Math.Sin(theta) + s.PointI.Y * Math.Cos(theta)),
                    new Point2D(s.PointJ.X * Math.Cos(theta) + s.PointJ.Y * Math.Sin(theta), -s.PointJ.X * Math.Sin(theta) + s.PointJ.Y * Math.Cos(theta))
                    );
                    rotatedSegments.Add(rotatedSeg);
                }

                return rotatedSegments;
            }
            else
            {
                return movedSegments;
            }

        }

        private double GetJxy(List<PerimeterLineSegment> movedSegments)
        {
            //421.1R-13 Equation B-11
            double J_xy = d * movedSegments.Sum(s =>
                s.Length / 6.0 * (2.0*s.PointI.X*s.PointI.Y+s.PointI.X*s.PointJ.Y+s.PointJ.X*s.PointI.Y+2.0*s.PointJ.X*s.PointJ.Y)
                );
            return J_xy;
        }

        private double GetJy(List<PerimeterLineSegment> movedSegments)
        {
            //421.1R-13 Equation B-8
            double J_y = d * movedSegments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.X, 2.0) + s.PointI.X * s.PointJ.X + Math.Pow(s.PointJ.X, 2.0))
                );
            return J_y;
        }

        private double GetJx(List<PerimeterLineSegment> movedSegments)
        {
            //421.1R-13 Equation B-9
            double J_x = d * movedSegments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.Y, 2.0) + s.PointI.Y * s.PointJ.Y + Math.Pow(s.PointJ.Y, 2.0))
                );
            return J_x;
        }

        List<PerimeterLineSegment> adjustedSegments;
        private Point2D FindPunchingPerimeterCentroid()
        {
            double SumLs = Segments.Sum(s => s.Length);
            double SumLsTimesY = Segments.Sum(s => s.Centroid.Y * s.Length);
            double SumLsTimesX = Segments.Sum(s => s.Centroid.X * s.Length);

            double X = SumLsTimesX / SumLs;
            double Y = SumLsTimesY / SumLs;

            return new Point2D(X, Y);
        }

        private double Get_b_o()
        {
            double b_o = Segments.Sum(s => s.Length);
            return b_o;
        }

        /// <summary>
        /// Column coefficient alpha per section 22.6.5.3
        /// </summary>
        /// <returns></returns>
        private double Get_alpha_s()
        {
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    return 40.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    return 30.0;
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    return 20.0;
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    return 20.0;
                    break;
                default:
                    return 40.0;
                    break;
            }
        }

        /// <summary>
        /// Ratio of column sides per footnote Table 22.6.5.2
        /// </summary>
        /// <returns></returns>
        private double GetBeta()
        {
           
            if (b_x>b_y)
            {
                return b_x / b_y;
            }
            else
            {
                return b_y / b_x;
            }
        }


        public double Get_gamma_vx()
        {
            throw new NotImplementedException();
        }

        public double Get_gamma_vy()
        {
            throw new NotImplementedException();
        }

    }
}

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
        /// Shear stress due to unbalanced moment going into the column-slab intersection through varying shear distribution per 421.1R-13 section 8.4.4.2.3 
        /// </summary>
        /// <param name="M_ux">Moment around X axis normal to slab edge or in the direction of corner bisecting ray. This moment must be reported at column centoid.</param>
        /// <param name="M_uy">Moment around Y axis parallel to slab edge or in the direction normal to corner bisecting ray. This moment must be reported at column centoid.</param>
        /// <param name="V_u">Punching shear force</param>
        /// <param name="AllowSinglePointStressRedistribution">Determines if the reduction of peak stress if the maximum stress occurs at a single point (per ACI 421.1R-13)</param>
        /// <returns>Maximum and minimum stress (maximum is additive to the concentic shear) </returns>
        public ResultOfShearStressDueToMoment GetCombinedShearStressDueToMomementAndShear(double M_ux, double M_uy,
            double V_u,  bool AllowSinglePointStressRedistribution=false)
        {
            //ColumnCenter = GetColumnCenter();

            double A_c = AdjustedSegments.Sum(s => s.Length) * d;

            double J_x = GetJx();
            double J_y = GetJy();
            double J_xy = GetJxy();

            double thetaRad = Get_thetaRad(J_xy, J_x, J_y);
            List<PerimeterLineSegment> RotatedSegments = GetRotatedSegments(AdjustedSegments, thetaRad);

            double y_O = GetPunchingPerimeterEccentricityY();
            double x_O = GetPunchingPerimeterEccentricityX();
            double M_x = M_ux + V_u * y_O;
            double M_y = M_uy + V_u * x_O;

            double l_x = Get_l_x(RotatedSegments);
            double l_y = Get_l_y(RotatedSegments);

            //calculate gammas
            double gamma_vx = Get_gamma_vx(l_x, l_y);
            double gamma_vy = Get_gamma_vy(l_x, l_y);

            List<double> shearStressValues = new List<double>();

            //Find cutoff points for redistribution case
            var PointsI = RotatedSegments.Select(seg => seg.PointI).ToList();
            var PointsJ = RotatedSegments.Select(seg => seg.PointJ).ToList();

            PointsI.AddRange(PointsJ);

            List<Point2D> UniquePoints = PointsI.Distinct().ToList();
            double YMax = UniquePoints.Max(p => p.Y);
            double YMin = UniquePoints.Min(p => p.Y);

            double XMax = UniquePoints.Max(p => p.X);
            double XMin = UniquePoints.Min(p => p.X);

            bool IsSinglePointY = false;
            double YMaxCutoff = 0.0;
            double YMinCutoff = 0.0;

            bool IsSinglePointX = false;
            double XMaxCutoff = 0.0;
            double XMinCutoff = 0.0;


            if (UniquePoints.Where(p => p.Y == YMax).ToList().Count == 1)
            {
                YMaxCutoff = YMax - 0.4 * d;
            }
            else
            {
                YMaxCutoff = YMax;
            }

            if (UniquePoints.Where(p => p.Y == YMin).ToList().Count == 1)
            {
                YMinCutoff = YMin + 0.4 * d;
            }
            else
            {
                YMinCutoff = YMin;
            }

            if (UniquePoints.Where(p => p.X == XMax).ToList().Count == 1)
            {
                XMaxCutoff = XMax - 0.4 * d;
            }
            else
            {
                XMaxCutoff = XMax;
            }

            if (UniquePoints.Where(p => p.X == XMin).ToList().Count == 1)
            {
                XMinCutoff = XMin + 0.4 * d;
            }
            else
            {
                XMinCutoff = XMin;
            }
            //double x = Math.Abs(point.X - PunchingPerimeterCentroid.X);
            //double y = Math.Abs(point.Y - PunchingPerimeterCentroid.Y);

            foreach (var p in UniquePoints)
            {
                double X = 0.0;
                double Y = 0.0;

                if (AllowSinglePointStressRedistribution == false)
                {

                    X = p.X;
                    Y = p.Y;
                }
                else
                {


                    X = p.X > XMaxCutoff ? XMaxCutoff : p.X;
                    X = p.X < XMinCutoff ? XMinCutoff : p.Y;

                    Y = p.Y > YMaxCutoff ? YMaxCutoff : p.Y;
                    Y = p.Y < YMinCutoff ? YMinCutoff : p.Y;
                }

                double v_u_I = GetShearStressFromMomentAndShear(M_x, M_y, V_u, J_x, J_y, A_c, gamma_vx, gamma_vy, X, Y);
                shearStressValues.Add(v_u_I);
            }

            double v_min = shearStressValues.Min();
            double v_max = shearStressValues.Max();

            ResultOfShearStressDueToMoment result = new ResultOfShearStressDueToMoment(v_max, v_min,gamma_vx,gamma_vy);
            return result;
        }





        private double GetShearStressFromMomentAndShear(double M_ux, double M_uy, double V_u,
            double J_x, double J_y, double A_c,
            double gamma_vx, double gamma_vy, double x, double y)
        {


            double v_u = ((V_u) / (A_c)) + ((gamma_vx * M_ux * y) / (J_x)) + ((gamma_vy * M_uy * x) / (J_y));

            return v_u;
        }

        #region Factor used to determine unbalanced moment
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

        #region Eccentricity for moment adjustment
        private double GetPunchingPerimeterEccentricityX()
        {
            double X_p = PunchingPerimeterCentroid.X;
            double X_c = ColumnCenter.X;
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    return Math.Abs(X_c - X_p);
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    return X_c - X_p; //expected negative in most cases
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    return X_p - X_c; //expected negative in most cases
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    return Math.Abs(X_c - X_p);
                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    return Math.Abs(X_c - X_p);
                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    return X_c - X_p; //expected negative in most cases
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    return X_p - X_c; //expected negative in most cases
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    return X_p - X_c; //expected negative in most cases
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    return X_c - X_p; //expected negative in most cases
                    break;
                default:
                    return Math.Abs(X_c - X_p);
                    break;
            }
        }

        private double GetPunchingPerimeterEccentricityY()
        {
            double Y_p = PunchingPerimeterCentroid.Y;
            double Y_c = ColumnCenter.Y;
            switch (ColumnType)
            {
                case PunchingPerimeterConfiguration.Interior:
                    return Math.Abs(Y_c - Y_p);
                    break;
                case PunchingPerimeterConfiguration.EdgeLeft:
                    return Math.Abs(Y_c - Y_p);
                    break;
                case PunchingPerimeterConfiguration.EdgeRight:
                    return Math.Abs(Y_c - Y_p);
                    break;
                case PunchingPerimeterConfiguration.EdgeTop:
                    return Y_p - Y_c; //typically negative
                    break;
                case PunchingPerimeterConfiguration.EdgeBottom:
                    return Y_c - Y_p; //typically positive
                    break;
                case PunchingPerimeterConfiguration.CornerLeftTop:
                    return Y_p - Y_c; //typically negative
                    break;
                case PunchingPerimeterConfiguration.CornerRightTop:
                    return  Y_p - Y_c; //typically positive
                    break;
                case PunchingPerimeterConfiguration.CornerRightBottom:
                    return  Y_c - Y_p; //typically positive
                    break;
                case PunchingPerimeterConfiguration.CornerLeftBottom:
                    return  Y_c - Y_p; //typically positive
                    break;
                default:
                    return Math.Abs(Y_c - Y_p);
                    break;
            }
        }
        #endregion

        #region Projection of critical section on principal axis
        private double Get_l_y(List<PerimeterLineSegment> RotatedSegments)
        {
            double YMin = RotatedSegments.Min(s => Math.Min(s.PointI.Y, s.PointJ.Y));
            double YMax = RotatedSegments.Max(s => Math.Min(s.PointI.Y, s.PointJ.Y));
            double ly = YMax - YMin;

            return ly;
        }

        private double Get_l_x(List<PerimeterLineSegment> RotatedSegments)
        {
            double XMin = RotatedSegments.Min(s => Math.Min(s.PointI.X, s.PointJ.X));
            double XMax = RotatedSegments.Max(s => Math.Min(s.PointI.X, s.PointJ.X));
            double lX = XMax - XMin;

            return lX;
        }
        #endregion

        #region Principal axis
        private List<PerimeterLineSegment> GetRotatedSegments(List<PerimeterLineSegment> adjustedSegments, double thetaRad)
        {
            if (ColumnType == PunchingPerimeterConfiguration.CornerLeftBottom ||
                ColumnType == PunchingPerimeterConfiguration.CornerLeftTop ||
                ColumnType == PunchingPerimeterConfiguration.CornerRightBottom ||
                ColumnType == PunchingPerimeterConfiguration.CornerRightTop)
            {


                List<PerimeterLineSegment> RotatedSegments = new List<PerimeterLineSegment>();
                foreach (var seg in adjustedSegments)
                {
                    Point2D pi = GetRotatedPoint(seg.PointI, thetaRad);
                    Point2D pj = GetRotatedPoint(seg.PointJ, thetaRad);
                    PerimeterLineSegment newSeg = new PerimeterLineSegment(pi, pj);
                    RotatedSegments.Add(newSeg);
                }
                return RotatedSegments;
            }
            else
            {
                return adjustedSegments;
            }
        }

        private Point2D GetRotatedPoint(Point2D point2D, double thetaRad)
        {
            double newX = point2D.X * Math.Cos(thetaRad) + point2D.Y * Math.Sin(thetaRad);
            double newY = -point2D.X * Math.Sin(thetaRad) + point2D.Y * Math.Cos(thetaRad);
            return new Point2D(newX, newY);
        }

        private double Get_thetaRad(double J_xy, double J_x, double J_y)
        {
            //ACI 421R-08 Equation B-10
            double theta = Math.Atan(-2.0 * J_xy / (J_x - J_y)) / 2.0;
            return theta;
        }

        #endregion

        #region Critical section properties


        private List<PerimeterLineSegment> AdjustSegments(Point2D cen)
        {
            List<PerimeterLineSegment> movedSegments = new List<PerimeterLineSegment>();
            foreach (var s in Segments)
            {

                PerimeterLineSegment adjustedSeg = new PerimeterLineSegment(
                    new Point2D(s.PointI.X - cen.X, s.PointI.Y - cen.Y),
                    new Point2D(s.PointJ.X - cen.X, s.PointJ.Y - cen.Y)
                    );
                movedSegments.Add(adjustedSeg);
            }


            if (this.ColumnType == PunchingPerimeterConfiguration.CornerLeftBottom ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerLeftTop ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerRightBottom ||
                this.ColumnType == PunchingPerimeterConfiguration.CornerRightTop
                )
            {
                double J_x_bar = GetJx();
                double J_y_bar = GetJy();
                double J_xy_bar = GetJxy();

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

        public double GetJxy()
        {
            //421.1R-13 Equation B-11
            double J_xy = d * AdjustedSegments.Sum(s =>
                s.Length / 6.0 * (2.0 * s.PointI.X * s.PointI.Y + s.PointI.X * s.PointJ.Y + s.PointJ.X * s.PointI.Y + 2.0 * s.PointJ.X * s.PointJ.Y)
                );
            return J_xy;
        }

        public double GetJy()
        {
            //421.1R-13 Equation B-8
            double J_y = d * AdjustedSegments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.X, 2.0) + s.PointI.X * s.PointJ.X + Math.Pow(s.PointJ.X, 2.0))
                );
            return J_y;
        }

        public double GetJx()
        {
            //421.1R-13 Equation B-9
            double J_x = d * AdjustedSegments.Sum(s =>
                s.Length / 3.0 * (Math.Pow(s.PointI.Y, 2.0) + s.PointI.Y * s.PointJ.Y + Math.Pow(s.PointJ.Y, 2.0))
                );
            return J_x;
        }

        List<PerimeterLineSegment> adjustedSegments;
        public List<PerimeterLineSegment> AdjustedSegments
        {
            get {

                return adjustedSegments;
            }
            }
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
        #endregion


        private Point2D _PunchingPerimeterCentroid;

        public Point2D PunchingPerimeterCentroid
        {
            get
            {
                if (_PunchingPerimeterCentroid == null)
                {
                    _PunchingPerimeterCentroid = FindPunchingPerimeterCentroid();
                }
                return _PunchingPerimeterCentroid;
            }
            set { _PunchingPerimeterCentroid = value; }
        }


    }
}

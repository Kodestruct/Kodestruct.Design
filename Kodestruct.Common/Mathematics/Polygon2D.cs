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
 
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Kodestruct.Common.Section;
//using Kodestruct.Common.Section.General;

//namespace Kodestruct.Common.Mathematics
//{
//    public class Polygon
//    {
//        public int NofContours;
//        public bool[] ContourIsHole;
//        public VertexList[] Contour;

//        public Polygon()
//        {
//        }

//        // path should contain only polylines ( use Flatten )
//        // furthermore the constructor assumes that all Subpathes of path except the first one are holes
//        public Polygon(GraphicsPath path)
//        {
//            NofContours = 0;
//            byte[] pathTypes = path.PathTypes;
//            PointF[] pathPoints = path.PathPoints;

//            foreach (byte b in pathTypes)
//            {
//                if ((b & ((byte)PathPointType.CloseSubpath)) != 0)
//                    NofContours++;
//            }

//            ContourIsHole = new bool[NofContours];
//            Contour = new VertexList[NofContours];
//            for (int i = 0; i < NofContours; i++)
//                ContourIsHole[i] = (i == 0);

//            int contourNr = 0;
//            ArrayList contour = new ArrayList();
//            for (int i = 0; i < pathPoints.Length; i++)
//            {
//                contour.Add(pathPoints[i]);
//                if ((path.PathTypes[i] & ((byte)PathPointType.CloseSubpath)) != 0)
//                {
//                    PointF[] pointArray = (PointF[])contour.ToArray(typeof(PointF));
//                    VertexList vl = new VertexList(pointArray);
//                    Contour[contourNr++] = vl;
//                    contour.Clear();
//                }
//            }
//        }

//        public static Polygon FromFile(string filename, bool readHoleFlags)
//        {
//            return GpcWrapper.ReadPolygon(filename, readHoleFlags);
//        }

//        public void AddContour(List<Point2D> points, bool contourIsHole)
//        {
//            int NumPts = points.Count;
//            PointF[] pointFs = new PointF[NumPts];
//            for (int i = 0; i < NumPts; i++)
//            {
//                pointFs[i] = new PointF((float)points[i].X,(float)points[i].Y);
//            }
//            VertexList vl = new VertexList(pointFs);
//            this.AddContour(vl, contourIsHole);
//        }

//        public void AddContour(VertexList contour, bool contourIsHole)
//        {
//            bool[] hole = new bool[NofContours + 1];
//            VertexList[] cont = new VertexList[NofContours + 1];

//            for (int i = 0; i < NofContours; i++)
//            {
//                hole[i] = ContourIsHole[i];
//                cont[i] = Contour[i];
//            }
//            hole[NofContours] = contourIsHole;
//            cont[NofContours++] = contour;

//            ContourIsHole = hole;
//            Contour = cont;
//        }

//        public GraphicsPath ToGraphicsPath()
//        {
//            GraphicsPath path = new GraphicsPath();

//            for (int i = 0; i < NofContours; i++)
//            {
//                PointF[] points = Contour[i].ToPoints();
//                if (ContourIsHole[i])
//                    Array.Reverse(points);
//                path.AddPolygon(points);
//            }
//            return path;
//        }

//        public override string ToString()
//        {
//            string s = "Polygon with " + NofContours.ToString() + " contours." + "\r\n";
//            for (int i = 0; i < NofContours; i++)
//            {
//                if (ContourIsHole[i])
//                    s += "Hole: ";
//                else
//                    s += "Contour: ";
//                s += Contour[i].ToString();
//            }
//            return s;
//        }

//        public Tristrip ClipToTristrip(GpcOperation operation, Polygon polygon)
//        {
//            return GpcWrapper.ClipToTristrip(operation, this, polygon);
//        }

//        public Polygon Clip(GpcOperation operation, Polygon polygon)
//        {
//            return GpcWrapper.Clip(operation, this, polygon);
//        }

//        public Tristrip ToTristrip()
//        {
//            return GpcWrapper.PolygonToTristrip(this);
//        }

//        public void Save(string filename, bool writeHoleFlags)
//        {
//            GpcWrapper.SavePolygon(filename, writeHoleFlags, this);
//        }
//    }
//}



////using System;
////using System.Collections;
////using System.Collections.Generic;
////using System.Drawing;
////using System.Drawing.Drawing2D;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Kodestruct.Common.Section;
////using Kodestruct.Common.Section.General;
////using Kodestruct.Common.Section.General.Gpc;

////namespace Kodestruct.Common.Mathematics
////{

////    public class Polygon2D 
////    {
////        public int NofContours;
////        public bool[] ContourIsHole;
////        public Point2DList[] Contour;

////        public Polygon2D()
////        {
////        }

////        // path should contain only polylines ( use Flatten )
////        // furthermore the constructor assumes that all Subpathes of path except the first one are holes
////        public Polygon2D(GraphicsPath path)
////        {
////            NofContours = 0;
////            foreach (byte b in path.PathTypes)
////            {
////                if ((b & ((byte)PathPointType.CloseSubpath)) != 0)
////                    NofContours++;
////            }

////            ContourIsHole = new bool[NofContours];
////            Contour = new Point2DList[NofContours];
////            for (int i = 0; i < NofContours; i++)
////                ContourIsHole[i] = (i == 0);

////            int contourNr = 0;
////            ArrayList contour = new ArrayList();
////            for (int i = 0; i < path.PathPoints.Length; i++)
////            {
////                contour.Add(path.PathPoints[i]);
////                if ((path.PathTypes[i] & ((byte)PathPointType.CloseSubpath)) != 0)
////                {
////                    PointF[] pointArray = (PointF[])contour.ToArray(typeof(PointF));
////                    Point2DList vl = new Point2DList(pointArray);
////                    Contour[contourNr++] = vl;
////                    contour.Clear();
////                }
////            }
////        }

////        public void AddContour(List<Point2D> contour, bool contourIsHole)
////        {
////            Point2DList contourPointList = new Point2DList(contour);
////            this.AddContour(contourPointList, contourIsHole);
////        }
////        public void AddContour(Point2DList contour, bool contourIsHole)
////        {
////            bool[] hole = new bool[NofContours + 1];
////            Point2DList[] cont = new Point2DList[NofContours + 1];

////            for (int i = 0; i < NofContours; i++)
////            {
////                hole[i] = ContourIsHole[i];
////                cont[i] = Contour[i];
////            }
////            hole[NofContours] = contourIsHole;
////            cont[NofContours++] = contour;

////            ContourIsHole = hole;
////            Contour = cont;
////        }


////        public override string ToString()
////        {
////            string s = "Polygon with " + NofContours.ToString() + " contours." + "\r\n";
////            for (int i = 0; i < NofContours; i++)
////            {
////                if (ContourIsHole[i])
////                    s += "Hole: ";
////                else
////                    s += "Contour: ";
////                s += Contour[i].ToString();
////            }
////            return s;
////        }


////    }

////}

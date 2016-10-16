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
 
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Kodestruct.Common.Mathematics
//{

//    public class Point2DList
//    {
//        public int NofVertices;
//        public Point2D[] Vertex;

//        public Point2DList()
//        {
//        }


//        public Point2DList(List<Point2D> Vertices)
//        {
//            NofVertices = Vertices.Count;
//            Vertex = new Point2D[NofVertices];
//            for (int i = 0; i < NofVertices; i++)
//                Vertex[i] = new Point2D((double)Vertices[i].X, (double)Vertices[i].Y);
//        }

//        public Point2DList(PointF[] p)
//        {
//            NofVertices = p.Length;
//            Vertex = new Point2D[NofVertices];
//            for (int i = 0; i < p.Length; i++)
//                Vertex[i] = new Point2D((double)p[i].X, (double)p[i].Y);
//        }

//        public GraphicsPath ToGraphicsPath()
//        {
//            GraphicsPath graphicsPath = new GraphicsPath();
//            graphicsPath.AddLines(ToPoints());
//            return graphicsPath;
//        }

//        public PointF[] ToPoints()
//        {
//            PointF[] vertexArray = new PointF[NofVertices];
//            for (int i = 0; i < NofVertices; i++)
//            {
//                vertexArray[i] = new PointF((float)Vertex[i].X, (float)Vertex[i].Y);
//            }
//            return vertexArray;
//        }

//        public GraphicsPath TristripToGraphicsPath()
//        {
//            GraphicsPath graphicsPath = new GraphicsPath();

//            for (int i = 0; i < NofVertices - 2; i++)
//            {
//                graphicsPath.AddPolygon(new PointF[3]{ new PointF( (float)Vertex[i].X,   (float)Vertex[i].Y ),
//                                                        new PointF( (float)Vertex[i+1].X, (float)Vertex[i+1].Y ),
//                                                        new PointF( (float)Vertex[i+2].X, (float)Vertex[i+2].Y )  });
//            }

//            return graphicsPath;
//        }

//        public override string ToString()
//        {
//            string s = "Polygon with " + NofVertices + " vertices: ";

//            for (int i = 0; i < NofVertices; i++)
//            {
//                s += Vertex[i].ToString();
//                if (i != NofVertices - 1)
//                    s += ",";
//            }
//            return s;
//        }
//    }

//}

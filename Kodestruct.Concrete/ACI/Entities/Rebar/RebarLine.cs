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
using System.Windows;

namespace Kodestruct.Concrete.ACI
{
    public class RebarLine 
    {


        private IRebarMaterial rebarMaterial;

        public IRebarMaterial Material
        {
            get { return rebarMaterial; }
            set { rebarMaterial = value; }
        }


        private bool isEpoxyCoated;

        public bool IsEpoxyCoated
        {
            get { return isEpoxyCoated; }
            set { isEpoxyCoated = value; }
        }

        int NumberOfSubdivisions;

        public double A_total { get; set; }

        public RebarLine(double A_total, Point2D StartPoint, Point2D EndPoint,
            IRebarMaterial rebarMaterial, bool setBackCornerBars, bool IsEpoxyCoated =false, int NumberOfSubdivisions = 20)
        {
            this.A_total = A_total;
            this.NodeI = StartPoint;
            this.NodeJ = EndPoint;
            this.isEpoxyCoated = IsEpoxyCoated;
            this.rebarMaterial = rebarMaterial;
            this.NumberOfSubdivisions = NumberOfSubdivisions;
            this.setBackCornerBars = setBackCornerBars;
        }

        public Point2D NodeI { get; set; }
        public Point2D NodeJ { get; set; }

        bool setBackCornerBars;

        List<RebarPoint> rebarPoints;
       public  List<RebarPoint> RebarPoints 
        {
            get {
                CalculateElements();
                return rebarPoints; }
        }

        

        protected virtual void CalculateElements()
        {
            List<RebarPoint> RebarPoints = new List<RebarPoint>();


            double  dx = NodeJ.X - NodeI.X;
            double  dy = NodeJ.Y - NodeI.Y;


            Vector seg = new Vector(dx, dy);
            int N = NumberOfSubdivisions;
            int NumberOfRebarPoints = NumberOfSubdivisions + 1;

            double segDx;
            double segDy;

            double PointArea = A_total / NumberOfRebarPoints;
            Rebar rebar = new Rebar(PointArea, rebarMaterial);


            if (setBackCornerBars == false)
            {
               
                segDx = dx / N;
                segDy = dy / N;

                RebarCoordinate coord1 = new RebarCoordinate(NodeI.X, NodeI.Y);
                RebarPoints.Add(new RebarPoint(rebar, coord1));

                for (int i = 0; i < NumberOfSubdivisions; i++)
                {
                    RebarCoordinate Pt = new RebarCoordinate(NodeI.X + (i + 1) * segDx, NodeI.Y + (i + 1) * segDy);
                    RebarPoints.Add(new RebarPoint(rebar, Pt));
                }
            }
            else
            {
                segDx = dx / (N +1.0);
                segDy = dy / (N +1.0);

                for (int i = 0; i < NumberOfSubdivisions; i++)
                {
                    RebarCoordinate Pt = new RebarCoordinate(NodeI.X + (i + 1) * segDx, NodeI.Y + (i + 1) * segDy);
                    RebarPoints.Add(new RebarPoint(rebar, Pt));
                }

            }
            this.rebarPoints = RebarPoints;
        }
    }
}

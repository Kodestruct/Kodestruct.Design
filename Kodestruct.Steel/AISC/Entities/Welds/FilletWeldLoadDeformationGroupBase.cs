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
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.Mathematics;
using Kodestruct.Steel.AISC.Exceptions;

namespace Kodestruct.Steel.AISC.SteelEntities.Welds
{
    /// <summary>
    /// Weld group is comprised of linear segments.
    /// </summary>
    public class FilletWeldLoadDeformationGroupBase: ConnectionGroup
    {
        /// <summary>
        /// Constructor (parameterless)
        /// </summary>
        public FilletWeldLoadDeformationGroupBase()
        {
            
        }

        public FilletWeldLoadDeformationGroupBase(List<FilletWeldLine> Lines)
        {
            this.lines = Lines;
        }

        /// <summary>
        /// Adds a straight linear segment to the weld segment collection.
        /// </summary>
        /// <param name="l1"></param>
        public void AddLine(FilletWeldLine l1)
        {
            if (lines==null)
            {
                lines = new List<FilletWeldLine>();
            }
            if (lines.Contains(l1))
            {
                throw new DuplicateWeldLineException();
            }
            else
            {
                lines.Add(l1);
            }
        }

        private List<FilletWeldLine> lines;

        public List<FilletWeldLine> Lines
        {
            get 
            { 
                return lines; 
            }
            set { lines = value; }
        }


        /// <summary>
        /// Calculates moment of inertia around X-axis of the group with respect to
        /// any 2D point in the plane of the weld.
        /// </summary>
        /// <param name="refPoint">Point to calculate moment of inertia around</param>
        /// <returns> rMoment of Inertia X</returns>
        public double CalculateMomentOfIntertiaX(Point2D refPoint)
        {
            double Ix = 0;
            if (lines==null)
            {
                throw new WeldSegmentsNotDefinedException();
            }
            foreach (var line in Lines)
            {
                double thisLineIx = line.GetInertiaXAroundPoint(refPoint);
                Ix = Ix + thisLineIx;
            }
            return Ix;
        }

        /// <summary>
        /// Calculates moment of inertia around Y-axis of the group with respect to
        /// any 2D point in the plane of the weld.
        /// </summary>
        /// <param name="refPoint">Point to calculate moment of inertia around</param>
        /// <returns> Moment of Inertia Y</returns>
        public double CalculateMomentOfIntertiaY(Point2D refPoint)
        {
            double Iy = 0;
            if (lines == null)
            {
                throw new WeldSegmentsNotDefinedException();
            }
            foreach (var line in Lines)
            {
                double thisLineIy = line.GetInertiaYAroundPoint(refPoint);
                Iy = Iy + thisLineIy;
            }
            return Iy;
        }

        public Point2D GetElasticCentroid()
        {
            double A_full = 0;
            double SumA_x=0;
            double SumA_y= 0;

            if (lines == null)
            {
                throw new WeldSegmentsNotDefinedException();
            }
            foreach (var l in Lines)
            {
                A_full = A_full + l.GetArea();
                SumA_x = SumA_x + l.GetSumAreaDistanceX(new Point2D(0, 0));
                SumA_y = SumA_y + l.GetSumAreaDistanceY(new Point2D(0, 0));
            }
            double cenX = SumA_x / A_full;
            double cenY = SumA_y / A_full;
            return new Point2D(cenX, cenY);
        }

        public double CalculatePolarMomentOfInertia(Point2D centroid)
        {
            double Ix = CalculateMomentOfIntertiaX(centroid);
            double Iy = CalculateMomentOfIntertiaY(centroid);
            double J = Ix + Iy;
            return J;
        }

        List<ILocationArrayElement> analyticalPoints;
        protected override List<ILocationArrayElement> GetICElements()
        {
            if (analyticalPoints == null)
            {
                analyticalPoints = new List<ILocationArrayElement>();
                foreach (var line in this.Lines)
                {
                    foreach (var weldElem in line.WeldElements)
                    {
                        analyticalPoints.Add(weldElem);
                    }
                }
            }
            return analyticalPoints;
        }

        protected override double GetElementForce(ILocationArrayElement element, Point2D center, ILocationArrayElement controllingWeld, double angle)
        {
            FilletWeldElement el = element as FilletWeldElement;
            FilletWeldElement cp = controllingWeld as FilletWeldElement;
            double elementForce =0;
            if (el!=null && cp !=null)
            {
                double theta = el.GetAngleTheta(center);
                        
                //this weld actual deformation
                double Delta_r = el.DistanceFromCentroid * (cp.LimitDeformation / cp.DistanceFromCentroid);
                //this weld ultimate deformation
                double pi = Delta_r / el.UltimateLoadDeformation;
                //double pi = Delta_r / el.LimitDeformation;
                //calculate element force
                elementForce = el.CalculateNominalShearStrength(pi, theta);


            }
            return elementForce;

        }
        /// <summary>
        /// Deformation at weld fracture From D.Lesik D.Kennedy. 
        /// Ultimate Strength of eccentrically loaded fillet weld connections.
        /// </summary>
        /// <param name="Leg"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        private double GetElementFractureDeformation(double Leg, double theta)
        {
            double Delta_u = Math.Min(1.087 * Leg * Math.Pow(theta + 6, -0.65), 0.17 * Leg);
            return Delta_u;
        }
        /// <summary>
        /// Deformation at ultimate force From D.Lesik D.Kennedy. 
        /// Ultimate Strength of eccentrically loaded fillet weld connections.
        /// </summary>
        /// <param name="Leg"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        private double GetElementUltimateForceDeformation(double Leg, double theta)
        {
            double Delta_u = Math.Min(0.209 * Leg * Math.Pow(theta + 2, -0.32), 0.17 * Leg);
            return Delta_u;
        }

        private List<FilletWeldElement> weldElements;

        private List<FilletWeldElement> WeldElements
        {
            get {
                if (weldElements == null)
                {
                    weldElements = GetWeldElements();
                }
                return weldElements; }

        }

        /// <summary>
        /// Combine weld elements from all lines into a single collection.
        /// </summary>
        /// <returns></returns>
        private List<FilletWeldElement> GetWeldElements()
        {
            List<FilletWeldElement> weldPoints = new List<FilletWeldElement>();

            foreach (var line in this.Lines)
            {
                foreach (var weldElem in line.WeldElements)
                {
                    FilletWeldElement el = weldElem as FilletWeldElement;
                    if (el != null)
                    {
                        weldPoints.Add(el);
                    }
                }
            }
            return weldPoints;
        }
        

        /// <summary>
        /// Finds the controlling element in the weld group.
        /// </summary>
        /// <param name="Center">Instantaneous center (IC) of rotation.</param>
        /// <returns></returns>
        protected override ILocationArrayElement FindUltimateDeformationElement(Point2D Center)
        {
            FilletWeldElement governingElement = null;
            double DeltaMaxToRMin = double.PositiveInfinity;

            foreach (var we in WeldElements)
            {
                double DeltaFracture_n = GetElementFractureDeformation(we.LegLength, we.GetAngleTheta(Center));
                double DeltaUltimate_n = GetElementUltimateForceDeformation(we.LegLength, we.GetAngleTheta(Center));
                double rn = we.GetDistanceToPoint(Center);

                //store ultmate deformation and distance to IC for further use.
                we.LimitDeformation = DeltaFracture_n;
                we.UltimateLoadDeformation = DeltaUltimate_n;
                we.DistanceFromCentroid = rn;
                
                if (DeltaFracture_n / rn < DeltaMaxToRMin)
                {
                    DeltaMaxToRMin = DeltaFracture_n / rn;
                    governingElement = we;
                }
            }
            return governingElement;
        }
    }
}

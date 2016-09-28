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
using Kodestruct.Concrete.ACI.Entities;


namespace Kodestruct.Concrete.ACI
{
    public class RebarSection
    {
        public RebarSection(RebarDesignation Designation)
        {
            this.Designation = Designation;
        }
        public RebarSection(RebarDesignation Designation,double Diameter,  double Area)
        {
            this.Designation = Designation;
            this.Diameter = Diameter;
            this.Area = Area;
        }

        public RebarSection(double Diameter, double Area)
        {
            this.Diameter = Diameter;
            this.Area = Area;
        }

        bool DesignationSet;

        private RebarDesignation designation;

        public RebarDesignation Designation
        {
            get { return designation; }
            set {
                DesignationSet = true;
                designation = value; }
        }

        private double db;

        public double Diameter
        {
            get {

                if (db == 0)
                {
                    CalculateProperties();
                } 
                return db; }
            set { db = value; }
        }

        private double A;

        public double Area
        {
            get {
                if (A==0)
                {
                    CalculateProperties();
                }
                return A; }
            set { A = value; }
        }

        private void CalculateProperties()
        {
            if (DesignationSet==true)
            {
                RebarSectionFactory factory = new RebarSectionFactory();
                RebarSection sec = factory.GetRebarSection(Designation);
                if (sec != null)
                {
                    this.A = sec.A;
                    this.db = sec.Diameter;
                } 
            }
        }
    }
}

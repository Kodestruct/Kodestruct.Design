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
using Kodestruct.Concrete.ACI.Entities;
 

namespace Kodestruct.Concrete.ACI
{
    public class Rebar : INonPrestressedReinforcement
    {
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private RebarDesignation designation;

        public RebarDesignation Designation
        {
            get { return designation; }
            set { 
                designation = value;
                if (Section!=null)
                {
                    
                    Section.Designation = value;
                }
            }
        }

        private double db;

        public double Diameter
        {
            get { return db; }
            set { 
                db = value;
                if (Section!=null)
                {
                    Section.Diameter = value;
                }
            }
        }

        private double A;

        public double Area
        {
            get { return A; }
            set { 
                A = value;
                if (Section!=null)
                {
                    Section.Area = value;
                }
            }
        }

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

        private RebarSection section;

        public RebarSection Section
        {
            get { return section; }
            set { section = value; }
        }
        


        public Rebar(double Diameter, bool IsEpoxyCoated, IRebarMaterial rebarMaterial)
        {
            Section = new RebarSection(Diameter, 0);
            this.db = Diameter;
            this.isEpoxyCoated = IsEpoxyCoated;
            this.rebarMaterial = rebarMaterial;
        }

        public Rebar(double Area, IRebarMaterial rebarMaterial)
        {
            Section = new RebarSection(0,Area);
            this.A = Area;
            this.rebarMaterial = rebarMaterial;
        }


        public double GetDesignForce()
        {
            double stress = this.rebarMaterial.GetDesignStress();
            double Area = this.A;
            return Area * stress;
        }
        public double GetDesignStress()
        {
            double stress = this.rebarMaterial.GetDesignStress();
            return stress;
        }


        public double GetForce(double Strain)
        {
            double stress = this.rebarMaterial.GetStress(Strain);
            double Area = this.A;
            return Area * stress;
        }

        public double GetStress(double Strain)
        {
            return this.rebarMaterial.GetStress(Strain);
        }


    }
}

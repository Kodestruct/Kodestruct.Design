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

namespace Kodestruct.Wood.NDS.NDS_2015.CLT
{
    public class CltSection
    {
        public int NumberOfLayers { get; set; }
        public double MajorDirectionLayerThickness { get; set; }
        public double MinorDirectionLayerThickness { get; set; }
        public double b { get; set; }
        public double E_major { get; set; }
        public double E_minor { get; set; }
        public CltSection(int NumberOfLayers, double MajorDirectionLayerThickness, double MinorDirectionLayerThickness, double Width,  double E_major, double E_minor)
        {
            this.NumberOfLayers = NumberOfLayers;
            this.MajorDirectionLayerThickness = MajorDirectionLayerThickness;
            this.MinorDirectionLayerThickness = MinorDirectionLayerThickness;
            this.E_major = E_major;
            this.E_minor = E_minor;
            this.b = Width;
            if (NumberOfLayers%2 == 0)
            {
                throw new Exception("Only odd number of layers is supported.");
            }
        }

        //public  double Get_EI_effective()
        //{
        //    List<Lamination> laminations = GetLaminations();
        //}

        //private double GetTotalThickness()
        //{
        //    double h_cumulative = 0;
        //    for (int i = 0; i < NumberOfLayers; i++)
        //    {
        //        if (i == 0) //major
        //        {

        //        }
        //        else
        //        {
        //            if (i % 2 == 0) // major
        //            {

        //            }
        //            else //minor
        //            {

        //            }

        //        }
        //    }
        //}

        //private List<Lamination> GetLaminations()
        //{
        //    List<Lamination> laminations = new List<Lamination>();
        //    for (int i = 0; i < NumberOfLayers; i++)
        //    {
        //        if (i == 0) //major
        //        {
        //            laminations.Add(new Lamination(MajorDirectionLayerThickness, b, E_major));
        //        }
        //        else
        //        {
        //            if (i % 2 == 0) // major
        //            {
        //                laminations.Add(new Lamination(MajorDirectionLayerThickness, b, E_major));
        //            }
        //            else //minor
        //            {
        //                laminations.Add(new Lamination(MinorDirectionLayerThickness, b, E_minor));
        //            }
     
        //        }
        //    }
        //}

        class Lamination
        {
            public double h { get; set; }
            public double b { get; set; }
            public double E { get; set; }
            public double z { get; set; }

            public Lamination()
            {

            }
            public Lamination(double h, double b, double E, double z)
            {
                this.b = b;
                this.h = h;
                this.E = E;
            }
        }
    }
}

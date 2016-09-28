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
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Kodestruct.Common.Section.General
//{
//    //this class stores information about a slice of section that is 
//    //used by  ISliceableSection interface
//    // this allows the use of simplified approach to when ISliceableSection functionality
//    //is required but the section cannot be easily described analytically

//    public class SectionSliceRectangular: ISectionSlice
//    {
//        public SectionSliceRectangular(double Width, double MinY, double MaxY)
//        {
//            this.Width = Width;
//            this.MinY = MinY;
//            this.MaxY = MaxY;
//        }
//        public double MinY { get; set; }
//        public double MaxY { get; set; }
//        public double Width { get; set; }

//        public double GetCentroidY()
//        {
//            return (MaxY - MinY) / 2.0;
//        }

//        public double GetArea()
//        {
//            return (MaxY - MinY) * Width;
//        }
//    }
//}

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
using System.Threading.Tasks;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section
{
    /// <summary>
    /// This class is a generic implementation of IMoveableSection.
    /// It is used for output of section slice in the ISliceableSection implementation.
    /// The IMoveableSection member values must be provided to this class
    /// in the constructor.
    /// </summary>
    public class SectionSlice : IMoveableSection
    {
        public Point2D PlasticCentroidCoordinate { get; set; }


        public Point2D GetElasticCentroidCoordinate()
        {
            throw new NotImplementedException();
        }



        public double YMax
        {
            get { throw new NotImplementedException(); }
        }

        public double YMin
        {
            get { throw new NotImplementedException(); }
        }

        public double XMax
        {
            get { throw new NotImplementedException(); }
        }

        public double XMin
        {
            get { throw new NotImplementedException(); }
        }

        public double A
        {
            get { throw new NotImplementedException(); }
        }


        public double I_x
        {
            get { throw new NotImplementedException(); }
        }
    }
}

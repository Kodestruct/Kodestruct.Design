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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public  class BoltFactory
    {
        string MaterialId;
        public BoltFactory(string MaterialId)
        {
            this.MaterialId = MaterialId;
        }
        public IBoltMaterial GetBoltMaterial()
        {
            IBoltMaterial m =null;
            switch (MaterialId)
            {
                //case "A108": m = new ThreadedBoltMaterial(65.0); break;
                case "A325": m=new BoltGroupAMaterial(); break;
                case "A490": m=new BoltGroupBMaterial(); break;
                case "F1852":m=new BoltGroupAMaterial();  break;
                //case "A36": m = new ThreadedBoltMaterial(58.0); break;
                //case "A193 Grade B7": m = new ThreadedBoltMaterial(100.0); break; //Can use higher value, up to 125 ksi for smaller diameters
                case "A307": m=new BoltA307Material();  break;
                case "A354GradeBC": m = new BoltGroupAMaterial(); break;  //This is per AISC spec. Design guide 1 gives higher values
                case "A354GradeBD": m = new BoltGroupBMaterial(); break;  //This is per AISC spec. Design guide 1 gives higher values
                case "A449": m=new BoltGroupAMaterial();break;
                //case "A572": break; //TODO: eliminate this from material selection node
                //case "A588": m = new ThreadedBoltMaterial(70.0); break; // for large diameters (over 4") this is unconservative.
                //case "A687": m = new ThreadedBoltMaterial(150.0); break;  //AISC indicates 150ksi MAX
                case "F1554Grade105": m = new ThreadedBoltMaterial(125.0); break;  //Design guide 1 Table 2.2
                case "F1554Grade55": m = new ThreadedBoltMaterial(75.0); break;    //Design guide 1 Table 2.2
                case "F1554Grade36": m = new ThreadedBoltMaterial(58.0); break;    //Design guide 1 Table 2.2
                //case "A572 Grade 42": m = new ThreadedBoltMaterial(60.0); break;     //AISC Manual Table 2-6
                //case "A572 Grade 50": m = new ThreadedBoltMaterial(65.0); break;     //AISC Manual Table 2-6
                //case "A572 Grade 55": m = new ThreadedBoltMaterial(70.0); break;     //AISC Manual Table 2-6
                //case "A572 Grade 60": m = new ThreadedBoltMaterial(75.0); break;     //AISC Manual Table 2-6
                //case "A572 Grade 65": m = new ThreadedBoltMaterial(80.0); break;     //AISC Manual Table 2-6
                default: throw new Exception("Unrecognized bolt material. Check input");

            }
                return m;
        }

        public IBoltBearing GetBearingBolt(double Diameter, BoltThreadCase ThreadType)
        {
            IBoltMaterial bm = null;
            IBoltBearing bb = null;
            CalcLog log = new CalcLog();
            switch (MaterialId)
            {
                case "A325": bb = new BoltBearingGroupA(Diameter, ThreadType, log); break;
                case "A490": bb = new BoltBearingGroupB(Diameter, ThreadType, log); break;
                case "F1852": bb = new BoltBearingGroupA(Diameter, ThreadType, log); break;
                case "A307": bm = new BoltA307Material(); bb=new BoltBearingThreadedGeneral(Diameter, ThreadType, bm, log); break;
                case "A354GradeBC": bb = new BoltBearingGroupA(Diameter, ThreadType, log); break;  
                case "A354GradeBD": bb = new BoltBearingGroupB(Diameter, ThreadType, log); break;  
                case "A449": bb = new BoltBearingGroupA(Diameter, ThreadType, log); break;
                case "F1554Grade105": bm = new ThreadedBoltMaterial(125.0); bb = new BoltBearingThreadedGeneral(Diameter, ThreadType, bm, log); break; 
                case "F1554Grade55": bm = new ThreadedBoltMaterial(75.0); bb = new BoltBearingThreadedGeneral(Diameter, ThreadType, bm, log); break;   
                case "F1554Grade36": bm = new ThreadedBoltMaterial(58.0); bb = new BoltBearingThreadedGeneral(Diameter, ThreadType, bm, log); break;   
                default: throw new Exception("Unrecognized bolt material. Check input");

            }
            return bb;
        }

        public IBoltSlipCritical GetSlipCriticalBolt(double Diameter, BoltThreadCase ThreadType, BoltFayingSurfaceClass SurfaceClass, BoltHoleType HoleType, BoltFillerCase FillerCase, double NumberOfSlipPlanes)
        {
            CalcLog log = new CalcLog();
            int NPlanes = (int)NumberOfSlipPlanes;
            IBoltSlipCritical bsc = null;
            switch (MaterialId)
            {
                case "A325": bsc = new BoltSlipCriticalGroupA(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                case "A490": bsc = new BoltSlipCriticalGroupB(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                case "F1852": bsc = new BoltSlipCriticalGroupA(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                case "A354GradeBC": bsc = new BoltSlipCriticalGroupA(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                case "A354GradeBD": bsc = new BoltSlipCriticalGroupB(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                case "A449": bsc = new BoltSlipCriticalGroupA(Diameter, ThreadType, SurfaceClass, HoleType, FillerCase, NPlanes, log); break;
                default: throw new Exception("Unrecognized bolt material or specified material cannot be used for high-strength bolting. Check input");

            }
            return bsc;
        }

        public IBoltBearing GetBearingBolt(double Diameter, string ThreadType)
        {
            BoltThreadCase Thread;
            if (ThreadType=="X")
            {
                Thread = BoltThreadCase.Excluded;
            }
            else
            {
                Thread = BoltThreadCase.Included;
            }
            return this.GetBearingBolt(Diameter, Thread);
        }
    }
}

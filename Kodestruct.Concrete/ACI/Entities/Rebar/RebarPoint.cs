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

namespace Kodestruct.Concrete.ACI
{
    public class RebarPoint
    {
        public RebarPoint()
        {

        }

        public RebarPoint(IRebar Rebar, RebarCoordinate Coordinate)
        {
            this.Rebar = Rebar;
            this.Coordinate = Coordinate;
        }

        public IRebar Rebar { get; set; }
        public RebarCoordinate Coordinate { get; set; }

        List<RebarStrain> strains;
        
        public void AddRebarStrain(double Strain, string LoadcaseName)
        {
            if (this.strains == null)
            {
                strains = new List<RebarStrain>();
            }
            else
            {
                var casesWithThisName = strains.Where(c => c.LoadCaseName == LoadcaseName).ToList();
                if (casesWithThisName!=null)
                {
                    throw new Exception(String.Format("Strain with loadcase {0} already exists", LoadcaseName));
                }
            }
            strains.Add(new RebarStrain(Strain,LoadcaseName));
           
        }
        
        public double GetStress(string LoadCaseName)
        {
            double fs;
            RebarStrain caseWithThisName = strains.Where(c => c.LoadCaseName == LoadCaseName).First();
            if (caseWithThisName !=null)
            {
                fs = Rebar.Material.GetStress(caseWithThisName.Strain);
            }
            else
            {
                throw new Exception (string.Format("No strain data found for load case name {0}.",LoadCaseName));
            }
            return fs;
        }
        public double GetForce(string LoadCaseName)
        {
            double f = GetStress(LoadCaseName);
            double A = Rebar.Area;
            return f * A;
        }
        
    }
}

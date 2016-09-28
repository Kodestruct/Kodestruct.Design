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
using Kodestruct.Wood.NDS.Entities;

namespace Kodestruct.Wood.NDS.Material
{
    public static class CommercialGradeStringConverter
    {
        public static string GetCommercialGradeString(CommercialGrade Grade)
        {
            string GradeString = "";
            switch (Grade)
            {
                case CommercialGrade.SelectStructural:
                    GradeString = "Select Structural";
                    break;
                case CommercialGrade.No1:
                    GradeString = "No.1";
                    break;
                case CommercialGrade.No2:
                    GradeString = "No.2";
                    break;
                case CommercialGrade.No3:
                    GradeString = "No.3";
                    break;
                case CommercialGrade.Stud:
                    GradeString = "Stud";
                    break;
                case CommercialGrade.Construction:
                    GradeString = "Construction";
                    break;
                case CommercialGrade.Standard:
                    GradeString = "Standard";
                    break;
                case CommercialGrade.Utility:
                    GradeString = "Utility";
                    break;
                case CommercialGrade.ClearStructural:
                    GradeString = "Clear Structural";
                    break;
                case CommercialGrade.SelectStructuralOpenGrain:
                    GradeString = "Select Structural Open Grain";
                    break;
                case CommercialGrade.No1OpenGrain:
                    GradeString = "No.1 Open Grain";
                    break;
                case CommercialGrade.No2OpenGrain:
                    GradeString = "No.2 Open Grain";
                    break;
                case CommercialGrade.No3OpenGrain:
                    GradeString = "No.3 Open Grain";
                    break;
                case CommercialGrade.No1AndBetter:
                    GradeString = "No.1 And Better";
                    break;
                default:
                    break;
            }
            return GradeString;
        }
    }
}

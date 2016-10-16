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

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads
{
    public class ComponentReportEntry
    {
        public string Description { get; set; }
        public double LoadValue { get; set; }
        public string LoadNotes { get; set; }
        public string ReferenceNotes { get; set; }
        public ComponentReportEntry()
        {


        }
        public ComponentReportEntry(string Description,  double LoadValue, string LoadNotes, string ReferenceNotes)
        {
           this.Description    =Description    ;
           this.LoadValue      =LoadValue      ;
           this.LoadNotes      =LoadNotes      ;
           this.ReferenceNotes =ReferenceNotes ;
        }
    }
}

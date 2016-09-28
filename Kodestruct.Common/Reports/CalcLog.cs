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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Newtonsoft.Json;


namespace Kodestruct.Common.CalculationLogger
{
    public class CalcLog : ICalcLog
    {
        private string calculatorID;

        public string CalculatorID
        {
            get { return calculatorID; }
            set { calculatorID = value; }
        }
        
        public CalcLog()
        {
            entries = new List<ICalcLogEntry>();
        }

        private List<ICalcLogEntry> entries;

        public List<ICalcLogEntry> GetEntriesList()
        {
            return entries;
        }

        public List<ICalcLogEntry>  Entries
        {
            get { return entries; }
            set { entries = value; }
        }
        

        public ICalcLogEntry CreateNewEntry()
        {
            CalcLogEntry entry = new CalcLogEntry();
            return entry;
        }

        public void AddEntry(ICalcLogEntry Entry)
        {
            entries.Add(Entry);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #region Serialization

       
        /// <summary>
        /// This constructor is required for the JSON deserializer to be able
        /// to identify concrete classes to use when deserializing the interface properties.
        /// </summary
        [JsonConstructor]
        public CalcLog(List<CalcLogEntry> entries)
        {
            this.entries = entries.Cast<ICalcLogEntry>().ToList();
        }

        #endregion
    }
}

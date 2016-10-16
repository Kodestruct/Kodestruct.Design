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
using System.Runtime.Serialization;
using System.Text;
using Kodestruct.Common.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Common.CalculationLogger
{

    public class CalcLogEntry : ICalcLogEntry
    {

        private string valueName;

        public string ValueName
        {
            get { return valueName; }
            set { valueName = value; }
        }


        private string variableValue;

        public string VariableValue
        {
            get { return variableValue; }
            set { variableValue = value; }
        }


        private string reference;

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }


        private string formulaID;

        public string FormulaID
        {
            get { return formulaID; }
            set { formulaID = value; }
        }

        private Dictionary<string,string> dependencyValues;

        public Dictionary<string,string> DependencyValues
        {
            get 
            {
                return dependencyValues; 
            }
            set 
            { 
                dependencyValues = value; 
            }
        }

        private string descriptionReference;

        public string DescriptionReference
        {
            get { return descriptionReference; }
            set { descriptionReference = value; }
        }


        private List<List<string>> tableData;

        public List<List<string>> TableData
        {
            get { return tableData; }
            set { tableData = value; }
        }


        private string templateTableTitle;

        public string TemplateTableTitle
        {
            get { return templateTableTitle; }
            set { templateTableTitle = value; }
        }
        

        public CalcLogEntry()
        {
            dependencyValues = new Dictionary<string, string>();
        }

        public void AddDependencyValue(string Key, string value)
        {
            DependencyValues.Add(Key, value);
        }

        public void AddDependencyValue(string Key, double value)
        {
            DependencyValues.Add(Key, value.ToString());
        }

        public Dictionary<string, string> GetDependencyValues()
        {
            return dependencyValues;
        }
    }
}

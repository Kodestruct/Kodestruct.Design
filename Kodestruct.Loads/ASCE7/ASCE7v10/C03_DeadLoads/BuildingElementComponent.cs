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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Kodestruct.Common.Data;
using Kodestruct.Loads.Properties;

namespace Kodestruct.Loads.ASCE.ASCE7_10.DeadLoads
{
    //individual component
    public class BuildingElementComponent
    {

        public BuildingElementComponent(string id) :
            this(id, 0, 0, 0.0, null)
        {

        }

        public BuildingElementComponent(string id, double numericValue):
            this(id,0,0, numericValue,null)
        {
           
        }
        public BuildingElementComponent(double numericValue, string customDescription) :
            this(null, 0, 0, numericValue, customDescription)
        {

        }

        public BuildingElementComponent(string id, int Option1, int Option2, double NumericValue, string CustomDescription)
        {
                this.Id = id;
                this.Option1 = Option1;
                this.Option2 = Option2;
                this.NumericValue = NumericValue;
                this.Description = CustomDescription;
                ComponentNote = "";

        }

        public string Id { get; set; }
        public int Option1 { get; set; }
        public int Option2 { get; set; }
        public double NumericValue { get; set; }
        public string Description { get; set; }

        public string CustomDescription { get; set; }
        public string ComponentNote { get; set; }

        public ComponentReportEntry GetComponentWeight()
        {
            //read the table

            #region Read Summary Table

            var SampleValue = new { ComponentId = "", Description = "", Load =0.0, HasLoad = true, ReferenceThickness= 0.0, HasReferenceThickness = true, Notes = "", SpecialCaseClassName = "", Reference =""}; // sample
            var LoadList = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.DeadLoadSummaryTable))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    double load, refThickness;
                    string[] Vals = line.Split(',');
                    if (Vals.Count() == 7)
                    {
                        string ComponentId = (string)Vals[0];
                        string Description = (string)Vals[1];
                        bool HasLoadValue = double.TryParse(Vals[2], out load); //if (ind1Res == true) Option1Index = ind1;
                        bool HasReferenceThickness = double.TryParse(Vals[4], out refThickness); //if (ind2Res == true) Option2Index = ind1;
                        string Notes = (string)Vals[3];
                        string SpecialCaseClassName = (string)Vals[5];
                        string Reference = (string)Vals[6];
                        LoadList.Add(
                            new
                            {
                                ComponentId = ComponentId,
                                Description = Description,
                                Load = load,
                                HasLoad = HasLoadValue,
                                ReferenceThickness = refThickness,
                                HasReferenceThickness = HasReferenceThickness,
                                Notes = Notes,
                                SpecialCaseClassName = SpecialCaseClassName,
                                Reference = Reference
                            });
                    }
                }

            }

            #endregion

            double q;
            string Note = null;
            string SourceReference = null;

            var inf = LoadList.Where(l => l.ComponentId == this.Id).FirstOrDefault();
            if (inf!=null)
            {
                //if component is VAR , create subclass
                if (inf.HasLoad==true)
                {
                    if (inf.HasReferenceThickness ==true)
                    {
                        double q_ref = inf.Load;
                        double t = NumericValue;
                        double t_ref = inf.ReferenceThickness;
                        q = q_ref * t / t_ref;
                        string ThicknessNote = "t=" + Math.Round(t, 3).ToString(CultureInfo.InvariantCulture) + " in. " + inf.Notes;
                        return new ComponentReportEntry(inf.Description, q, ThicknessNote, inf.Reference);
                    }
                    else
                    {
                        q = inf.Load;
                        return new ComponentReportEntry(inf.Description, q, inf.Notes, inf.Reference);
                    }
                }
                else
                {
                    string className = inf.SpecialCaseClassName;
                    if (className!=null)
                    {
                        //create class instance
                        Assembly execAssembly = Assembly.GetExecutingAssembly();
                        AssemblyName assemblyName = new AssemblyName(execAssembly.FullName);
                        string execAssemblyName = assemblyName.Name;
                        string typeStr = execAssemblyName + ".ASCE.ASCE7_10.DeadLoads.Components." + className;
                        try
                        {
                            Type specialClassType = execAssembly.GetType(typeStr);
                            //IBuildingComponent componentClass = (IBuildingComponent)Activator.CreateInstance(specialClassType);
                            IBuildingComponent componentClass = (IBuildingComponent)Activator.CreateInstance(specialClassType, Option1, Option2,NumericValue);
                            q = componentClass.Weight;
                            Note = componentClass.Notes;
                            SourceReference = inf.Reference;
                            //inf.Notes is by definition empty
                            string description = null;
                            if (this.CustomDescription!=null)
                            {
                                description = this.CustomDescription;
                                Note = "";
                                SourceReference = "";
                            }
                            else
                            {
                                description = inf.Description;
                            }
                            return new ComponentReportEntry(description, q, Note, SourceReference);
                        }
                        catch
                        {
                            throw new DeadLoadInvalidParametersException(Id);
                        }

                    }
                    else
                    {
                        throw new DeadLoadInvalidParametersException(Id);
                    }

                }
            }
            else
            {
                throw new DeadLoadIdNotFoundException(Id);
            }

        }
    }


}

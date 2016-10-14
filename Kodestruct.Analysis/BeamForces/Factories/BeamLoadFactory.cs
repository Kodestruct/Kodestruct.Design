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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Interfaces;

namespace Kodestruct.Analysis
{
    public class BeamLoadFactory : IBeamLoadFactory
    {


       //public IParameterExtractor ParameterExtractor  {get;set;}
        protected BeamFactoryData d {get;set;}
       public BeamLoadFactory(BeamFactoryData data)
       {
           this.d = data;
       }
        //public BeamLoadFactory(IParameterExtractor Extractor)
        //{
        //    this.ParameterExtractor = Extractor;
        //}
        public LoadBeam GetLoad(string BeamCaseId)
        {
            LoadBeam Load = null;
            string LoadGroup =  BeamCaseId.Substring(1, 1); //simple, overhang, etc
            string loadingType = BeamCaseId.Substring(2, 1); //A-concentrated, B-distributed etc ...
            string subCase = BeamCaseId.Substring(BeamCaseId.Length - 1, 1); // specific sub-case

                    //L = ParameterExtractor.GetParam("L");
                    //P = ParameterExtractor.GetParam("P");
                    //M = ParameterExtractor.GetParam("M");
                    //w = ParameterExtractor.GetParam("w");
                    //LoadDimension_a = ParameterExtractor.GetParam("LoadDimension_a");
                    //LoadDimension_b = ParameterExtractor.GetParam("LoadDimension_b");
                    //LoadDimension_c= L-LoadDimension_a-LoadDimension_b; //e.GetParam("LoadDimension_c");
                    //P1 = ParameterExtractor.GetParam("P1");
                    //P2 = ParameterExtractor.GetParam("P2");
                    //M1 = ParameterExtractor.GetParam("M1");
                    //M2 = ParameterExtractor.GetParam("M2");

            Load = GetBeamLoad(loadingType,subCase);

             return Load;
        }

        public virtual LoadBeam GetBeamLoad(string loadingType, string subCase)
        {
        LoadBeam Load = null;

             switch (loadingType)
            {
                case "A": //concentrated loads
                    return GetConcentratedCase(subCase);

                case "B": //distributed loads
                    return GetUniformDistributedCase(subCase);

                case "C": //partially distributed loads
                    return GetPartialDistributedCase(subCase);

                case "D": //Varying loads
                     return GetVaryingCase(subCase);

                case "E": //Moments
                     return GetMomentCase(subCase);

                default:
                    break;
            }
            return Load;
        }

        public virtual LoadBeam GetConcentratedCase(string subCase)
        {
            LoadBeam Load = null;

            switch (subCase)
            {
                case "1":
                    Load = new LoadConcentratedSpecial(d.P);
                    break;
                case "2":
                    Load = new LoadConcentratedGeneral(d.P, d.a_load);
                    break;
                case "3":
                    Load = new LoadConcentratedDoubleSymmetrical(d.P, d.a_load);
                    break;
                case "4":
                    Load = new LoadConcentratedDoubleUnsymmetrical(d.P1, d.P2, d.a_load, d.b_load);
                    break;
                case "5":
                    Load = new LoadConcentratedCenterWithEndMoments(d.P, d.M1, d.M2);
                    break;
                default:
                    Load = null;
                    break;
            }
            return Load;
        }
        public virtual LoadBeam GetUniformDistributedCase(string subCase)
        {
            LoadBeam Load = null;
            switch (subCase)
            {
                case "1":
                    Load = new LoadDistributedUniform(d.w);
                    break;
                case "2":
                    Load = new LoadDistributedUniformWithEndMoments(d.w, d.M1, d.M2);
                    break;
            }
            return Load;
        }
        public virtual LoadBeam GetPartialDistributedCase(string subCase)
        {
            LoadBeam Load = null;
                    Load = new LoadDistributedGeneral(d.w, d.a_load, d.a_load + d.b_load);
            return Load;
        }
        public virtual LoadBeam GetVaryingCase(string subCase)
        {
            LoadBeam Load = null;
            switch (subCase)
            {
                case "1":
                    Load = new LoadDistributedUniform(d.w, LoadDistributedSpecialCase.Triangle);
                    break;
                case "2":
                    Load = new LoadDistributedUniform(d.w, LoadDistributedSpecialCase.DoubleTriangle);
                    break;
            }
            return Load;
        }
        public virtual LoadBeam GetMomentCase(string subCase)
        {
            LoadBeam Load = null;
            switch (subCase)
            {
                case "1":
                    Load = new LoadMomentLeftEnd(d.M);
                    break;
                case "2":
                    Load = new LoadMomentGeneral(d.M, d.a_load);
                    break;
                case "3":
                    Load = new LoadMomentBothEnds(d.M1, d.M2);
                    break;
                case "4":
                    Load = new LoadMomentRightEnd(d.M);
                    break;
            }
            return Load;
        }
    }
}

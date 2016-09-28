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
using Kodestruct.Analysis.Section;

namespace Kodestruct.Analysis.Torsion
{
    public class TorsionalFunctionFactory
    {
        public ITorsionalFunction GetTorsionalFunction(string Case, double E, double G, double J, double L, double z, double T, double C_w, double t, double alpha)
        {
            TorsionalFunctionCase FunctionCase = (TorsionalFunctionCase)Enum.Parse(typeof(TorsionalFunctionCase), Case);
            return GetTorsionalFunction(FunctionCase,E, G, J, L, z, T, C_w, t, alpha);
        }
        public ITorsionalFunction  GetTorsionalFunction(TorsionalFunctionCase Case, double E, double G,double J,double L,double z, double T, double C_w,double t,double alpha)
        {
            TorsionalParameters torProps = new TorsionalParameters();
            double a = torProps.Get_a(C_w, E, G, J);

            ITorsionalFunction function = null;
            switch (Case)
            {
                case TorsionalFunctionCase.Case1:   function =new  TorsionalFunctionCase1(G,J,L,z,T);     break;                              
                case TorsionalFunctionCase.Case2:   function =new  TorsionalFunctionCase2(G,J,L,z,a, T);     break;                                              
                case TorsionalFunctionCase.Case3:   function =new  TorsionalFunctionCase3(G,J,L,z,a,T,alpha);     break;                                      
                case TorsionalFunctionCase.Case4:   function =new  TorsionalFunctionCase4(G,J,L,z,a,t);     break;                                              
                case TorsionalFunctionCase.Case5:   function =new  TorsionalFunctionCase5(G,J,L,z,a,t);     break;                                          
                case TorsionalFunctionCase.Case6:   function =new  TorsionalFunctionCase6(G,J,L,z,a,T,alpha);     break;                                             
                case TorsionalFunctionCase.Case7:   function =new  TorsionalFunctionCase7(G,J,L,z,a,t);     break;                                                 
                case TorsionalFunctionCase.Case8:   function =new  TorsionalFunctionCase8(G,J,L,z,a,t);     break;                                             
                case TorsionalFunctionCase.Case9:   function =new  TorsionalFunctionCase9(G,J,L,z,a,T,alpha);     break;                                                
                case TorsionalFunctionCase.Case10:  function =new  TorsionalFunctionCase10(G,J,L,z,a,t,alpha);     break;                                                
                case TorsionalFunctionCase.Case11:  function =new  TorsionalFunctionCase11(G,J,L,z,a,t);     break;
                case TorsionalFunctionCase.Case12: function = new  TorsionalFunctionCase12(G, J, L, z, a, t); break;
            }

            return function;
        }


    }
}

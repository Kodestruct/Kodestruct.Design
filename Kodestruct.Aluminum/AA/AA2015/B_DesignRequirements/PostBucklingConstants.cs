using Kodestruct.Aluminum.AA.Entities.Enums;
using Kodestruct.Aluminum.AA.Entities.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.DesignRequirements
{
    public partial class BucklingConstantFactory
    {

        private double _k_1;

        public double k_1
        {
            get {
                _k_1 = Get_k_1();
                return _k_1; }

        }

        private double Get_k_1()
        {
            throw new NotImplementedException();
        }


        private double _k_2;

        public double k_2
        {
            get
            {
                _k_2 = Get_k_2();
                return _k_2;
            }

        }

        private double Get_k_2()
        {
            throw new NotImplementedException();
        }
        
        
    }
}

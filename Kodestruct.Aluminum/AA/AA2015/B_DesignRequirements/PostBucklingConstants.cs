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
            double k_1=0;

            switch (BucklingType)
            {
                case BucklingType.UniformCompression:
                    if (Material.Temper.StartsWith("T5")
                        || Material.Temper.StartsWith("T6")
                        || Material.Temper.StartsWith("T7")
                        || Material.Temper.StartsWith("T8")
                        || Material.Temper.StartsWith("T9"))
                    {
                        if (WeldCase == Entities.WeldCase.WeldAffected)
                        {
                            return 0.5;
                        }
                        else
                        {
                            return 0.35;
                        }
                    }
                    break;
                case BucklingType.FlexuralCompression:
                    return 0.5;
                    break;
                default:
                    throw new Exception("Postbuckling constant k_1 cannot be calculated for selected buckling type");
                    break;
            }
            return k_1;
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
            double k_2 = 0;

            switch (BucklingType)
            {
                case BucklingType.UniformCompression:
                    if (Material.Temper.StartsWith("T5")
                        || Material.Temper.StartsWith("T6")
                        || Material.Temper.StartsWith("T7")
                        || Material.Temper.StartsWith("T8")
                        || Material.Temper.StartsWith("T9"))
                    {
                        if (WeldCase == Entities.WeldCase.WeldAffected)
                        {
                            return 2.04;
                        }
                        else
                        {
                            return 2.27;
                        }
                    }
                    break;
                case BucklingType.FlexuralCompression:
                    return 2.04;
                    break;
                default:
                    throw new Exception("Postbuckling constant k_2 cannot be calculated for selected buckling type");
                    break;
            }
            return k_2; 
        }
        
        
    }
}

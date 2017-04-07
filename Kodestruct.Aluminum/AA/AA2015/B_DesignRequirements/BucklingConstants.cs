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
        private enum MaterialCase
        {
            Case1,
            Case2
        }

        private MaterialCase GetMaterialCase()
           
        {
            if (Material.Temper.StartsWith("O") ||
                Material.Temper.StartsWith("H") ||
                Material.Temper.StartsWith("T1") ||
                Material.Temper.StartsWith("T2") ||
                Material.Temper.StartsWith("T3") ||
                Material.Temper.StartsWith("T4") )
            {
                return MaterialCase.Case1;
            }
            else
            {
                        return MaterialCase.Case2;
            }

        }

        BucklingType    BucklingType        {get; set;}
        SubElementType  SubElementType   { get; set; }
        IAluminumMaterial Material { get; set; }

        public BucklingConstantFactory(BucklingType BucklingType, SubElementType SubElementType, IAluminumMaterial Material)
        {
            this.BucklingType    =BucklingType  ;
            this.SubElementType = SubElementType;
            this.Material = Material;
        }
        private double _B;

        public double B
        {
            get {
                _B = Get_B();
                return _B; }
        }

        private double Get_B()
        {
            MaterialCase c = GetMaterialCase();
            double kappa = 1.0;

            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {
                if (c == MaterialCase.Case1)
                {
                    double B_c = F_cy * (1 + Math.Pow((((F_cy) / (1000 * kappa))), 1/2.0));
                    return B_c;
                }
                else
                {
                    double B_c = F_cy * (1 + Math.Pow((((F_cy) / (2250 * kappa))), 1/2.0));
                    return B_c;
                }
            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:
                        if (SubElementType == SubElementType.Flat)
                        {
                            if (c == MaterialCase.Case1)
                            {
                                double B_p = F_cy * (1 + Math.Pow((((F_cy) / (440 * kappa))), 1 / 3.0));
                                return B_p;
                            }
                            else
                            {
                                double B_p = F_cy * (1 + Math.Pow((((F_cy) / (1500 * kappa))), 1/3.0));
                                return B_p;
                            }
                            
                        }
                        else
                        {
                            throw new Exception("Curved sub-elements are not supported");
                        }
                        break;
                    case BucklingType.FlexuralCompression:
                        if (SubElementType == SubElementType.Flat)
                        {
                            if (c == MaterialCase.Case1)
                            {
                                double B_br = 1.3 * F_cy * (1 + Math.Pow((((F_cy) / (340 * kappa))), 1/3.0));
                                return B_br;
                            }
                            else
                            {
                                double B_br = 1.3 * F_cy * (1 + Math.Pow((((F_cy) / (340 * kappa))), 1 / 3.0));
                                return B_br;
                            }
                        }
                        else
                        {
                            throw new Exception("Curved sub-elements are not supported");
                        }
                        break;
                    case BucklingType.Shear:
                        if (SubElementType == SubElementType.Flat)
                        {
                            if (c == MaterialCase.Case1)
                            {
                                double B_s=F_sy*(1+Math.Pow((((F_sy) / (240*kappa))), 1/3.0));
                                return B_s;
                            }
                            else
                            {
                                double B_s = F_sy * (1 + Math.Pow((((F_sy) / (800 * kappa))), 1/3.0));
                                return B_s;
                            }
                        }
                        else
                        {
                            throw new Exception("Curved sub-elements are not supported");
                        }
                        break;
                    default:
                        throw new Exception("BucklingType not recognized.");
                        break;
                }
            }
           
        }



        private double _D;

        public double D
        {
            get {
                _D = Get_D();
                return _D; }
        }

        private double Get_D()
        {

            MaterialCase c = GetMaterialCase();

            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {

            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    case BucklingType.FlexuralCompression:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    case BucklingType.Shear:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    default:
                        break;
                }
            }
            throw new NotImplementedException();
        }

        private double _C;

        public double C
        {
            get {
                _C = Get_C();
                return _C; }
        }

        private double Get_C()
        {
            MaterialCase c = GetMaterialCase();

            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {

            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    case BucklingType.FlexuralCompression:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    case BucklingType.Shear:
                        if (SubElementType == SubElementType.Flat)
                        {

                        }
                        else
                        {

                        }
                        break;
                    default:
                        break;
                }
            }
            throw new NotImplementedException();
        }

        private double _F_cy;

        public double F_cy
        {
            get {
                _F_cy = this.Material.F_cy;
                return _F_cy; }

        }

        private double _F_sy;

        public double F_sy
        {
            get
            {
                _F_sy = this.Material.F_sy;
                return _F_cy;
            }

        }
    }
}

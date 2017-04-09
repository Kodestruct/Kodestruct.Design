using Kodestruct.Aluminum.AA.Entities;
using Kodestruct.Aluminum.AA.Entities.Enums;
using Kodestruct.Aluminum.AA.Entities.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.DesignRequirements
{
    public partial class MaterialBucklingConstantProvider
    {
        BucklingType BucklingType { get; set; }
        SubElementType SubElementType { get; set; }
        //IAluminumMaterial Material { get; set; }
        IAluminumMaterial Material;
        WeldCase WeldCase { get; set; }
        MaterialCase MatCase;

        public MaterialBucklingConstantProvider(BucklingType BucklingType, SubElementType SubElementType, IAluminumMaterial Material, WeldCase WeldCase)
        {
            this.BucklingType = BucklingType;
            this.SubElementType = SubElementType;
            this.Material = Material;
            this.WeldCase = WeldCase;
            string temper = Material.Temper;
        }

        #region B


        private double _B;

        public double B
        {
            get
            {
                if (_B == 0)
                {
                    _B = Get_B();
                }

                return _B;
            }
        }

        private double Get_B()
        {

            double kappa = 1.0;

            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {
                if (MatCase == MaterialCase.Case1)
                {
                    double B_c = F_cy * (1 + Math.Pow((((F_cy) / (1000 * kappa))), 1 / 2.0));
                    return B_c;
                }
                else
                {
                    double B_c = F_cy * (1 + Math.Pow((((F_cy) / (2250 * kappa))), 1 / 2.0));
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
                            if (MatCase == MaterialCase.Case1)
                            {
                                double B_p = F_cy * (1 + Math.Pow((((F_cy) / (440 * kappa))), 1 / 3.0));
                                return B_p;
                            }
                            else
                            {
                                double B_p = F_cy * (1 + Math.Pow((((F_cy) / (1500 * kappa))), 1 / 3.0));
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
                            if (MatCase == MaterialCase.Case1)
                            {
                                double B_br = 1.3 * F_cy * (1 + Math.Pow((((F_cy) / (340 * kappa))), 1 / 3.0));
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
                            if (MatCase == MaterialCase.Case1)
                            {
                                double B_s = F_sy * (1 + Math.Pow((((F_sy) / (240 * kappa))), 1 / 3.0));
                                return B_s;
                            }
                            else
                            {
                                double B_s = F_sy * (1 + Math.Pow((((F_sy) / (800 * kappa))), 1 / 3.0));
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



        #endregion

        #region D


        private double _D;

        public double D
        {
            get
            {
                if (_D == 0)
                {
                    _D = Get_D();
                }

                return _D;
            }
        }

        private double Get_D()
        {
            double E = Material.E;
            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {
                double B_c = B;
                if (MatCase == MaterialCase.Case1)
                {
                    double D_c = ((B_c) / (20.0)) * Math.Pow((((6.0 * B_c) / (E))), 0.5);
                    return D_c;
                }
                else
                {
                    double D_c = ((B_c) / (10.0)) * Math.Pow((((6.0 * B_c) / (E))), 0.5);
                    return D_c;
                }
            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:
                        if (SubElementType == SubElementType.Flat)
                        {
                            double B_p = B;
                            if (MatCase == MaterialCase.Case1)
                            {
                                
                                double D_p = ((B_p) / (20.0)) * Math.Pow((((6.0 * B_p) / (E))), 0.5);
                                return D_p;
                            }
                            else
                            {
                                double D_p = ((B_p) / (10.0)) * Math.Pow((((6.0 * B_p) / (E))), 0.5);
                                return D_p;
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
                            double B_br = B;
                            if (MatCase == MaterialCase.Case1)
                            {
                                double D_br = ((B_br) / (20.0)) * Math.Pow((((6.0 * B_br) / (E))), 0.5);
                                return D_br;
                            }
                            else
                            {
                                double D_br = ((B_br) / (20.0)) * Math.Pow((((6.0 * B_br) / (E))), 0.5);
                                return D_br;
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
                            double B_s =B;
                            if (MatCase == MaterialCase.Case1)
                            {
                                
                                double D_s = ((B_s) / (20.0)) * Math.Pow((((6.0 * B_s) / (E))), 0.5);
                                return D_s;
                            }
                            else
                            {
                               
                                double D_s = ((B_s) / (10.0)) * Math.Pow((((6.0 * B_s) / (E))), 0.5);
                                return D_s;
                            }

                        }
                        else
                        {
                            throw new Exception("Curved sub-elements are not supported");
                        }
                        break;
                    default:
                        throw new Exception("Buckling type not recognized");
                        break;
                }
            }

        }


        #endregion

        #region C


        private double _C;

        public double C
        {
            get
            {
                if (_C == 0)
                {
                    _C = Get_C();
                }

                return _C;
            }
        }

        private double Get_C()
        {


            if (BucklingType == Entities.Enums.BucklingType.MemberBuckling)
            {
                double D_c = D;
                double B_c = B;
                if (MatCase == MaterialCase.Case1)
                {
                    double C_c = ((2.0 * B_c) / (3.0 * D_c));
                    return C_c;
                }
                else
                {
                    double C_c = 0.41 * ((B_c) / (D_c));
                    return C_c;
                }

            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:

                        double D_p = D;
                        double B_p = B;
                        if (SubElementType == SubElementType.Flat)
                        {
                            double C_p = ((2.0 * B_p) / (3.0 * D_p));
                            return C_p;
                        }
                        else
                        {
                            double C_p = 0.41 * ((B_p) / (D_p));
                            return C_p;
                        }
                        break;
                    case BucklingType.FlexuralCompression:
                        double D_br = D;
                        double B_br = B;

                        if (SubElementType == SubElementType.Flat)
                        {
                            double C_br = ((2.0 * B_br) / (3.0 * D_br));
                            return C_br;
                        }
                        else
                        {
                            double C_br = ((2.0 * B_br) / (3.0 * D_br));
                            return C_br;
                        }
                        break;
                    case BucklingType.Shear:
                        double D_s = D;
                        double B_s = D;
                        if (SubElementType == SubElementType.Flat)
                        {
                            double C_s = ((2.0 * B_s) / (3.0 * D_s));
                            return C_s;
                        }
                        else
                        {
                            double C_s = 0.41 * ((B_s) / (D_s));
                            return C_s;
                        }
                        break;
                    default:
                        throw new Exception("Buckling type not recognized");
                        break;
                }
            }

        }

        #endregion

        #region Material properties

        private double _F_cy;

        public double F_cy
        {
            get
            {
                _F_cy = this.Material.F_cy;
                return _F_cy;
            }

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

        //private double _E;

        //public double E
        //{
        //    get
        //    {
        //        _E = Material.E;
        //        return _E;
        //    }
        //    set { _E = value; }
        //}
        
        #endregion


        
        
    }
}

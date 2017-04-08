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
        WeldCase WeldCase { get; set; }

        public BucklingConstantFactory(BucklingType BucklingType, SubElementType SubElementType, IAluminumMaterial Material, WeldCase WeldCase)
        {
            this.BucklingType    =BucklingType  ;
            this.SubElementType = SubElementType;
            this.Material = Material;
            this.WeldCase = WeldCase;
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
                
                if (c == MaterialCase.Case1)
                {
                    D_c = ((B_c) / (20.0)) * Math.Pow((((6.0 * B_c) / (E))), 0.5); 
                    return D_c;
                }
                else
                {
                    D_c = ((B_c) / (10.0)) * Math.Pow((((6.0 * B_c) / (E))), 0.5); 
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
                            if (c == MaterialCase.Case1)
                            {
                                D_p=((B_p) / (20.0))*Math.Pow((((6.0*B_c) / (E))), 0.5); 
                                return D_p;
                            }
                            else
                            {
                                D_p=((B_p) / (10.0))*Math.Pow((((6.0*B_c) / (E))), 0.5); 
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
                           if (c == MaterialCase.Case1)
                            {
                                D_br=((B_c) / (20.0))*Math.Pow((((6.0*B_c) / (E))), 0.5);
                                return D_br;
                            }
                            else
                            {
                                D_br=((B_c) / (20.0))*Math.Pow((((6.0*B_c) / (E))), 0.5);
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
                            if (c == MaterialCase.Case1)
                            {
                                D_s=((B_s) / (20.0))*Math.Pow((((6.0*B_s) / (E))), 0.5);
                                return D_s;
                            }
                            else
                            {
                                D_s = ((B_s) / (10.0)) * Math.Pow((((6.0 * B_s) / (E))), 0.5);
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
                if (c == MaterialCase.Case1)
                {
                     C_c = ((2.0 * B_c) / (3.0 * D_c));
                    return C_c;
                }
                else
                {
                    C_c = 0.41 * ((B_c) / (D_c));
                    return C_c;
                }
                
            }
            else
            {
                switch (BucklingType)
                {
                    case BucklingType.UniformCompression:
                        if (SubElementType == SubElementType.Flat)
                        {
                            C_p = ((2.0 * B_p) / (3.0 * D_p));
                            return C_p;
                        }
                        else
                        {
                            C_p = 0.41 * ((B_p) / (D_c));
                            return C_p;
                        }
                        break;
                    case BucklingType.FlexuralCompression:
                        if (SubElementType == SubElementType.Flat)
                        {
                            C_br = ((2.0 * B_br) / (3.0* D_br));
                            return C_br;
                        }
                        else
                        {
                            C_br = ((2.0 * B_br) / (3.0 * D_br));
                            return C_br;
                        }
                        break;
                    case BucklingType.Shear:
                        if (SubElementType == SubElementType.Flat)
                        {
                            C_s = ((2.0 * B_s) / (3.0 * D_s));
                            return C_s;
                        }
                        else
                        {
                            C_s = 0.41 * ((B_s) / (D_s));
                            return C_s;
                        }
                        break;
                    default:
                        throw new Exception("Buckling type not recognized");
                        break;
                }
            }
            
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

        private double _E;

        public double E
        {
            get {
                _E = Material.E;
                return _E; }
            set { _E = value; }
        }

        #region B
        private double _B_s;

        public double B_s
        {
            get
            {
                return this.B;
                _B_s = _B_s;
            }
            set { _B_s = value; }
        }

        private double _B_c;

        public double B_c
        {
            get
            {
                _B_c = this.B;
                return _B_c;
            }
            set { _B_c = value; }
        }

        private double _B_br;

        public double B_br
        {
            get
            {
                _B_br = this.B;
                return _B_br;
            }
            set { _B_br = value; }
        }

        private double _B_p;

        public double B_p
        {
            get
            {
                _B_p = this.B;
                return _B_p;
            }
            set { _B_p = value; }
        }
        #endregion

        #region C
        private double _C_s;

        public double C_s
        {
            get
            {
                return this.C;
                _C_s = _C_s;
            }
            set { _C_s = value; }
        }

        private double _C_c;

        public double C_c
        {
            get
            {
                _C_c = this.C;
                return _C_c;
            }
            set { _C_c = value; }
        }

        private double _C_br;

        public double C_br
        {
            get
            {
                _C_br = this.C;
                return _C_br;
            }
            set { _C_br = value; }
        }

        private double _C_p;

        public double C_p
        {
            get
            {
                _C_p = this.C;
                return _C_p;
            }
            set { _C_p = value; }
        }
        #endregion

        #region D
        private double _D_s;

        public double D_s
        {
            get
            {
                return this.D;
                _D_s = _D_s;
            }
            set { _D_s = value; }
        }

        private double _D_c;

        public double D_c
        {
            get
            {
                _D_c = this.D;
                return _D_c;
            }
            set { _D_c = value; }
        }

        private double _D_br;

        public double D_br
        {
            get
            {
                _D_br = this.D;
                return _D_br;
            }
            set { _D_br = value; }
        }

        private double _D_p;

        public double D_p
        {
            get
            {
                _D_p = this.D;
                return _D_p;
            }
            set { _D_p = value; }
        } 
        #endregion
    }
}

using Kodestruct.Aluminum.AA.Entities;
using Kodestruct.Aluminum.AA.Entities.Enums;
using Kodestruct.Aluminum.AA.Entities.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.DesignRequirements.LocalBuckling
{
    public class FlexuralLocalBucklingElement
    {

        public double b { get; set; }
        public double t { get; set; }
        public IAluminumMaterial Material { get; set; }
        double M_np   {get; set;}
        double S_xc { get; set; }
        public LateralSupportType LateralSupportType { get; set; }
        SubElementType SubElementType { get; set; }
        WeldCase WeldCase { get; set; }

        public FlexuralLocalBucklingElement(IAluminumMaterial Material, double b, double t, LateralSupportType LateralSupportType, double M_np, double S_xc,
            WeldCase WeldCase, SubElementType SubElementType = SubElementType.Flat )
        {
            this.Material = Material;
            this.b = b;
            this.t = t;
            this.M_np = M_np;
            this.S_xc = S_xc;
            this.LateralSupportType = LateralSupportType;
        }
        public double GetCriticalStress()
        {
            double F_e = GetFe_();
            double lamda_1 = GetLambda1();
            double lamda_2 = GetLambda2();
            double lamda_eq = GetLambda_eq(F_e);
            double E = Material.E;
            BucklingConstantFactory bcf = new BucklingConstantFactory(BucklingType.FlexuralCompression, this.SubElementType, Material, WeldCase);
            double C_p = bcf.C;
            double B_p = bcf.B;
            double k_2 = bcf.k_2;

            double F_b = 0;

            if (lamda_eq<=lamda_1)
            {
                F_b = ((M_np) / (S_xc));
            }
            else if (lamda_eq<lamda_2)
            {
                F_b=((M_np) / (S_xc))-(((M_np) / (S_xc))-((Math.Pow(Math.PI, 2)*E) / (Math.Pow(C_p, 2))))*(((lamda_eq-lamda_1) / (C_p-lamda_1)));
            }
            else
            {
                F_b = ((k_2 * Math.Sqrt(B_p * E)) / (lamda_eq));
            }
            return F_b;
        }

        private double GetLambda_eq(double F_e)
        {
            double E = Material.E;
            double lambda_eq = Math.PI*Math.Sqrt(((E) / (F_e)));
            return lambda_eq;
        }

        private double GetLambda2()
        {
            BucklingConstantFactory bcf = new BucklingConstantFactory(BucklingType.FlexuralCompression, this.SubElementType, Material, WeldCase);
            double C_p = bcf.C;
            double lambda2 = C_p;
            return lambda2;
        }

        private double GetLambda1()
        {
            BucklingConstantFactory bcf = new BucklingConstantFactory(BucklingType.FlexuralCompression, this.SubElementType, Material, WeldCase);
            double F_cy = this.Material.F_cy;
            double B_p = bcf.B;
            double D_p = bcf.D;
            double lambda_1 = ((B_p - F_cy) / (D_p));
            return lambda_1;
        }

        private double GetFe_()
        {
            double E = Material.E;
            double F_e = 0;
            switch (LateralSupportType)
            {
                case LateralSupportType.BothEdges:
                    F_e = ((Math.Pow(Math.PI, 2) * E) / (Math.Pow((1.6 * b * t), 2)));
                    break;
                case LateralSupportType.OneEdge:
                    F_e = ((Math.Pow(Math.PI, 2) * E) / (Math.Pow((5.0 * b * t), 2)));
                    break;
                default:
                    throw new Exception("This value LateralSupportType for variable is not supported");
                    break;
            }
            return F_e;
        }
    }
}

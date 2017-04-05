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
            throw new NotImplementedException();
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
        
    }
}

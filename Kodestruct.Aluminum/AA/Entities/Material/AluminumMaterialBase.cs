using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.Entities.Material
{
    public class AluminumMaterialBase
    {

        
        protected virtual void ReadMaterialProperties()
        {
            
        }

        private double _F_cy;

        public double F_cy
        {
            get {
                if (this.Temper.StartsWith("H") && this._IsWelded == false)
                {
                    _F_cy= 0.9*F_ty;
                }
                else
	            {
                    _F_cy = F_ty;
	            }

                return _F_cy; }
            set { _F_cy = value; }
        }
        


        private double _F_sy;

        public double F_sy
        {
            get {
                _F_sy = 0.6 * _F_ty;
                return _F_sy; }
            set { _F_sy = value; }
        }


        private double  _F_su;

        public double  F_su
        {
            get {
                _F_su = 0.6 * F_tu;
                return _F_su; }
            set { _F_su = value; }
        }
        

        private double _E;

        public double E
        {
            get {
                _E = 10100.0;
                return _E; }
            set { _E = value; }
        }
        private double _F_ty;

        public double F_ty
        {
            get {
                ReadMaterialProperties();
                return _F_ty; }
            set { _F_ty = value; }
        }


        private double _F_tyw;

        public double F_tyw
        {
            get {
                ReadMaterialProperties();
                return _F_tyw; }
            set { _F_tyw = value; }
        }
        

        private double _F_tu;

        public double F_tu
        {
            get { 
                ReadMaterialProperties();
                return _F_tu; }
            set { _F_tu = value; }
        }

        private double _F_tuw;

        public double F_tuw
        {
            get { 
                ReadMaterialProperties();
                return _F_tuw; }
            set { _F_tuw = value; }
        }


        private double _k_t;

        public double k_t
        {
            get { 
                ReadMaterialProperties();
                return _k_t; }
            set { _k_t = value; }
        }


        private string _Temper;

        public string Temper
        {
            get { return _Temper; }
            set { _Temper = value; }
        }

        private bool  _IsWelded;

        public bool  IsWelded
        {
            get { return _IsWelded; }
            set { _IsWelded = value; }
        }
        
    }
}

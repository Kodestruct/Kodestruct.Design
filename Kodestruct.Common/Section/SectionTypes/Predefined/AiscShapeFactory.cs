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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Exceptions;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;


namespace Kodestruct.Common.Section.Predefined
{
    public class AiscShapeFactory
    {

        public ISection GetShape(string ShapeId, AngleOrientation AngleOrientation = AngleOrientation.LongLegVertical, AngleRotation AngleRotation = AngleRotation.FlatLegBottom)
        {
            ShapeTypeSteel shapeType = DetermineShapeType(ShapeId);
            return  GetShape( ShapeId, shapeType,  AngleOrientation, AngleRotation);
        }

        private ShapeTypeSteel DetermineShapeType(string ShapeId)
        {
            if (ShapeId.StartsWith("W") || ShapeId.StartsWith("S") || ShapeId.StartsWith("HP"))
            {
                return ShapeTypeSteel.IShapeRolled;
            }
            else if (ShapeId.StartsWith("C") || ShapeId.StartsWith("MC"))
            {
                return ShapeTypeSteel.Channel;
            }
            else if (ShapeId.StartsWith("L") )
            {
                return ShapeTypeSteel.Angle;
            }
            else if (ShapeId.StartsWith("2L"))
            {
                return ShapeTypeSteel.DoubleAngle;
            }
            else if (ShapeId.StartsWith("WT") || ShapeId.StartsWith("MT") || ShapeId.StartsWith("ST"))
            {
                return ShapeTypeSteel.TeeRolled;
            }
            else if (ShapeId.StartsWith("Pipe"))
            {
                return ShapeTypeSteel.CircularHSS;
            }
            else if (ShapeId.StartsWith("HSS"))
            {
                List<string> substrs = ShapeId.Split('X').ToList();
                if (substrs.Count == 2)
                {
                    return ShapeTypeSteel.CircularHSS;
                }
                else
                {
                    return ShapeTypeSteel.RectangularHSS;
                }
            }
            else
            {
                throw new Exception("Shape not recognized. Specify AISC database name.");
            }

        }
        public ISection GetShape(string ShapeId, ShapeTypeSteel shapeType, AngleOrientation AngleOrientation = AngleOrientation.LongLegVertical, AngleRotation AngleRotation = AngleRotation.FlatLegBottom)
        { 

            string DEFAULT_EXCEPTION_STRING = "Selected shape is not supported. Specify a different shape.";
            AiscCatalogShape cs = new AiscCatalogShape(ShapeId,null);
            CalcLog log = new CalcLog();
            ISection sec = null;

            switch (shapeType)
            {
                case ShapeTypeSteel.IShapeRolled:
                    sec = new PredefinedSectionI(cs);
                    break;
                case ShapeTypeSteel.IShapeBuiltUp:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.Channel:
                    sec = new PredefinedSectionChannel(cs);
                    break;
                case ShapeTypeSteel.Angle:
                    sec = new PredefinedSectionAngle(cs, AngleOrientation,AngleRotation);
                    break;
                case ShapeTypeSteel.TeeRolled:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.TeeBuiltUp:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.DoubleAngle:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.CircularHSS:
                    sec = new PredefinedSectionCHS(cs);
                    break;
                case ShapeTypeSteel.RectangularHSS:
                    sec = new PredefinedSectionRHS(cs);
                    break;
                case ShapeTypeSteel.Box:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.Rectangular:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.Circular:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                case ShapeTypeSteel.IShapeAsym:
                    throw new Exception(DEFAULT_EXCEPTION_STRING);
                    break;
                default:
                    break;
            }

            return sec;
        }


        public ISliceableSection GetSliceableSection(string ShapeId, ShapeTypeSteel shapeType)
        {
            ISection section = this.GetShape(ShapeId, ShapeTypeSteel.IShapeRolled);
            ISliceableSection sec;

            switch (shapeType)
            {
                case ShapeTypeSteel.IShapeRolled:
                            PredefinedSectionI catI = section as PredefinedSectionI;
                             sec = new SectionIRolled("", catI.d, catI.b_fTop, catI.t_f, catI.t_w, catI.k);
                    break;
                case ShapeTypeSteel.IShapeBuiltUp:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.Channel:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.Angle:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.TeeRolled:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.TeeBuiltUp:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.DoubleAngle:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.CircularHSS:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.RectangularHSS:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.Box:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.Rectangular:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.Circular:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                case ShapeTypeSteel.IShapeAsym:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
                default:
                    throw new ShapeTypeNotSupportedException("ISliceableSection property ");
                    break;
            }

            return sec;

        }
        private double GetAngleGap(string ShapeId)
        {
            double gap=0;

            string[] strings = ShapeId.Split('X');
            if (strings.Count() == 4)
            {
                string gapStr = strings[3];
                string gs;
                if (gapStr.Contains("LLBB") || gapStr.Contains("SLBB"))
                {
                    gs = gapStr.Substring(0, gapStr.Length - 5);
                }
                else
                {
                    gs = gapStr;
                }
                string[] gsFraction = gs.Split('/');
                if (gsFraction.Count() ==2)
                {
                    double num = double.Parse(gsFraction[0]);
                    double den = double.Parse(gsFraction[1]);
                    gap = num / den;
                }
            }

            return gap;
        }


    }
}

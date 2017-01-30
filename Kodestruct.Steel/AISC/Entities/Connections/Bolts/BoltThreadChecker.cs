using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.Entities.Bolts
{
    public class BoltThreadChecker
    {
        /// <summary>
        /// Determines if the X or N condition can be assumed without additional inspections, based on the minimum ply thickness
        /// </summary>
        /// <param name="NutCondition"></param>
        /// <param name="d_b">Bolt diameter</param>
        /// <param name="t_p">Minimum ply thickness</param>
        /// <returns></returns>
        public static BoltThreadCase DetermineThreadCase(BoltNutCondition NutCondition, double d_b, double t_p)
        {
            //Based on
            //AISC Night School. W. Thornton
            //Bracing Connections and Related Topics
            //Session 2: The Uniform Force Method
            //UPDATED: September 30, 2014
            //Page 27 of handouts.

            BoltThreadCase tc = BoltThreadCase.Included;

            if (d_b!=0.75 && d_b!=0.875 && d_b!=1.0 && d_b!=1.125 && d_b != 1.25)
            {
                throw new Exception("Bolt diameter not recognized. Use standard nominal diameters between 3/4 in. and 1 1/4 in.");

            }
            else //standard diameter
            {
                if (d_b==0.75 || d_b == 0.875)
                {
                    switch (NutCondition)
                    {
                        case BoltNutCondition.NoWasher:
                            tc = t_p > 0.641 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break; 
                        case BoltNutCondition.Washer:
                            tc = t_p > 0.485 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.WasherAndStickThrough:
                            tc = t_p > 0.235 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                    }
                }
                else if (d_b==1)
                {
                    switch (NutCondition)
                    {
                        case BoltNutCondition.NoWasher:
                            tc = t_p > 0.766 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.Washer:
                            tc = t_p > 0.610 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.WasherAndStickThrough:
                            tc = t_p > 0.360 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                    }
                }

                else if (d_b==1.125)
                {
                    switch (NutCondition)
                    {
                        case BoltNutCondition.NoWasher:
                            tc = t_p > 0.891 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.Washer:
                            tc = t_p > 0.735 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.WasherAndStickThrough:
                            tc = t_p > 0.485 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                    }
                }
                else
                {
                    switch (NutCondition)
                    {
                        case BoltNutCondition.NoWasher:
                            tc = t_p > 0.781 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.Washer:
                            tc = t_p > 0.625 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                        case BoltNutCondition.WasherAndStickThrough:
                            tc = t_p > 0.375 ? BoltThreadCase.Excluded : BoltThreadCase.Included;
                            break;
                    }
                }
            }
            return tc;
        }
    }
}

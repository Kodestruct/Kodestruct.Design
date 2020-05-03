#region Copyright
/*Copyright (C) 2015 Konstantin Udilovich

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

namespace Kodestruct.Common.Mathematics
{
    public static class DoubleExtensions
    {
        public static double ToRadians(this double val)
        {
            if (val != 0)
            {
                return (Math.PI / 180) * val; 
            }
            else
            {
                return 0;
            }
        }


        public static double ToDegrees(this double val)
        {
            if (val !=0)
            {
                return (180.0 / Math.PI) * val; 
            }
            else
            {
                return 0;
            }
        }

        public static bool AlmostEqualsWithAbsTolerance(this double a, double b, double maxAbsoluteError)
        {
            double diff = Math.Abs(a - b);

            if (a.Equals(b))
            {
                // shortcut, handles infinities
                return true;
            }

            return diff <= maxAbsoluteError;
        }

        /// <summary>
        /// Checks if numbers are approximately equal distinguishing between rounding up and down
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ToleranceUp"></param>
        /// <param name="ToleranceDown"></param>
        /// <returns></returns>
        public static bool InRange(this double a, double b, double ToleranceUp, double ToleranceDown)
        {
          

            if (a.Equals(b))
            {
                // shortcut, handles infinities
                return true;
            }
            else if (a>b)
            {
                return a - b <= ToleranceUp;
            }
            else
            {
                return b - a <= ToleranceDown;
            }
        
        }

        public static double Ceiling(this double i, double Precision)
        {
            return ((int)Math.Ceiling(i / Precision)) * Precision;
        }
        public static double Floor(this double i, double Precision)
        {
            return ((int)Math.Floor(i / Precision)) * Precision;
        }

        //https://stackoverflow.com/questions/38891250/convert-to-fraction-inches
        public static string ToFraction64(this double value)
        {
            // denominator is fixed
            int denominator = 64;
            // integer part, can be signed: 1, 0, -3,...
            int integer = (int)value;
            // numerator: always unsigned (the sign belongs to the integer part)
            // + 0.5 - rounding, nearest one: 37.9 / 64 -> 38 / 64; 38.01 / 64 -> 38 / 64
            int numerator = (int)((Math.Abs(value) - Math.Abs(integer)) * denominator + 0.5);

            // some fractions, e.g. 24 / 64 can be simplified:
            // both numerator and denominator can be divided by the same number
            // since 64 = 2 ** 6 we can try 2 powers only 
            // 24/64 -> 12/32 -> 6/16 -> 3/8
            // In general case (arbitrary denominator) use gcd (Greatest Common Divisor):
            //   double factor = gcd(denominator, numerator);
            //   denominator /= factor;
            //   numerator /= factor;
            while ((numerator % 2 == 0) && (denominator % 2 == 0))
            {
                numerator /= 2;
                denominator /= 2;
            }

            // The longest part is formatting out

            // if we have an actual, not degenerated fraction (not, say, 4 0/1)
            if (denominator > 1)
                if (integer != 0) // all three: integer + numerator + denominator
                    return string.Format("{0} {1}/{2}", integer, numerator, denominator);
                else if (value < 0) // negative numerator/denominator, e.g. -1/4
                    return string.Format("-{0}/{1}", numerator, denominator);
                else // positive numerator/denominator, e.g. 3/8
                    return string.Format("{0}/{1}", numerator, denominator);
            else
                return integer.ToString(); // just an integer value, e.g. 0, -3, 12...  
        }

        //public static double[] Minimum(this double[] values)
        //{
        //    double Minimum = double.PositiveInfinity;
        //    for (int i = 0; i < values.Length; i++)
        //        if (values[i] < Minimum)
        //        {
        //            Minimum = values[i];
        //        }
        //    return Minimum;
        //}
    //}

    //public static class DoubleExtensions
    //{
        //SOURCE: https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Precision.cs
        //        https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Precision.Equality.cs
        //        http://referencesource.microsoft.com/#WindowsBase/Shared/MS/Internal/DoubleUtil.cs
        //        http://stackoverflow.com/questions/2411392/double-epsilon-for-equality-greater-than-less-than-less-than-or-equal-to-gre

        /// <summary>
        /// The smallest positive number that when SUBTRACTED from 1D yields a result different from 1D.
        /// The value is derived from 2^(-53) = 1.1102230246251565e-16, where IEEE 754 binary64 &quot;double precision&quot; floating point numbers have a significand precision that utilize 53 bits.
        ///
        /// This number has the following properties:
        ///     (1 - NegativeMachineEpsilon) &lt; 1 and
        ///     (1 + NegativeMachineEpsilon) == 1
        /// </summary>
        public const double NegativeMachineEpsilon = 1.1102230246251565e-16D; //Math.Pow(2, -53);

        /// <summary>
        /// The smallest positive number that when ADDED to 1D yields a result different from 1D.
        /// The value is derived from 2 * 2^(-53) = 2.2204460492503131e-16, where IEEE 754 binary64 &quot;double precision&quot; floating point numbers have a significand precision that utilize 53 bits.
        ///
        /// This number has the following properties:
        ///     (1 - PositiveDoublePrecision) &lt; 1 and
        ///     (1 + PositiveDoublePrecision) &gt; 1
        /// </summary>
        public const double PositiveMachineEpsilon = 2D * NegativeMachineEpsilon;

        /// <summary>
        /// The smallest positive number that when SUBTRACTED from 1D yields a result different from 1D.
        ///
        /// This number has the following properties:
        ///     (1 - NegativeMachineEpsilon) &lt; 1 and
        ///     (1 + NegativeMachineEpsilon) == 1
        /// </summary>
        public static readonly double MeasuredNegativeMachineEpsilon = MeasureNegativeMachineEpsilon();

        private static double MeasureNegativeMachineEpsilon()
        {
            double epsilon = 1D;

            do
            {
                double nextEpsilon = epsilon / 2D;

                if ((1D - nextEpsilon) == 1D) //if nextEpsilon is too small
                    return epsilon;

                epsilon = nextEpsilon;
            }
            while (true);
        }

        /// <summary>
        /// The smallest positive number that when ADDED to 1D yields a result different from 1D.
        ///
        /// This number has the following properties:
        ///     (1 - PositiveDoublePrecision) &lt; 1 and
        ///     (1 + PositiveDoublePrecision) &gt; 1
        /// </summary>
        public static readonly double MeasuredPositiveMachineEpsilon = MeasurePositiveMachineEpsilon();

        private static double MeasurePositiveMachineEpsilon()
        {
            double epsilon = 1D;

            do
            {
                double nextEpsilon = epsilon / 2D;

                if ((1D + nextEpsilon) == 1D) //if nextEpsilon is too small
                    return epsilon;

                epsilon = nextEpsilon;
            }
            while (true);
        }

        const double DefaultDoubleAccuracy = NegativeMachineEpsilon * 10D;

        public static bool IsClose(this double value1, double value2)
        {
            return IsClose(value1, value2, DefaultDoubleAccuracy);
        }

        public static bool IsClose(this double value1, double value2, double maximumAbsoluteError)
        {
            if (double.IsInfinity(value1) || double.IsInfinity(value2))
                return value1 == value2;

            if (double.IsNaN(value1) || double.IsNaN(value2))
                return false;

            double delta = value1 - value2;

            //return Math.Abs(delta) <= maximumAbsoluteError;

            if (delta > maximumAbsoluteError ||
                delta < -maximumAbsoluteError)
                return false;

            return true;
        }

        public static bool LessThan(this double value1, double value2)
        {
            return (value1 < value2) && !IsClose(value1, value2);
        }

        public static bool GreaterThan(this double value1, double value2)
        {
            return (value1 > value2) && !IsClose(value1, value2);
        }

        public static bool LessThanOrClose(this double value1, double value2)
        {
            return (value1 < value2) || IsClose(value1, value2);
        }

        public static bool GreaterThanOrClose(this double value1, double value2)
        {
            return (value1 > value2) || IsClose(value1, value2);
        }

        public static bool IsOne(this double value)
        {
            double delta = value - 1D;

            //return Math.Abs(delta) <= PositiveMachineEpsilon;

            if (delta > PositiveMachineEpsilon ||
                delta < -PositiveMachineEpsilon)
                return false;

            return true;
        }

        public static bool IsZero(this double value)
        {
            //return Math.Abs(value) <= PositiveMachineEpsilon;

            if (value > PositiveMachineEpsilon ||
                value < -PositiveMachineEpsilon)
                return false;

            return true;
        }

        //https://stackoverflow.com/questions/2453951/c-sharp-double-tostring-formatting-with-two-decimal-places-but-no-rounding
        public static string To6DecimalPlaces(this double val)
        {
            double x = Math.Truncate(val * 1000000) / 1000000;
            //string s = string.Format("{0:N6}", x);
            string s = x.ToString();
            return s;
        }
    }
}

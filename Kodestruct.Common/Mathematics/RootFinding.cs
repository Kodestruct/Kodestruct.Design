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
    //http://www.codeproject.com/Articles/79541/Three-Methods-for-Root-finding-in-C

    public delegate double FunctionOfOneVariable(double x);

    public class RootFinding
    {
        const int maxIterations = 50;

        public static double Bisect
        (
            FunctionOfOneVariable f,
            double left,
            double right,
            double tolerance = 1e-6,
            double target = 0.0
        )
        {
            // extra info that callers may not always want
            int iterationsUsed;
            double errorEstimate;

            return Bisect(f, left, right, tolerance, target, out iterationsUsed, out errorEstimate);
        }

        public static double Bisect
        (
            FunctionOfOneVariable f,
            double left,
            double right,
            double tolerance,
            double target,
            out int iterationsUsed,
            out double errorEstimate
        )
        {
            if (tolerance <= 0.0)
            {
                string msg = string.Format("Tolerance must be positive. Recieved {0}.", tolerance);
                throw new ArgumentOutOfRangeException(msg);
            }

            iterationsUsed = 0;
            errorEstimate = double.MaxValue;

            // Standardize the problem.  To solve f(x) = target,
            // solve g(x) = 0 where g(x) = f(x) - target.
            FunctionOfOneVariable g = delegate(double x) { return f(x) - target; };


            double g_left = g(left);  // evaluation of f at left end of interval
            double g_right = g(right);
            double mid;
            double g_mid;
            if (g_left * g_right >= 0.0)
            {
                string str = "Invalid starting bracket. Function must be above target on one end and below target on other end.";
                string msg = string.Format("{0} Target: {1}. f(left) = {2}. f(right) = {3}", str, g_left + target, g_right + target);
                throw new ArgumentException(msg);
            }

            double intervalWidth = right - left;

            for
            (
                iterationsUsed = 0;
                iterationsUsed < maxIterations && intervalWidth > tolerance;
                iterationsUsed++
            )
            {
                intervalWidth *= 0.5;
                mid = left + intervalWidth;

                if ((g_mid = g(mid)) == 0.0)
                {
                    errorEstimate = 0.0;
                    return mid;
                }
                if (g_left * g_mid < 0.0)           // g changes sign in (left, mid)    
                    g_right = g(right = mid);
                else                            // g changes sign in (mid, right)
                    g_left = g(left = mid);
            }
            errorEstimate = right - left;
            return left;
        }

        public static double Brent
        (
            FunctionOfOneVariable f,
            double left,
            double right,
            double tolerance = 1e-6,
            double target = 0.0
        )
        {
            // extra info that callers may not always want
            int iterationsUsed;
            double errorEstimate;

            return Brent(f, left, right, tolerance, target, out iterationsUsed, out errorEstimate);
        }

        public static double Brent
        (
            FunctionOfOneVariable g,
            double left,
            double right,
            double tolerance,
            double target,
            out int iterationsUsed,
            out double errorEstimate
        )
        {
            if (tolerance <= 0.0)
            {
                string msg = string.Format("Tolerance must be positive. Recieved {0}.", tolerance);
                throw new ArgumentOutOfRangeException(msg);
            }

            errorEstimate = double.MaxValue;

            // Standardize the problem.  To solve g(x) = target,
            // solve f(x) = 0 where f(x) = g(x) - target.
            FunctionOfOneVariable f = delegate(double x) { return g(x) - target; };

            // Implementation and notation based on Chapter 4 in
            // "Algorithms for Minimization without Derivatives"
            // by Richard Brent.

            double c, d, e, fa, fb, fc, tol, m, p, q, r, s;

            // set up aliases to match Brent's notation
            double a = left; double b = right; double t = tolerance;
            iterationsUsed = 0;

            fa = f(a);
            fb = f(b);

            if (fa * fb > 0.0)
            {
                string str = "Invalid starting bracket. Function must be above target on one end and below target on other end.";
                string msg = string.Format("{0} Target: {1}. f(left) = {2}. f(right) = {3}", str, target, fa + target, fb + target);
                throw new ArgumentException(msg);
            }

        label_int:
            c = a; fc = fa; d = e = b - a;
        label_ext:
            if (Math.Abs(fc) < Math.Abs(fb))
            {
                a = b; b = c; c = a;
                fa = fb; fb = fc; fc = fa;
            }

            iterationsUsed++;

            double machine_epsilon = Math.Pow(2.0, -53);
            tol = 2.0 * machine_epsilon * Math.Abs(b) + t;

            errorEstimate = m = 0.5 * (c - b);
            if (Math.Abs(m) > tol && fb != 0.0) // exact comparison with 0 is OK here
            {
                // See if bisection is forced
                if (Math.Abs(e) < tol || Math.Abs(fa) <= Math.Abs(fb))
                {
                    d = e = m;
                }
                else
                {
                    s = fb / fa;
                    if (a == c)
                    {
                        // linear interpolation
                        p = 2.0 * m * s; q = 1.0 - s;
                    }
                    else
                    {
                        // Inverse quadratic interpolation
                        q = fa / fc; r = fb / fc;
                        p = s * (2.0 * m * q * (q - r) - (b - a) * (r - 1.0));
                        q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                    }
                    if (p > 0.0)
                        q = -q;
                    else
                        p = -p;
                    s = e; e = d;
                    if (2.0 * p < 3.0 * m * q - Math.Abs(tol * q) && p < Math.Abs(0.5 * s * q))
                        d = p / q;
                    else
                        d = e = m;
                }
                a = b; fa = fb;
                if (Math.Abs(d) > tol)
                    b += d;
                else if (m > 0.0)
                    b += tol;
                else
                    b -= tol;
                if (iterationsUsed == maxIterations)
                    return b;

                fb = f(b);
                if ((fb > 0.0 && fc > 0.0) || (fb <= 0.0 && fc <= 0.0))
                    goto label_int;
                else
                    goto label_ext;
            }
            else
                return b;
        }


   }
}

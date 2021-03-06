// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.ML.Probabilistic.Math
{
    using System;

    public class Region
    {
        public Vector Lower, Upper;

        public int Dimension
        {
            get
            {
                return Lower.Count;
            }
        }

        public Region(int dimension)
        {
            Lower = Vector.Zero(dimension);
            Upper = Vector.Zero(dimension);
        }

        public Region(Region that)
        {
            Lower = Vector.Copy(that.Lower);
            Upper = Vector.Copy(that.Upper);
        }

        public double GetLogVolume()
        {
            double logVolume = 0;
            for (int i = 0; i < Dimension; i++)
            {
                logVolume += Math.Log(Upper[i] - Lower[i]);
            }
            return logVolume;
        }

        public bool Contains(Vector x)
        {
            for (int i = 0; i < Dimension; i++)
            {
                if (x[i] < Lower[i] || x[i] >= Upper[i]) return false;
            }
            return true;
        }

        public Vector GetMidpoint()
        {
            Vector x = Vector.Zero(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                double lower = Lower[i];
                double upper = Upper[i];
                double midpoint;
                if (double.IsNegativeInfinity(lower))
                {
                    if (double.IsPositiveInfinity(upper))
                        midpoint = 0.0;
                    else
                        midpoint = upper - 1;
                }
                else if (double.IsPositiveInfinity(upper))
                {
                    midpoint = lower + 1;
                }
                else
                {
                    midpoint = 0.5 * (lower + upper);
                }
                x[i] = midpoint;
            }
            return x;
        }

        public Vector Sample()
        {
            Vector x = Vector.Zero(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                x[i] = Uniform(Lower[i], Upper[i]);
            }
            return x;
        }

        public static double Uniform(double lowerBound, double upperBound)
        {
            return Rand.Double() * (upperBound - lowerBound) + lowerBound;
        }

        public override string ToString()
        {
            return ToString("g4");
        }

        public string ToString(string format)
        {
            return string.Format("[{0},{1}]", Lower.ToString(format), Upper.ToString(format));
        }
    }
}

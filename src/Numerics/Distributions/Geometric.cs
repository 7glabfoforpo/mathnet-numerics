﻿// <copyright file="Geometric.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Collections.Generic;
using MathNet.Numerics.Properties;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Discrete Univariate Geometric distribution.
    /// The Geometric distribution is a distribution over positive integers parameterized by one positive real number.
    /// This implementation of the Geometric distribution will never generate 0's.
    /// <a href="http://en.wikipedia.org/wiki/Geometric_distribution">Wikipedia - geometric distribution</a>.
    /// </summary>
    /// <remarks><para>The distribution will use the <see cref="System.Random"/> by default. 
    /// Users can set the random number generator by using the <see cref="RandomSource"/> property.</para>
    /// <para>The statistics classes will check all the incoming parameters whether they are in the allowed
    /// range. This might involve heavy computation. Optionally, by setting Control.CheckDistributionParameters
    /// to <c>false</c>, all parameter checks can be turned off.</para></remarks>
    public class Geometric : IDiscreteDistribution
    {
        System.Random _random;

        double _p;

        /// <summary>
        /// Initializes a new instance of the Geometric class.
        /// </summary>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        public Geometric(double p)
        {
            _random = MersenneTwister.Default;
            SetParameters(p);
        }

        /// <summary>
        /// Initializes a new instance of the Geometric class.
        /// </summary>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        public Geometric(double p, System.Random randomSource)
        {
            _random = randomSource ?? MersenneTwister.Default;
            SetParameters(p);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return "Geometric(p = " + _p + ")";
        }

        /// <summary>
        /// Checks whether the parameters of the distribution are valid. 
        /// </summary>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        /// <returns><c>true</c> when the parameters are valid, <c>false</c> otherwise.</returns>
        static bool IsValidParameterSet(double p)
        {
            return p >= 0.0 && p <= 1.0;
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the parameters are out of range.</exception>
        void SetParameters(double p)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(p))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            _p = p;
        }

        /// <summary>
        /// Gets or sets the probability of generating a one. Range: 0 ≤ p ≤ 1.
        /// </summary>
        public double P
        {
            get { return _p; }
            set { SetParameters(value); }
        }

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public System.Random RandomSource
        {
            get { return _random; }
            set { _random = value ?? MersenneTwister.Default; }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get { return 1.0/_p; }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get { return (1.0 - _p)/(_p*_p); }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return Math.Sqrt(1.0 - _p)/_p; }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { return ((-_p*Math.Log(_p, 2.0)) - ((1.0 - _p)*Math.Log(1.0 - _p, 2.0)))/_p; }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        /// <remarks>Throws a not supported exception.</remarks>
        public double Skewness
        {
            get { return (2.0 - _p)/Math.Sqrt(1.0 - _p); }
        }

        /// <summary>
        /// Gets the mode of the distribution.
        /// </summary>
        public int Mode
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the median of the distribution.
        /// </summary>
        public int Median
        {
            get { return (int) Math.Ceiling(-Constants.Ln2/Math.Log(1 - _p)); }
        }

        /// <summary>
        /// Gets the smallest element in the domain of the distributions which can be represented by an integer.
        /// </summary>
        public int Minimum
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the largest element in the domain of the distributions which can be represented by an integer.
        /// </summary>
        public int Maximum
        {
            get { return int.MaxValue; }
        }

        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public double Probability(int k)
        {
            if (k <= 0)
            {
                return 0.0;
            }

            return Math.Pow(1.0 - _p, k - 1)*_p;
        }

        /// <summary>
        /// Computes the log probability mass (lnPMF) at k, i.e. ln(P(X = k)).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the log probability mass function.</param>
        /// <returns>the log probability mass at location <paramref name="k"/>.</returns>
        public double ProbabilityLn(int k)
        {
            if (k <= 0)
            {
                return Double.NegativeInfinity;
            }

            return ((k - 1)*Math.Log(1.0 - _p)) + Math.Log(_p);
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        public double CumulativeDistribution(double x)
        {
            return 1.0 - Math.Pow(1.0 - _p, x);
        }

        /// <summary>
        /// Returns one sample from the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        /// <returns>One sample from the distribution implied by <paramref name="p"/>.</returns>
        static int SampleUnchecked(System.Random rnd, double p)
        {
            return p == 1.0 ? 1 : (int) Math.Ceiling(-Math.Log(1.0 - rnd.NextDouble(), 1.0 - p));
        }

        /// <summary>
        /// Samples a Geometric distributed random variable.
        /// </summary>
        /// <returns>A sample from the Geometric distribution.</returns>
        public int Sample()
        {
            return SampleUnchecked(_random, _p);
        }

        /// <summary>
        /// Samples an array of Geometric distributed random variables.
        /// </summary>
        /// <returns>a sequence of samples from the distribution.</returns>
        public IEnumerable<int> Samples()
        {
            while (true)
            {
                yield return SampleUnchecked(_random, _p);
            }
        }

        /// <summary>
        /// Samples a random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        public static int Sample(System.Random rnd, double p)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(p))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            return SampleUnchecked(rnd, p);
        }

        /// <summary>
        /// Samples a sequence of this random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="p">The probability (p) of generating one. Range: 0 ≤ p ≤ 1.</param>
        public static IEnumerable<int> Samples(System.Random rnd, double p)
        {
            if (Control.CheckDistributionParameters && !IsValidParameterSet(p))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            while (true)
            {
                yield return SampleUnchecked(rnd, p);
            }
        }
    }
}

/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements. See the NOTICE file distributed with this
 * work for additional information regarding copyright ownership. The ASF
 * licenses this file to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using System.Collections;

namespace MarkedUp.HyperLogLog
{
    /// <summary>
    /// A single HyperLogLog data structure containing a single bitset.
    /// 
    /// HyperLogLog is based off of the paper by Flajolet, Gandouet, and Meunier which can be found here: http://algo.inria.fr/flajolet/Publications/FlFuGaMe07.pdf
    /// 
    /// Long story short: HyperLogLog is great at using probabilistic estimation to the determine the number of unique items in a set using a bare minimum amount of memory (with some acceptable loss of accuracy.)
    /// 
    /// It's interesting to folks like MarkedUp, who write analytics software, because it allows us to uniquely count events / people / things in an idempotent, memory-efficient way.
    /// </summary>
    public class HyperLogLog
    {

        #region Constants

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 2^32 
        /// </summary>
        public const double Pow2_32 = 4294967297;

        /// <summary>
        /// Natural Log (LN) of 2
        /// </summary>
        public const double LN2 = 0.693147180559945;

        #endregion

        #region Internal members

        /// <summary>
        /// The actual contents of the bitset
        /// </summary>
        protected BitArray Bits;

        /// <summary>
        /// Represents the "B" value of the HLL algorithm. The greater the B value, the larger the bitset, and the greater the accuracy.
        /// 
        /// In English: this represents the power of 2 bits.
        /// 
        /// So a B value of 4 == 2^4 == 16 bits.
        /// 
        /// The value for B must fall inside [4,16] otherwise a validation error is thrown.
        /// </summary>
        protected int B;

        /// <summary>
        /// M is the length of the bit array, calculated by 2^<see cref="B"/>
        /// </summary>
        protected int M;

        /// <summary>
        /// Alpha is the harmonic mean value used per the HyperLogLog calculation
        /// </summary>
        protected double Alpha;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor accepting a default "B" value of 10.
        /// </summary>
        /// <param name="b">The B value for this HyperLogLog counter. 
        /// 
        /// Must fall inside range [4,16] or the constructor will throw an exception.
        /// The greater the b value, the greater the memory usage and accuracy.
        /// </param>
        public HyperLogLog(int b = 10)
        {
            if(b < 4 || b > 16) throw new ArgumentOutOfRangeException("b", string.Format("HyperLogLog accuracy of {0} is not supported. Please use a B value between 4 and 16.", b));
            B = b;
            M = 2 ^ b;
            Alpha = ComputeAlpha(M);
            Bits = InitializeArray(M);
        }

        #endregion

        /// <summary>
        /// Computes the Alpha value using pre-calculated values from the Integral expressed in equation 3 of the HyperLogLog paper.
        /// </summary>
        private static double ComputeAlpha(int m)
        {
            if (m == 16)
                return 0.673d;
            if (m == 32)
                return 0.697d;
            if (m == 64)
                return 0.709d;
            return 0.7213/(1d + 1.079/m);
        }

        /// <summary>
        /// Initializes the content of the <see cref="BitArray"/> to zero using the specified length <see cref="m"/>
        /// </summary>
        private static BitArray InitializeArray(int m)
        {
            return new BitArray(m, false);
        }

        /// <summary>
        /// Log base 2 of a number
        /// </summary>
        /// <param name="x">Input variable</param>
        public static double Log2(double x)
        {
            return Math.Log(x)/LN2;
        }

    }
}

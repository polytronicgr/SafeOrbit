﻿
/*
MIT License

Copyright (c) 2016 Erkin Ekici - undergroundwires@safeorb.it

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Security.Cryptography;
using SafeOrbit.Cryptography.Random.RandomGenerators;

namespace SafeOrbit.Cryptography.Random
{
    /// <summary>
    /// Helper methods for the implementations.
    /// </summary>
    public abstract class RandomBase : ICryptoRandom
    {
        private readonly RandomNumberGenerator _generator;
        protected RandomBase() : this(SafeRandomGenerator.StaticInstance)
        {
        }
        internal RandomBase(RandomNumberGenerator generator)
        {
            _generator = generator;
        }

        public byte[] GetBytes(int length)
        {
            var data = new byte[length];
            _generator.GetBytes(data);
            return data;
        }

        public int GetInt()
        {
            var scale = uint.MaxValue;
            //The code then divides scale by the uint.MaxValue.
            //This produces a value between 0.0 and 1.0, not including 1.0.
            //(This is why I didn’t want scale to be uint.MaxValue–so the result
            //of this division would be less than 1.0.) It multiplies this value
            //by the difference between the maximum and minimum desired values and
            //adds the result to the minimum value.
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                var fourBytes = GetBytes(4);
                // Convert that into an uint.
                scale = BitConverter.ToUInt32(fourBytes, 0);
            }
            return (int)scale;
        }

#if NETFRAMEWORK
        public byte[] GetNonZeroBytes(int length)
        {
            var data = new byte[length];
            _generator.GetNonZeroBytes(data);
            return data;
        }
#endif

        public int GetInt(int min, int max)
        {
            if (min == max)
                return min;
            var scale = uint.MaxValue;
            //The code then divides scale by the uint.MaxValue.
            //This produces a value between 0.0 and 1.0, not including 1.0.
            //(This is why I didn’t want scale to be uint.MaxValue–so the result
            //of this division would be less than 1.0.) It multiplies this value
            //by the difference between the maximum and minimum desired values and
            //adds the result to the minimum value.
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                var fourBytes = GetBytes(4);
                // Convert that into an uint.
                scale = BitConverter.ToUInt32(fourBytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                          (scale / (double)uint.MaxValue));
        }

        public bool GetBool()
        {
            var data = new byte[1];
            _generator.GetBytes(data);
            var @byte = data[0];
            return (@byte & 0x80) != 0;
        }
        /// <exception cref="ArgumentOutOfRangeException"><param name="upperBound"/> must be greater than <param name="lowerBound"/></exception>
        protected void EnsureParameters(int lowerBound, int upperBound)
        {
            if (lowerBound > upperBound)
            {
                throw new ArgumentOutOfRangeException(nameof(upperBound), upperBound, $"{nameof(upperBound)} must be greater than {nameof(lowerBound)}");
            }
        }
    }
}

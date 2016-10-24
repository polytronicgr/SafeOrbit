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
using System.Linq;
using NUnit.Framework;
using SafeOrbit.Utilities;
using SafeOrbit.Tests;

namespace SafeOrbit.UnitTests.Utilities
{
    [TestFixture]
    public class FastTests : TestsBase
    {
        [Test]
        public void FastFor_1000Factorials_FasterThanForLoop()
        {
            //arrange
            var iterations = 1000;
            Func<int, int> factorial = n => n == 0 ? 1 :
                Enumerable.Range(1, n).Aggregate((acc, x) => acc * x);
            var expectedMax = base.Measure(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    factorial.Invoke(i);
                }
            });
            //act
            var actual = base.Measure(() =>
            {
                Fast.For(0, iterations, (i) => factorial.Invoke(i));
            });
            //assert
            Assert.That(actual, Is.LessThan(expectedMax));
        }
    }
}

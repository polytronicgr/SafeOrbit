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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeOrbit.Memory;

namespace SafeOrbit.Memory.SafeContainerServices.Instance.Providers
{
    /// <seealso cref="TransientInstanceProvider{TImplementation}"/>
    /// <seealso cref="IInstanceProvider"/>
    [TestFixture]
    public class TransientInstanceProviderTests
    {
        [Test]
        public void LifeTime_Is_Transient()
        {
            //arrange
            var expected = LifeTime.Transient;
            var sut = GetSut<object>();
            //act
            var actual = sut.LifeTime;
            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }
        [Test]
        public void CanProtectState_Is_False()
        {
            var sut = GetSut<DateTime>();
            Assert.That(sut.CanProtectState, Is.False);
        }
        [Test]
        public void GetInstance_Returns_Instance_Of_Argument()
        {
            //arrange
            var expected = typeof (DateTime);
            var sut = GetSut<DateTime>();
            //act
            var actual = sut.GetInstance();
            //assert
            Assert.That(actual, Is.InstanceOf(expected));
        }
        [Test]
        public void GetInstance_Returns_New_Instance_On_Each_Call()
        {
            //arrange
            var sut = GetSut<InstanceTestClass>();
            //act
            var instance1 = sut.GetInstance();
            var instance2 = sut.GetInstance();
            var instance3 = sut.GetInstance();
            //assert
            Assert.That(instance1, Is.Not.EqualTo(instance2));
            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        private static TransientInstanceProvider<T> GetSut<T>() where T : new()
        {
            return new TransientInstanceProvider<T>(InstanceProtectionMode.NoProtection);
        }
    }
}

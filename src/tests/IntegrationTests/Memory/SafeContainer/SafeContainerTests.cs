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

using NUnit.Framework;

namespace SafeOrbit.Memory
{
    /// <seealso cref="SafeContainer" />
    /// <seealso cref="ISafeContainer" />
    /// <seealso cref="SafeContainerProtectionMode" />
    [TestFixture]
    public class SafeContainerTests
    {
        private static ISafeContainer GetSut() => new SafeContainer();

        [Test]
        public void Get_AfterDisablingProtection_ReturnsInstance()
        {
            var from = SafeContainerProtectionMode.NonProtection;
            var to = SafeContainerProtectionMode.FullProtection;
            var sut = new SafeContainer(from);
            sut.Register<IInstanceTestClass, InstanceTestClass>(LifeTime.Singleton);
            sut.Verify();
            var actual = sut.Get<IInstanceTestClass>();
            sut.SetProtectionMode(to);
            var expected = sut.Get<IInstanceTestClass>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Get_AfterEnablingProtection_ReturnsInstance()
        {
            var from = SafeContainerProtectionMode.FullProtection;
            var to = SafeContainerProtectionMode.NonProtection;
            var sut = new SafeContainer(from);
            sut.Register<IInstanceTestClass, InstanceTestClass>(LifeTime.Singleton);
            sut.Verify();
            var actual = sut.Get<IInstanceTestClass>();
            sut.SetProtectionMode(to);
            var expected = sut.Get<IInstanceTestClass>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Get_ForSingletonRegistration_returnsSameInstance()
        {
            //arrange
            var sut = GetSut();
            sut.Register<IInstanceTestClass, InstanceTestClass>(LifeTime.Singleton);
            sut.Verify();
            //act
            var instance = sut.Get<IInstanceTestClass>();
            var instance1 = sut.Get<IInstanceTestClass>();
            var instance2 = sut.Get<IInstanceTestClass>();
            var instance3 = sut.Get<IInstanceTestClass>();
            var instance4 = sut.Get<IInstanceTestClass>();
            var instance5 = sut.Get<IInstanceTestClass>();
            //assert
            Assert.That(instance1, Is.EqualTo(instance));
            Assert.That(instance2, Is.EqualTo(instance));
            Assert.That(instance3, Is.EqualTo(instance));
            Assert.That(instance4, Is.EqualTo(instance));
            Assert.That(instance5, Is.EqualTo(instance));
        }

        [Test]
        public void Get_ForTransientRegistration_returnsDifferentInstances()
        {
            //arrange
            var sut = GetSut();
            sut.Register<IInstanceTestClass, InstanceTestClass>(LifeTime.Transient);
            sut.Verify();
            //act
            var instance1 = sut.Get<IInstanceTestClass>();
            var instance2 = sut.Get<IInstanceTestClass>();
            var instance3 = sut.Get<IInstanceTestClass>();
            //assert
            Assert.That(instance1, Is.Not.EqualTo(instance2));
            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [Test]
        public void NonTypedCustomFuncRegistration_returnsResultFromFunc()
        {
            //arrange
            var sut = GetSut();
            var expected = new InstanceTestClass(88);
            sut.Register(() => expected);
            sut.Verify();
            //act
            var actual = sut.Get<InstanceTestClass>();
            //assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void TypedCustomFuncRegistration_returnsResultFromFunc()
        {
            //arrange
            var sut = GetSut();
            var expected = new InstanceTestClass(88);
            sut.Register<IInstanceTestClass, InstanceTestClass>(() => expected);
            sut.Verify();
            //act
            var actual = sut.Get<IInstanceTestClass>();
            //assert
            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}
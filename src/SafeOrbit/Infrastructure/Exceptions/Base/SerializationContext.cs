
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
using System.Linq.Expressions;
using SafeOrbit.Exceptions.SerializableException;

namespace SafeOrbit.Exceptions
{
    public class SerializationContext : ISerializationContext
    {
        public ICollection<ISerializationPropertyInfo> PropertyInfos { get; } = new List<ISerializationPropertyInfo>();

        public void Add<T>(Expression<Func<T>> property)
        {
            var name = GetName(property);
            var value = property.Compile()();

            PropertyInfos.Add(new SerializationPropertyInfo(
                value: value,
                propertyName: name,
                type: typeof(T)
                ));
        }

        private static string GetName<T>(Expression<Func<T>> exp)
        {
            var body = exp.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression) exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
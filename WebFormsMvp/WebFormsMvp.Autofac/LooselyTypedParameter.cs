using System;
using Autofac.Core;

namespace WebFormsMvp.Autofac
{
    internal class LooselyTypedParameter : ConstantParameter
    {
        public LooselyTypedParameter(Type type, object value)
            : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
        {
            if (type == null) throw new ArgumentNullException("type");
        }
    }
}
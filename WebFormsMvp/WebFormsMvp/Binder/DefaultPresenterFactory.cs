using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace WebFormsMvp.Binder
{
    public class DefaultPresenterFactory : IPresenterFactory
    {
        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            var buildMethod = GetBuildMethod(presenterType, viewType);
            var presenter = (IPresenter)buildMethod.Invoke(null, new[] { viewInstance });
            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            if (presenter is IDisposable)
            {
                ((IDisposable)presenter).Dispose();
            }
        }

        DynamicMethod GetBuildMethod(Type presenterType, Type viewType)
        {
            return GetBuildMethod(presenterType, new Type[] { viewType });
        }

        static readonly IDictionary<IntPtr, DynamicMethod> buildMethodCache = new Dictionary<IntPtr, DynamicMethod>();
        DynamicMethod GetBuildMethod(Type type, Type[] constructorArgumentTypes)
        {
            var constructor = type.GetConstructor(constructorArgumentTypes);
            if (constructor == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing expected constructor.", type.FullName));
            }

            // Using DynamicMethod and ILGenerator allows us to hold on to a
            // JIT-ed constructor call, which gives us an insanely fast way
            // to create type instances on the fly. This provides a surprising
            // performance improvement over basic reflection in applications
            // that create lots of presenters, which is common.
            var dynamicMethod = new DynamicMethod("DynamicConstructor", type, constructorArgumentTypes, type.Module, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}
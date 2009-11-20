using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Globalization;

namespace WebFormsMvp.Binder
{
    public class DefaultPresenterFactory : IPresenterFactory
    {
        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null)
                throw new ArgumentNullException("presenterType");

            if (viewType == null)
                throw new ArgumentNullException("viewType");

            if (viewInstance == null)
                throw new ArgumentNullException("viewInstance");

            var buildMethod = GetBuildMethod(presenterType, viewType);
            
            try
            {
                return (IPresenter)buildMethod.Invoke(null, new[] { viewInstance });
            }
            catch (TargetInvocationException ex)
            {
                throw new InvalidOperationException
                (
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "An exception was thrown whilst trying to create an instance of {0}. Check the InnerException for more information.",
                        presenterType.FullName),
                    ex.InnerException
                );
            }
        }

        public void Release(IPresenter presenter)
        {
            var disposablePresenter = presenter as IDisposable;
            if (disposablePresenter != null)
            {
                disposablePresenter.Dispose();
            }
        }

        static readonly IDictionary<IntPtr, DynamicMethod> buildMethodCache = new Dictionary<IntPtr, DynamicMethod>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "CAS protected members are not exposed, only used internally")]
        internal static DynamicMethod GetBuildMethod(Type type, params Type[] constructorArgumentTypes)
        {
            return buildMethodCache.GetOrCreateValue(type.TypeHandle.Value,
                () => GetBuildMethodInternal(type, constructorArgumentTypes));
        }

        internal static DynamicMethod GetBuildMethodInternal(Type type, params Type[] constructorArgumentTypes)
        {
            var constructor = type.GetConstructor(constructorArgumentTypes);
            if (constructor == null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} is missing expected constructor. We looked for a custructor that took parameters of: ({1}).",
                    type.FullName,
                    string.Join(", ", constructorArgumentTypes.Select(t => t.FullName).ToArray())),
                    "type");
            }

            // Using DynamicMethod and ILGenerator allows us to hold on to a
            // JIT-ed constructor call, which gives us an insanely fast way
            // to create type instances on the fly. This provides a surprising
            // performance improvement over basic reflection in applications
            // that create lots of presenters, which is common.
            var dynamicMethod = new DynamicMethod("DynamicConstructor", type, constructorArgumentTypes, type.Module, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            for (var i = 0; i < constructorArgumentTypes.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_S, i);
            }
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}
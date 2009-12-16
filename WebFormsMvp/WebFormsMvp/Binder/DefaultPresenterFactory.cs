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
            catch (Exception ex)
            {
                var originalException = ex;
                
                if (ex is TargetInvocationException && ex.InnerException != null)
                    originalException = ex.InnerException;

                throw new InvalidOperationException
                (
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "An exception was thrown whilst trying to create an instance of {0}. Check the InnerException for more information.",
                        presenterType.FullName),
                    originalException
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

        static readonly IDictionary<long, DynamicMethod> buildMethodCache = new Dictionary<long, DynamicMethod>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "CAS protected members are not exposed, only used internally")]
        internal static DynamicMethod GetBuildMethod(Type presenterType, Type viewType)
        {
            // We need to scope the cache against both the presenter type and the view type.
            // Combining both type handles using a bit shift gives us a reliable but fast
            // way to key the cache.
            var cacheKey =
                ((Int64)presenterType.TypeHandle.Value << 32) +
                (Int64)viewType.TypeHandle.Value;

            return buildMethodCache.GetOrCreateValue(cacheKey,
                () => GetBuildMethodInternal(presenterType, viewType));
        }

        internal static DynamicMethod GetBuildMethodInternal(Type presenterType, Type viewType)
        {
            var constructor = presenterType.GetConstructor(new[] { viewType });
            if (constructor == null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} is missing an expected constructor. We tried to execute code equivalent to: new {0}({1} view). Add a constructor with a compatible signature, or set PresenterBinder.Factory to something that can supply constructor dependencies.",
                    presenterType.FullName,
                    viewType.FullName),
                    "presenterType");
            }

            // Using DynamicMethod and ILGenerator allows us to hold on to a
            // JIT-ed constructor call, which gives us an insanely fast way
            // to create type instances on the fly. This provides a surprising
            // performance improvement over basic reflection in applications
            // that create lots of presenters, which is common.
            var dynamicMethod = new DynamicMethod("DynamicConstructor", presenterType, new[]{ viewType }, presenterType.Module, false);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}
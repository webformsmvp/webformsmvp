using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IDictionary<RuntimeTypeHandle, Type> viewTypeToPresenterTypeCache
            = new Dictionary<RuntimeTypeHandle, Type>();

        readonly IBuildManager buildManager;

        ///<summary>
        /// Creates a new instance of the ConventionBasedPresenterDiscoveryStrategy class.
        ///</summary>
        ///<param name="buildManager">The IBuildManager implementation to use.</param>
        public ConventionBasedPresenterDiscoveryStrategy(IBuildManager buildManager)
        {
            this.buildManager = buildManager;
        }

        /// <summary />
        public IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances, ITraceContext traceContext)
        {
            if (hosts == null)
                throw new ArgumentNullException("hosts");

            if (viewInstances == null)
                throw new ArgumentNullException("viewInstances");

            return viewInstances
                .Select(v => GetBinding(v, buildManager))
                .Where(b => b != null)
                .ToArray();
        }

        static PresenterBinding GetBinding(IView viewInstance, IBuildManager buildManager)
        {
            var viewType = viewInstance.GetType().BaseType;
            Type presenterType = null;

            if (viewTypeToPresenterTypeCache.ContainsKey(viewType.TypeHandle))
            {
                // Get presenter type from cache
                presenterType = viewTypeToPresenterTypeCache[viewType.TypeHandle];
            }
            else
            {
                // Get presenter class name from view instance type name
                string presenterClassName = GetPresenterClassNameFromControlTypeName(viewType);

                // TODO: Get presenter class name from implemented IView interfaces

                // Create candidate presenter type names
                var candidatePresenterTypeNames = GenerateCandidatePresenterTypeNames(viewType, presenterClassName);

                // Ask the build manager to load each type until one is found
                foreach (var typeName in candidatePresenterTypeNames.Distinct())
                {
                    presenterType = buildManager.GetType(typeName, false);
                    
                    if (presenterType == null) continue;

                    if (!typeof(IPresenter).IsAssignableFrom(presenterType))
                        presenterType = null;
                    else
                        break;
                }

                // Add to cache
                if (presenterType != null)
                {
                    lock (viewTypeToPresenterTypeCache)
                    {
                        viewTypeToPresenterTypeCache[viewType.TypeHandle] = presenterType;
                    }
                }
            }
            
            return presenterType == null ? null :
                new PresenterBinding(presenterType, viewType, BindingMode.Default, new[] { viewInstance });
        }

        static IEnumerable<string> GenerateCandidatePresenterTypeNames(Type viewType, string presenterClassName)
        {
            var candidatePresenterTypeNames = new List<string>();

            var assemblyName = viewType.Assembly.GetName().Name;
            var assemblyNameMinusWeb = TrimFromEnd(assemblyName, ".Web");

            // Assembly name - ".Web" + ".Logic.Presenters"
            candidatePresenterTypeNames.Add(assemblyNameMinusWeb + ".Logic.Presenters." + presenterClassName);

            // Assembly name - ".Web" + ".Logic.Presenters"
            candidatePresenterTypeNames.Add(assemblyNameMinusWeb + ".Logic.Presenters." + presenterClassName);

            // Assembly name - ".Web" + ".Presenters"
            candidatePresenterTypeNames.Add(assemblyNameMinusWeb + ".Presenters." + presenterClassName);

            // Assembly name - ".Web" + ".Logic"
            candidatePresenterTypeNames.Add(assemblyNameMinusWeb + ".Logic." + presenterClassName);

            // Assembly name - ".Web"
            candidatePresenterTypeNames.Add(assemblyNameMinusWeb + "." + presenterClassName);

            // Assembly name + ".Logic.Presenters"
            candidatePresenterTypeNames.Add(assemblyName + ".Logic.Presenters." + presenterClassName);

            // Assembly name + ".Presenters"
            candidatePresenterTypeNames.Add(assemblyName + ".Presenters." + presenterClassName);

            // Assembly name + ".Logic"
            candidatePresenterTypeNames.Add(assemblyName + ".Logic." + presenterClassName);

            // Assembly name
            candidatePresenterTypeNames.Add(assemblyName + "." + presenterClassName);

            // Same location as view instance, e.g. MyApp.Web.Controls.HelloWorldControl => MyApp.Web.Controls.HelloWorldPresenter
            candidatePresenterTypeNames.Add(viewType.Namespace + "." + presenterClassName);

            return candidatePresenterTypeNames;
        }

        static string GetPresenterClassNameFromControlTypeName(Type viewType)
        {
            string presenterClassName;
            if (viewType.Name.EndsWith("UserControl", StringComparison.OrdinalIgnoreCase))
            {
                presenterClassName = TrimFromEnd(viewType.Name, "UserControl");
            }
            else if (viewType.Name.EndsWith("Control", StringComparison.OrdinalIgnoreCase))
            {
                presenterClassName = TrimFromEnd(viewType.Name, "Control");
            }
            else if (viewType.Name.EndsWith("View", StringComparison.OrdinalIgnoreCase))
            {
                presenterClassName = TrimFromEnd(viewType.Name, "View");
            }
            else
            {
                presenterClassName = viewType.Name;
            }
            presenterClassName += "Presenter";
            return presenterClassName;
        }

        static string TrimFromEnd(string source, string suffix)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(suffix))
                return source;
            var length = source.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            return length > 0 ? source.Substring(0, length) : source;
        }
    }
}
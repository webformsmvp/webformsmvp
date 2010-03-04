using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IBuildManager buildManager;

        ///<summary>
        /// Creates a new instance of the ConventionBasedPresenterDiscoveryStrategy class.
        ///</summary>
        ///<param name="buildManager">The IBuildManager implementation to use.</param>
        public ConventionBasedPresenterDiscoveryStrategy(IBuildManager buildManager)
        {
            if (buildManager == null)
                throw new ArgumentNullException("buildManager");

            this.buildManager = buildManager;
        }

        /// <summary />
        public virtual IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances, ITraceContext traceContext)
        {
            if (hosts == null)
                throw new ArgumentNullException("hosts");

            if (viewInstances == null)
                throw new ArgumentNullException("viewInstances");

            return viewInstances
                .Select(v => GetBinding(v, buildManager, ViewInstanceSuffixes, CandidatePresenterTypeFullNameFormats, traceContext))
                .Where(b => b != null)
                .ToArray();
        }

        static readonly IEnumerable<string> defaultViewInstanceSuffixes =
            new[]
            {
                "UserControl",
                "Control",
                "View",
                "Page",
                "Handler",
                "WebService",
                "Service"
            };

        /// <summary>
        /// Override this property to extend the list of suffixes that are automatically stripped from view instances when generating presenter type name candidates.
        /// </summary>
        protected virtual IEnumerable<string> ViewInstanceSuffixes
        {
            get { return defaultViewInstanceSuffixes; }
        }

        // The order of these format strings is important as we yield return to facilitate short circuiting
        // the enumerator as soon as we find a matching type name. The list should be ordered such that the most
        // commonly used naming pattern is at the top and the least used at the bottom.
        static readonly IEnumerable<string> defaultCandidatePresenterTypeFullNameFormats =
            new[]
            {
                "{namespace}.Logic.Presenters.{presenter}",
                "{namespace}.Presenters.{presenter}",
                "{namespace}.Logic.{presenter}",
                "{namespace}.{presenter}"
            };

        /// <summary>
        /// Override this property to extend the list of format strings used to generate candidate names for presenter types.
        /// </summary>
        protected virtual IEnumerable<string> CandidatePresenterTypeFullNameFormats {
            get { return defaultCandidatePresenterTypeFullNameFormats; }
        }

        static readonly IDictionary<RuntimeTypeHandle, Type> viewTypeToPresenterTypeCache = new Dictionary<RuntimeTypeHandle, Type>();
        internal static PresenterBinding GetBinding(IView viewInstance, IBuildManager buildManager, IEnumerable<string> viewInstanceSuffixes, IEnumerable<string> presenterTypeFullNameFormats, ITraceContext traceContext)
        {
            var viewType = viewInstance.GetType();

            var cachedPresenterType = viewTypeToPresenterTypeCache.GetOrCreateValue(viewType.TypeHandle, () =>
            {
                viewType = viewInstance.GetType();
                var presenterType = default(Type);

                // Use the base type for pages & user controls as that is the code-behind file
                // TODO: Ensure using BaseType still works in WebSite projects with code-beside files instead of code-behind files
                if (viewType.Namespace == "ASP" &&
                    (typeof(Page).IsAssignableFrom(viewType) || typeof(Control).IsAssignableFrom(viewType)))
                {
                    viewType = viewType.BaseType;
                }

                // Get presenter type name from view instance type name
                var presenterTypeNames = new List<string> { GetPresenterTypeNameFromViewTypeName(viewType, viewInstanceSuffixes) };

                // Get presenter type names from implemented IView interfaces
                presenterTypeNames.AddRange(GetPresenterTypeNamesFromViewInterfaceTypeNames(viewType.GetViewInterfaces()));

                // Create candidate presenter type full names
                var candidatePresenterTypeFullNames = GenerateCandidatePresenterTypeFullNames(viewType, presenterTypeNames, presenterTypeFullNameFormats);

                // Ask the build manager to load each type until one is found
                foreach (var typeFullName in candidatePresenterTypeFullNames.Distinct())
                {
                    presenterType = buildManager.GetType(typeFullName, false);

                    if (presenterType == null)
                    {
                        traceContext.Write(typeof(ConventionBasedPresenterDiscoveryStrategy), () => string.Format(CultureInfo.InvariantCulture,
                            "Looked for, but did not find, a presenter with type name {0}",
                            typeFullName
                        ));
                        continue;
                    }

                    if (!typeof(IPresenter).IsAssignableFrom(presenterType))
                    {
                        traceContext.Write(typeof(ConventionBasedPresenterDiscoveryStrategy), () => string.Format(CultureInfo.InvariantCulture,
                            "Found potential presenter with type name {0} but it does not implement IPresenter!",
                            typeFullName
                        ));
                        presenterType = null;
                    }
                    else
                    {
                        traceContext.Write(typeof(ConventionBasedPresenterDiscoveryStrategy), () => string.Format(CultureInfo.InvariantCulture,
                            "Found presenter with type name {0}",
                            typeFullName
                        ));
                        break;
                    }
                }

                return presenterType;
            });

            return cachedPresenterType == null
                ? null
                : new PresenterBinding(cachedPresenterType, viewType, BindingMode.Default, new[] { viewInstance });
        }

        internal static IEnumerable<string> GetPresenterTypeNamesFromViewInterfaceTypeNames(IEnumerable<Type> viewInterfaces)
        {
            // Trim the "I" and "View" from the start & end respectively of the interface names
            return viewInterfaces
                .Where(i => i.Name != "IView" && i.Name != "IView`1")
                .Select(i => i.Name.TrimStart('I').TrimFromEnd("View"));
        }

        internal static string GetPresenterTypeNameFromViewTypeName(Type viewType, IEnumerable<string> viewInstanceSuffixes)
        {
            // Check for existance of supported suffixes and if found, remove and use result as basis for presenter type name
            // e.g. HelloWorldControl => HelloWorldPresenter
            //      WidgetsWebService => WidgetsPresenter
            var presenterTypeName = (from suffix in viewInstanceSuffixes
                                     where viewType.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                                     select viewType.Name.TrimFromEnd(suffix))
                                     .FirstOrDefault();
            return (string.IsNullOrEmpty(presenterTypeName) ? viewType.Name : presenterTypeName) + "Presenter";
        }

        static IEnumerable<string> GenerateCandidatePresenterTypeFullNames(Type viewType, IEnumerable<string> presenterTypeNames, IEnumerable<string> presenterTypeFullNameFormats)
        {
            // We assume the assembly name is the same as the namespace or that minus ".Web"
            var assemblyName = viewType.Assembly.GetName().Name;
            var assemblyNameMinusWeb = assemblyName.TrimFromEnd(".Web");

            foreach (var presenterTypeName in presenterTypeNames)
            {
                // Same location as view instance, e.g. MyApp.Web.Controls.HelloWorldControl => MyApp.Web.Controls.HelloWorldPresenter
                yield return viewType.Namespace + "." + presenterTypeName;

                foreach (var typeNameFormat in presenterTypeFullNameFormats)
                {
                    yield return typeNameFormat.Replace("{namespace}", assemblyNameMinusWeb)
                                               .Replace("{presenter}", presenterTypeName);
                }

                if (assemblyName == assemblyNameMinusWeb) continue;

                foreach (var typeNameFormat in presenterTypeFullNameFormats)
                {
                    yield return typeNameFormat.Replace("{namespace}", assemblyName)
                                               .Replace("{presenter}", presenterTypeName);
                }
            }
        }
    }
}
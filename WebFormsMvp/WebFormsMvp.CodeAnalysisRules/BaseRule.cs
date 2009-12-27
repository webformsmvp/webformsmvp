using System;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace WebFormsMvp.CodeAnalysisRules
{
    public abstract class BaseRule : BaseIntrospectionRule
    {
        protected BaseRule(string name)
            : base(
                
                // The name of the rule (must match exactly to an entry in the manifest XML)
                name,
                
                // The name of the manifest XML file
                typeof(BaseRule).Assembly.GetName().Name + ".WebFormsMvp.CodeAnalysisRules",
                
                // The assembly to find the manifest XML in
                typeof(BaseRule).Assembly)
        {
        }

        internal static TypeNode GetIPresenterTypeNode(TypeNode inspectingType)
        {
            return inspectingType
                .DeclaringModule
                .GetType(Identifier.For("WebFormsMvp"), Identifier.For("IPresenter"), true);
        }
    }
}
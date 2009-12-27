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
                typeof(BaseRule).Assembly.GetName().Name + ".Rules",
                
                // The assembly to find the manifest XML in
                typeof(BaseRule).Assembly)
        {
        }

        internal static bool IsPresenterImplementation(TypeNode type)
        {
            if (type.NodeType != NodeType.Class) return false;

            // If it's an abstract base class, let them do what they want
            if (type.IsAbstract) return false;

            var iPresenter = GetIPresenterTypeNode(type);
            
            // If we can't even find IPresenter, bail out
            if (iPresenter == null) return false;

            return type.IsAssignableTo(iPresenter);
        }

        internal static TypeNode GetIPresenterTypeNode(TypeNode inspectingType)
        {
            return inspectingType
                .DeclaringModule
                .GetType(Identifier.For("WebFormsMvp"), Identifier.For("IPresenter"), true);
        }

        internal static TypeNode GetIView1TypeNode(TypeNode inspectingType)
        {
            return inspectingType
                .DeclaringModule
                .GetType(Identifier.For("WebFormsMvp"), Identifier.For("IView`1"), true);
        }

        internal static TypeNode GetBasePresenterTypeNode(TypeNode inspectingType)
        {
            return inspectingType
                .DeclaringModule
                .GetType(Identifier.For("WebFormsMvp"), Identifier.For("Presenter`1"), true);
        }
    }
}
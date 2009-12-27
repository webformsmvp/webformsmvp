using System;
using Microsoft.FxCop.Sdk;

namespace WebFormsMvp.CodeAnalysisRules
{
    public class PresentersShouldUseProvidedBaseImplementation : BaseRule
    {
        public PresentersShouldUseProvidedBaseImplementation()
            : base("PresentersShouldUseProvidedBaseImplementation")
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            if (type == null) return null;

            if (!IsPresenterImplementation(type)) return null;

            var basePresenter = GetBasePresenterTypeNode(type);
            if (basePresenter == null)
                throw new InvalidOperationException("Failed to find WebFormsMvp.Presenter`1 even though we found WebFormsMvp.IPresenter.");

            var baseType = type;
            // We have an extra level of base type checking here so that we skip System.Object
            while (baseType.BaseType != null &&
                   baseType.BaseType.BaseType != null)
            {
                baseType = baseType.BaseType;
            }

            if (baseType.Template != basePresenter)
            {
                return new ProblemCollection { new Problem(
                    GetResolution(type.FullName)) {
                    Certainty = 100,
                    FixCategory = FixCategories.NonBreaking,
                    MessageLevel = MessageLevel.Warning
                }};
            }

            return null;
        }
    }
}
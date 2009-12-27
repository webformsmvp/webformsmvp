using System;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace WebFormsMvp.CodeAnalysisRules
{
    public class PresentersShouldUseConsistentViewType : BaseRule
    {
        public PresentersShouldUseConsistentViewType()
            : base("PresentersShouldUseConsistentViewType")
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            if (type == null) return Problems;

            if (!IsPresenterImplementation(type)) return Problems;

            var basePresenter = GetBasePresenterTypeNode(type);
            if (basePresenter == null)
                throw new InvalidOperationException("Failed to find WebFormsMvp.Presenter`1 even though we found WebFormsMvp.IPresenter.");

            var presenterBaseType = type;
            // We have an extra level of base type checking here so that we skip System.Object
            while (presenterBaseType.BaseType != null &&
                   presenterBaseType.BaseType.BaseType != null)
            {
                presenterBaseType = presenterBaseType.BaseType;
            }

            if (presenterBaseType.Template != basePresenter) return Problems;

            var viewTypeFromGenericTypeArgument = presenterBaseType.TemplateArguments.Single();

            var iViewType = GetIViewTypeNode(type);
            if (iViewType == null)
                throw new InvalidOperationException("Failed to find WebFormsMvp.IView even though we found WebFormsMvp.IPresenter.");

            var badParameters = type
                .GetConstructors()
                .Cast<InstanceInitializer>()
                .SelectMany(c => c.Parameters.Where(p => p.Type.IsAssignableTo(iViewType)))
                .Where(p => p.Type != viewTypeFromGenericTypeArgument);

            foreach(var param in badParameters)
            {
                Problems.Add(new Problem(GetResolution(new[]
                             {
                                 type.Name.Name,
                                 viewTypeFromGenericTypeArgument.Name.Name,
                                 param.Type.Name.Name
                             }),
                             param)
                             {
                                 Certainty = 100,
                                 FixCategory = FixCategories.NonBreaking,
                                 MessageLevel = MessageLevel.Error
                             });
            }

            return Problems;
        }
    }
}
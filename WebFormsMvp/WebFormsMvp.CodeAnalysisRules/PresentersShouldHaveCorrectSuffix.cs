using System;
using System.Linq;
using Microsoft.FxCop.Sdk;

namespace WebFormsMvp.CodeAnalysisRules
{
    public class PresentersShouldHaveCorrectSuffix : BaseRule
    {
        public PresentersShouldHaveCorrectSuffix()
            : base("PresentersShouldHaveCorrectSuffix")
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            if (type == null || type.NodeType != NodeType.Class)
            {
                return null;
            }

            // If it's an abstract base class, let them do what they want
            if (type.IsAbstract)
            {
                return null;
            }

            var iPresenter = GetIPresenterTypeNode(type);

            // We only care if the type implements IPresenter
            if (iPresenter == null ||
                !type.IsAssignableTo(iPresenter))
            {
                return null;
            }

            if (!type.Name.Name.EndsWith("Presenter", StringComparison.Ordinal))
            {
                return new ProblemCollection { new Problem(
                    GetResolution(type.FullName)) {
                    Certainty = 100,
                    FixCategory = FixCategories.Breaking,
                    MessageLevel = MessageLevel.Warning
                    }};
            }

            return null;
        }
    }
}
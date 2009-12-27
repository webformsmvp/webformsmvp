using System;
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
            if (type == null) return Problems;

            if (!IsPresenterImplementation(type)) return Problems;

            if (!type.Name.Name.EndsWith("Presenter", StringComparison.Ordinal))
            {
                Problems.Add(new Problem(
                    GetResolution(type.FullName)) {
                    Certainty = 100,
                    FixCategory = FixCategories.Breaking,
                    MessageLevel = MessageLevel.Warning
                });
            }

            return Problems;
        }
    }
}
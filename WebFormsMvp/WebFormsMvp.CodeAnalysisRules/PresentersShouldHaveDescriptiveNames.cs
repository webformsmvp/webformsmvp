using System;
using Microsoft.FxCop.Sdk;

namespace WebFormsMvp.CodeAnalysisRules
{
    public class PresentersShouldHaveDescriptiveNames : BaseRule
    {
        public PresentersShouldHaveDescriptiveNames()
            : base("PresentersShouldHaveDescriptiveNames")
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            if (type == null) return null;

            if (!IsPresenterImplementation(type)) return null;
            
            if (type.Name.Name.Equals("Presenter", StringComparison.OrdinalIgnoreCase))
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
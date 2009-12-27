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
            if (type == null) return Problems;

            if (!IsPresenterImplementation(type)) return Problems;
            
            if (type.Name.Name.Equals("Presenter", StringComparison.OrdinalIgnoreCase))
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
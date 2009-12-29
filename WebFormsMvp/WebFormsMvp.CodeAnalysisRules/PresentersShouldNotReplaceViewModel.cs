using System;
using Microsoft.FxCop.Sdk;
using Microsoft.VisualStudio.CodeAnalysis.Extensibility;

namespace WebFormsMvp.CodeAnalysisRules
{
    public class PresentersShouldNotReplaceViewModel : BaseRule
    {
        public PresentersShouldNotReplaceViewModel()
            : base("PresentersShouldNotReplaceViewModel")
        {
        }

        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;

            if (method == null) return Problems;

            if (!IsPresenterImplementation(method.DeclaringType)) return Problems;

            VisitMethod(method);

            return Problems;
        }

        public override void VisitMethodCall(MethodCall call)
        {
            // We're looking for a call to IView<TModel>.set_Model which would be done
            // via IL similar to: callvirt WebFormsMvp.IView<TModel>.set_Model

            if (IsViewSetModelCall(call))
            {
                Problems.Add(new Problem(
                                 GetResolution(), call)
                             {
                                 Certainty = 100,
                                 FixCategory = FixCategories.NonBreaking,
                                 MessageLevel = MessageLevel.Error
                             });
            }
        }

        static bool IsViewSetModelCall(MethodCall call)
        {
            if (call.NodeType != NodeType.Callvirt) return false;

            var calleeMemberBinding = call.Callee as MemberBinding;
            if (calleeMemberBinding == null) return false;

            if (!calleeMemberBinding.BoundMember.Name.Name.Equals("set_Model", StringComparison.Ordinal)) return false;

            var iViewTemplate = GetIView1TypeNode(calleeMemberBinding.BoundMember.DeclaringType);
            if (iViewTemplate == null)
                throw new InvalidOperationException("Failed to find WebFormsMvp.IView`1 even though we found WebFormsMvp.IPresenter.");

            if (calleeMemberBinding.BoundMember.DeclaringType.Template != iViewTemplate) return false;

            return true;
        }
    }
}
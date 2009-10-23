using System;
using System.Linq;
using System.Security.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsMvp.UnitTests
{
    internal static class TestHelper
    {
        /// <summary>
        /// Creates an AppDomain based on the passed TestContext to provide isolation for a unit test.
        /// </summary>
        internal static AppDomain CreateAppDomain(TestContext testContext)
        {
            return AppDomain.CreateDomain(
                testContext.TestName + "_AppDomain",
                new Evidence(AppDomain.CurrentDomain.Evidence),
                new AppDomainSetup { ApplicationBase = testContext.TestDeploymentDir }
            );
        }

        /// <summary>
        /// Runs a delegate in its own AppDomain for isolation during unit testing.
        /// </summary>
        internal static void Isolate(TestContext testContext, CrossAppDomainDelegate action, Action<AppDomain> assertion)
        {
            AppDomain appDomain = null;
            try
            {
                appDomain = CreateAppDomain(testContext);
                appDomain.DoCallBack(action);
                assertion.Invoke(appDomain);
            }
            finally
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);
            }
        }
    }
}
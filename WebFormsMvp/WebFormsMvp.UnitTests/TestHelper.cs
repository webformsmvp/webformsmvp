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
        private static AppDomain CreateAppDomain(TestContext testContext)
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
        internal static void Isolate(this TestContext testContext, Action testAction)
        {
            if (testAction == null)
                throw new ArgumentNullException("testAction");

            AppDomain appDomain = null;
            try
            {
                appDomain = CreateAppDomain(testContext);
                appDomain.SetData("testAction", testAction);
                appDomain.DoCallBack(() =>
                {
                    try
                    {
                        var marshalledAction = AppDomain.CurrentDomain.GetData("testAction") as Action;
                        marshalledAction.Invoke();
                    }
                    catch (Exception ex)
                    {
                        AppDomain.CurrentDomain.SetData("Exception", ex);
                    }
                });
                var testActionEx = appDomain.GetData("Exception") as Exception;
                if (testActionEx != null)
                {
                    throw testActionEx;
                }
            }
            finally
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);
            }
        }
    }
}
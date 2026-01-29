// =========================================================================
// Copyright 2021 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================
using System;
using System.Reflection;

namespace DataMocker.UITest
{
    /// <summary>
    /// Extension methods for MAUI UITestContext integration.
    /// Provides helper methods to initialize DataMocker with MAUI UI tests.
    /// </summary>
    public static class UITestContextExtensions
    {
        /// <summary>
        /// Initializes DataMocker framework for the current test using MAUI UITestContext.
        /// This method should be called in the test setup method.
        /// </summary>
        /// <param name="testContext">The MAUI UITestContext instance (typically from UITestBase.Context).</param>
        /// <param name="testDevice">The test device platform.</param>
        /// <param name="testType">The type of the test class.</param>
        /// <param name="additionalParams">Additional parameters to pass to the framework.</param>
        /// <remarks>
        /// Example usage in a test class:
        /// <code>
        /// [SetUp]
        /// public void BeforeEachTest()
        /// {
        ///     UITestContextExtensions.InitializeDataMocker(Context, Device, GetType());
        /// }
        /// </code>
        /// </remarks>
        public static void InitializeDataMocker(IUITestContext testContext, TestDevice testDevice, Type testType, string additionalParams = "")
        {
            TestInitializer.Initialize(testContext, testDevice, testType, additionalParams);
        }

        /// <summary>
        /// Creates an IUITestContext wrapper from an object that implements Invoke method.
        /// This is useful for adapting existing MAUI UITestContext instances.
        /// </summary>
        /// <param name="context">The context object that has an Invoke method.</param>
        /// <returns>An IUITestContext wrapper.</returns>
        public static IUITestContext AsUITestContext(this object context)
        {
            if (context is IUITestContext directContext)
            {
                return directContext;
            }

            // Use reflection to find and call Invoke method
            var invokeMethod = context.GetType().GetMethod("Invoke", new[] { typeof(string), typeof(string) });
            if (invokeMethod == null)
            {
                throw new ArgumentException("The context object must have an Invoke(string, string) method.", nameof(context));
            }

            return new ReflectiveUITestContext(context, invokeMethod);
        }

        private class ReflectiveUITestContext : IUITestContext
        {
            private readonly object _context;
            private readonly MethodInfo _invokeMethod;

            public ReflectiveUITestContext(object context, MethodInfo invokeMethod)
            {
                _context = context;
                _invokeMethod = invokeMethod;
            }

            public void Invoke(string methodName, string argument)
            {
                _invokeMethod.Invoke(_context, new object[] { methodName, argument });
            }
        }
    }
}

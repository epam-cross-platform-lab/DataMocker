// =========================================================================
// Copyright 2019 EPAM Systems, Inc.
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
using NUnit.Framework;
using DataMocker.UITest;

namespace DataMockerSample.UITests
{
    /// <summary>
    /// Base test class for MAUI UI tests.
    /// This class provides integration with DataMocker framework.
    /// </summary>
    /// <remarks>
    /// For MAUI UITest framework, you should inherit from UITestBase and call
    /// InitializeDataMocker in your SetUp method, or inherit from this class
    /// and set the Context and Device properties.
    /// </remarks>
    public abstract class BaseTest
    {
        /// <summary>
        /// The MAUI UITestContext. This should be set by the test framework
        /// (typically from UITestBase.Context in MAUI UITest framework).
        /// </summary>
        protected object? Context { get; set; }

        /// <summary>
        /// The test device platform. This should be set by the test framework
        /// (typically from UITestBase.Device in MAUI UITest framework).
        /// </summary>
        protected TestDevice Device { get; set; }

        /// <summary>
        /// Provides access to UI interaction methods through dynamic dispatch.
        /// This property allows existing tests to continue using 'app' syntax.
        /// </summary>
        protected dynamic? app => Context;

        [SetUp]
        public virtual void BeforeEachTest()
        {
            if (Context == null)
            {
                throw new InvalidOperationException(
                    "Context must be set. In MAUI UITest, set Context = base.Context in your constructor or override BeforeEachTest.");
            }

            // Map the device if not already set
            if (Device == default)
            {
                Device = MapTestDevice();
            }

            // Wrap the context for DataMocker and initialize
            var dataMockerContext = Context.AsUITestContext();
            DataMocker.UITest.TestInitializer.Initialize(dataMockerContext, Device, GetType());
        }

        /// <summary>
        /// Maps the test device. Override this if your framework uses a different TestDevice enum.
        /// </summary>
        protected virtual TestDevice MapTestDevice()
        {
            // Try to get Device property from the test framework
            var deviceProperty = GetType().GetProperty("Device", 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.NonPublic);

            if (deviceProperty != null)
            {
                var deviceValue = deviceProperty.GetValue(this);
                if (deviceValue != null)
                {
                    return MapDeviceValue(deviceValue);
                }
            }

            // Default to Android
            return TestDevice.Android;
        }

        private TestDevice MapDeviceValue(object deviceValue)
        {
            var deviceString = deviceValue.ToString() ?? "";
            return deviceString.ToLowerInvariant() switch
            {
                "android" => TestDevice.Android,
                "ios" => TestDevice.iOS,
                "windows" => TestDevice.Windows,
                "mac" or "catalyst" => TestDevice.Mac,
                _ => TestDevice.Android
            };
        }
    }
}

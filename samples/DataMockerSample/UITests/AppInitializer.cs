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
using DataMocker.UITest;

namespace DataMockerSample.UITests
{
    /// <summary>
    /// App initializer for MAUI UI tests.
    /// </summary>
    /// <remarks>
    /// In MAUI UITest framework, app initialization is typically handled by the test framework itself.
    /// This class provides helper methods if custom initialization is needed.
    /// </remarks>
    public static class AppInitializer
    {
        /// <summary>
        /// Gets the app package identifier for the specified platform.
        /// </summary>
        public static string GetAppPackageId(TestDevice device)
        {
            return device switch
            {
                TestDevice.Android => "com.epam.DataMockerSample",
                TestDevice.iOS => "com.epam.DataMockerSample",
                TestDevice.Windows => "com.epam.DataMockerSample",
                TestDevice.Mac => "com.epam.DataMockerSample",
                _ => throw new NotSupportedException($"Device {device} is not supported")
            };
        }

        /// <summary>
        /// Gets the device identifier for iOS (if needed).
        /// </summary>
        public static string? GetIOSDeviceIdentifier()
        {
            // Return null to use default device, or specify a device identifier
            return null; // "06FACE51-369A-4AD2-9591-377A87A39982"
        }
    }
}

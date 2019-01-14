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
using System.Configuration;
using Xamarin.UITest;

namespace DataMocker.UITest
{   
    /// <summary>Wrapper for the MockFrasmeworkConfiguration.json file.</summary>
    public class MockFrameworkConfiguration
    {
        /// <summary>
        /// Gets or sets the URL to the mock server.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the delay for mock server.
        /// </summary>
        /// <value>The delay for mock server.</value>
        public int Delay { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:DataMocker.UITest.MockFrameworkConfiguration"/>
        /// use embedded device.
        /// </summary>
        /// <value><c>true</c> if use embedded device; otherwise, <c>false</c>.</value>
        public bool UseEmbeddedDevice { get; set; }

        /// <summary>Gets or sets the android backdoor method.</summary>
        /// <value>The android backdoor method.</value>
        public string AndroidBackdoorMethod { get; set; }

        /// <summary>
        /// Gets or sets the ios backdoor method.
        /// </summary>
        /// <value>The ios backdoor method.</value>
        public string IosBackdoorMethod { get; set; }

        internal string GetBackDoorName(Platform platform)
        {
            return platform == Platform.Android ? AndroidBackdoorMethod : IosBackdoorMethod;
        }
    }
}

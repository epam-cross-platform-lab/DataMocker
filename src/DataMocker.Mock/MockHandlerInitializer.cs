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
using System.Net.Http;
using System.Reflection;
using DataMocker.Mock.Handlers;

namespace DataMocker.Mock
{
    /// <summary>
    /// Intialize new mock handlers depending on arsg string.
    /// </summary>
    public class MockHandlerInitializer : IMockHandlerIntializer
    {
        private readonly MockEnvironmentConfig _appEnvironmentConfig;
        private readonly Assembly _resourceAssembly;

        /// <summary>Initializes a new instance of the <see cref="T:DataMocker.Mock.MockHandlerInitializer"></see> class.</summary>
        /// <param name="args">Arguments string from backdoor method.</param>
        /// <param name="assembly">Assembly with mock data.</param>
        public MockHandlerInitializer(string args, Assembly assembly)
        {
            _appEnvironmentConfig = new MockEnvironmentConfig();
            _appEnvironmentConfig.Initialize(args);
            _resourceAssembly = assembly;
        }

        /// <summary>
        ///  Gets a new instance of mock handler.
        /// </summary>
        /// <returns>Mock handler based on <see cref="T:System.Net.Http.HttpMessageHandler"/>.</returns>
        public HttpMessageHandler GetMockerHandler()
        {
            if (!string.IsNullOrWhiteSpace(_appEnvironmentConfig.RemoteUrl))
            {
                return new RemoteHostHttpHandler(new MockRequestBuilder(_appEnvironmentConfig));
            }

            return new EmbeddedResourceHttpHandler(new MockRequestBuilder(_appEnvironmentConfig), _resourceAssembly);
        }
    }
}
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
using System.Net.Http;
using System.Reflection;
using DataMocker.Mock.Handlers;

namespace DataMocker.Mock
{
    /// <summary>
    /// Intialize new mock handlers depending on args string.
    /// </summary>
    public class MockHandlerInitializer : IMockHandlerIntializer
    {
        /// <summary>
        ///     The app environment config.
        /// </summary>
        protected readonly MockEnvironmentConfig _appEnvironmentConfig;
       
        /// <summary>
        ///     The resource assembly.
        /// </summary>
       protected readonly Assembly _resourceAssembly;

        /// <summary>Initializes a new instance of the <see cref="T:DataMocker.Mock.MockHandlerInitializer"></see> class.</summary>
        /// <param name="args">Arguments string from backdoor method.</param>
        /// <param name="assembly">Assembly with mock data.</param>
        public MockHandlerInitializer(string args, Assembly assembly)
            : this(ToEnvironmentConfig(args), assembly)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.MockHandlerInitializer"/> class.
        /// </summary>
        /// <param name="environmentConfig">Environment config.</param>
        /// <param name="assembly">Assembly.</param>
        public MockHandlerInitializer(MockEnvironmentConfig environmentConfig, Assembly assembly)
        {
            _appEnvironmentConfig = environmentConfig;
            _resourceAssembly = assembly;
        }

        /// <summary>
        ///     Gets a new instance of mock handler.
        /// </summary>
        /// <returns>Mock handler based on <see cref="T:System.Net.Http.HttpMessageHandler"/>.</returns>
        public virtual HttpMessageHandler GetMockerHandler()
        {
            if (!string.IsNullOrWhiteSpace(_appEnvironmentConfig.RemoteUrl))
            {
                if (!_appEnvironmentConfig.WriteMode)
                {
                    return new RemoteHostHttpHandler(RequestBuilder());
                }

                return new WriteToMockServerHttpHandler(RequestBuilder(), _appEnvironmentConfig.RemoteUrl);
            }

            return new EmbeddedResourceHttpHandler(RequestBuilder(), _resourceAssembly);
        }

        /// <summary>
        ///     Creates new instance of MockRequestBuilder.
        /// </summary>
        /// <returns>The <see cref="T:System.Net.Http.MockRequestBuilder"/>.</returns>
        protected virtual MockRequestBuilder RequestBuilder()
        {
            return new MockRequestBuilder(_appEnvironmentConfig);
        }

        private static MockEnvironmentConfig ToEnvironmentConfig(string args)
        {
            var config = new MockEnvironmentConfig();
            config.Initialize(args);
            return config;
        }
    }
}
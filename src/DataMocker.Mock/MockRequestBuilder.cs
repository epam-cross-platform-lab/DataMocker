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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DataMocker.SharedModels;
using DataMocker.SharedModels.Resources;

namespace DataMocker.Mock
{
    /// <summary>
    ///     Mock request builder.
    /// </summary>
    public class MockRequestBuilder
    {
        private readonly MockEnvironmentConfig _appEnvironmentConfig;

        /// <summary>
        ///     Gets the remote URL.
        /// </summary>
        /// <value>The remote URL.</value>
        public string RemoteUrl => _appEnvironmentConfig.RemoteUrl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.MockRequestBuilder"/> class.
        /// </summary>
        /// <param name="appEnvironmentConfig">App environment config.</param>
        public MockRequestBuilder(MockEnvironmentConfig appEnvironmentConfig)
        {
            _appEnvironmentConfig = appEnvironmentConfig;
        }

        /// <summary>
        ///     Creates new instance of MockRequest.
        /// </summary>
        /// <returns>The <see cref="T:DataMocker.Mock.MockRequest"/>.</returns>
        /// <param name="message">Message.</param>
        /// <param name="checkRouting">If set to <c>true</c> check routing.</param>
        public virtual MockRequest MockRequest(HttpRequestMessage message, bool checkRouting)
        {
            return ParseUrlToMockRequest(
                message.RequestUri,
                message.Method.Method,
                _appEnvironmentConfig?.TestScenarios?.ToList(),
                _appEnvironmentConfig?.TestName,
                _appEnvironmentConfig?.SharedFolder?.ToList(),
                _appEnvironmentConfig?.Language,
                checkRouting,
                message.Content?.ReadAsStringAsync()?.Result);
        }

        /// <summary>
        ///     Generates mock file name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="url">URL.</param>
        protected virtual string FileName(Uri url)
        {
            return string.Join("_", url.Segments.Select(s => s.Trim('/')));
        }

        private MockRequest ParseUrlToMockRequest(Uri uri,
            string httpMethod,
            IList<string> testScenarioList,
            string testName,
            IList<string> sharedFoldersList,
            string language,
            bool checkRouting,
            string body = null)
        {
            var mockRequest = new MockRequest
            {
                HttpMethod = httpMethod,
                TestScenarioList = testScenarioList,
                Language = language,
                TestName = testName,
                SharedFoldersList= sharedFoldersList,
                Body = body ?? string.Empty,
                FileName = FileName(uri),
            };

            mockRequest.Hash = new FixedLength(
                new ResourceHashCode(
                    string.Concat(uri.AbsoluteUri, mockRequest.Body)
                ),
                length: 8
            ).ToHexString();
            
            if (!checkRouting)
            {
                return mockRequest;
            }

            var routedUrl = Routes.RoutedNameByUrl(uri);

            if (string.IsNullOrWhiteSpace(routedUrl))
            {
                return mockRequest;
            }

            mockRequest.FileName = routedUrl;
            mockRequest.IsRouted = true;

            return mockRequest;
        }
    }
}
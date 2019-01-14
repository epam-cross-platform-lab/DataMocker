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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DataMocker.SharedModels;
using DataMocker.SharedModels.Resources;

namespace DataMocker.Mock
{
    internal class MockRequestBuilder
    {
        private readonly MockEnvironmentConfig _appEnvironmentConfig;

        internal string RemoteUrl => _appEnvironmentConfig.RemoteUrl;

        internal MockRequestBuilder(MockEnvironmentConfig appEnvironmentConfig)
        {
            _appEnvironmentConfig = appEnvironmentConfig;
        }

        internal MockRequest MockRequest(HttpRequestMessage message, bool checkRouting)
        {
            return ParseUrlToMockRequest(
                message.RequestUri,
                message.Method.Method,
                _appEnvironmentConfig?.TestScenarios,
                _appEnvironmentConfig?.TestName,
                _appEnvironmentConfig?.SharedFolder,
                _appEnvironmentConfig?.Language,
                checkRouting,
                message.Content?.ReadAsStringAsync()?.Result);
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
            mockRequest.Hash = ResourceHashCreator.Create(uri.AbsoluteUri , mockRequest.Body);
            
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

        private static string FileName(Uri url)
        {
            return string.Join("_", url.Segments.Select(s => s.Trim('/'))); 
        }
    }
}
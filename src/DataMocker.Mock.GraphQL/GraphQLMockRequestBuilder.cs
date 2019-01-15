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
using System.Text.RegularExpressions;
using DataMocker.SharedModels;
using DataMocker.SharedModels.Resources;
using Newtonsoft.Json;

namespace DataMocker.Mock.GraphQL
{
    /// <summary>
    ///     GraphQL mock request builder.
    /// </summary>
    public class GraphQLMockRequestBuilder : MockRequestBuilder
    {
        private readonly GraphQLMockEnvironmentConfig _appEnvironmentConfig;

        /// <summary>
        ///     Gets the remote URL.
        /// </summary>
        /// <value>The remote URL.</value>
        public string GraphQLUrl => _appEnvironmentConfig.GraphQLUrl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockRequestBuilder"/> class.
        /// </summary>
        /// <param name="appEnvironmentConfig">App environment config.</param>
        public GraphQLMockRequestBuilder(GraphQLMockEnvironmentConfig appEnvironmentConfig) : base(appEnvironmentConfig)
        {
            _appEnvironmentConfig = appEnvironmentConfig;
        }

        /// <summary>
        ///     Creates new instance of MockRequest.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="message">Message.</param>
        /// <param name="checkRouting">If set to <c>true</c> check routing.</param>
        public override MockRequest MockRequest(HttpRequestMessage message, bool checkRouting)
        {
            if (string.Equals(message.RequestUri.AbsoluteUri, GraphQLUrl))
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
            return base.MockRequest(message, checkRouting);
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
                SharedFoldersList = sharedFoldersList,
                Body = body ?? string.Empty,
                FileName = FileName(uri, body),
            };
            mockRequest.Hash = ResourceHashCreator.Create(uri.AbsoluteUri, mockRequest.Body);

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

        private static string FileName(Uri url, string body)
        {
            var graphQLsegments = new List<string>();
            GraphQLRequest requestBody = JsonConvert.DeserializeObject<GraphQLRequest>(body);
            if (!string.IsNullOrEmpty(requestBody.Query))
            {
                var matches = Regex.Matches(requestBody.Query, @"([a-zA-Z])\w+\(");

                foreach (Match match in matches)
                {
                    var indexInQuery = requestBody.Query.IndexOf(match.Value, StringComparison.Ordinal);
                    if (!(indexInQuery != 0 && requestBody.Query[indexInQuery - 1] == '@'))
                    {
                        graphQLsegments.Add(match.Value.Replace("(", string.Empty));
                    }
                }
            }

            return string.Join("_", url.Segments.Select(s => s.Trim('/')).Concat(graphQLsegments));
        }
    }
}

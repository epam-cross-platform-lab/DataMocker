// =========================================================================
// Copyright 2018 EPAM Systems, Inc.
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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataMocker.SharedModels;
using Newtonsoft.Json;

namespace DataMocker.Mock.Handlers
{
    /// <summary>
    /// RemoteHostHttpHandler simulate server behaivior by using mock server.
    /// </summary>
    public class RemoteHostHttpHandler : MockHttpHandler
    {   
        internal RemoteHostHttpHandler(
            MockRequestBuilder mockRequestBuilder)
            : base(mockRequestBuilder)
        {
        }

        /// <summary>
        /// Return Stream from mock server's response's content.
        /// </summary>
        /// <param name="mockRequest">Mock request.</param>
        /// <returns>Task Stream with wanted mock data.</returns>
        protected override async Task<Stream> DataStream(MockRequest mockRequest)
        {
            using (var httpClient = new HttpClient())
            {
                var dataMockerVersion = Assembly.GetAssembly(typeof(RemoteHostHttpHandler)).GetName().Version.ToString();
                httpClient.DefaultRequestHeaders.Add("user-agent",dataMockerVersion);
                var requestUri = new Uri(MockRequestBuilder.RemoteUrl);
                var httpContent = HttpContent(mockRequest);
                var responseMessage = await httpClient.PostAsync(requestUri, httpContent);

                if (responseMessage.StatusCode != HttpStatusCode.NotFound)
                {
                    return await responseMessage.Content.ReadAsStreamAsync();
                }

                return null;
            }
        }

        private static HttpContent HttpContent(MockRequest mockRequestData)
        {
            return new StringContent(JsonConvert.SerializeObject(mockRequestData), Encoding.UTF8, "application/json");
        }
    }
}
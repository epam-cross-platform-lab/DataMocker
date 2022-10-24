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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataMocker.SharedModels;
using DataMocker.Mock.DatesReplacing;
using Newtonsoft.Json;

namespace DataMocker.Mock.Handlers
{
    /// <summary>
    /// Base abstract class for handlers in mock test framework.
    /// </summary>
    public abstract class MockHttpHandler : HttpMessageHandler
    {
        internal readonly MockRequestBuilder MockRequestBuilder;

        internal MockHttpHandler(MockRequestBuilder mockRequestBuilder)
        {
            MockRequestBuilder = mockRequestBuilder;
        }

        /// <summary>
        /// Method processes HttpRequestMessages.
        /// </summary>
        /// <param name="request">Handled request.</param>
        /// <returns>Task HttpResponseMessage with mocked data.</returns>
        public virtual async Task<HttpResponseMessage> Call(HttpRequestMessage request)
        {
            switch (request.Method.Method)
            {
                case WebRequestMethods.Http.Get:
                    return await ReadData(request);
                case WebRequestMethods.Http.Post:
                    if (request.Content == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                    return await ReadData(request);
                case WebRequestMethods.Http.Put:
                    if (request.Content == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                    return await RequestWithoutResponse(request);
                case "DELETE":
                    return await RequestWithoutResponse(request);
                default: return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Return stream for mock request.
        /// </summary>
        /// <param name="mockRequest">Mock request.</param>
        /// <returns>The task Stream with wanted mock data.</returns>
        protected abstract Task<Stream> DataStream(MockRequest mockRequest);


        /// <summary>Catch an HTTP request and begins its processing.</summary>
        /// <param name="request">The HTTP request message to catch.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="request">request</paramref> was null.</exception>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await Call(request);
        }

        private async Task<HttpResponseMessage> ReadData(HttpRequestMessage request)
        {
            var mockRequest = MockRequestBuilder.MockRequest(request, true);
            using (var stream = await DataStream(mockRequest))
            {
                var resourceStream = stream;
                if (resourceStream == null && mockRequest.IsRouted)
                {
                    resourceStream = await DataStream(MockRequestBuilder.MockRequest(request, false));
                }

                if (resourceStream == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                return ReadData(resourceStream);
            }
        }

        private HttpResponseMessage ReadData(Stream resourceStream)
        {
            using (var reader = new StreamReader(resourceStream))
            {
                var data = reader.ReadToEnd();

                if (data == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(new JsonWithDatesReplacements(data).ToJsonWithDynamicDates())
                };
            }
        }

        private async Task<HttpResponseMessage> RequestWithoutResponse(HttpRequestMessage request)
        {
            var response = await ReadData(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return response;
            }

            return new HttpResponseMessage
            {
                StatusCode = JsonConvert.DeserializeObject<HttpStatusCode>(await response.Content.ReadAsStringAsync())
            };
        }
    }
}

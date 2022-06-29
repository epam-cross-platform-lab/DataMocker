using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataMocker.SharedModels;
using Newtonsoft.Json;

namespace DataMocker.Mock.Handlers
{
    public class WriteToMockServerHttpHandler : DelegatingHandler
    {
        private string _mockServerUrl;
        private MockRequestBuilder _mockRequestBuilder;

        public WriteToMockServerHttpHandler(MockRequestBuilder mockRequestBuilder, string mockServerUrl)
        {
            _mockServerUrl = mockServerUrl;
            _mockRequestBuilder = mockRequestBuilder;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request, cancellationToken);
                var content = HttpContent(new SaveRequestContent
                { Response = await response.Content.ReadAsStringAsync(), MockRequest = _mockRequestBuilder.MockRequest(request, false) });
                await httpClient.PutAsync(new Uri(_mockServerUrl),
                    content,
                    cancellationToken);
                return response;
            }
        }

        private static StringContent HttpContent(SaveRequestContent content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                "application/json");
        }
    }
}
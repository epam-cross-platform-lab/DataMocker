using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataMocker.SharedModels;
using DataMocker.Mock.DatesReplacing;
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
                var jsonContent = new JsonWithDynamicDates(await response.Content.ReadAsStringAsync()).ToJsonWithDatesReplacements();
                var content = HttpContent(new SaveRequestContent
                { Response = jsonContent, MockRequest = _mockRequestBuilder.MockRequest(request, false) });
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
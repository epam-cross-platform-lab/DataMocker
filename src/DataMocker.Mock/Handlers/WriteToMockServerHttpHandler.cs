using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DataMocker.Mock.Handlers
{
    public class WriteToMockServerHttpHandler : DelegatingHandler
    {
        private string _mockServerUrl;

        public WriteToMockServerHttpHandler(string mockServerUrl)
        {
            _mockServerUrl = mockServerUrl;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var saveRequestToMockServer = new HttpRequestMessage(HttpMethod.Post, _mockServerUrl);
            return response;
        }
    }
}
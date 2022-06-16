using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataMocker.SharedModels;
using Newtonsoft.Json;

namespace DataMocker.Mock.Handlers
{
    public class WriteToMockServerHttpHandler : HttpMessageHandler
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
                try
                {
                    var clonedRequest = await CloneHttpRequestMessageAsync(request);
                    var response = await httpClient.SendAsync(clonedRequest, cancellationToken);
                    var content = HttpContent(new SaveRequestContent
                        { Response = await response.Content.ReadAsStringAsync(), MockRequest = _mockRequestBuilder.MockRequest(request, false) });
                    var r = await httpClient.PutAsync(new Uri(_mockServerUrl),
                        content,
                        cancellationToken);
                    return response;
                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                    throw;
                }
            }
        }

        private static StringContent HttpContent(SaveRequestContent content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                "application/json");
        }
        
        public static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api");

            var ms = new MemoryStream();
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }


            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}
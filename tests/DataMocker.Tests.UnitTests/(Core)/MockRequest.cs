using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMocker.Mock;
using DataMocker.Mock.Handlers;
using DataMocker.Test.Data;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests.Core
{
    public class MockRequest
    {
        private readonly string url;
        private readonly string args;
        private readonly string body;

        public MockRequest(string url, string args, string body = null)
        {
            this.url = url;
            this.args = args;
            this.body = body;
        }

        public async Task<TestDataItem> PostAsync()
        {
            using (var client = new HttpClient(ResourceHandler(args)))
            {
                var response = await client.PostAsync(url, new StringContent(body));

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TestDataItem>(result);
            }
        }

        public async Task<TestDataItem> GetAsync()
        {
            using (var client = new HttpClient(ResourceHandler(args)))
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TestDataItem>(result);
            }
        }

        public async Task<HttpStatusCode> PutAsync()
        {
            using (var client = new HttpClient(ResourceHandler(args)))
            {
                var response = await client.PutAsync(
                    url,
                    new StringContent(
                        JsonConvert.SerializeObject(new TestDataItem { Url = url })
                    )
                );

                return response.StatusCode;
            }
                
        }

        public async Task<HttpStatusCode> DeleteAsync()
        {
            using (var client = new HttpClient(ResourceHandler(args)))
            {
                var response = await client.DeleteAsync(url);
                return response.StatusCode;
            }
        }

        private EmbeddedResourceHttpHandler ResourceHandler(string environmentArgs)
        {
            return (EmbeddedResourceHttpHandler)new MockHandlerInitializer(
                environmentArgs,
                typeof(MockDataComponent).Assembly
            ).GetMockerHandler();
        }
    }
}


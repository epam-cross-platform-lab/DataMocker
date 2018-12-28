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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMocker.Mock;
using DataMocker.Mock.Handlers;
using DataMocker.Test.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    public class BaseMockResourceTests
    {
        private HttpClient client;

        [TestInitialize]
        public void BeforeEachTest()
        {
            client?.Dispose();
            client = null;
            Routes.Clear();
        }

        protected async Task<TestDataItem> MakePostRequest(string url, string args, string body)
        {
            var handler = GetResourceHandler(args);
            client = new HttpClient(handler);
            var response = await client.PostAsync(url, new StringContent(body));

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TestDataItem>(result);
        }
        
        protected async Task<TestDataItem> MakeGetRequest(string url, string args)
        {
            var handler = GetResourceHandler(args);
            client = new HttpClient(handler);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TestDataItem>(result);
        }

        protected async Task<HttpStatusCode> MakePutRequest(string url, string args)
        {
            var handler = GetResourceHandler(args);
            client = new HttpClient(handler);
            var response = await client.PutAsync(url,
                new StringContent(JsonConvert.SerializeObject(new TestDataItem { Url = url })));

            return response.StatusCode;
        }

        protected async Task<HttpStatusCode> MakeDeleteRequest(string url, string args)
        {
            var handler = GetResourceHandler(args);
            client= new HttpClient(handler);
            var response = await client.DeleteAsync(url);
            return response.StatusCode;
        }

        private EmbeddedResourceHttpHandler GetResourceHandler(string environmentArgs)
        {
            var handlerInitializer = new MockHandlerInitializer(environmentArgs, typeof(MockDataComponent).Assembly);
            return (EmbeddedResourceHttpHandler)handlerInitializer.GetMockerHandler();
        }

        protected string GetEnvironmentArgsString(IList<string> testScenarios = null, 
                                                  string testName = null,
                                                  string fileFormat = null, 
                                                  string language = null, 
                                                  IList<string> sharedFoldersList = null, 
                                                  string remoteUrl = null, 
                                                  int delay = 0)
        {
            var environment = new EnvironmentArgs
            {
                Delay = delay,
                RemoteUrl = remoteUrl,
                Language = language,
                SharedFolderPath=sharedFoldersList,
                TestName = testName,
                TestScenario = testScenarios
            };

            return JsonConvert.SerializeObject(environment);
        }
    }
}
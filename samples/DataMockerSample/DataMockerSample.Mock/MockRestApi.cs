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
using System.Net.Http;
using System.Threading.Tasks;
using DataMocker;
using DataMockerSample.Common.Api;

namespace DataMockerSample.Mock.Services
{
    public class MockRestApi : IRestApi
    {
        protected HttpClient _client;
        
        public MockRestApi(IMockHandlerIntializer mockHandlerInitializer)
        {
            _client=new HttpClient(mockHandlerInitializer.GetMockerHandler());
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri) => _client.GetAsync(uri);

        public Task<HttpResponseMessage> PostAsync(Uri uri, HttpContent content) => _client.PostAsync(uri, content);

        public Task<HttpResponseMessage> PutAsync(Uri uri, HttpContent content) => _client.PutAsync(uri, content);

        public Task<HttpResponseMessage> DeleteAsync(Uri uri) => _client.DeleteAsync(uri);
    }
}
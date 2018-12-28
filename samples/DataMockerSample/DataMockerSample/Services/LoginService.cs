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
using System.Net.Http;
using System.Threading.Tasks;
using DataMockerSample.Common.Api;
using DataMockerSample.Dto;
using Newtonsoft.Json;

namespace DataMockerSample.Services
{
    public class LoginService : ILoginService
    {
        private readonly IRestApi _api;

        public string Token { get; set; }

        public LoginService(IRestApi api)
        {
            _api = api;
        }

        public async Task<bool> Login(string username, string password)
        {
            var response = await _api.PostAsync(new Uri("http://datamocker.com/login"),
                new StringContent(
                    JsonConvert.SerializeObject(new UserCreditsDto {UserName = username, Password = password})));

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            Token = await response.Content.ReadAsStringAsync();
            return !string.IsNullOrWhiteSpace(Token);
        }

        public void Logout()
        {
            Token = null;
        }
    }
}
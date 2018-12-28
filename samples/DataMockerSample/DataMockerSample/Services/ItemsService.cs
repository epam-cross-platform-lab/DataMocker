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
using System.Net.Http;
using System.Threading.Tasks;
using DataMockerSample.Common.Api;
using DataMockerSample.Dto;
using Newtonsoft.Json;

namespace DataMockerSample.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IRestApi _api;

        public ItemsService(IRestApi api)
        {
            _api = api;
        }

        public async Task<IEnumerable<ItemDto>> GetItems(string userToken)
        {
            var response = await _api.PostAsync(new Uri("http://datamocker.com/items"),
                new StringContent(userToken));

            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<IEnumerable<ItemDto>>(await response.Content.ReadAsStringAsync()) : null;
        }

        public async Task<ItemDto> GetItem(string userToken, int id)
        {
            var response = await _api.PostAsync(new Uri("http://datamocker.com/items"+$"/{id}"),
                new StringContent(userToken));

            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ItemDto>(await response.Content.ReadAsStringAsync()) : null;
        }

        public async Task<bool> PutItem(string userToken, ItemDto item)
        {
            var response = await _api.PutAsync(new Uri("http://datamocker.com/items" + $"/{item.Id}"),
                new StringContent(userToken + JsonConvert.SerializeObject(item)));

            return response.IsSuccessStatusCode;
        }
    }
}
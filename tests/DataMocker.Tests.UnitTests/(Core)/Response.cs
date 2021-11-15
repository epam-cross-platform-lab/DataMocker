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
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    public class Response
    {
        public Response()
        {
        }

        public Response(string url)
            : this(url, null)
        {
        }

        public Response(string url, string route)
        {
            Url = url;
            Route = route;
        }

        public string Route { get; set; }

        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Response item
                && Route == item.Route
                && Url == item.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Route, Url);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
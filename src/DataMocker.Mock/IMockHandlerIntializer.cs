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
using System.Net.Http;

namespace DataMocker
{
    /// <summary>
    ///  Provide interface to mock handler intializers.
    /// </summary>
    public interface IMockHandlerIntializer
    {
        /// <summary>Method should create and return  mock http handler based on HttpMessageHandler.</summary>
        /// <returns>Should ruturn MockHttpHandler based on HttpMessageHandler</returns>
        HttpMessageHandler GetMockerHandler();
    }
}
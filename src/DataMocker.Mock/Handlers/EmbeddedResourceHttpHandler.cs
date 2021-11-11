// =========================================================================
// Copyright 2021 EPAM Systems, Inc.
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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DataMocker.Mock.Resourses;
using DataMocker.SharedModels;
using DataMocker.SharedModels.Resources;

namespace DataMocker.Mock.Handlers
{
    /// <summary>
    /// EmbeddedResourceHttpHandler simulate server behaivior by using app's EmbeddedResources.
    /// </summary>
    public class EmbeddedResourceHttpHandler : MockHttpHandler
    {
        private readonly Assembly _resourceAssembly;

        private const string Separator = ".";

        internal EmbeddedResourceHttpHandler(
            MockRequestBuilder mockRequestBuilder, Assembly resourceAssembly)
            : base(mockRequestBuilder)
        {
            _resourceAssembly = resourceAssembly;
        }

        /// <summary>
        /// Return EmbededdedResourceStream for mock mockRequest.
        /// </summary>
        /// <param name="mockRequest">Mock request.</param>
        /// <returns>Task Stream with wanted mock data.</returns>
        protected override async Task<Stream> DataStream(MockRequest mockRequest)
        {
            var mockResource = new MockResource(ResourceStream(), mockRequest);
            var stream = mockResource.ToStream();

            if (stream == null)
            {
                return null;
            }

            return await Task.FromResult(stream);
        }

        private IResourceStream ResourceStream()
        {
            return new EmbeddedResourceStream(_resourceAssembly, $"{_resourceAssembly.FullName.Remove(_resourceAssembly.FullName.IndexOf(','))}{Separator}Data", Separator);
        }
    }
}

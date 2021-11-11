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

namespace DataMocker.SharedModels.Resources
{
    /// <summary>Interface for resource stream.</summary>
    public interface IResourceStream
    {
        /// <summary>Should give resource stream by path and name.</summary>
        /// <param name="resourceNameParts">Path to resource and resource name.</param>
        /// <returns>Resource stream based on System.IO.Stream .</returns>
        Stream Stream(params string[] resourceNameParts);

        /// <summary>
        /// Stream the specified resourceName.
        /// </summary>
        /// <returns>The stream.</returns>
        /// <param name="resourceName">Resource name.</param>
        Stream Stream(string resourceName);

        /// <summary>
        /// Gets the requested resource.
        /// </summary>
        /// <value>The requested resource.</value>
        string RequestedResource { get; }
    }
}
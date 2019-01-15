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
using System.IO;
using System.Reflection;
using DataMocker.SharedModels.Resources;

namespace DataMocker.Mock.Resourses
{
    internal class EmbeddedResourceStream : IResourceStream
    {
        private readonly Assembly _assembly;
        private readonly string _rootPath;
        private readonly string _separator;
        private string _requestedResource;

        public string RequestedResource => _requestedResource;

        #region IResourceStream members

        Stream IResourceStream.Stream(params string[] resourceNameParts)
        {
            var paramsToJoin = new string[resourceNameParts.Length + 1];
            paramsToJoin[0] = _rootPath;
            for (var i = 1; i < paramsToJoin.Length; i++)
            {
                paramsToJoin[i] = resourceNameParts[i - 1];
            }

            var resourceKey = string.Join(_separator, paramsToJoin, 0, paramsToJoin.Length);
            return Stream(resourceKey);
        }

#endregion

        internal EmbeddedResourceStream(Assembly assembly, string rootPath, string separator)
        {
            _assembly = assembly;
            _rootPath = rootPath;
            _separator = separator;
        }

        public Stream Stream(string resourceName)
        {
            _requestedResource = resourceName;
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[DataMocker] trying to get {resourceName} {Environment.NewLine}");
#endif
            return _assembly.GetManifestResourceStream(resourceName);
        }
    }
}

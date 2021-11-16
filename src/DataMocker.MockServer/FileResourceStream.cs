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
using System;
using System.IO;
using DataMocker.SharedModels.Resources;

namespace DataMocker.MockServer
{
    public class FileResourceStream : IResourceStream
    {
        private readonly string _rootPath;
        private string _requestedResource;

        public FileResourceStream(string rootPath)
        {
            _rootPath = rootPath;
        }

        string IResourceStream.RequestedResource => _requestedResource;

        Stream IResourceStream.Stream(params string[] resourceNameParts)
        {
            return (this as IResourceStream).Stream(
                new ResourceKey(_rootPath, resourceNameParts).ToString()
            );
        }

        Stream IResourceStream.Stream(string resourceName)
        {
            _requestedResource = resourceName;
            Console.WriteLine($"try to find {resourceName} from {Directory.GetCurrentDirectory()}");
            return new ResourceFile(resourceName).ToStream();
        }
    }
}
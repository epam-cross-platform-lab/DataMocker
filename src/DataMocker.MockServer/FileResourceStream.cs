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
using System.IO;
using DataMocker.SharedModels.Resources;

namespace DataMocker.MockServer
{
    public class FileResourceStream : IResourceStream

    {
        private readonly string _rootPath;

        private string _requestedResource;
        
        #region IResourceStream members

        string IResourceStream.RequestedResource => _requestedResource;


        Stream IResourceStream.Stream(string resourceName)
        {
            Stream stream = null;

            _requestedResource = resourceName;
            Console.WriteLine($"try to find {resourceName} from {Directory.GetCurrentDirectory()}");
            if (File.Exists(resourceName))
            {
                try
                {
                    stream = File.Open(resourceName, FileMode.Open);
                }
                catch(Exception e)
                {
                    var s = e.Message;
                }
            }
            return stream;

        }

        Stream IResourceStream.Stream(params string[] resourceNameParts)
        {
            var paramsToJoin = new string[resourceNameParts.Length + 1];

            paramsToJoin[0] = MockDataPath();

            for (var i = 1; i < paramsToJoin.Length; i++)
            {
                paramsToJoin[i] = resourceNameParts[i - 1];
            }
            var resourceKey = string.Join(Path.DirectorySeparatorChar.ToString(), paramsToJoin, 0,
                paramsToJoin.Length);                            
            return (this as IResourceStream).Stream(resourceKey);
        }                                         

        #endregion                                

        public FileResourceStream(string rootPath)
        {
            _rootPath = rootPath;
        }

        private string MockDataPath()
        {
            return _rootPath.Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
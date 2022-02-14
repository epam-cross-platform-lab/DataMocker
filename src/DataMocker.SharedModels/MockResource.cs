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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataMocker.SharedModels.Resources;

namespace DataMocker.SharedModels
{
    /// <summary>
    /// Intialize resource stream for mock request
    /// </summary>
    public class MockResource
    {
        private readonly IResourceStream _resourceStream;

        private readonly MockRequest _mockRequest;

        /// <summary>
        /// Gets the requested resource path.
        /// </summary>
        /// <value>The requested resource path.</value>
        public string RequestedResourcePath => _resourceStream.RequestedResource;

        /// <summary>
        /// Take resource stream and mock request
        /// </summary>
        /// <param name="resourceStream">resource stream</param>
        /// <param name="mockRequest">mock request</param>
        public MockResource(
            IResourceStream resourceStream,
            MockRequest mockRequest)
        {
            _resourceStream = resourceStream;
            _mockRequest = mockRequest;
        }
        
        /// <summary>
        ///  Return resource stream which initialized with mock request info
        /// </summary>
        /// <returns>resource stream for mock request</returns>
        public Stream ToStream()
        {
            return ToStream(new ResourceFromRequest(_mockRequest));
        }

        private Stream ToStream(IResourceName fileName)
        {
            IList<string> testScenarios = null;
            if (_mockRequest.TestScenarioList != null || !string.IsNullOrEmpty(_mockRequest.TestName))
            {
                testScenarios = _mockRequest.TestScenarioList ?? new List<string>();
                testScenarios.Add(_mockRequest.TestName);

                var i = 0;
                do
                {
                    var stream = GetResourceStreamAccordingLocale(
                        testScenarios.Take(testScenarios.Count - i++).ToList(),
                        fileName,
                        _mockRequest.Language
                    );
                    if (stream != null)
                    {
                        return stream;
                    }
                } while (i < testScenarios.Count);
            }

            return GetResourceStreamFromSharedFolders(fileName)
                ?? GetResourceStreamAccordingLocale(
                        testScenarios?.Take(0).ToList() ?? new List<string> (),
                        fileName,
                        _mockRequest.Language
                   );
        }

        private Stream GetResourceStreamAccordingLocale(
            List<string> path,
            IResourceName fileName,
            string language,
            bool isSharedBranch = false)
        {
            if (!string.IsNullOrWhiteSpace(language) && (path.Count <= _mockRequest.TestScenarioList.Count - 2 || isSharedBranch))
            {
                var pathWithLang = path.ToList();
                pathWithLang.Add(language);
                var stream = GetResourceStreamWithHash(pathWithLang, fileName);
                if (stream != null)
                {
                    return stream;
                }
            }

            return GetResourceStreamWithHash(path, fileName);
        }

        private Stream GetResourceStreamFromSharedFolders(IResourceName fileName)
        {
            var sharedFoldersPath = _mockRequest.SharedFoldersList;

            if (sharedFoldersPath == null || !sharedFoldersPath.Any())
            {
                return null;
            }

            var i = 0;
            do
            {
                var stream = GetResourceStreamAccordingLocale(sharedFoldersPath.Take(sharedFoldersPath.Count - i++).ToList(), fileName, _mockRequest.Language, true);
                if (stream != null)
                {
                    return stream;
                }
            } while (i <= sharedFoldersPath.Count);
            return null;
        }

        private Stream GetResourceStreamWithHash(IReadOnlyCollection<string> path, IResourceName fileName)
        {
            return _resourceStream.Stream(path.Concat(new[] {fileName.ToString(true)}).ToArray()) ??
                   _resourceStream.Stream(path.Concat(new[] {fileName.ToString(false)}).ToArray());
        }
    }
}

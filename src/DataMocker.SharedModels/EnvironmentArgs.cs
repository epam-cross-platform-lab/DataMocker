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

namespace DataMocker
{
    /// <summary>
    /// EnvironmentArgs provides application with information about current UI test. 
    /// </summary>
    public class EnvironmentArgs
    {
        /// <summary>Get or set name of the runned test.</summary>
        /// <returns>A name of the runned test.</returns>
        public string TestName { get; set; }

        /// <summary>Get or set list of current test scenarios.</summary>
        /// <returns>List of current test scenarios.</returns>
        public IList<string> TestScenario { get; set; }

        /// <summary>Gets or sets a list of shared folders.</summary>
        /// <returns>The shared folder path.</returns>
        public IList<string> SharedFolderPath { get; set; }

        /// <summary>Get or set a value of delay for requests to the mock server.</summary>
        /// <returns>A value of delay for requests to the mock server.</returns>
        public int Delay { get; set; }

        /// <summary>Get or set current culture info name of the runned test.</summary>
        /// <returns>Current culture info name of the runned test.</returns>
        public string Language { get; set; }

        /// <summary>Get or set the url of mock data server./// </summary>
        /// <returns>The url of mock data server.</returns>
        public string RemoteUrl { get; set; }
    }
}

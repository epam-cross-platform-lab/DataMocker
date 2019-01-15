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
using System.Collections.Generic;

namespace DataMocker.SharedModels
{
    /// <summary>
    /// Mock request stores and process data about handled http request and runned test
    /// </summary>
    public class MockRequest
    {
        /// <summary>
        /// Get or set file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Get or set type of request
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        ///  Get or set list of current test scenarios
        /// </summary>
        public IList<string> TestScenarioList { get; set; }

        /// <summary>
        ///  Get or set name of runned test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets or set list of shared folders.
        /// </summary>
        public IList<string> SharedFoldersList { get; set; }


        /// <summary>
        /// Get or set runned test's culture info
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Get or set content of http request  
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Property indicates is url routed by mock framework routes
        /// </summary>
        public bool IsRouted { get; set; }

        /// <summary>
        /// Get or set hash of wanted resource file
        /// </summary>
        public string Hash{ get; set; }
    }
}

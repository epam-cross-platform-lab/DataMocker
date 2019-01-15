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
using System.Collections.Generic;
using System.Globalization;

namespace DataMocker.Mock
{
    /// <summary>
    ///     Class representing a condition of test environment 
    /// </summary>
    public class MockEnvironmentConfig
    {
        /// <summary>
        ///     Gets a value of request delay
        /// </summary>
        /// <returns></returns>
        public int RequestDelay { get; private set; }

        /// <summary>
        ///     Gets the name of the test.
        /// </summary>
        /// <value>The name of the test.</value>
        public string TestName { get; private set; }

        /// <summary>
        ///     Gets the remote URL.
        /// </summary>
        /// <value>The remote URL.</value>
        public string RemoteUrl { get; private set; }

        /// <summary>
        ///     Gets the test scenarios.
        /// </summary>
        /// <value>The test scenarios.</value>
        public IList<string> TestScenarios { get; private set; }

        /// <summary>
        ///     Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public string Language { get; private set; }

        /// <summary>
        ///     Gets the shared folder.
        /// </summary>
        /// <value>The shared folder.</value>
        public IList<string> SharedFolder { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.MockEnvironmentConfig"/> class.
        /// </summary>
        public MockEnvironmentConfig() 
        { 
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.MockEnvironmentConfig"/> class.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public MockEnvironmentConfig(EnvironmentArgs args) => Initialize(args);

        /// <summary>
        ///     Initialize with specified args.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public virtual void Initialize(string args)
        {
            DefaultEnvironmentValues();

            if (string.IsNullOrWhiteSpace(args))
            {
                return;
            }

            var environmentArguments = Newtonsoft.Json.JsonConvert.DeserializeObject<EnvironmentArgs>(args);
            if (environmentArguments == null)
            {
                return;
            }

            Initialize(environmentArguments);
        }

        /// <summary>
        ///     Initialize the specified environmentArguments.
        /// </summary>
        /// <param name="environmentArguments">Environment arguments.</param>
        protected void Initialize(EnvironmentArgs environmentArguments)
        {
            TestName = environmentArguments.TestName;
            SharedFolder = environmentArguments.SharedFolderPath;
            TestScenarios = environmentArguments.TestScenario;
            RequestDelay = environmentArguments.Delay;
            Language = environmentArguments.Language?.Replace("-", "_");

            try
            {
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(Language);
            }
            catch (Exception)
            {
                CultureInfo.DefaultThreadCurrentUICulture = null;
            }


            RemoteUrl = environmentArguments.RemoteUrl;
        }

        private void DefaultEnvironmentValues()
        {
            TestName = string.Empty;
            RequestDelay = 200;
        }
    }
}
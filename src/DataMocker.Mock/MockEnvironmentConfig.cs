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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("DataMocker.Tests.UnitTests")]
namespace DataMocker.Mock
{
    /// <summary>
    /// Class representing a condition of test environment 
    /// </summary>
    internal class MockEnvironmentConfig
    {
        /// <summary>
        /// Gets a value of 
        /// </summary>
        /// <returns></returns>
        public int RequestDelay { get; private set; }

        public string TestName { get; private set; }

        public string RemoteUrl { get; private set; }

        public IList<string> TestScenarios { get; private set; }

        public string Language { get; private set; }

        public IList<string> SharedFolder { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Initialize(string args)
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

            TestName = environmentArguments.TestName;
            SharedFolder = environmentArguments.SharedFolderPath;
            TestScenarios = environmentArguments.TestScenario;
            RequestDelay = environmentArguments.Delay;
            Language = environmentArguments.Language?.Replace("-","_");

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
            RequestDelay = 2000;
        }
    }
}
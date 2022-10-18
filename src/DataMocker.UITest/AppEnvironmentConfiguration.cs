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
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.UITest;

namespace DataMocker.UITest
{
	internal class AppEnvironmentConfiguration
	{
		private readonly IApp _app;
		private readonly Platform _platform;
        private readonly Assembly _assembly;
		
		internal AppEnvironmentConfiguration(IApp app, Platform platform, Assembly assembly)
		{
			_app = app;
			_platform = platform;
            _assembly = assembly;
		}

        internal void Setup(TestMetaData testMetaData, string additionalParam)
        {
            var mockConfig = GetMockFrameworkConfiguration(_assembly);
            var backdoorMethodName = mockConfig?.GetBackDoorName(_platform);
            if (!string.IsNullOrWhiteSpace(backdoorMethodName))
            {
                _app.Invoke(backdoorMethodName, new EnvironmentArguments(testMetaData, mockConfig, additionalParam).ToString());
            }

        }

        private static MockFrameworkConfiguration GetMockFrameworkConfiguration(Assembly assembly)
        {
            using (var stream = assembly.GetManifestResourceStream(assembly.ManifestModule.Name.Replace("dll", string.Empty) + "MockFrameworkConfiguration.json")
            )
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var cofigString = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<MockFrameworkConfiguration>(cofigString);
                }
            }
        }
	}
}

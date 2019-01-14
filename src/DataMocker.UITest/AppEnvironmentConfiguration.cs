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
using Newtonsoft.Json;
using Xamarin.UITest;

namespace DataMocker.UITest
{
	internal class AppEnvironmentConfiguration
	{
		private readonly IApp _app;
		private readonly Platform _platform;
		
		internal AppEnvironmentConfiguration(IApp app, Platform platform)
		{
			_app = app;
			_platform = platform;
		}

        internal void Setup(TestMetaData testMetaData)
		{
            var mockConfig = GetMockFrameworkConfiguration();
            var backdoorMethodName = mockConfig?.GetBackDoorName(_platform);
            if (!string.IsNullOrWhiteSpace(backdoorMethodName))
            {
                _app.Invoke(backdoorMethodName, new EnvironmentArguments(testMetaData, mockConfig).ToString()); 
            }
		}

        private static MockFrameworkConfiguration GetMockFrameworkConfiguration()
        {
            try
            {

                var location = Assembly.GetExecutingAssembly().Location;
                using (var stream = File.Open(location.Remove(location.LastIndexOf('/') + 1) + "MockFrameworkConfiguration.json", FileMode.Open))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        var cofigString = streamReader.ReadToEnd();
                        return JsonConvert.DeserializeObject<MockFrameworkConfiguration>(cofigString);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
	}
}

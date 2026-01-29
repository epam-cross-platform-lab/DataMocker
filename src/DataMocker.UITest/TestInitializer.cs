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
using System.Globalization;
using System.Linq;
using DataMocker.UITest.Attributes;

namespace DataMocker.UITest
{
    /// <summary>Initialize test scenario before executing tests</summary>
    public class TestInitializer
    {
        private static IUITestContext? App;

        private static TestDevice Platform;

        /// <summary>Initialize framework in the assumption of the instance of the current app and current platform.</summary>
        /// <param name="app">Current app UITest context.</param>
        /// <param name="platform">Current platform.</param>
        /// <param name="testType">Current test type.</param>
        /// <param name="additionalParams">Additional user's params.</param>
        public static void Initialize(IUITestContext app, TestDevice platform, Type testType, string additionalParams = "")
        {
            Platform = platform;
            App = app;
            SetupAppEnvironmentVariables(testType, additionalParams);
            SetupTestLanguage(testType);
        }

        private static void SetupAppEnvironmentVariables(Type testType, string additionalParams)
        {
            var testMetaData = new TestMetaData(testType);
            new AppEnvironmentConfiguration(App!, Platform, testType.Assembly).Setup(testMetaData, additionalParams);
        }

        private static void SetupTestLanguage(Type testType)
        {
            var languageAttribute = testType.GetCustomAttributes(typeof(LanguageAttribute), true).FirstOrDefault();
            var attribute = languageAttribute as LanguageAttribute;
            CultureInfo.DefaultThreadCurrentUICulture = attribute?.CultureInfo ?? CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = attribute?.CultureInfo ?? CultureInfo.CurrentUICulture;
        }
    }
}
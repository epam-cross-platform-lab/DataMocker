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
using System.Globalization;
using System.Linq;
using DataMocker.UITest.Attributes;
using Xamarin.UITest;

namespace DataMocker.UITest
{
    /// <summary>Initialize test scenario before executing tests</summary>
    public class TestInitializer
    {
        private static IApp App;

        private static Platform Platform;

        /// <summary>Initialize framework in the assumption of the instance ofthe current app and current platfom.</summary>
        /// <param name="app">Current app.</param>
        /// <param name="platform">Current platform.</param>
        /// <param name="testType">Current test type.</param>
        public static void Initialize(IApp app, Platform platform, Type testType)
        {
            Platform = platform;
            App = app;
            SetupAppEnvironmentVariables(testType);
            SetupTestLanguage(testType);
        }

        private static void SetupAppEnvironmentVariables(Type testType)
        {
            var testMetaData = new TestMetaData(testType);
            new AppEnvironmentConfiguration(App, Platform).Setup(testMetaData);
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
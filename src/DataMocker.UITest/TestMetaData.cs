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
using System.Collections.Generic;
using System.Linq;
using DataMocker.UITest.Attributes;
using NUnit.Framework;

namespace DataMocker.UITest
{
	internal class TestMetaData
	{
	    internal TestMetaData(Type testScenarioType)
            : this(TestScenarioNames.GetTestScenarioNames(testScenarioType), testScenarioType)
		{
		}

        protected internal TestMetaData(IList<string> testScenarios, Type testScenarioType)
			: this(testScenarios, ContextTestName(), testScenarioType)
		{
		}

        protected internal TestMetaData(IList<string> testScenarios, string testName, Type testScenarioType)
			: this(testScenarios, testName, TestLanguage(testScenarioType), testScenarioType)
		{
		}

        protected  internal TestMetaData(IList<string> testScenarios, string testName, string language, Type testScenarioType) 
            : this(testScenarios, testName, language ,TestScenarioNames.GetSharedFoledrsNames(testScenarioType), testScenarioType)
        {

        }

	    internal TestMetaData(IList<string> testScenarios, string testName, string language,IList<string> sharedFolderPath, Type testScenarioType)
        {
            TestScenarioType = testScenarioType;
            TestScenarios = testScenarios;
            TestName = testName;
            SharedFolderPath = sharedFolderPath;
            Language = language;
        }

	    internal IList<string> TestScenarios { get; private set; }

	    internal IList<string> SharedFolderPath { get; private set; }

	    internal string TestName { get; private set; }

	    internal string Language { get; private set; }

	    internal Type TestScenarioType { get; private set; }

	    internal static string ContextTestName()
		{
			return TestContext.CurrentContext.Test.Name.Split('.').Last();
		}

        private static string TestLanguage(Type testType)
		{
            var languageAttribute = testType.GetCustomAttributes(typeof(LanguageAttribute), true).FirstOrDefault();
            var attribute = languageAttribute as LanguageAttribute;
            return attribute?.CultureInfo?.Name;
		}
	}
}

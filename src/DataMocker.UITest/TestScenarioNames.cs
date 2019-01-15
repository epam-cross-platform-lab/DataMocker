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
using System.Linq;
using DataMocker.UITest.Attributes;

namespace DataMocker.UITest
{
	class TestScenarioNames
	{
        public static IList<string> GetTestScenarioNames(Type testScenarionType)
        {
            var scenarionNames = new List<string>();
            Type objectType = typeof(object);
            do
            {
                scenarionNames.Add(testScenarionType.Name);
                testScenarionType = testScenarionType.BaseType;
            } while (testScenarionType != null && testScenarionType != objectType);
            scenarionNames.Reverse();
            return scenarionNames;
        }

        public static IList<string> GetSharedFoledrsNames(Type testScenarioType)
        {
            var mockDataAttibutes = testScenarioType.GetCustomAttributes(typeof(MockDataAttribute), true).Cast<MockDataAttribute>().ToList();
            var scenarioTypeMockAttibutes = new List<string>(mockDataAttibutes.Count);
            for (var i = (int)MockDataAttributePriority.High; i >= (int)MockDataAttributePriority.Low; i--)
            {
                scenarioTypeMockAttibutes.AddRange(mockDataAttibutes
                                                   .Where(mda => mda.Priority == (MockDataAttributePriority)i).Select(m => m.MockFolder));
            }
            scenarioTypeMockAttibutes.Reverse();
            return scenarioTypeMockAttibutes;
        }
	}
}

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
using System.Threading.Tasks;
using DataMocker.Mock;
using DataMocker.Tests.UnitTests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class SharedFoldersLogicTests : BaseMockResourceTests
    {
        [TestMethod]
        public async Task GetItemFromSharedFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var sharedFolders = new List<string> { "SharedFolder" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/shared/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, null, sharedFolders);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task GetItemFromSharedFolderWithHashCodeAndRoute()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var sharedFolders = new List<string> { "SharedFolder" };
            var testName = "ItemFromTestFolder";
            var route = "{controller}/{method}/{id?}";
            var url = "http://tests.com/shared/item/1";

            //Act
            Routes.AddRoute(route);
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, null, sharedFolders);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task GetItemFromSharedFolderWithCustomLocale()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var sharedFolders = new List<string> { "SharedFolder" };
            var testName = "ItemFromTestFolder";
            var language = "ru";
            var url = "http://tests.com/shared/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language, sharedFolders);
            var response = await new ResourceRequest(
                url,
                args.ToString(),
                new Response(url).ToString()
            ).PostAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }
    }
}

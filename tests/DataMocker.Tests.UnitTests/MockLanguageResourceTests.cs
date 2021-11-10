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
using System.Net;
using System.Threading.Tasks;
using DataMocker.Tests.UnitTests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class MockLanguageResourceTests : BaseMockResourceTests
    {
        #region Get

        #region GetWithoutHash

        [TestMethod]
        public async Task Get_JsonItem_FromLanguageFolder()
        {
            //Assert
            var url = "http://tests.com/language/item";

            //Act
            var args = new EnvironmentArgsString(
                new List<string>
                {
                    "MockResourceTests",
                    "LangMockResourceTests"
                },
                "ItemFromFolder",
                MockFilesFormats.Json,
                "fr-FR"
            );
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_FromLanguageFolder_FromTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/language/ts/item";
            var language = "fr-FR";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_FromLanguage_FromTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/language/ts/test/item";
            var language = "fr-FR";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }
        #endregion

        #region GetWithHash
        [TestMethod]
        public async Task Get_JsonItem_WithHashCode_FromLanguageFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var url = "http://tests.com/language/item?hash=true";
            var language = "fr-FR";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_WithHashCode_FromLanguageFolder_FromTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/language/ts/item?hash=true";
            var language = "fr-FR";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_WithHash_FromLanguage_FromTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/language/ts/test/item?hash=true";
            var language = "fr-FR";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var response = await new MockRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }
        #endregion

        #endregion

        #region Post

        [TestMethod]
        public async Task Post_JsonItem_WithDifferentHashCode_ToLanguageFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var mainUrl = "http://tests.com/language/item";
            var language = "fr-FR";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var firstItem = await new MockRequest(urlWithFirstQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithFirstQuery })).PostAsync();
            var secondItem = await new MockRequest(urlWithSecondQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithSecondQuery })).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }

        [TestMethod]
        public async Task Post_JsonItem_WithDifferentHashCode_ToLanguageFolder_ToTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var mainUrl = "http://tests.com/language/ts/item";
            var language = "fr-FR";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var firstItem = await new MockRequest(urlWithFirstQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithFirstQuery })).PostAsync();
            var secondItem = await new MockRequest(urlWithSecondQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithSecondQuery })).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }

        [TestMethod]
        public async Task Post_JsonItem_WithDifferentHashCode_ToLanguageFolder_ToTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var mainUrl = "http://tests.com/language/ts/test/item";
            var language = "fr-FR";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var firstItem = await new MockRequest(urlWithFirstQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithFirstQuery })).PostAsync();
            var secondItem = await new MockRequest(urlWithSecondQuery, args.ToString(),
                JsonConvert.SerializeObject(new TestDataItem { Url = urlWithSecondQuery })).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }
        #endregion

        #region Put

        #region PutWithoutHash

        [TestMethod]
        public async Task RightStatusCode_AfrerPut_JsonItem_ToLanguage()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotModified;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task RightStatusCode_AfrerPut_JsonItem_ToLanguage_ToTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.UnsupportedMediaType;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
        [TestMethod]
        public async Task RightStatusCode_AfrerPut_JsonItem_ToLanguage_ToTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotAcceptable;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/test/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
        #endregion

        #region PutWithHash
        [TestMethod]
        public async Task NotAcceptableStatusCode_AfrerPut_WithHashCode_JsonItem_ToLanguage()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotAcceptable;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task NotModifiedStatusCode_AfrerPut_JsonItem_WithHashCode_ToLanguage_ToTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotModified;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
        [TestMethod]
        public async Task ConflictStatusCode_AfrerPut_JsonItem_WithHashCode_ToLanguage_ToTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Conflict;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/test/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }


        #endregion
        #endregion

        #region Delete

        #region DeleteWithoutHash

        [TestMethod]
        public async Task AcceptedStatusCode_AfrerDelete_JsonItem_ToLanguageFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Accepted;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task ForbiddenStatusCode_AfrerDelete_JsonItem_FromLanguage_FromTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Forbidden;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task GoneStatusCode_AfrerDelete_JsonItem_FromLanguage_FromTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Gone;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/test/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
        #endregion

        #region DeleteWithHash

        [TestMethod]
        public async Task BadRequestStatusCode_AfrerDelete_JsonItem_WithHashCode_ToLanguageFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.BadRequest;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task OKStatusCode_AfrerDelete_JsonItem_WithHashCode_FromLanguage_FromTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.OK;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTS";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task GoneStatusCode_AfrerDelete_JsonItem_WithHashCode_FromLanguage_FromTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Gone;
            var testScenario = new List<string> { "MockResourceTests", "LangMockResourceTests" };
            var testName = "ItemFromTestFolder";
            var language = "fr-FR";
            var url = "http://tests.com/language/ts/test/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json, language);
            var actual = await new MockRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        #endregion
        #endregion
    }
}
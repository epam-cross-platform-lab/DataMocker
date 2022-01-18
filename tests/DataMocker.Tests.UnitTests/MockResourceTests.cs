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
    public class MockResourceTests : BaseMockResourceTests
    {
        #region Get

        #region GetWithoutHash

        [TestMethod]
        public async Task Get_JsonItem_FromRootFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_FromTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_FromTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }
        #endregion

        #region GetWithHash

        [TestMethod]
        public async Task Get_JsonItem_WithHashCode_FromRootFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_WithHashCode_FromTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }

        [TestMethod]
        public async Task Get_JsonItem_WithHashCode_FromTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var response = await new ResourceRequest(url, args.ToString()).GetAsync();

            //Assert
            Assert.AreEqual(url, response?.Url);
        }
        #endregion
        #endregion

        #region Post

        [TestMethod]
        public async Task Post_TwoJsonItem_WithDifferentHashCode_ToRootFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var mainUrl = "http://tests.com/root/item";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var firstItem = await new ResourceRequest(
                urlWithFirstQuery,
                args.ToString(),
                new Response(urlWithFirstQuery).ToString()
            ).PostAsync();
            var secondItem = await new ResourceRequest(
                urlWithSecondQuery,
                args.ToString(),
                new Response(urlWithSecondQuery).ToString()
            ).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }

        [TestMethod]
        public async Task Post_TwoJsonItem_WithDifferentHashCode_ToTestScenarioFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var mainUrl = "http://tests.com/ts/item";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var firstItem = await new ResourceRequest(urlWithFirstQuery, args.ToString(),
                JsonConvert.SerializeObject(new Response(urlWithFirstQuery))).PostAsync();
            var secondItem = await new ResourceRequest(urlWithSecondQuery, args.ToString(),
                JsonConvert.SerializeObject(new Response(urlWithSecondQuery))).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }

        [TestMethod]
        public async Task Post_TwoJsonItem_WithDifferentHashCode_ToTestFolder()
        {
            //Assert
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var mainUrl = "http://tests.com/ts/test/item";
            var urlWithFirstQuery = mainUrl + "?par=1";
            var urlWithSecondQuery = mainUrl + "?par=2";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var firstItem = await new ResourceRequest(urlWithFirstQuery, args.ToString(),
                JsonConvert.SerializeObject(new Response(urlWithFirstQuery))).PostAsync();
            var secondItem = await new ResourceRequest(urlWithSecondQuery, args.ToString(),
                JsonConvert.SerializeObject(new Response(urlWithSecondQuery))).PostAsync();

            //Assert
            Assert.AreEqual(new Uri(firstItem.Url).LocalPath, new Uri(secondItem.Url).LocalPath);
            Assert.AreNotEqual(new Uri(firstItem.Url), new Uri(secondItem.Url));
        }
        #endregion

        #region Put

        #region PutWithoutHash

        [TestMethod]
        public async Task NonAuthoritativeInformationStatusCode_AfrerPut_JsonItem_ToRoot()
        {
            //Assert
            var expectedValue = HttpStatusCode.NonAuthoritativeInformation;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task CreatedStatusCode_AfrerPut_JsonItem_ToTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Created;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task OKStatusCode_AfrerPut_JsonItem_ToTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.OK;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        #endregion

        #region PutWithHash
        [TestMethod]
        public async Task UnauthorizedStatusCode_AfrerPut_JsonItem_WithHashCode_ToRoot()
        {
            //Assert
            var expectedValue = HttpStatusCode.Unauthorized;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task BadRequestStatusCode_AfrerPut_JsonItem_WithHashCode_ToTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.BadRequest;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task NotAcceptableStatusCode_AfrerPut_JsonItem_WithHashCode_ToTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotAcceptable;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).PutAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }


        #endregion

        #endregion

        #region Delete

        #region DeleteWithouHash

        [TestMethod]
        public async Task OKStatusCode_AfrerDelete_JsonItem_FromRootFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.OK;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task NotFoundStatusCode_AfrerDelete_JsonItem_FromTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotFound;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task GoneStatusCode_AfrerDelete_JsonItem_FromTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Gone;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }
        #endregion

        #region DeleteWithHash
        [TestMethod]
        public async Task BadRequestStatusCode_AfrerDelete_JsonItem_WithHashCode_FromRootFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.BadRequest;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromRoot";
            var url = "http://tests.com/root/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task ForbiddenStatusCode_AfrerDelete_JsonItem_WithHashCode_FromTestScenarioFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.Forbidden;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTS";
            var url = "http://tests.com/ts/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestMethod]
        public async Task NotModifiedStatusCode_AfrerDelete_JsonItem_WithHashCode_FromTestFolder()
        {
            //Assert
            var expectedValue = HttpStatusCode.NotModified;
            var testScenario = new List<string> { "MockResourceTests" };
            var testName = "ItemFromTestFolder";
            var url = "http://tests.com/ts/test/item?hash=true";

            //Act
            var args = new EnvironmentArgsString(testScenario, testName, MockFilesFormats.Json);
            var actual = await new ResourceRequest(url, args.ToString()).DeleteAsync();

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        #endregion
        #endregion
    }
}
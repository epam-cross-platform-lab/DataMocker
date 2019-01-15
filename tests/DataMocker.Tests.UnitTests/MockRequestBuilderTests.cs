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
using System.Net.Http;
using DataMocker.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json; 

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class MockRequestBuilderTests
    {
        [TestInitialize]
        public void Setup()
        {
            Routes.Clear();
        }

        [TestMethod]
        public void MockRequest_AfterBuild_WithRouting()
        {
            //Assign
            const string expectedValue = "_controller_41";
            const string url = "http://example.com/controller/method/41";
            var appEnvironmentConfig = GetEnvironmentConfig(MockFilesFormats.Json, new List<string> { nameof(MockRequestBuilderTests) }, nameof(MockRequest_AfterBuild_WithRouting));
            Routes.AddRoute("{controller}/method/{id}");
            const bool isRequestRouted = true;

            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            var mockrequestBuilder = new MockRequestBuilder(appEnvironmentConfig);
            var mockRequest = mockrequestBuilder.MockRequest(message, isRequestRouted);

            //Assert
            Assert.AreEqual(expectedValue, mockRequest.FileName);
        }

        [TestMethod]
        public void MockRequest_AfterBuild_WithoutRouting()
        {
            //Assign
            const string expectedValue = "_controller_method_41";
            const string url = "http://example.com/controller/method/41";
            var appEnvironmentConfig = GetEnvironmentConfig(MockFilesFormats.Json,new List<string>{ nameof(MockRequestBuilderTests)}, nameof(MockRequest_AfterBuild_WithRouting));
            Routes.AddRoute("{controller}/method/{id}");
            const bool isRequestRouted = false;

            //Act
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            var mockrequestBuilder = new MockRequestBuilder(appEnvironmentConfig);
            var mockRequest = mockrequestBuilder.MockRequest(message, isRequestRouted);

            //Assert
            Assert.AreEqual(expectedValue,mockRequest.FileName);
        }

        private static MockEnvironmentConfig GetEnvironmentConfig(string json,IList<string> testScenario, string testName)
        {
            var arg= new EnvironmentArgs {TestName = testName,TestScenario = testScenario};
            var config = new MockEnvironmentConfig();
            config.Initialize(JsonConvert.SerializeObject(arg));
            return config;
        }
    }
}

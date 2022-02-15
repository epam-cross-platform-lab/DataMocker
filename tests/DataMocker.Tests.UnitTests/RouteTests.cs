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
using DataMocker.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class RouteTests
    {
        [TestInitialize]
        public void Setup()
        {
            Routes.Clear();
        }

        [TestMethod]
        public void RoutedFileName_RouteUrl()
        {
            //Assign
            var route = "controller/{method}/{id}";
            var routedUrl = "http://example.com/controller/method/41";

            //Assert
            Assert.AreEqual(
                "_method_41",
                GetRoutedName(route, routedUrl)
            );
        }

        [TestMethod]
        public void RoutedFileName_RouteUrl_WithQuery()
        {
            //Assign
            var route = "controller/{method}/{id}?";
            var routedUrl = "http://example.com/controller/method/41?par=1&tet=ssss";

            //Assert
            Assert.AreEqual(
                "_method_41",
                GetRoutedName(route, routedUrl)
            );
        }

        [TestMethod]
        public void RoutedFileNameByQuery_WhenRouteUrl_WithQuery()
        {
            //Assign
            var route = "apiscanning.php?filter={id}&date={id2}";
            var routedUrl = "http://test.server.be:80/webservice/apiscanning.php?filter=getData&date=2022-02-11";

            //Assert
            Assert.AreEqual(
                "_getData_2022-02-11",
                GetRoutedName(route, routedUrl)
            );
        }

        [TestMethod]
        public void RoutedFileName_RouteWithOptionalSegment_WhenUrlSegmentsLenghtSameWithRouteRuleLenght()
        {
            //Assign
            var expectedFileName = "_method";
            var route = "controller/{method}/{id?}";
            var routedUrl = "http://example.com/controller/method/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void RoutedFileName_RouteWithOptionalSegment_WhenUrlSegmentsLenghtLessThanWithRouteRuleLenght()
        {
            //Assign
            var expectedFileName = "_method";
            var route = "controller/{method}/{id?}";
            var routedUrl = "http://example.com/controller/method";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void RoutedFileName_RouteWithOptionalSegment_WhenRouteContainOnlyOptionalSegments()
        {
            //Assign
            var expectedFileName = "_controller_method_41";
            var route = "controller/method/41/{id?}";
            var routedUrl = "http://example.com/controller/method/41/22";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void RoutedFileName_RouteWithOptionalSegment_WhenRouteContainSeveralOptionalSegments()
        {
            //Assign
            var expectedFileName = "_controller_method";
            var route = "controller/method/{sd?}/{id?}";
            var routedUrl = "http://example.com/controller/method/41/22";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void RoutedFileName_RouteUrl_WithSlashAtTheEnd()
        {
            //Assign
            var expectedFileName = "_method_41";
            var route = "controller/{method}/{id}";
            var routedUrl = "http://example.com/controller/method/41/";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void SkipRoute_RouteUnroutedUrl_WhenUrlSegmentsLessThenRouteSegments()
        {
            //Assign
            var route = "{controller}/{method}/{id}";
            var routedUrl = "http://example.com/segment/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.IsNull(actualFileName);
        }

        [TestMethod]
        public void SkipRoute_RouteUnroutedUrl_WhenUrlSegmentsLessThenRouteSegments_AndSomeSegmentsEqual()
        {
            //Assign
            var route = "controller/{method}/{id}";
            var routedUrl = "http://example.com/controller/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.IsNull(actualFileName);
        }

        [TestMethod]
        public void FirstRoutedFileName_RouteWithSeveralRules_WhenFirstRouteWithOptionalSegment()
        {
            //Assign
            Routes.AddRoute("{controller}/{method}/{id?}");
            var expectedFileName = "_controller_method";
            var route = "controller/{method}/{id}";
            var routedUrl = "http://example.com/controller/method/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void FirstRoutedFileName_RouteWithSeveralRules_WhenSecondRouteWithOptionalSegment()
        {
            //Assign
            Routes.AddRoute("controller/{method}/{id}");
            var expectedFileName = "_method_41";
            var route = "controller/{method}/{id?}";
            var routedUrl = "http://example.com/controller/method/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void TestRoutesOrder3()
        {
            //Assign
            Routes.AddRoute("controller/method/{id?}");
            var expectedFileName = "_controller_method";
            var route = "controller/{method}/{id?}";
            var routedUrl = "http://example.com/controller/method/41";

            //Act
            var actualFileName = GetRoutedName(route, routedUrl);

            //Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [TestMethod]
        public void TestDeletingRoute1()
        {
            //Assign
            var route = "controller/method/{id?}";
            var url = "http://example.com/controller/method/41";

            //Act
            Routes.AddRoute(route);
            var uri = new Uri(url);
            Routes.DeleteRoute(route);
            var name = Routes.RoutedNameByUrl(uri);

            //Assert
            Assert.IsNull(name);
        }

        [TestMethod]
        public void TestDeletingRoute2()
        {
            //Assign
            var expectedFileName = "_method";
            var url = "http://example.com/controller/method/41";
            var mainRoute = "controller/{method}/{id?}";
            var routeForDelete = "controller/method/{id?}";

            //Act
            Routes.AddRoute(routeForDelete);
            Routes.AddRoute(mainRoute);
            var uri = new Uri(url);
            Routes.DeleteRoute(routeForDelete);
            var name = Routes.RoutedNameByUrl(uri);

            //Assert
            Assert.AreEqual(expectedFileName, name);
        }   

        private string GetRoutedName(string routeRule, string urlString)
        {
            Routes.AddRoute(routeRule);
            Routes.AddRoute("api/v1/{firstSegment}/{secondSegment}/{variativeSegment?}");
            return Routes.RoutedNameByUrl(new Uri(urlString));
        }
    }
}
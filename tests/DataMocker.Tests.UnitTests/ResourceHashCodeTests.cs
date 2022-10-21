using System;
using System.Web;
using DataMocker.SharedModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class ResourceHashCodeTests
    {
        [TestMethod]
        public void ResourceHashCode_ToHexString_IgnoresDatesInQueryParams()
        {
            // Arrange
            var uri = PrepareUriWithDate(DateTime.Now);
            var uriForExpectedResult = PrepareUriWithDate(default(DateTime));
            IHashCode resourceHash = new ResourceHashCode(uri, string.Empty);
            IHashCode expectedResourceHash = new ResourceHashCode(uriForExpectedResult, string.Empty);

            // Act
            var hash = resourceHash.ToHexString();
            var expectedHash = expectedResourceHash.ToHexString();

            // Assert
            Assert.IsNotNull(hash);
            Assert.AreEqual(hash, expectedHash);
        }

        [TestMethod]
        public void ResourceHashCode_ToHexString_IgnoresDatesInRequestBody()
        {
            //Assert
            var uri = new Uri("http://example.com/test");

            var body = new
            {
                Date = DateTime.Now,
                Offset = DateTimeOffset.UtcNow,
                Name = "name"
            };
            var bodyForExpectedResult = $"{{\"Date\":\"\",\"Offset\":\"\",\"Name\":\"name\"}}";


            // Act
            IHashCode hash = new ResourceHashCode(uri, JsonConvert.SerializeObject(body));
            IHashCode expectedHash = new ResourceHashCode(uri, bodyForExpectedResult);

            // Assert
            Assert.AreEqual(hash.ToHexString(), expectedHash.ToHexString());
        }

        private static Uri PrepareUriWithDate(DateTime date)
        {
            var uriBuilder = new UriBuilder("http://example.com/test");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["date"] = date.ToString();
            parameters["baz"] = "baz";
            uriBuilder.Query = parameters.ToString();
            var uri = uriBuilder.Uri;
            return uri;
        }
    }
}

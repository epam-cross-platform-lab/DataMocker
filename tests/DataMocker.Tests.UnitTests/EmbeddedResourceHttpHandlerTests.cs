using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMocker.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class EmbeddedResourceHttpHandlerTests
    {
        private string _resourceUri = "http://wdc-logcnt.eurocenter.be:80/webservice/apiscanning.php?filter=getListCmr&date=2022-02-11";

        [TestMethod]
        public async Task ResourceNotFound()
        {
            Routes.AddRoute("webservice/apiscanning.php?{id}");
            var response = await new HttpClient(
                new MockHandlerInitializer(
                    "",
                    this.GetType().Assembly
                ).GetMockerHandler()
            ).GetAsync(_resourceUri);

            Assert.AreEqual(
                 HttpStatusCode.NotFound,
                 response.StatusCode
            );

            Assert.AreEqual(
                 "Resource file hasn't been found: [get_webservice_apiscanning_php.json, get_webservice_apiscanning_php(191EC4BC).json]",
                 await response.Content.ReadAsStringAsync()
            );
        }
    }
}

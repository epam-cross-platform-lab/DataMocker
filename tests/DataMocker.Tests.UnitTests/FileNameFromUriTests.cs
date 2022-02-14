using System;
using DataMocker.SharedModels.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class FileNameFromUriTests
    {
        [TestMethod]
        public void FromUri()
        {
            Assert.AreEqual(
                "_webservice_apiscanning_php",
                new ResourceFromUri(
                    new Uri("http://wdc-logcnt.eurocenter.be:80/webservice/apiscanning.php?filter=getListCmr&date=2022-02-11")
                ).ToString()
            );
        }
    }
}

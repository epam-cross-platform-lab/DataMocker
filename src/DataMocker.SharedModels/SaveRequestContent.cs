using System.Net.Http;

namespace DataMocker.SharedModels
{
    public class SaveRequestContent
    {
        public string Response { get; set; }

        public MockRequest MockRequest { get; set; }
    }
}
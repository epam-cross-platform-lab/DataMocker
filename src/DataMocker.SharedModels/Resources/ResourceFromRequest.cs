namespace DataMocker.SharedModels.Resources
{
    public class ResourceFromRequest : IResourceName
    {
        private readonly MockRequest _mockRequest;

        public ResourceFromRequest(MockRequest mockRequest)
        {
            _mockRequest = mockRequest;
        }

        public string ToString(bool withHash)
        {
            return new JsonResourceFileName(
                _mockRequest.HttpMethod.ToLower() + _mockRequest.FileName,
                _mockRequest.Hash
            ).ToString(withHash);
        }

        public override string ToString()
        {
            return ToString(true);
        }
    }
}

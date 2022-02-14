using System;
using System.Linq;

namespace DataMocker.SharedModels.Resources
{
    /// <summary>
    /// Generates mock file name.
    /// </summary>
    public class ResourceFromUri
    {
        private readonly Uri _url;

        public ResourceFromUri(Uri url)
        {
            _url = url;
        }

        public override string ToString()
        {
            return string.Join("_", _url.Segments.Select(s => s.Trim('/').Replace('.', '_')));
        }
    }
}

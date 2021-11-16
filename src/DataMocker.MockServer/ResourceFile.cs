using System;
using System.IO;

namespace DataMocker.MockServer
{
    public class ResourceFile
    {
        private readonly string _resourceName;

        public ResourceFile(string resourceName)
        {
            _resourceName = resourceName;
        }

        public Stream ToStream()
        {
            Stream stream = null;
            if (File.Exists(_resourceName))
            {
                stream = File.Open(_resourceName, FileMode.Open);
            }
            return stream;
        }
    }
}


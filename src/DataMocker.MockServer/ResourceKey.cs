using System;
using System.IO;

namespace DataMocker.MockServer
{
    public class ResourceKey
    {
        private readonly string _rootPath;
        private readonly string[] _resourceNameParts;

        public ResourceKey(string rootPath, params string[] resourceNameParts)
        {
            _rootPath = rootPath;
            _resourceNameParts = resourceNameParts;
        }

        public override string ToString()
        {
            var paramsToJoin = new string[_resourceNameParts.Length + 1];
            paramsToJoin[0] = MockDataPath();

            for (var i = 1; i < paramsToJoin.Length; i++)
            {
                paramsToJoin[i] = _resourceNameParts[i - 1];
            }
            return string.Join(
                Path.DirectorySeparatorChar.ToString(),
                paramsToJoin,
                0,
                paramsToJoin.Length
            );
        }

        private string MockDataPath()
        {
            return _rootPath.Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}


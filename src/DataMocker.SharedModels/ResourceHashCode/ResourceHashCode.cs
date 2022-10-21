// =========================================================================
// Copyright 2021 EPAM Systems, Inc.
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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

[assembly: InternalsVisibleTo("DataMocker.Mock")]
[assembly: InternalsVisibleTo("DataMocker.MockServer")]
[assembly: InternalsVisibleTo("DataMocker.Mock.GraphQL")]
namespace DataMocker.SharedModels
{
    internal class ResourceHashCode : IHashCode
    {
        private readonly string _body;
        private readonly Encoding _encoding;
        private readonly Uri _uri;

        public ResourceHashCode(Uri uri, string body)
        {
            _body = body;
            _uri = uri;
            _encoding = Encoding.UTF8;
        }

        string IHashCode.ToHexString()
        {
            return HexStringFromBytes(SHA1Bytes(string.Concat(_uri.AbsoluteUri, _body)));
        }

        private byte[] SHA1Bytes(string input)
        {
            byte[] hash;
            using (var sha1 = SHA1.Create())
            {
                hash = sha1.ComputeHash(_encoding.GetBytes(input));
            }

            return hash;
        }

        private string HexStringFromBytes(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                // can be "x2" if you want lowercase
                var hex = b.ToString("X2");
                sb.Append(hex);
            }

            return sb.ToString();
        }
    }
}

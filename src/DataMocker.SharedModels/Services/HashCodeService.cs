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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataMocker.SharedModels.Services
{
    internal class HashCodeService : IHashCodeService
    {
        private static readonly Encoding Encoding;

        static HashCodeService()
        {
            Encoding = Encoding.UTF8;
        }

        string IHashCodeService.GetHashCodeAndConvertToX2(string input)
        {
            var hash = GetHashCodeInternal(input);
            return HexStringFromBytes(hash);
        }

        private static byte[] GetHashCodeInternal(string input)
        {
            byte[] hash;
            using (var sha1 = SHA1.Create())
            {
                hash = sha1.ComputeHash(Encoding.GetBytes(input));
            }

            return hash;
        }

        private static string HexStringFromBytes(IEnumerable<byte> bytes)
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

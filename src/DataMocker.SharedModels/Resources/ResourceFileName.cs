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
namespace DataMocker.SharedModels.Resources
{
    internal class ResourceFileName
    {
        private readonly string _fileName;

        internal string HashCode { get; }

        internal ResourceFileName(string fileName, string hashCode)
        {
            _fileName = fileName;
            HashCode = hashCode;
        }

        internal string ToString(bool withHash)
        {
            return FileName(_fileName, withHash ? WrappedHashCodeString(HashCode) : string.Empty);
        }

        protected virtual string FileName(string fileName, string hashCode)
        {
            return string.Format(fileName + hashCode);
        }

        private static string WrappedHashCodeString(string hashCode)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(hashCode))
            {
                result = $"({hashCode})";
            }

            return result;
        }
    }
}

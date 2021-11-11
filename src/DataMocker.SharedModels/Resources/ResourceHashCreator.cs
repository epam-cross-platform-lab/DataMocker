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
using System.Runtime.CompilerServices;
using DataMocker.SharedModels.Services;

[assembly: InternalsVisibleTo("DataMocker.Mock")]
[assembly: InternalsVisibleTo("DataMocker.MockServer")]
[assembly: InternalsVisibleTo("DataMocker.Mock.GraphQL")]
namespace DataMocker.SharedModels.Resources
{
    internal class ResourceHashCreator
    { 
        internal static string Create(string url, string body)
        {
            var hashSource = string.Concat(url, body);
            IHashCodeService hashCodeService= new HashCodeService();
            var hashCode = hashCodeService.GetHashCodeAndConvertToX2(hashSource);
            return hashCode.Substring(0, 8);
        }
    }
}

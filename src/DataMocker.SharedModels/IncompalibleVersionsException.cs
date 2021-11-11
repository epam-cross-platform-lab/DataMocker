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
using System.Net;

namespace DataMocker.SharedModels
{
    /// <summary>
    /// Exception which will be throwed if incompatible versions of mock framework are installed in app and mock server 
    /// </summary>
    public class IncompalibleVersionsException : Exception
    {
        /// <summary>
        /// Return mock framework version installed in the mock server 
        /// </summary>
        public string ServerVersion { get; }

        /// <summary>
        /// Return mock framework version installed in the application 
        /// </summary>
        public string ClientVersion { get; }

        /// <summary>
        /// Exception message
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// Constructor takes client and server version of mock test freamework
        /// </summary>
        /// <param name="serverVersion">server version of mock test freamework</param>
        /// <param name="clientVersion">client version of mock test freamework</param>
        public IncompalibleVersionsException(string serverVersion, string clientVersion)
        {
            ServerVersion = serverVersion;
            ClientVersion = clientVersion;
            Message = $"Incompatible client ({ClientVersion}) and server ({ServerVersion}) versions";
        }
    }
}
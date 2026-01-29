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
namespace DataMocker.UITest
{
    /// <summary>
    /// Interface for MAUI UITest context compatibility.
    /// Provides abstraction for invoking backdoor methods in MAUI UI tests.
    /// </summary>
    public interface IUITestContext
    {
        /// <summary>
        /// Invokes a backdoor method on the app with the specified method name and argument.
        /// </summary>
        /// <param name="methodName">The name of the backdoor method to invoke.</param>
        /// <param name="argument">The argument to pass to the backdoor method.</param>
        void Invoke(string methodName, string argument);
    }
}

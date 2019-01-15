// =========================================================================
// Copyright 2019 EPAM Systems, Inc.
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
namespace DataMocker.UITest.Attributes
{
    /// <summary>Order for the <see cref="T:DataMocker.UITest.Attributes.MockDataAttribute"/>.</summary>
    public enum MockDataAttributePriority : int
    {
        /// <summary>Low priority.</summary>
        Low = 0,
        /// <summary>Medium priority.</summary>
        Medium = 1,
        /// <summary>High priority.</summary>
        High=2
    }
}
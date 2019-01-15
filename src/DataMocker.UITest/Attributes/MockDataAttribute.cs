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
using System;

namespace DataMocker.UITest.Attributes
{
    /// <summary>Gets shared folder for ui test scenario</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class MockDataAttribute : Attribute
	{
        /// <summary>Gets name of the shared folder.</summary>
		public string MockFolder
		{
			get;
			private set;
		}

        /// <summary>Gets the priority of the shaserd folder.</summary>
	    public MockDataAttributePriority Priority
        {
            get;
            private set;
        }

        /// <summary>Initialize new instance of <see cref="T:DataMocker.UITest.Attributes.MockDataAttribute"/> from folder name and folder priority.</summary>
        /// <param name="mockFolder">Name of the shared folder.</param>
        /// <param name="priority">Shared folder priority.</param>
        public MockDataAttribute(string mockFolder, MockDataAttributePriority priority = MockDataAttributePriority.Medium)
		{
			MockFolder = mockFolder;
            Priority = priority;
		}
	}
}

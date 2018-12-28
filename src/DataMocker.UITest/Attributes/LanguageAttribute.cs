// =========================================================================
// Copyright 2018 EPAM Systems, Inc.
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
using System.Globalization;

namespace DataMocker.UITest.Attributes
{
    /// <summary>Set custom locale for ui test scenario</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LanguageAttribute : Attribute
    {
        /// <summary>Gets current test scenario's locale.</summary>
        public CultureInfo CultureInfo
        {
            get;
            private set;
        }

        /// <summary>Initialize new instance of <see cref="T:DataMocker.UITest.Attributes.LanguageAttribute"/> from language name.</summary>
        /// <param name="langName">Name of the custom locale.</param>
        public LanguageAttribute(string langName)
        {
            var c = new CultureInfo(langName);
            if (c != null)
            {
                CultureInfo = c;
            }
        }

        /// <summary>Initialize new instance of <see cref="T:DataMocker.UITest.Attributes.LanguageAttribute"/> from language code.</summary>
        /// <param name="langCode">Code of custom locale.</param>
        public LanguageAttribute(int langCode)
        {
            var c = new CultureInfo(langCode);
            if (c != null)
            {
                CultureInfo = c;
            }
        }
    }
}
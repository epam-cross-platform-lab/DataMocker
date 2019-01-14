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
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace DataMockerSample.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .PreferIdeSettings()
                    .EnableLocalScreenshots()
                    .InstalledApp("com.epam.DataMockerSample")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .PreferIdeSettings()
                .EnableLocalScreenshots()
                .InstalledApp("com.epam.DataMockerSample")
                .DeviceIdentifier("06FACE51-369A-4AD2-9591-377A87A39982")
                .StartApp();
        }
    }
}

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
using NUnit.Framework;
using Xamarin.UITest;
using DataMockerSample.UITests.Constants;
using DataMockerSample.UITests.Resources;

namespace DataMockerSample.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class MobileTest : BaseTest
    {
        public MobileTest(Platform platform):base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {
            app.WaitForElement(TestResources.LoginScreen_LoginButtonText, LoginScreen.LoginButtonMissingMessage);
        }

        [Test]
        public void Login()
        {
            app.EnterText(TestResources.LoginScreen_UserPlaceholderText, TestResources.CommonUser_UserName);
            app.EnterText(TestResources.LoginScreen_PasswordPlaceholderText, TestResources.CommonUser_Password);
            app.Tap(TestResources.LoginScreen_LoginButtonText);
            app.WaitForElement(TestResources.ItemsPage_FirstElementTitle,"First item didn't appear");
            app.WaitForElement(TestResources.ItemsPage_SecondElementTitle, "Second item didn't appear");
            app.WaitForElement(TestResources.ItemsPage_ThirdEleementTitle, "Third item didn't appear");
        }

        [Test]
        public void OpenItem()
        {
            app.EnterText(TestResources.LoginScreen_UserPlaceholderText, TestResources.CommonUser_UserName);
            app.EnterText(TestResources.LoginScreen_PasswordPlaceholderText, TestResources.CommonUser_Password);
            app.Tap(TestResources.LoginScreen_LoginButtonText);
            app.Tap(TestResources.ItemsPage_FirstElementTitle);
            app.WaitForElement(TestResources.ItemPage_TitleLabelText,"Item didn't open");
        }

        [Test]
        public void EditItem()
        {
            app.EnterText(TestResources.LoginScreen_UserPlaceholderText, TestResources.CommonUser_UserName);
            app.EnterText(TestResources.LoginScreen_PasswordPlaceholderText, TestResources.CommonUser_Password);
            app.Tap(TestResources.LoginScreen_LoginButtonText);
            app.Tap(TestResources.ItemsPage_FirstElementTitle);
            if (platform == Platform.Android) 
            {
              app.ClearText(TestResources.ItemPage_FirstDescrtiptionOldText);
              app.EnterText(TestResources.ItemPage_FirstDescriptionNewText);
            }
            else if (platform == Platform.iOS)  
            {
                app.ClearText(c => c.Id("ItemDescription"));
                app.EnterText(c => c.Id("ItemDescription"),TestResources.ItemPage_FirstDescriptionNewText);
            }
            app.Tap(TestResources.ItemPage_SaveButtonText);
            app.WaitForElement(TestResources.ItemsPage_FirstElementTitle,"Item did'n changed");
        }
    }
}


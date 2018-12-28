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
using Autofac;
using DataMockerSample.Common.Api;
using DataMockerSample.Pages;
using DataMockerSample.Services;
using DataMockerSample.ViewModels;
using Xamarin.Forms;

namespace DataMockerSample
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
            InitContainer();
        }

        protected override void OnStart()
        {
        }

        private void InitContainer()
        {
            if (Container == null)
            {
                RegisterMockHandlerInitializer(null);
            }
        }

        public static void RegisterMockHandlerInitializer(string environmentArguments)
        {
            var builder = new ContainerBuilder();
            if (string.IsNullOrWhiteSpace(environmentArguments))
            {
                environmentArguments = "{\"TestName\":\"ShowChangeDataOnFly\",\"TestScenario\":[\"NonTestWork\"],\"SharedFolderPath\":[],\"Delay\":500,\"Language\":null}";
            }
            var api = Mock.MockDataComponent.GetMockApi(environmentArguments);
            builder.RegisterInstance(api).As<IRestApi>();
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            builder.RegisterType<ItemsService>().As<IItemsService>().SingleInstance();

            builder.RegisterType(typeof(LoginViewModel));
            builder.RegisterType(typeof(EditItemViewModel));
            builder.RegisterType(typeof(ItemsViewModel));

            Container = builder.Build();
            Current.MainPage = new NavigationPage(new LoginPage(Container.Resolve<LoginViewModel>()));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;
using DataMockerSample.Services;
using DataMockerSample.Pages;

namespace DataMockerSample.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand _loginCommand;

        private string _userName, _password;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(async ()=>await Login()));

        public LoginViewModel(ILoginService loginService) : base(loginService)
        {
        }

        private async  Task Login()
        {
            IsLoading = true;
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            var isLoggedIn = await loginService.Login(UserName, Password);
            IsLoading = false;
            if (isLoggedIn)
            {
                var ivm = App.Container.Resolve<ItemsViewModel>();
                Application.Current.MainPage =
                               new NavigationPage(new ItemsPage(ivm))
                    {
                        BarTextColor = Color.Bisque,
                        BarBackgroundColor = Color.DarkBlue
                    };
            }
            else
            {
                await CurrentPage.DisplayAlert("Login failed", "Wrong username or password", "Ok");
            }
            
        }

        
    }
}
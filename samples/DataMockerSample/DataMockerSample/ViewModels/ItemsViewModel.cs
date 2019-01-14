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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using DataMockerSample.Dto;
using DataMockerSample.Pages;
using DataMockerSample.Services;
using Xamarin.Forms;

namespace DataMockerSample.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IItemsService _itemsService;

        private ObservableCollection<ItemDto> _items;

        private ICommand _logoutCommand, _openItemCommand, _refreshCommand;

        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new Command(async () => await Logout()));

        public ICommand OpenItemCommand => _openItemCommand ?? (_openItemCommand = new Command<ItemDto>(
                                               async item => { await OpenItem(item); }));

        public ICommand RefreshCommand => _refreshCommand ??
                                          (_refreshCommand = new Command(async () =>
                                          {
                                              await LoadItems();
                                          }));

        public ObservableCollection<ItemDto> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ItemsViewModel(IItemsService itemsService, ILoginService loginService) : base(loginService)
        {
            _itemsService = itemsService;
        }

        public void OnPageAppeared(object sender, EventArgs args)
        {
            Task.Run(async () => await LoadItems());
        }

        private async Task LoadItems()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(1000);
                var items = await _itemsService.GetItems(loginService.Token);
                Items = new ObservableCollection<ItemDto>(items);
                IsLoading = false;
            }
            catch(Exception e)
            {
                var s = e.Message;
            }
        }

        private async Task OpenItem(ItemDto item)
        {
            var viewModel = App.Container.Resolve<EditItemViewModel>();
            viewModel.Initialize(item?.Id);
            await CurrentPage.Navigation.PushAsync(new EditItemPage(viewModel));
        }

        private async Task Logout()
        {
            var logoutAnswer = await CurrentPage.DisplayAlert("Exit", "Do you really want to leave?", "Yes", "No");
            if (logoutAnswer)
            {
                loginService.Logout();
                Application.Current.MainPage =
                               new NavigationPage(new LoginPage(App.Container.Resolve<LoginViewModel>()));
            }
        }
    }
}
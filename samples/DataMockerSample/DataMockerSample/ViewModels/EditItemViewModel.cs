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
using System.Threading.Tasks;
using System.Windows.Input;
using DataMockerSample.Dto;
using DataMockerSample.Services;
using Xamarin.Forms;

namespace DataMockerSample.ViewModels
{
    public class EditItemViewModel: BaseViewModel
    {
        private readonly IItemsService _itemsService;
        private int? _id;
        private ItemDto _item;
        private ICommand _saveCommand;

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(async () => { await Save(); }));

        public ItemDto Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public EditItemViewModel(IItemsService itemsService, ILoginService loginService) : base(loginService)
        {
            _itemsService = itemsService;
        }

        public void Initialize(int? id)
        {
            _id = id;
        }

        public void OnPageAppeared(object sender, EventArgs args)
        {
            if (_id.HasValue)
            {
                Task.Run(async () => { Item = await _itemsService.GetItem(loginService.Token, _id.Value); });
                return;
            }
            Item= new ItemDto();
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Item.Title) || string.IsNullOrWhiteSpace(Item.Text))
            {
                await CurrentPage.DisplayAlert("Warning", "Some fields are empty!", "Ok");
                return;
            }
            var result = await _itemsService.PutItem(loginService.Token, Item);

            if (!result)
            {
                await CurrentPage.DisplayAlert("Error", "Server error", "Ok");
            }
            else
            {
                await CurrentPage.Navigation.PopAsync();
            }
        }

    }
}
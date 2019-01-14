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
using System.Windows.Input;
using Xamarin.Forms;

namespace DataMockerSample.Controls
{
    public class ExtendedListView : ListView
    {
        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(ExtendedListView));

        public ExtendedListView()
        {
            ItemTapped += OnItemTapped;
        }


        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null || ItemClickCommand == null || !ItemClickCommand.CanExecute(e))
            {
                return;
            }

            ItemClickCommand.Execute(e.Item);
            SelectedItem = null;
        }
    }
}
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public partial class MenuPage : ContentPage
    {
        ItemViewModel vm;
        public MenuPage()
        {
            InitializeComponent();
            vm = new ItemViewModel();
            listView.ItemsSource = vm.Items;
			//BindingContext = vm;

			DateTime dateTime = DateTime.UtcNow.Date;
            System.Diagnostics.Debug.WriteLine(dateTime.ToString("dd/MM/yyyy"));

		}
    }
}

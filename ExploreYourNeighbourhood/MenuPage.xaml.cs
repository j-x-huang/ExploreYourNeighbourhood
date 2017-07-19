using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
			listView.ItemTemplate = new DataTemplate(typeof(CustomCell));

		}
    }
}

﻿using System;
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
            //Show items
            listView.ItemsSource = vm.Items;


		}
    }
}

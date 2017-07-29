using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public partial class LocationPage : ContentPage
    {
		Geocoder geoCoder;

		public LocationPage()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }

		async void Handle_ClickedAsync(object sender, System.EventArgs e)
		{
			List<LocationModel> LocationInformation = await AzureManager.AzureManagerInstance.GetLocationInformation();

			LocationList.ItemsSource = LocationInformation;

		}
    }
}

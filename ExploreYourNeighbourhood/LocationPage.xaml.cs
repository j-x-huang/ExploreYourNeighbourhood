using System;
using System.Collections.Generic;
using Plugin.Geolocator;
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

			foreach (LocationModel model in LocationInformation)
			{

				var position = new Position(Convert.ToDouble(model.Latitude), Convert.ToDouble(model.Longitude));
				var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
				foreach (var address in possibleAddresses)
					model.City = address;
			}

			LocationList.ItemsSource = LocationInformation;

		}
    }
}

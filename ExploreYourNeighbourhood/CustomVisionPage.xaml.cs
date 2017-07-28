using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;


using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public partial class CustomVisionPage : ContentPage
    {
        string itemToPost = "";
        Geocoder geoCoder;

        public CustomVisionPage()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }
		
        private async void loadCamera(object sender, EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				PhotoSize = PhotoSize.Medium,
				Directory = "Sample",
				Name = $"{DateTime.UtcNow}.jpg"
			});

			if (file == null)
				return;

			image.Source = ImageSource.FromStream(() =>
			{
				return file.GetStream();
			});
			resultBtn.IsVisible = false;

			await MakePredictionRequest(file);
		}

		static byte[] GetImageAsByteArray(MediaFile file)
		{
			var stream = file.GetStream();
			BinaryReader binaryReader = new BinaryReader(stream);
			return binaryReader.ReadBytes((int)stream.Length);
		}

		async Task MakePredictionRequest(MediaFile file)
		{
			var client = new HttpClient();
			var probabilityList = new List<double>();
			var itemList = new List<string>();
            StatusLabel.Text = "Analysing..";


			client.DefaultRequestHeaders.Add("Prediction-Key", "b582db3effb145019697ca4ea0b6a6f6");

			string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/3756e514-0165-4d5b-946b-5751936002ff/image?iterationId=414ec412-072e-4cf5-a99f-329762fdd796";

			HttpResponseMessage response;

			byte[] byteData = GetImageAsByteArray(file);

			using (var content = new ByteArrayContent(byteData))
			{

				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    JObject rss = JObject.Parse(responseString);
                    //Querying with LINQ
                    //Get all Prediction Values
                    var Probability = from p in rss["Predictions"] select (string)p["Probability"];
                    var Tag = from p in rss["Predictions"] select (string)p["Tag"];

					foreach (var item in Tag)
					{
                        itemList.Add(item);
						//System.Diagnostics.Debug.WriteLine(item);

					}

                    foreach (var item in Probability)
                    {
						
                        if (item.ToString().ToLower().Contains("e"))
						{
							double x = Double.Parse(item.ToString(), System.Globalization.NumberStyles.Float);
							probabilityList.Add(x);
						}
						else
						{
							probabilityList.Add(Convert.ToDouble(item.ToString()));
						}
						//System.Diagnostics.Debug.WriteLine(item);


					}

                    if (probabilityList.Max() > 0.5)
                    {
                        int maxIndex = probabilityList.IndexOf(probabilityList.Max());
                        var itemName = itemList[maxIndex];
                        string realName = "";
                        switch (itemName)
                        {
                            case "Stop":
                                realName = "Stop Sign";
                                break;
                            case "Post":
                                realName = "Post Box";
                                break;
                            case "Bench":
                                realName = "Park Bench";
                                break;
                            case "Booth":
                                realName = "Phone Booth";
                                break;
                            case "Pohutukawa":
                                realName = "Pohutukawa";
                                break;

                        }
                        StatusLabel.Text = "You have found a " + realName;
                        itemToPost = realName;
                        resultBtn.IsVisible=true;
                    } else {
                        StatusLabel.Text = "It appears that the object you have found is not on the list";
                    }
                } else {
                    StatusLabel.Text = "Bad server response. Please try again.";
                }

				//Get rid of file once we have finished using it
				file.Dispose();
			}
		}

		private async void SendData(object sender, EventArgs e)
        {
            resultBtn.IsVisible = false;
            resultsLabel.Text = "Saving..";
            await postLocationAsync();
        }

		async Task postLocationAsync()
		{

			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
            resultsLabel.Text = "Saving.. Getting location";
			var position = await locator.GetPositionAsync(10000);


            var locationPin = new Position(position.Latitude, position.Longitude);
            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(locationPin);

            List<string> addressList = possibleAddresses.ToList();
			string city = "";

            if (addressList.Count() > 0) {
                city = addressList[0];
                //System.Diagnostics.Debug.WriteLine(city);
                string[] splitArray=city.Split('\n');
                string cityShort = "";
                if (splitArray.Count() > 2) {
                    cityShort = splitArray[2] + '\n' + splitArray[1];
                }

				DateTime dateTime = DateTime.UtcNow.Date;

				LocationModel model = new LocationModel()
				{
					City = cityShort,
					Item = itemToPost,
					CurrentDate = dateTime.ToString("dd/MM/yyyy")

				};
                resultsLabel.Text = "Saving.. Uploading to cloud";
                Task postInfo = AzureManager.AzureManagerInstance.PostLocationInformation(model);
                await postInfo;
                if (postInfo.IsCompleted)
                    resultsLabel.Text = "Saved!";
                //await AzureManager.AzureManagerInstance.PostLocationInformation(model);

            } else {
                resultBtn.IsVisible = true;
                resultsLabel.Text = "Save failed";
            }


		}
    }
}

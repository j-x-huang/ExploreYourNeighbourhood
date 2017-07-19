using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public partial class CustomVisionPage : ContentPage
    {
        public CustomVisionPage()
        {
            InitializeComponent();
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

			client.DefaultRequestHeaders.Add("Prediction-Key", "b582db3effb145019697ca4ea0b6a6f6");

			string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/3756e514-0165-4d5b-946b-5751936002ff/image?iterationId=896a826d-cb3a-48fc-9fc8-fac64d198b6b";

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

                    foreach (var item in Probability)
                    {
                        PredictionLabel.Text += item + "\n";

                    }
                } else {
                    StatusLabel.Text = "Bad server response";
                }

				//Get rid of file once we have finished using it
				file.Dispose();
			}
		}
    }
}

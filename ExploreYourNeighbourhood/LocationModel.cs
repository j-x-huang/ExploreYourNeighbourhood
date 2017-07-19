using System;
using Newtonsoft.Json;
namespace ExploreYourNeighbourhood
{
    public class LocationModel
    {
        
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public string Latitude { get; set; }

		public string City { get; set; }


	}
}

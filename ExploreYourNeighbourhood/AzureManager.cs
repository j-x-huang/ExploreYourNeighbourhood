using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace ExploreYourNeighbourhood
{
    public class AzureManager
    {
		private IMobileServiceTable<LocationModel> locationTable;
		private static AzureManager instance;
		private MobileServiceClient client;

		private AzureManager()
		{
			this.client = new MobileServiceClient("https://jxh-myhotdog.azurewebsites.net");
			this.locationTable = this.client.GetTable<LocationModel>();

		}

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}
		public async Task<List<LocationModel>> GetLocationInformation()
		{
            return await this.locationTable.ToListAsync();
		}

        public async Task PostLocationInformation(LocationModel locationModel)
		{
            await this.locationTable.InsertAsync(locationModel);
		}

	}
}

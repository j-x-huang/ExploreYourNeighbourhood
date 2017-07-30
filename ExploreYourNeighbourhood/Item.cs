using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public class Item
    {
        public string Name { get; set; }
        public string ImgSrc { get; set; }
        public string Difficulty { get; set; }
        public string Colour { get; set; }

        //items that users need to find
        public List<Item> GetItems()
        {
            List<Item> itemList = new List<Item>()
            {
                new Item(){ Name="Park bench", ImgSrc="bench.jpg", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Stop sign", ImgSrc="stop.png", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Post box", ImgSrc="crop_post.jpg", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Pohutukawa tree", ImgSrc="crop_poh.jpg", Difficulty="Medium", Colour="#ff8b0f" },
                new Item(){ Name="Telephone booth", ImgSrc="booth.jpg", Difficulty="Hard", Colour="Red"}
            };
            return itemList;
        }
	}

    public class ItemViewModel
    {
        public List<Item> Items { get; set; }
         
        public ItemViewModel()
        {
            Items = new Item().GetItems();
        }
    }

}

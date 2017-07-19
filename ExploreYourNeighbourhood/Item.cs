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

        public List<Item> GetItems()
        {
            List<Item> itemList = new List<Item>()
            {
                new Item(){ Name="Park bench", ImgSrc="parkBench.jpg", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Stop sign", ImgSrc="stop.jpg", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Post box", ImgSrc="post.jpg", Difficulty="Easy", Colour="Green" },
                new Item(){ Name="Pohutukawa tree", ImgSrc="poh.jpg", Difficulty="Medium", Colour="#ff8b0f" },
                new Item(){ Name="Telephone booth", ImgSrc="booth", Difficulty="Hard", Colour="Red"}
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
    /*
    public class Data
    {
        public static ObservableCollection<Item> itemList = new ObservableCollection<Item>() {
            new Item(){ Name="Park bench", ImgSrc="bench.png" },
            new Item(){ Name="Stop sign", ImgSrc="bench.png" }
        };

    }
    */
}

using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ExploreYourNeighbourhood
{
    public class Item
    {
        public string Name { get; set; }
        public ImageSource ImgSrc { get; set; }
    }

    public class Data
    {
        public static ObservableCollection<Item> itemList = new ObservableCollection<Item>() {
            new Item(){ Name="Park bench", ImgSrc=ImageSource.FromFile("bench.png") },
            new Item(){ Name="Stop sign", ImgSrc=ImageSource.FromFile("bench.png") }
        };
    }
}

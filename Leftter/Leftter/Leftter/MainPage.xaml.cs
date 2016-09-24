using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Leftter
{
    public partial class MainPage : ContentPage
    {
        // 動的にList<ListItem>を追加するのに使う
        ObservableCollection<ListItem> listItems = new ObservableCollection<ListItem>();
        Position position;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = listItems;

            sendButton.Clicked += async delegate
            {
                AddListItem(setEntry.Text);
                setEntry.Text = string.Empty;

                //IGeolocator locator = CrossGeolocator.Current;
                //locator.DesiredAccuracy = 50;

                //if(locator.IsGeolocationAvailable)
                //    if(locator.IsGeolocationEnabled)
                //    {
                //        position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                //        AddListItem("Latitude:\t" + position.Latitude);
                //        AddListItem("Longitude:\t" + position.Longitude);
                //    }

                list.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
            };            
        }

        void AddListItem(string text)
        {
            listItems.Add(new ListItem { TextItem = text , DetailItem = "text"});
        }
    }
}

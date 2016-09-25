using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Firebase.Xamarin.Database;

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
        }

        private void SendButtonClicked(object sender, EventArgs e)
        {
            AddListItem(setEditor.Text, DateTime.Now.ToString());
            setEditor.Text = string.Empty;

            GetGPS();
        }

        private async void GetButtonClicked(object sender, EventArgs e)
        {
            var firebase = new FirebaseClient("https://test-da012.firebaseio.com/");

            var child = firebase.Child("messages");
            var orderedChild = Firebase.Xamarin.Database.Query.QueryFactoryExtensions.OrderByKey(child);
            var limttedChild = Firebase.Xamarin.Database.Query.QueryExtensions.LimitToFirst(orderedChild, 2);
            var items = await limttedChild.OnceAsync<ListItem>();

            foreach(var item in items)
            {
                AddListItem($"{item.Key}");
            }
        }

        private async void GetGPS()
        {
            IGeolocator locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            if(locator.IsGeolocationAvailable)
                if(locator.IsGeolocationEnabled)
                {
                    try
                    {
                        position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                        AddListItem("Latitude:\t" + position.Latitude + "\nLongitude:\t" + position.Longitude);
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Could not get GPS data.", "OK");
                    }
            }

            logListView.ScrollTo(listItems.Last(), ScrollToPosition.End, true);
        }

        void AddListItem(string text)
        {
            listItems.Add(new ListItem { TextItem = text, DetailItem = string.Empty });
        }

        void AddListItem(string text, string detail)
        {
            listItems.Add(new ListItem { TextItem = text,  DetailItem = detail });
        }
    }
}
